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
 * pldotnet_main.h
 *
 */

#ifndef PLDOTNET_MAIN_H_
#define PLDOTNET_MAIN_H_

#include <postgres.h>
#include <access/htup_details.h>
#include <catalog/pg_proc.h>
#include <catalog/pg_type.h>
#include <funcapi.h>
#include <glib.h>
#include <utils/builtins.h>
#include <utils/fmgrprotos.h>
#include <utils/syscache.h>
#include <utils/memutils.h>

#include "pldotnet_hostfxr.h"

#define QUOTE(name) #name
#define STR(macro) QUOTE(macro)
#define nullptr ((void *)0)

extern PGDLLIMPORT bool check_function_bodies;

extern GHashTable *procedures;

extern char *root_path;
extern char *dnldir;

/**
 * @brief Writes formatted output to a string using snprintf and checks if the
 * output exceeds the size of the buffer.
 *
 * If the output exceeds the size of the buffer, this macro calls elog with an
 * error message indicating that the string was too long for the buffer.
 *
 * @param dst Pointer to the destination buffer where the formatted output will
 * be written.
 * @param size Size of the destination buffer.
 * @param fmt Format string that specifies how the formatted output should be
 * written.
 * @param ... List of arguments for the format specifiers in the format string.
 */
#define SNPRINTF(dst, size, fmt, ...)                                      \
    if (snprintf(dst, size, fmt, __VA_ARGS__) >= size) {                   \
        elog(ERROR, "[pldotnet] (%s:%d) String too long for buffer: " fmt, \
             __FILE__, __LINE__, __VA_ARGS__);                             \
    }

typedef enum pldotnet_Language { csharp, fsharp } pldotnet_Language;

typedef struct MemoryContextWrapper {
    MemoryContext prev;
    MemoryContext curr;
} MemoryContextWrapper;

typedef struct pldotnet_PathConfig {
    char prefix[MAXPGPATH];
    char config_path[MAXPGPATH];
    char library_path[MAXPGPATH];
} pldotnet_PathConfig;

typedef struct pldotnet_Result {
    Datum value;
    bool is_null;
} pldotnet_Result;

typedef struct pldotnet_UserFunctionDeclaration {
    const char *language;
    const char *func_name;
    Oid func_ret_type;
    const char *func_param_names;
    const Oid *func_param_types;
    const char *func_body;
    Oid func_oid;
    bool support_null_input;
} pldotnet_UserFunctionDeclaration;

/**
 * @brief The call_handler will be called to execute the procedural
 * language's functions.  The call handler receives a pointer to a
 * FunctionCallInfoData struct containing argument values and information
 * about the called function, and it is expected to return a Datum result.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for fmgr-compatible
 * functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
Datum plcsharp_call_handler(PG_FUNCTION_ARGS);

/**
 * @brief The inline_handler will be called to execute an anonymous code
 * block (DO command) in this language.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for
 * fmgr-compatible functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
Datum plcsharp_inline_handler(PG_FUNCTION_ARGS);

/**
 * @brief The validator function will inspect the function body for syntactical
 * correctness, but it can also look at other properties of the function,
 * for example if the language cannot handle certain argument types. To
 * signal an error, the validator function should use the ereport()
 * function. The return value of the function is ignored.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for fmgr-compatible
 * functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
Datum plcsharp_validator(PG_FUNCTION_ARGS);

/**
 * @brief The call_handler will be called to execute the procedural
 * language's functions. The call handler receives a pointer to a
 * FunctionCallInfoData struct containing argument values and information
 * about the called function, and it is expected to return a Datum result.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for fmgr-compatible
 * functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
Datum plfsharp_call_handler(PG_FUNCTION_ARGS);

/**
 * @brief The inline_handler will be called to execute an anonymous code
 * block (DO command) in this language.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for
 * fmgr-compatible functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
Datum plfsharp_inline_handler(PG_FUNCTION_ARGS);

/**
 * @brief The validator function will inspect the function body for syntactical
 * correctness, but it can also look at other properties of the function,
 * for example if the language cannot handle certain argument types. To
 * signal an error, the validator function should use the ereport()
 * function. The return value of the function is ignored.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for fmgr-compatible
 * functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
Datum plfsharp_validator(PG_FUNCTION_ARGS);

/**
 * @brief Build the config paths related to .NET.
 *
 */
bool pldotnet_BuildPaths(void);

/**
 * @brief Sets the assembly_loader object.
 *
 * @return true if the assembly_loader was already defined or was found
 * correctly.
 * @return false if the assembly_loader was not found.
 */
bool pldotnet_SetNetLoader(void);

/**
 * @brief Sets the .NET methods for the C function pointers.
 *
 * @return true if all the .NET functions were found.
 * @return false if no .NET functions were found.
 */
bool pldotnet_SetDotNetMethods(void);

/**
 * @brief Calls the "elog" function to report a message of PostgreSQL.
 *
 * @param level The message level. For example, INFO, ERROR, WARNING, etc...
 * @param message The message that will be reported.
 */
extern void pldotnet_Elog(int level, char *message);

#endif  // PLDOTNET_MAIN_H_
