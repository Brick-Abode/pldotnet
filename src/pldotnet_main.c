/*
 * PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as
 *             procedural languages (PL)
 *
 *
 * Copyright (c) 2023 Brick Abode
 *
 * This code is subject to the terms of the PostgreSQL License.
 * The full text of the license can be found in the LICENSE file
 * at the top level of the pldotnet repository.
 *
 * pldotnet_main.c
 *
 */

#include "pldotnet_main.h"

/*
 * Exported functions
 */
PG_FUNCTION_INFO_V1(plcsharp_call_handler);
PG_FUNCTION_INFO_V1(plcsharp_inline_handler);
PG_FUNCTION_INFO_V1(plcsharp_validator);
PG_FUNCTION_INFO_V1(plfsharp_call_handler);
PG_FUNCTION_INFO_V1(plfsharp_inline_handler);
PG_FUNCTION_INFO_V1(plfsharp_validator);
PG_FUNCTION_INFO_V1(plvisualbasic_call_handler);
PG_FUNCTION_INFO_V1(plvisualbasic_inline_handler);
PG_FUNCTION_INFO_V1(plvisualbasic_validator);

/*
 * START: declaring variables
 */

/*
 * Directories where C#/F# projects for user code are built when
 * USE_DOTNETBUILD is defined. Otherwise that is where our C#/F# compiler
 * projects are located. Default for Linux is /var/lib/PlDotNet/
 */
char *root_path = NULL;
char *dnldir = STR(PLDOTNET_ENGINE_DIR);

GHashTable *procedures = nullptr;

dotnet_loader assembly_loader = nullptr;

compile_user_fn compile_user_function = nullptr;
build_datum_list_fn build_datum_list = nullptr;
add_datum_to_list_fn add_datum_to_list = nullptr;
free_generic_gchandle_fn free_generic_gchandle = nullptr;
unload_assemblies_fn unload_assemblies = nullptr;
run_user_fn run_user_function = nullptr;

pldotnet_PathConfig path_config;

/*
 * END: declaring variables
 */

/*
 * START: declaring static functions
 */

/**
 * @brief The main handler function, which receives an additional bool argument
 * to deal with both normal and inline calls.
 *
 * @param fcinfo The standard parameter list for fmgr-compatible functions.
 * @param is_inline Wether the function is inline or not.
 * @param language The .NET language (C# or F#).
 * @return The datum that can be stored in a PostgreSQL table.
 */
static Datum pldotnet_generic_handler(FunctionCallInfo fcinfo, bool is_inline,
                                      pldotnet_Language language);

 /**
 * @brief The validator function will inspect the function body for syntactical
 * correctness, but it can also look at other properties of the function,
 * for example if the language cannot handle certain argument types.
 * This functions calls the C# function to create and compile the user function
 * into .NET Framework.
 *
 * @param fcinfo The standard parameter list for fmgr-compatible functions.
 * @param language The .NET language (C# or F#).
 * @return a void datum.
 */
static Datum pldotnet_validator(FunctionCallInfo fcinfo,
                                pldotnet_Language language);

/**
 * @brief This function starts to building the output paths into a static
 * pldotnet_PathConfig, then it parses the procedure data into a valid source
 * code. This second step produces a valid pldotnet_UserFunctionDeclaration
 * which contains useful information regarding the current function. While
 * building pldotnet_UserFunctionDeclaration, it tries to find a previous cached
 * function aiming to avoid reloading stuff from .NET.
 *
 * Case 1: If there is no previous cached function, it loads .NET, gets the
 * function pointers and calls Engine.Compile() and Engine.Run() to retrieve the
 * desired results. After calling the user function, it saves the current
 * pldotnet_UserFunctionDeclaration into a global hash table called procedures
 * (see pldotnet_main.h).
 *
 *  Case 2: if there is a previous cached function, then it just calls
 * Engine.RunUserFunction()
 * @param fcinfo The standard parameter list for fmgr-compatible functions.
 * @param is_inline Wether the function is inline or not.
 * @param language The .NET language (C# or F#).
 * @return The datum that can be stored in a PostgreSQL table.
 */
static Datum pldotnet_CompileAndRunUserFunction(const FunctionCallInfo fcinfo,
                                                bool is_inline,
                                                pldotnet_Language language);

/**
 * @brief Returns the declared function or creates one.
 *
 * @param oid the function ID.
 * @param fcinfo
 * @param proc
 * @param is_inline whether it is a inline function or not
 * @param validation whether the function should be validated.
 * @param language The .NET language (C# or F#).
 * @return pldotnet_UserFunctionDeclaration* the function created by the user.
 */
static pldotnet_UserFunctionDeclaration *pldotnet_GetFunctionDecl(
    Oid oid, FunctionCallInfo fcinfo, HeapTuple proc, bool is_inline,
    bool validation, pldotnet_Language language);

/**
 * @brief This function tries to build a valid pldotnet_UserFunctionDeclaration.
 * It searchs for information on PG SysCache and it stores the required data
 * into the last argument (pldotnet_UserFunctionDeclaration *function_decl) this
 * structure is meant to be saved into a hash table.
 *
 * @param oid the function ID.
 * @param fcinfo
 * @param proc
 * @param is_inline whether it is a inline function or not.
 * @param validation whether the function should be validated.
 * @param function_decl the struct that contains all the function information
 * @param language the programming language (C# or F#)
 * @return true it it succeeds.
 * @return false otherwise.
 */
static bool pldotnet_BuildFunctionDecl(
    Oid oid, FunctionCallInfo fcinfo, HeapTuple proc, bool is_inline,
    bool validation, pldotnet_UserFunctionDeclaration *function_decl,
    pldotnet_Language language);

/**
 * @brief Set the function information to the pldotnet_GetSourceCode object
 * if the function is not null.
 *
 * @param fcinfo the function information
 * @param proc
 * @param procst
 * @param is_inline whether the functions is inline
 * @param validation whetter the function should be validated
 * @param user_function_decl the struct that stores the function information
 * @param language the programming language (C# or F#)
 * @return true if the process was successful
 * @return false if the source code could not be obtained
 */
static bool pldotnet_GetSourceCode(
    FunctionCallInfo fcinfo, HeapTuple proc, Form_pg_proc procst,
    bool is_inline, bool validation,
    pldotnet_UserFunctionDeclaration *user_function_decl,
    pldotnet_Language language);

/**
 * @brief Find and return a specified .NET method.
 *
 * @param library_path The .NET project path.
 * @param dotnet_type The .NET type (the namespace and class).
 * @param dotnet_type_method The .NET function name.
 * @param delegate_type_name The .NET delegate function name.
 * @return void* the C function pointer that points to the specified .NET method
 */
static void *pldotnet_GetDotNetMethod(const char *library_path,
                                      const char *dotnet_type,
                                      const char *dotnet_type_method,
                                      const char *delegate_type_name);

/**
 * @brief Calls the CompileUserFunction function from the .NET environment.
 * Thus, this function will generate and dynamically compile the user code,
 * which is defined through the "declaration" argument.
 *
 * @param declaration The object that contains the user's function information.
 * @return true if the process succeeded.
 * @return false if the process failed.
 */
static bool pldotnet_CompileUserFunction(
    pldotnet_UserFunctionDeclaration *declaration);

/**
 * @brief Saves or replaces the user function in the hash table.
 *
 * @param function the object that contains the user function information
 * @param insert whether the function should be inserted. On the other hand,
 * it will be replaced.
 */
static void pldotnet_SaveFunction(pldotnet_UserFunctionDeclaration *function,
                                  bool insert);

/**
 * @brief Returns the function body as a char*.
 *
 * @param proc
 * @param procst
 * @return const char* the function body.
 */
static const char *pldotnet_GetFunctionBody(HeapTuple proc,
                                            Form_pg_proc procst);

/**
 * @brief Returns the SQL parameter names as a char*.
 *
 * @param proc the HeapTuple object.
 * @param procst the Form_pg_proc object.
 * @return const char* the SQL parameters.
 */
static const char *pldotnet_GetSqlParamsName(HeapTuple proc,
                                             Form_pg_proc procst);

/**
 * @brief Returns the OIDs of the SQL function as an int*.
 *
 * @param proc the HeapTuple object.
 * @param procst the Form_pg_proc object.
 * @return const Oid* with the OID of the arguments.
 */
static const Oid *pldotnet_GetSqlParamsType(HeapTuple proc,
                                            Form_pg_proc procst);

/**
 * @brief Creates a list of IntPtr using C# methods and then adds the user
 * arguments in this list and returns the created list.
 *
 * @param fcinfo the FunctionCallInfo object.
 * @param procst the Form_pg_proc object.
 * @return void* the List<IntPtr> that contains the Datums.
 */
static void *pldotnet_BuildArgumentList(FunctionCallInfo fcinfo,
                                        Form_pg_proc procst);

/**
 * @brief Return the Datum of an specific argument.
 *
 * @param fcinfo the function information
 * @param index the argument index
 * @return Datum of the provided index of the arguments list.
 */
static Datum pldotnet_GetArgDatum(FunctionCallInfo fcinfo, size_t index);

/**
 * @brief Maps the NULL Datums in the user argument list.
 *
 * @return bool* the pointer to the nullmap of user arguments.
 */
static bool *pldotnet_BuildNullArgumentList(FunctionCallInfo fcinfo,
                                            Form_pg_proc procst);

/**
 * @brief Check that the datum argument is null.
 *
 * @param fcinfo the function information
 * @param index the argument index
 * @return true if the referent argument is null.
 * @return false if the referent argument is non-null.
 */
static bool pldotnet_CheckNullArgument(FunctionCallInfo fcinfo, size_t index);

/**
 * @brief Retuns the Postgres Heap.
 *
 * @param oid the function ID.
 * @return HeapTuple
 */
static HeapTuple pldotnet_GetPostgresHeapTuple(Oid oid);

/**
 * @brief Calls the ReleaseSysCache function from utils/syscache.h.
 *
 * @param proc the HeapTuple object
 */
static void pldotnet_ReleasePostgresHeapTuple(HeapTuple proc);

/**
 * @brief Allocate to the memory a pldotnet_UserFunctionDeclaration
 * struct and return it.
 *
 * @return pldotnet_UserFunctionDeclaration*
 */
static pldotnet_UserFunctionDeclaration *pldotnet_CreateFunctionDecl(void);

/**
 * @brief Finds the pldotnet function declared by the user through its ID.
 *
 * @param function_id the function ID.
 * @return pldotnet_UserFunctionDeclaration*
 */
static pldotnet_UserFunctionDeclaration *pldotnet_FindFunctionDecl(
    int function_id);

/**
 * @brief Reset the pldotnet function.
 *
 * @param function_decl the function that will be reset
 */
static void pldotnet_ResetFunctionDecl(
    pldotnet_UserFunctionDeclaration *function_decl);

/**
 * @brief Creates a new memory context.
 *
 * @param config
 */
static void pldotnet_StartNewMemoryContext(MemoryContextWrapper *config);

/**
 * @brief Reverts the created memory context.
 *
 * @param config
 */
static void pldotnet_ResetMemoryContext(MemoryContextWrapper *config);

/*
 * START: implementing functions
 */

Datum plcsharp_call_handler(PG_FUNCTION_ARGS) {
    return pldotnet_generic_handler(fcinfo, false, csharp);
}

Datum plcsharp_inline_handler(PG_FUNCTION_ARGS) {
    Datum result = pldotnet_generic_handler(fcinfo, true, csharp);
    unload_assemblies(fcinfo->flinfo->fn_oid);
    return result;
}

Datum plcsharp_validator(PG_FUNCTION_ARGS) {
    return pldotnet_validator(fcinfo, csharp);
}

Datum plfsharp_call_handler(PG_FUNCTION_ARGS) {
    return pldotnet_generic_handler(fcinfo, false, fsharp);
}

Datum plfsharp_inline_handler(PG_FUNCTION_ARGS) {
    Datum result = pldotnet_generic_handler(fcinfo, true, fsharp);
    unload_assemblies(fcinfo->flinfo->fn_oid);
    return result;
}

Datum plfsharp_validator(PG_FUNCTION_ARGS) {
    return pldotnet_validator(fcinfo, fsharp);
}

Datum plvisualbasic_call_handler(PG_FUNCTION_ARGS) {
    return pldotnet_generic_handler(fcinfo, false, visual_basic);
}
Datum plvisualbasic_inline_handler(PG_FUNCTION_ARGS) {
    Datum result = pldotnet_generic_handler(fcinfo, true, visual_basic);
    unload_assemblies(fcinfo->flinfo->fn_oid);
    return result;
}
Datum plvisualbasic_validator(PG_FUNCTION_ARGS) {
    return pldotnet_validator(fcinfo, visual_basic);
}

bool pldotnet_BuildPaths(void) {
    const char json_path_suffix[] =
        "/bin/Release/net6.0/PlDotNET."
        "runtimeconfig.json";
    const char dll_path_suffix[] = "/bin/Release/net6.0/PlDotNET.dll";

    SNPRINTF(path_config.prefix, MAXPGPATH, "%s", root_path);
    SNPRINTF(path_config.config_path, MAXPGPATH, "%s%s", root_path,
             json_path_suffix);
    SNPRINTF(path_config.library_path, MAXPGPATH, "%s%s", root_path,
             dll_path_suffix);
    return true;
}

bool pldotnet_SetNetLoader(void) {
    if (assembly_loader != nullptr)
        return true;

    assembly_loader =
        GetNetLoadAssemblySetup(path_config.config_path, path_config.prefix);

    return nullptr != assembly_loader;
}

bool pldotnet_SetDotNetMethods(void) {
    const char *library_path = path_config.library_path;

    compile_user_function = (compile_user_fn)pldotnet_GetDotNetMethod(
        library_path, "PlDotNET.Engine, PlDotNET", "CompileUserFunction",
        "PlDotNET.Engine+DelCompileUserFunction, PlDotNET");

    run_user_function = (run_user_fn)pldotnet_GetDotNetMethod(
        library_path, "PlDotNET.Engine, PlDotNET", "RunUserFunction",
        "PlDotNET.Engine+DelRunUserFunction, PlDotNET");

    build_datum_list = (build_datum_list_fn)pldotnet_GetDotNetMethod(
        library_path, "PlDotNET.Engine, PlDotNET", "BuildDatumList",
        "PlDotNET.Engine+DelBuildDatumList, PlDotNET");

    add_datum_to_list = (add_datum_to_list_fn)pldotnet_GetDotNetMethod(
        library_path, "PlDotNET.Engine, PlDotNET", "AddDatumToList",
        "PlDotNET.Engine+DelAddDatumToList, PlDotNET");

    free_generic_gchandle = (free_generic_gchandle_fn)pldotnet_GetDotNetMethod(
        library_path, "PlDotNET.Engine, PlDotNET", "FreeGenericGCHandle",
        "PlDotNET.Engine+DelFreeGenericGCHandle, PlDotNET");

    unload_assemblies = (unload_assemblies_fn)pldotnet_GetDotNetMethod(
        library_path, "PlDotNET.Engine, PlDotNET", "UnloadAssemblies",
        "PlDotNET.Engine+DelUnloadAssemblies, PlDotNET");

    return nullptr != compile_user_function && nullptr != run_user_function &&
           nullptr != build_datum_list && nullptr != add_datum_to_list &&
           nullptr != free_generic_gchandle && nullptr != unload_assemblies;
}

void pldotnet_Elog(int level, char *message) {
    elog(level, "%s", message);
}

static Datum pldotnet_generic_handler(FunctionCallInfo fcinfo, bool is_inline,
                                      pldotnet_Language language) {
    MemoryContextWrapper memory_context;
    Datum retval = 0;

    PG_TRY();
    {
        /* START NEW MEM CONTEXT */
        pldotnet_StartNewMemoryContext(&memory_context);

        retval =
            pldotnet_CompileAndRunUserFunction(fcinfo, is_inline, language);

        /* REVERT PREV MEM CONTEXT */
        pldotnet_ResetMemoryContext(&memory_context);
    }
    PG_CATCH();
    { PG_RE_THROW(); }

    PG_END_TRY();

    return retval;
}

static Datum pldotnet_validator(FunctionCallInfo fcinfo,
                                pldotnet_Language language) {
    MemoryContextWrapper memory_context;
    HeapTuple proc;
    Oid funcoid = PG_GETARG_OID(0);

    if (!check_function_bodies)
        return (Datum)0;

    PG_TRY();
    {
        /* START NEW MEM CONTEXT */
        pldotnet_StartNewMemoryContext(&memory_context);

        proc = pldotnet_GetPostgresHeapTuple(funcoid);

        pldotnet_GetFunctionDecl(funcoid, fcinfo, proc, false, true, language);

        pldotnet_ReleasePostgresHeapTuple(proc);

        /* REVERT PREV MEM CONTEXT */
        pldotnet_ResetMemoryContext(&memory_context);
    }
    PG_CATCH();
    { PG_RE_THROW(); }
    PG_END_TRY();

    PG_RETURN_VOID();
}

static Datum pldotnet_CompileAndRunUserFunction(const FunctionCallInfo fcinfo,
                                                bool is_inline,
                                                pldotnet_Language language) {
    HeapTuple proc;
    Form_pg_proc procst;
    pldotnet_UserFunctionDeclaration *function_decl = nullptr;
    void *arglist = nullptr;
    bool *nullmap = nullptr;
    pldotnet_Result output;
    int res;
    output.value = (Datum)0;
    output.is_null = false;

    proc = pldotnet_GetPostgresHeapTuple(fcinfo->flinfo->fn_oid);

    function_decl = pldotnet_GetFunctionDecl(fcinfo->flinfo->fn_oid, fcinfo,
                                             proc, is_inline, false, language);

    procst = (Form_pg_proc)GETSTRUCT(proc);

    pldotnet_ReleasePostgresHeapTuple(proc);

    if (nullptr == function_decl || nullptr == run_user_function)
        elog(ERROR, "[pldotnet]: Could not load function_decl");

    arglist = pldotnet_BuildArgumentList(fcinfo, procst);
    nullmap = function_decl->support_null_input
                  ? pldotnet_BuildNullArgumentList(fcinfo, procst)
                  : nullptr;

    res = run_user_function(function_decl->func_oid, arglist, &nullmap[0],
                            (void *)&output);

    if (res != 0)
        elog(ERROR, "PL.NET function \"%s\".", function_decl->func_name);

    if (output.is_null)
        fcinfo->isnull = true;

    free_generic_gchandle(arglist);
    if (nullmap)
        pfree(nullmap);

    return output.value;
}

static pldotnet_UserFunctionDeclaration *pldotnet_GetFunctionDecl(
    Oid oid, FunctionCallInfo fcinfo, HeapTuple proc, bool is_inline,
    bool validation, pldotnet_Language language) {
    pldotnet_UserFunctionDeclaration *decl = pldotnet_FindFunctionDecl(oid);
    bool found = nullptr != decl;

    if (found && !validation)
        return decl;

    decl = pldotnet_CreateFunctionDecl();

    pldotnet_BuildFunctionDecl(oid, fcinfo, proc, is_inline, validation, decl,
                               language);

    if (!is_inline)
        pldotnet_SaveFunction(decl, !found);

    return decl;
}

static bool pldotnet_BuildFunctionDecl(
    Oid oid, FunctionCallInfo fcinfo, HeapTuple proc, bool is_inline,
    bool validation, pldotnet_UserFunctionDeclaration *function_decl,
    pldotnet_Language language) {
    Form_pg_proc procst = (Form_pg_proc)GETSTRUCT(proc);

    if (nullptr == function_decl)
        elog(ERROR, "[pldotnet]: Invalid argument, function_decl is null");

    /* save some basic data */
    function_decl->func_oid = (uint32_t)oid;
    function_decl->support_null_input = procst->proisstrict ? false : true;
    function_decl->func_ret_type = procst->prorettype;

    if (!pldotnet_GetSourceCode(fcinfo, proc, procst, is_inline, validation,
                                function_decl, language))
        elog(ERROR, "[pldotnet]: Could not obtain the source code");

    if (!pldotnet_CompileUserFunction(function_decl))
        elog(ERROR, "PL.NET function \"%s\".", function_decl->func_name);

    return true;
}

static bool pldotnet_GetSourceCode(
    FunctionCallInfo fcinfo, HeapTuple proc, Form_pg_proc procst,
    bool is_inline, bool validation,
    pldotnet_UserFunctionDeclaration *user_function_decl,
    pldotnet_Language language) {
    char *lang = language == csharp ? "csharp" : language == fsharp ? "fsharp" : "visualbasic";

    if (nullptr == user_function_decl)
        elog(ERROR, "[pldotnet]: Invalid argument: user_function_decl is null");

    if (is_inline) {
        user_function_decl->language = lang;
        user_function_decl->func_name = "plcsharp_inline_block";
        user_function_decl->func_ret_type = VOIDOID;
        user_function_decl->func_body =
            ((InlineCodeBlock *)DatumGetPointer(PG_GETARG_DATUM(0)))
                ->source_text;
    } else {
        user_function_decl->language = lang;
        user_function_decl->func_name = NameStr(procst->proname);
        user_function_decl->func_ret_type = procst->prorettype;
        user_function_decl->func_body = pldotnet_GetFunctionBody(proc, procst);
        user_function_decl->func_param_names =
            pldotnet_GetSqlParamsName(proc, procst);
        user_function_decl->func_param_types =
            pldotnet_GetSqlParamsType(proc, procst);
    }
    return true;
}

static void *pldotnet_GetDotNetMethod(const char *library_path,
                                      const char *dotnet_type,
                                      const char *dotnet_type_method,
                                      const char *delegate_type_name) {
    int rc;
    void *dotnet_method = nullptr;

    rc = assembly_loader(library_path, dotnet_type, dotnet_type_method,
                         delegate_type_name, nullptr, (void **)&dotnet_method);

    if (nullptr == dotnet_method)
        elog(ERROR, "[pldotnet]: Could not get_function_pointer(%s)",
             delegate_type_name);

    if (0 != rc)
        elog(ERROR,
             "[pldotnet]: Could not "
             "load_assembly_and_get_function_pointer(%s)",
             delegate_type_name);

    return dotnet_method;
}

static HeapTuple pldotnet_GetPostgresHeapTuple(Oid oid) {
    HeapTuple proc = SearchSysCache1(PROCOID, ObjectIdGetDatum(oid));
    if (!HeapTupleIsValid(proc))
        elog(ERROR, "[pldotnet]: Cache lookup failed for function %u", oid);
    return proc;
}

static inline void pldotnet_ReleasePostgresHeapTuple(HeapTuple proc) {
    ReleaseSysCache(proc);
}

static pldotnet_UserFunctionDeclaration *pldotnet_CreateFunctionDecl(void) {
    pldotnet_UserFunctionDeclaration *decl;
    MemoryContext mem = CurrentMemoryContext;

    /* change to top mem context */
    MemoryContextSwitchTo(TopMemoryContext);

    decl = (pldotnet_UserFunctionDeclaration *)palloc(
        sizeof(pldotnet_UserFunctionDeclaration));

    pldotnet_ResetFunctionDecl(decl);

    /* revert to previous mem context */
    MemoryContextSwitchTo(mem);

    return decl;
}

pldotnet_UserFunctionDeclaration *pldotnet_FindFunctionDecl(int function_id) {
    gpointer value =
        g_hash_table_lookup(procedures, GUINT_TO_POINTER(function_id));

    if (nullptr != value)
        return (pldotnet_UserFunctionDeclaration *)value;

    return nullptr;
}

static void pldotnet_ResetFunctionDecl(
    pldotnet_UserFunctionDeclaration *function_decl) {
    if (nullptr == function_decl)
        return;

    function_decl->language = nullptr;
    function_decl->func_name = nullptr;
    function_decl->func_ret_type = 0;
    function_decl->func_param_names = nullptr;
    function_decl->func_param_types = nullptr;
    function_decl->func_body = nullptr;
    function_decl->func_oid = 0;
    function_decl->support_null_input = true;
}

static void pldotnet_StartNewMemoryContext(MemoryContextWrapper *config) {
    config->prev = CurrentMemoryContext;
    config->curr = AllocSetContextCreate(
        TopMemoryContext, "PL/NET func_exec_ctx", ALLOCSET_SMALL_SIZES);

    if (nullptr == config->curr)
        elog(ERROR, "Could not create a new memory context");

    MemoryContextSwitchTo(config->curr);
}

static void pldotnet_ResetMemoryContext(MemoryContextWrapper *config) {
    if (nullptr == config)
        return;

    if (config->prev)
        MemoryContextSwitchTo(config->prev);

    if (config->curr)
        MemoryContextDelete(config->curr);
}

static bool pldotnet_CompileUserFunction(
    pldotnet_UserFunctionDeclaration *declaration) {
    int test = compile_user_function(
        (Oid)declaration->func_oid, (void *)declaration->func_name,
        (Oid)declaration->func_ret_type, (void *)declaration->func_param_names,
        (Oid *)declaration->func_param_types, (void *)declaration->func_body,
        declaration->support_null_input, (void *)declaration->language);
    return 0 == test;
}

static void pldotnet_SaveFunction(pldotnet_UserFunctionDeclaration *function,
                                  bool insert) {
    if (insert)
        g_hash_table_insert(procedures, GUINT_TO_POINTER(function->func_oid),
                            (gpointer)function);
    else
        g_hash_table_replace(procedures, GUINT_TO_POINTER(function->func_oid),
                             (gpointer)function);
}

static const char *pldotnet_GetFunctionBody(HeapTuple proc,
                                            Form_pg_proc procst) {
    bool isnull = false;
    Datum prosrc = SysCacheGetAttr(PROCOID, proc, Anum_pg_proc_prosrc, &isnull);
    if (isnull)
        elog(ERROR, "null prosrc");
    return TextDatumGetCString(prosrc);
}

static const char *pldotnet_GetSqlParamsName(HeapTuple proc,
                                             Form_pg_proc procst) {
    int nargs = 0;
    char *sql_params = nullptr;
    const char *space = " ";
    size_t buffer_size = 0;
    Oid *types;
    char **names, *modes;
    int total;

    if (!procst->pronargs)
        return nullptr;

    /* number of argument that are not OUT or TABLE */
    total = get_func_arg_info(proc, &types, &names, &modes);

    for (int i = 0; i < total; i++) {
        if (modes &&
            (modes[i] == PROARGMODE_OUT || modes[i] == PROARGMODE_TABLE))
            continue; /* skip OUT arguments */

        nargs++;
        buffer_size += strlen(names[i]);
    }

    sql_params = (char *)palloc0(buffer_size + strlen(space) * nargs);

    for (int i = 0, pos = 0; i < total; i++) {
        if (modes &&
            (modes[i] == PROARGMODE_OUT || modes[i] == PROARGMODE_TABLE))
            continue; /* skip OUT arguments */

        /* copy the arg name */
        strcat(sql_params, names[i]);
        if (pos < nargs - 1)
            strcat(sql_params, space);
        pos++;
    }

    return sql_params;
}

static const Oid *pldotnet_GetSqlParamsType(HeapTuple proc,
                                            Form_pg_proc procst) {
    int nargs = 0;
    Oid *sql_types = nullptr;
    Oid *types;
    char **names, *modes;
    int total;

    if (!procst->pronargs)
        return nullptr;

    /* number of argument that are not OUT or TABLE */
    total = get_func_arg_info(proc, &types, &names, &modes);

    if (modes == nullptr) {
        nargs = total;
    } else {
        for (int i = 0; i < total; i++) {
            if (modes[i] != PROARGMODE_OUT && modes[i] != PROARGMODE_TABLE) {
                nargs++;
            }
        }
    }

    sql_types = (Oid *)palloc0(sizeof(Oid) * nargs);

    for (int i = 0, pos = 0; i < total; i++) {
        if (modes &&
            (modes[i] == PROARGMODE_OUT || modes[i] == PROARGMODE_TABLE))
            continue; /* skip OUT arguments */

        sql_types[pos++] = types[i];
    }

    return sql_types;
}

static void *pldotnet_BuildArgumentList(FunctionCallInfo fcinfo,
                                        Form_pg_proc procst) {
    void *list = build_datum_list();
    for (int16_t i = 0; i < procst->pronargs; ++i) {
        Datum argdatum = pldotnet_GetArgDatum(fcinfo, i);
        add_datum_to_list(list, (void *)argdatum);
    }
    return list;
}

static inline Datum pldotnet_GetArgDatum(FunctionCallInfo fcinfo,
                                         size_t index) {
#if PG_VERSION_NUM >= 120000
    return fcinfo->args[index].value;
#else
    return fcinfo->arg[index];
#endif
}

static bool *pldotnet_BuildNullArgumentList(FunctionCallInfo fcinfo,
                                            Form_pg_proc procst) {
    int nargums = procst->pronargs;
    bool *isnull = (bool *)palloc(sizeof(bool) * nargums);

    for (int i = 0; i < nargums; ++i) {
        isnull[i] = pldotnet_CheckNullArgument(fcinfo, i);
    }
    return isnull;
}

static inline bool pldotnet_CheckNullArgument(FunctionCallInfo fcinfo,
                                              size_t index) {
#if PG_VERSION_NUM >= 120000
    return fcinfo->args[index].isnull;
#else
    return fcinfo->argnull[index];
#endif
}

/*
 * END: implementing functions
 */
