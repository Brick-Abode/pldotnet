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
 * pldotnet_hostfxr.h
 *
 */

#ifndef PLDOTNET_HOSTFXR_H_
#define PLDOTNET_HOSTFXR_H_

#include <coreclr_delegates.h>
#include <nethost.h>
#include <stdbool.h>
#include <stddef.h>
#include <stdint.h>

#if defined(_WIN32)
#define CORECLR_DELEGATE_CALLTYPE __stdcall
#define HOSTFXR_CALLTYPE __cdecl
#ifdef _WCHAR_T_DEFINED
typedef wchar_t char_t;
#else
typedef unsigned short char_t;
#endif
#else
#define CORECLR_DELEGATE_CALLTYPE
#define HOSTFXR_CALLTYPE
typedef char char_t;
#endif

enum hostfxr_delegate_type {
    hdt_com_activation,
    hdt_load_in_memory_assembly,
    hdt_winrt_activation,
    hdt_com_register,
    hdt_com_unregister,
    hdt_load_assembly_and_get_function_pointer
};

typedef void *hostfxr_handle;

typedef struct hostfxr_initialize_parameters {
    size_t size;
    const char_t *host_path;
    const char_t *dotnet_root;
} hostfxr_initialize_parameters;

typedef int32_t(HOSTFXR_CALLTYPE *hostfxr_initialize_for_runtime_config_fn)(
    const char_t *runtime_config_path,
    const hostfxr_initialize_parameters *parameters,
    hostfxr_handle *host_context_handle);

typedef int32_t(HOSTFXR_CALLTYPE *hostfxr_set_runtime_property_value_fn)(
    const hostfxr_handle host_context_handle, const char_t *name,
    const char_t *value);

typedef int32_t(HOSTFXR_CALLTYPE *hostfxr_get_runtime_delegate_fn)(
    const hostfxr_handle host_context_handle, enum hostfxr_delegate_type type,
    void **delegate);

typedef int32_t(HOSTFXR_CALLTYPE *hostfxr_close_fn)(
    const hostfxr_handle host_context_handle);

typedef load_assembly_and_get_function_pointer_fn dotnet_loader;

/**
 * @brief A function pointer that references Engine.CompileUserFunction().
 *
 */
typedef int(CORECLR_DELEGATE_CALLTYPE *compile_user_fn)(
    uint32_t functionId, char *func_name, uint32_t func_ret_type, bool retset,
    bool is_trigger, char *func_param_names, uint32_t *func_param_types,
    char *param_modes, int num_output_values, char *func_body,
    bool support_null_input, char *dotnet_language);

/**
 * @brief A function pointer that references Engine.RunUserFunction().
 *
 */
typedef int(CORECLR_DELEGATE_CALLTYPE *run_user_fn)(
    uint32_t functionId, uint64_t call_id, int call_mode, void *arguments,
    int num_arguments, bool *nullmap, void *output);

/**
 * @brief A function pointer that references Engine.RunUserTFunction().
 *
 */
typedef int(CORECLR_DELEGATE_CALLTYPE *run_user_tg_fn)(
    uint32_t functionId, int call_mode, void *old_row, void *new_row,
    char *triggerName, char *triggerWhen, char *triggerLevel,
    char *triggerOperation, int relationId, char *tableName, char *tableSchema,
    char **arguments, int nargs);

/**
 * @brief A function pointer that references Engine.BuildDatumList().
 *
 */
typedef void *(CORECLR_DELEGATE_CALLTYPE *build_datum_list_fn)(void);

/**
 * @brief A function pointer that references Engine.AddDatumToList().
 *
 */
typedef void(CORECLR_DELEGATE_CALLTYPE *add_datum_to_list_fn)(void *list,
                                                              void *datum);

/**
 * @brief A function pointer that references Engine.FreeGenericGCHandle().
 *
 */
typedef void(CORECLR_DELEGATE_CALLTYPE *free_generic_gchandle_fn)(
    void *gchandle);

/**
 * @brief A function pointer that references Engine.UnloadAssemblies().
 *
 */
typedef void(CORECLR_DELEGATE_CALLTYPE *unload_assemblies_fn)(
    uint32_t functionId);

/** @brief Loads dotnet using the HostFXR.  HostFXR "finds and resolves
 * the runtime and all the frameworks the app needs", which in our
 * case is via `nethost`, "which is used by native apps (any app
 * which is not .NET Core) to load .NET Core code dynamically"
 *
 * See these URLs for more background:
 * https://github.com/dotnet/runtime/blob/main/docs/design/features/host-components.md
 * https://github.com/dotnet/runtime/blob/main/docs/design/features/sharedfx-lookup.md
 *
 * @return 1 on success, 0 on failure
 */
int pldotnet_LoadHostfxr(void);

/**
 * @brief Load and initialize .NET Core and get desired function pointer for
 * scenario.
 *
 * @param config_path the config path.
 * @param host_base_path the host base path.
 *
 * @return pointer to .NET's function to load an assembly and get the function
 * pointer, or NULL on error.
 */
load_assembly_and_get_function_pointer_fn GetNetLoadAssemblySetup(
    const char_t *config_path, const char_t *host_base_path);

/**
 * @brief Using the nethost library, this function discovers the location of
 * hostfxr and get exports IF the hostfxr was not loaded yet.
 *
 * @return Returns whether the hostfxr was loaded.
 */
bool pldotnet_LoadHostFxrIfNeeded(void);

#endif  // PLDOTNET_HOSTFXR_H_
