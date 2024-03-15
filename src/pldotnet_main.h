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
#include <commands/trigger.h>
#include <executor/spi.h>  // For SPI_getrelname and SPI_getnspname
#include <funcapi.h>
#include <glib.h>
#include <utils/array.h>
#include <utils/builtins.h>
#include <utils/fmgrprotos.h>
#include <utils/syscache.h>
#include <utils/typcache.h>
#include <utils/memutils.h>
#include <utils/lsyscache.h>  // type_is_rowtype()
#include <utils/rel.h>
#include <assert.h>

#include "pldotnet_hostfxr.h"
#include "pldotnet_spi.h"

#define QUOTE(name) #name
#define STR(macro) QUOTE(macro)

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

typedef enum {
    CALL_NORMAL = 1,       // Normal, non-SRF function
    CALL_SRF_FIRST = 2,    // First call to an SRF; create and cache
    CALL_SRF_NEXT = 3,     // Next call to an SRF
    CALL_SRF_CLEANUP = 4,  // SRF is done; you may remove it from the cache
    CALL_TRIGGER = 5       // Called as a trigger
} CallType;

typedef enum {
    RETURN_ERROR = 0,           // We encountered an error
    RETURN_NORMAL = 1,          // Normal return to CALL_NORMAL
    RETURN_SRF_NEXT = 2,        // SRF return to CALL_SRF_NEXT
    RETURN_SRF_DONE = 3,        // We have no more values
    RETURN_TRIGGER_SKIP = 4,    // Abort the trigger event
    RETURN_TRIGGER_MODIFY = 5,  // The row has been modified
} ReturnMode;

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
    size_t length;
    bool is_null;  // for top-level NULL on RECORD
    Datum *values;
    bool *nulls;
    Oid *oids;
    bool *updated;
} pldotnet_Result;

typedef struct pldotnet_UserFunctionDeclaration {
    const char *language;
    const char *func_name;
    Oid func_ret_type;
    char *func_param_names;
    Oid *func_param_types;
    char *func_param_modes;
    int num_args;
    int num_input_args;
    int num_output_values;  // 0 for a normal function (1 return), <n> for a
                            // record (INOUT/OUT)
    const char *func_body;
    Oid func_oid;
    bool support_null_input;
    bool retset;
    bool is_trigger;
} pldotnet_UserFunctionDeclaration;

typedef struct cb_data {  // old-school C inheritance here
    MemoryContextCallback cb_record;
    uint32_t functionId;
    uint64_t call_id;
} cb_data;

/**
 * Set a result in pldotnet_Result.  Returns 0 on success.
 */
extern PGDLLEXPORT int pldotnet_SetResult(pldotnet_Result *output, int offset,
                                          Datum value, bool is_null, Oid oid);

/**
 * Get a result from pldotnet_Result.
 */
extern PGDLLEXPORT int pldotnet_GetResult(pldotnet_Result *output, int offset,
                                          Datum *value, bool *is_null,
                                          Oid *oid);

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
extern Datum plcsharp_call_handler(PG_FUNCTION_ARGS);

/**
 * @brief The inline_handler will be called to execute an anonymous code
 * block (DO command) in this language.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for
 * fmgr-compatible functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
extern Datum plcsharp_inline_handler(PG_FUNCTION_ARGS);

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
extern Datum plcsharp_validator(PG_FUNCTION_ARGS);

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
extern Datum plfsharp_call_handler(PG_FUNCTION_ARGS);

/**
 * @brief The inline_handler will be called to execute an anonymous code
 * block (DO command) in this language.
 *
 * @param PG_FUNCTION_ARGS The standard parameter list for
 * fmgr-compatible functions.
 *
 * @return The datum that can be stored in a PostgreSQL table.
 */
extern Datum plfsharp_inline_handler(PG_FUNCTION_ARGS);

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
extern Datum plfsharp_validator(PG_FUNCTION_ARGS);

/**
 * @brief Build the config paths related to .NET.
 *
 */
extern bool pldotnet_BuildPaths(void);

/**
 * @brief Sets the assembly_loader object.
 *
 * @return true if the assembly_loader was already defined or was found
 * correctly.
 * @return false if the assembly_loader was not found.
 */
extern bool pldotnet_SetNetLoader(void);

/**
 * @brief Sets the .NET methods for the C function pointers.
 *
 * @return true if all the .NET functions were found.
 * @return false if no .NET functions were found.
 */
extern bool pldotnet_SetDotNetMethods(void);

/**
 * @brief Calls the "elog" function to report a message of PostgreSQL.
 *
 * @param level The message level. For example, INFO, ERROR, WARNING, etc...
 * @param message The message that will be reported.
 */
extern PGDLLEXPORT void pldotnet_Elog(int level, char *message);

/**
 * @brief Returns the PostgreSQL version.
 *
 * @return char* the PostgreSQL version like "14.7"
 */
extern PGDLLEXPORT char *pldotnet_GetPostgreSqlVersion(void);

/**
 * @brief Creates a new memory context.
 *
 * @param config
 */
void pldotnet_StartNewMemoryContext(MemoryContextWrapper *config);

/**
 * @brief Reverts the created memory context.
 *
 * @param config
 */
void pldotnet_ResetMemoryContext(MemoryContextWrapper *config);

/**
 * @brief Returns current size of the pldotnet_Result
 *
 * @param result Pointer to the pldotnet_Result structure to be checked.
 * @return Number of fields in the structure
 */
extern PGDLLEXPORT int pldotnet_GetResultLength(pldotnet_Result *result);

/**
 * @brief Resizes the result structure.
 *
 * This function is used to resize the result structure to accommodate a
 * specified length.
 *
 * @param r The result structure to be resized.
 * @param length The new length of the result structure.
 */
extern PGDLLEXPORT void pldotnet_ResizeResult(struct pldotnet_Result *r,
                                              size_t length);

#endif  // PLDOTNET_MAIN_H_
