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
 * pldotnet_hostfxr.c
 *
 */

#include "pldotnet_hostfxr.h"

#include <assert.h>
#include <dlfcn.h>
#include <limits.h>
#include <postgres.h>
#include <stdio.h>

#define nullptr ((void *)0)
#define MAX_PATH PATH_MAX

/*
 * START: declaring functions
 */

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
extern int pldotnet_LoadHostfxr(void);

/*
 * END: declaring functions
 */

/*
 * START: declaring variables
 */

static hostfxr_initialize_for_runtime_config_fn init_fptr;
static hostfxr_set_runtime_property_value_fn set_runtime_properties_ptr;
static hostfxr_get_runtime_delegate_fn get_delegate_fptr;
static hostfxr_close_fn close_fptr;

/*
 * END: declaring variables
 */

/*
 * START: implementing functions
 */

static void *pldotnet_dlopen(const char *path) {
    /* Allows us to add windows support later */
    return dlopen(path, RTLD_LAZY | RTLD_LOCAL);
}

static void *pldotnet_dlsym(void *handle, const char *symbol) {
    /* Allows us to add windows support later */
    return dlsym(handle, symbol);
}

// Returns 0 on success, (other) on error
static int pldotnet_GetHostFxrPath(char_t *buffer, size_t bufferSize) {
    FILE *out = popen(
        "dpkg -L dotnet-hostfxr-6.0 | grep libhostfxr.so | head -1 | xargs "
        "dirname",
        "r");
    const char *aux = "/libhostfxr.so";
    size_t currentLength;
    int written;

    if (NULL == out) { return -11; }

    while (fgets(buffer, bufferSize, out) != NULL) puts(buffer);
    buffer[strlen(buffer) - 1] = '\0';  // Removing the newline character
    currentLength = strlen(buffer);

    // Using snprintf to append the `aux` string
    written = snprintf(buffer + currentLength,
                       bufferSize - currentLength,
                       "%s",
                       aux);
    pclose(out);
    if ((written < 0) || (written >= bufferSize - currentLength)) {
        return -2;  // Indicate an snprintf error
    }
    return 0;
}


int pldotnet_LoadHostfxr(void) {
    void *lib;
    /* Pre-allocate a large buffer for the path to hostfxr */
    char_t hostfxr_path[MAX_PATH];
    size_t buffer_size = sizeof(hostfxr_path) / sizeof(char_t);

    if (get_hostfxr_path(hostfxr_path, &buffer_size, nullptr) != 0)
        if (pldotnet_GetHostFxrPath(hostfxr_path, buffer_size) != 0)
            return 0;

    /* Load hostfxr and get desired exports */
    lib = pldotnet_dlopen(hostfxr_path);
    init_fptr = (hostfxr_initialize_for_runtime_config_fn)pldotnet_dlsym(
        lib, "hostfxr_initialize_for_runtime_config");
    get_delegate_fptr = (hostfxr_get_runtime_delegate_fn)pldotnet_dlsym(
        lib, "hostfxr_get_runtime_delegate");
    set_runtime_properties_ptr =
        (hostfxr_set_runtime_property_value_fn)pldotnet_dlsym(
            lib, "hostfxr_set_runtime_property_value");
    close_fptr = (hostfxr_close_fn)pldotnet_dlsym(lib, "hostfxr_close");

    return (init_fptr && get_delegate_fptr && set_runtime_properties_ptr &&
            close_fptr);
}

load_assembly_and_get_function_pointer_fn GetNetLoadAssemblySetup(
    const char_t *config_path, const char_t *host_base_path) {
    int rc;
    static void *load_assembly_and_get_function_pointer = nullptr;
    hostfxr_handle cxt = nullptr;

    if (load_assembly_and_get_function_pointer != nullptr)
        return load_assembly_and_get_function_pointer;

    rc = init_fptr(config_path, nullptr, &cxt);

    if (rc > 1 || rc < 0 || cxt == nullptr) {
        fprintf(stderr, "Init failed: %x\n", rc);
        close_fptr(cxt);
        return nullptr;
    }

    if (nullptr != host_base_path)
        set_runtime_properties_ptr(cxt, "APP_CONTEXT_BASE_DIRECTORY",
                                   (char *)host_base_path);

    /* Get the load assembly function pointer */
    rc = get_delegate_fptr(cxt, hdt_load_assembly_and_get_function_pointer,
                           &load_assembly_and_get_function_pointer);
    close_fptr(cxt);

    if (rc != 0 || load_assembly_and_get_function_pointer == nullptr)
        elog(ERROR, "Get delegate failed: %x\n", rc);

    return (load_assembly_and_get_function_pointer_fn)
        load_assembly_and_get_function_pointer;
}

bool pldotnet_LoadHostFxrIfNeeded(void) {
    static bool hostfxr_loaded = false;

    if (!hostfxr_loaded) {
        hostfxr_loaded = pldotnet_LoadHostfxr();
    }

    return hostfxr_loaded;
}

/*
 * END: implementing functions
 */
