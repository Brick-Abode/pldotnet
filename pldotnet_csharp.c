/* 
 * PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as 
 *                      procedural languages (PL)
 * 
 * 
 * Copyright 2019-2020 Brick Abode
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * pldotnet_csharp.c - Postgres PL handlers for C# and functions
 *
 */
#include "pldotnet_csharp.h"
#include "pldotnet_hostfxr.h" /* needed for pldotnet_LoadHostfxr() */
#include "pldotnet_composites.h"
#include <math.h>

static pldotnet_FuncInOutInfo func_inout_info;

/* Declare extension variables/structs here */
PGDLLEXPORT Datum plcsharp_call_handler(PG_FUNCTION_ARGS);
PGDLLEXPORT Datum plcsharp_validator(PG_FUNCTION_ARGS);
#if PG_VERSION_NUM >= 90000
PGDLLEXPORT Datum plcsharp_inline_handler(PG_FUNCTION_ARGS);
#endif

static int plcsharp_BuildBlockComposites(char * composites_decl,
                                         FunctionCallInfo fcinfo,
                                         Form_pg_proc procst);
static char  *plcsharp_BuildBlockArgsDecl( FunctionCallInfo fcinfo,
                                    Form_pg_proc procst );
static char  *plcsharp_BuildBlockCallFuncCall(Form_pg_proc procst);
static char  *plcsharp_BuildBlockUserFuncDecl(Form_pg_proc procst,
                                              HeapTuple proc);
static int   GetSizeNullableHeader(int argnm_size, Oid arg_type, int narg);
static int   GetSizeNullableFooter(Oid ret_type);
static bool  IsNullable(Oid type);
static char  *pldotnet_CreateCStructLibargs(FunctionCallInfo fcinfo,
                                                           Form_pg_proc procst);
static Datum pldotnet_GetNetResult(char * libargs, Oid rettype,
                                                       FunctionCallInfo fcinfo);
static int   GetSizeArgsNullArray(int nargs);
static int   pldotnet_PublicDeclSize(Oid type);
static const char * pldotnet_GetNullableTypeName(Oid id);
static inline void plcsharp_BuildPaths(void);
bool pldotnet_CheckArgIsArray(Datum datum, Oid oid, int narg);

load_assembly_and_get_function_pointer_fn
                                         load_assembly_and_get_function_pointer;

char csharp_config_path[MAXPGPATH];
char csharp_lib_path[MAXPGPATH];
char csharp_srclib_path[MAXPGPATH];
bool hostfxr_loaded = false;
bool paths_defined = false;

#if PG_VERSION_NUM >= 90000
#define CODEBLOCK \
  ((InlineCodeBlock *) DatumGetPointer(PG_GETARG_DATUM(0)))->source_text

const char public_bool[] = "\n[MarshalAs(UnmanagedType.U1)]public ";
const char public_string_utf8[] = "\n[MarshalAs(UnmanagedType.LPUTF8Str)]public ";
const char public_struct[] = "\n[MarshalAs(UnmanagedType.Struct)]public ";
const char public_[] = "\npublic ";
/* nullable related constants */
const char resu_nullable_value[] = "libargs.resu = resu_nullable.GetValueOrDefault();\n";
const char resu_nullable_flag[] = "libargs.resunull = !resu_nullable.HasValue;\n";
const char argsnull_str[] = "libargs.argsnull";
const char nullable_suffix[] = "_nullable";
const char resu_flag_str[] = "bool resunull;";
const char arg_flag_str[] = "bool[] argsnull;";

/* C# CODE TEMPLATE */
static char cs_block_header[] = "            \n\
using System;                               \n\
using System.Dynamic;                       \n\
using System.Collections.Generic;           \n\
using System.Runtime.InteropServices;       \n\
namespace PlDotNETUserSpace                 \n\
{                                           \n\
    enum TypeOid                            \n\
    {                                       \n\
       BOOLOID    = 16,                     \n\
       INT8OID    = 20,                     \n\
       INT2OID    = 21,                     \n\
       INT4OID    = 23,                     \n\
       FLOAT4OID  = 700,                    \n\
       FLOAT8OID  = 701,                    \n\
       VARCHAROID = 1043,                   \n\
       NUMERICOID = 1700,                   \n\
    }                                       \n\
    public static class UserClass           \n\
    {                                       \n";
/********** cs_block_composites *********
 *   [StructLayout(LayoutKind.Sequential,Pack=1)]
 *   public struct CompositeName1;
 *   {
 *       public fielType1 fieldName1;
 *       public fielType2 fieldName2;
 *       ...
 *       public fielTypeN fieldNameN;
 *   }
 *
 *   [StructLayout(LayoutKind.Sequential,Pack=1)]
 *   public struct CompositeNameN;
 *    { ... }
 *
 */
static char cs_block_args_header[] = "\
        [StructLayout(LayoutKind.Sequential,Pack=1)]\n\
        public struct LibArgs               \n\
        {";
/*********** cs_block_args_decl **********
 *          public argType1 argname1;
 *          public argType2 argname2;
 *           ...
 *	        public returnT resu;
 */
static char cs_block_callfunc_header[] = "             \n\
        }                                    \n\
        public static int CallFunction(IntPtr arg, int argLength)\n\
        {                                    \n\
            if (argLength != System.Runtime.InteropServices.Marshal.SizeOf(typeof(LibArgs)))\n\
                return 1;                    \n\
            LibArgs libargs = Marshal.PtrToStructure<LibArgs>(arg);\n";
/*********** cs_block_callfunc_call **********
 *          libargs.resu = FUNC(libargs.argname1, libargs.argname2, ...);
 */

/*********** cs_block_userfunc_decl **********
 *          returnT FUNC(argType1 argname1, argType2 argname2, ...)
 *          {
 *               What is in the SQL function code here
 *          }
 */
static char cs_block_footer[] = "              \n\
            decimal[] ArrayToDecimal(string[]str) {\n\
                int i=0;\n\
                decimal[] decarr = new decimal[str.Length];\n\
                foreach(string s in str)\n\
                    decarr[i++] = Convert.ToDecimal(s);\n\
                return decarr;\n\
            }\n\
            Marshal.StructureToPtr<LibArgs>(libargs, arg, false);\n\
            return 0;                         \n\
        }                                     \n\
    }                                         \n\
}";


static char block_inline_header[] = "             \n\
using System;                               \n\
using System.Runtime.InteropServices;       \n\
namespace PlDotNETUserSpace                 \n\
{                                           \n\
    public static class UserClass           \n\
    {";
static char block_inline_callfunc[] = "             \n\
        public static int CallFunction(IntPtr arg, int argLength)\n\
        {";                                   
/* block_inline_usercode  Function body */
static char block_inline_footer[] = "             \n\
	    return 0;                           \n\
	}                                       \n\
     }                                      \n\
}";

static int
pldotnet_PublicDeclSize(Oid type)
{
    Oid id;
    Form_pg_type typeinfo;
    HeapTuple typ;

    switch (type)
    {
        case INT4OID:
        case INT8OID:
        case INT2OID:
        case FLOAT4OID:
        case FLOAT8OID:
        case NUMERICOID:
            return strlen(public_);
        case BOOLOID:
            return strlen(public_bool);
        case BPCHAROID:
        case VARCHAROID:
        case TEXTOID:
            return strlen(public_string_utf8);
        default:
            typ = SearchSysCache(TYPEOID,
                                  ObjectIdGetDatum(type), 0, 0, 0);
            if (!HeapTupleIsValid(typ))
            {
                elog(ERROR, "[pldotnet]: cache lookup failed for type %u",

        type);
            }
            typeinfo = (Form_pg_type) GETSTRUCT(typ);
            id = typeinfo->typtype;
            ReleaseSysCache(typ);
            if (id == TYPTYPE_COMPOSITE)
                return strlen(public_struct);
    }
    return  0;
}

char *
pldotnet_PublicDecl(Oid type)
{
    Oid id;
    Form_pg_type typeinfo;
    HeapTuple typ;

    switch (type)
    {
        case INT4OID:
        case INT8OID:
        case INT2OID:
        case FLOAT4OID:
        case FLOAT8OID:
        case NUMERICOID:
            return (char *)&public_;
        case BOOLOID:
            return (char *)&public_bool;
        case BPCHAROID:
        case VARCHAROID:
        case TEXTOID:
            return (char *)&public_string_utf8;
        default:
            typ = SearchSysCache(TYPEOID,
                                  ObjectIdGetDatum(type), 0, 0, 0);
            if (!HeapTupleIsValid(typ))
            {
                elog(ERROR, "[pldotnet]: cache lookup failed for type %u",

        type);
            }
            typeinfo = (Form_pg_type) GETSTRUCT(typ);
            id = typeinfo->typtype;
            ReleaseSysCache(typ);
            if (id == TYPTYPE_COMPOSITE)
                return (char *)&public_struct;
    }
    return  0;
}

static int
plcsharp_BuildBlockComposites(char * composite_decls, FunctionCallInfo fcinfo,
                                                           Form_pg_proc procst)
{
    int cursize=0, i;
    Oid *argtype = procst->proargtypes.values;
    Form_pg_type typeinfo;
    HeapTuple type;
    TupleDesc tupdesc;

    for (i = 0;i < procst->pronargs; i++)
    {

        /* TODO: review this */
        if ( pldotnet_IsSimpleType(argtype[i]) ||
             pldotnet_IsTextType(argtype[i]) )
            continue;

        type = SearchSysCache(TYPEOID, ObjectIdGetDatum(argtype[i]), 0, 0, 0);
        if (!HeapTupleIsValid(type))
        {
            elog(ERROR, "[pldotnet]: cache lookup failed for type %u",
                                                                    argtype[i]);
        }
        typeinfo = (Form_pg_type) GETSTRUCT(type);
        if (typeinfo->typtype == TYPTYPE_COMPOSITE)
        {
            tupdesc = lookup_rowtype_tupdesc(argtype[i], typeinfo->typtypmod);
            pldotnet_GetStructFromCompositeTuple( composite_decls + cursize,
                           1024 - cursize, fcinfo->arg[i], typeinfo, tupdesc);

            ReleaseTupleDesc(tupdesc);
            cursize += strlen(composite_decls);
        }
        ReleaseSysCache(type);
    }
    return 0;
}


static char *
plcsharp_BuildBlockArgsDecl(FunctionCallInfo fcinfo, Form_pg_proc procst)
{
    char *block2str, *str_ptr;
    Oid *argtype = procst->proargtypes.values; /* Indicates the args type */
    Oid rettype = procst->prorettype; /* Indicates the return type */
    int nargs = procst->pronargs;
    const char semicon[] = ";";
    char argname[] = " argN";
    char result[] = " resu"; /*  have to be same size argN */
    int i, cursize = 0, totalsize = 0;
    /* nullable related */
    bool nullable_arg_flag = false;
    int null_flags_size = 0, return_null_flag_size = 0;
    bool isarr = false;
    Oid type;

    if (!pldotnet_TypeSupported(rettype))
    {
        elog(ERROR, "[pldotnet]: unsupported type on return");
        return 0;
    }

    for (i = 0; i < nargs; i++)
    {
        isarr = pldotnet_CheckArgIsArray(fcinfo->arg[i], argtype[i], i);
        type = isarr ? func_inout_info.arrayinfo[i].typelem : argtype[i];

        if (!pldotnet_TypeSupported(type))
        {
            elog(ERROR, "[pldotnet]: unsupported type on arg %d", i);
            return 0;
        }

        if (isarr)
            totalsize += strlen(func_inout_info.arrayinfo[i].csharpdecl);
        else
        {
            totalsize += pldotnet_PublicDeclSize(type) +
                         strlen(pldotnet_GetNetTypeName(type, true)) +
                         strlen(argname) + strlen(semicon);
        }

        if (IsNullable(argtype[i]))
            nullable_arg_flag = true;
    }
    
    return_null_flag_size = strlen(public_bool) + strlen(resu_flag_str);

    if (nullable_arg_flag)
        null_flags_size = GetSizeArgsNullArray(nargs);

    totalsize += pldotnet_PublicDeclSize(rettype) +
                 strlen(pldotnet_GetNetTypeName(rettype, true)) +
                 null_flags_size + return_null_flag_size +
                 strlen(result) + strlen(semicon) + 1;

    block2str = (char *) palloc0(totalsize);

    if (nullable_arg_flag)
    {
        SNPRINTF((char *)block2str, totalsize
            , "\n[MarshalAs(UnmanagedType.ByValArray,ArraySubType=UnmanagedType.U1,SizeConst=%d)]public %s"
            , nargs, arg_flag_str);
        cursize = strlen(block2str);
    }

    str_ptr = (char *)(block2str + cursize);
    SNPRINTF(str_ptr,totalsize - cursize,"%s%s",public_bool,resu_flag_str);
    cursize += strlen(str_ptr);

    for (i = 0; i < nargs; i++)
    {
        str_ptr = (char *)(block2str + cursize);
        if (pldotnet_IsArray(i, &func_inout_info))
        {
            SNPRINTF(str_ptr, totalsize - cursize,"%s",
                     func_inout_info.arrayinfo[i].csharpdecl);
        }
        else
        {
            /* review nargs > 9 */
            SNPRINTF(argname, strlen(argname)+1, " arg%d", i);
            SNPRINTF(str_ptr,totalsize - cursize,"%s%s%s%s"
                        , pldotnet_PublicDecl(argtype[i])
                        , pldotnet_GetNetTypeName(argtype[i], true)
                        , argname, semicon);
        }
        cursize += strlen(str_ptr);
    }

    /* result */
    str_ptr = (char *)(block2str + cursize);

    SNPRINTF(str_ptr,totalsize - cursize,"%s%s%s%s"
                ,pldotnet_PublicDecl(rettype)
                ,pldotnet_GetNetTypeName(rettype, true), result, semicon);

    return block2str;
}

static char *
plcsharp_BuildBlockCallFuncCall(Form_pg_proc procst)
{
    char *block2str, *str_ptr, *resu_var;
    int cursize = 0, i, totalsize;
    const char beginFun[] = "(";
    char * func;
    const char libargs[] = "libargs.";
    const char strconvert[] = ".ToString()"; /* Converts func return */
    const char todecimal[] = "Convert.ToDecimal(";
    const char arr_todecimal[] = "ArrayToDecimal(";
    const char result[] = "            libargs.resu=";
    const char nullable_result[] = "            resu_nullable=";
    const char comma[] = ",";
    char argname[] = "argN";
    const char end_fun[] = ")";
    const char semicolon[] = ";";
    int nargs = procst->pronargs;
    Oid *argtype = procst->proargtypes.values; /* Indicates the args type */
    Oid rettype = procst->prorettype; /* Indicates the return type */

    /* Function name */
    func = NameStr(procst->proname);

    if (IsNullable(rettype))
    {
        int resu_var_size = strlen(pldotnet_GetNullableTypeName(rettype)) 
                + strlen(nullable_result) + 1;
        resu_var = (char *)palloc0(resu_var_size);
        SNPRINTF(resu_var, resu_var_size, "%s%s"
                   , pldotnet_GetNullableTypeName(rettype)
                   , nullable_result);
    } 
    else
        resu_var = (char *)result;

    /* TODO:  review for nargs > 9 */
    if (nargs == 0)
    {
         int block_size;

         if (rettype == NUMERICOID)
         {
            block_size = strlen(resu_var) + strlen(func) + strlen(beginFun)
                                 + strlen(end_fun) + strlen(strconvert)
                                 + strlen(semicolon) + 1;
            block2str = (char *)palloc0(block_size);
            SNPRINTF(block2str,block_size,"%s%s%s%s%s%s"
                       , resu_var, func, beginFun
                       , end_fun, strconvert, semicolon);
         }
         else
         {
            block_size = strlen(resu_var) + strlen(func) + strlen(beginFun)
                                 + strlen(end_fun) + strlen(semicolon) + 1;
            block2str = (char *)palloc0(block_size);
            SNPRINTF(block2str,block_size,"%s%s%s%s%s"
                        ,resu_var, func, beginFun, end_fun, semicolon);
         }
         return block2str;
    }

    totalsize = strlen(resu_var) + strlen(func) + strlen(beginFun) +
                    (strlen(libargs) + strlen(argname)) * nargs
                     + strlen(end_fun) + strlen(semicolon) + 1;

    for (i = 0; i < nargs; i++) /* Get number of Numeric argr */
    {
        if (argtype[i] == NUMERICOID)
            totalsize += strlen(todecimal) + strlen(end_fun);
        else if (pldotnet_IsArray(i, &func_inout_info) &&
                 func_inout_info.arrayinfo[i].typelem == NUMERICOID)
            totalsize += strlen(arr_todecimal) + strlen(end_fun);
    }

    if (rettype == NUMERICOID)
        totalsize += strlen(strconvert);

    if (nargs > 1)
        totalsize += (nargs - 1) * strlen(comma);

    block2str = (char *) palloc0(totalsize);
    SNPRINTF(block2str, totalsize - cursize, "%s%s%s", resu_var, func, beginFun);
    cursize = strlen(resu_var) + strlen(func) + strlen(beginFun);

    for (i = 0; i < nargs; i++)
    {
        SNPRINTF(argname, strlen(argname)+1, "arg%d", i); /* review nargs > 9 */
        str_ptr = (char *)(block2str + cursize);

        if (argtype[i] == NUMERICOID)
        {
            SNPRINTF(str_ptr,totalsize-cursize,"%s%s%s%s",
                     todecimal, libargs, argname, end_fun);
        }
        else if (pldotnet_IsArray(i, &func_inout_info) &&
                func_inout_info.arrayinfo[i].typelem == NUMERICOID)
        {
            SNPRINTF(str_ptr,totalsize-cursize,"%s%s%s%s",
                     arr_todecimal, libargs, argname, end_fun);
        }
        else
        {
            SNPRINTF(str_ptr, totalsize-cursize, "%s%s", libargs, argname);
        }

        if (i + 1 < nargs)  /* comma required */
        {
            cursize = strlen(block2str);
            str_ptr = (char *)(block2str + cursize);
            SNPRINTF(str_ptr, totalsize-cursize, "%s", comma);
        }
        cursize = strlen(block2str);
    }

    str_ptr = (char *)(block2str + cursize);
    if (rettype == NUMERICOID)
    {
        SNPRINTF(str_ptr, totalsize-cursize, "%s%s%s"
                   , end_fun, strconvert, semicolon);
    }
    else
    {
        SNPRINTF(str_ptr, totalsize-cursize, "%s%s", end_fun, semicolon);
    }

    return block2str;

}

static int
GetSizeArgsNullArray(int nargs)
{
    const char public_bool_array[] =
        "\n[MarshalAs(UnmanagedType.ByValArray,ArraySubType=UnmanagedType.U1,SizeConst=)]public ";
    int n_digits_args = 0;

    if (nargs > 0)
        n_digits_args = floor(log10(abs(nargs))) + 1;

    return (strlen(public_bool_array) + n_digits_args + strlen(arg_flag_str));
}

/*
 * Returns the size of typical C# line converting
 * a struct argument to a nullable C# type argument
 */
static int
GetSizeNullableHeader(int argnm_size, Oid arg_type, int narg)
{
    int total_size = 0;
    char *question_mark = "?";
    char *equal_char = "=";
    char *parenthesis_char = "("; /* same for ')' */
    char *square_bracket_char = "["; /* same for ']' */
    char *colon_char = ":";
    char *semicolon_char = ";";
    char *newline_char = "\n";
    char *null_str = "null";
    int n_digits_arg = 0;

    if (narg == 0)
        /* Edge case treatment since log10(0) == -HUGE_VAL */
        n_digits_arg = floor(log10(abs(1))) + 1;
    else
        n_digits_arg = floor(log10(abs(narg))) + 1;

    switch (arg_type)
    {
        case INT2OID:
        case INT4OID:
        case INT8OID:
        case BOOLOID:
            /* template: 
             * bool? <arg>=argsnull[i]? (bool?)null : <arg>_nullable; */
            total_size = strlen(pldotnet_GetNullableTypeName(arg_type))
                + argnm_size + strlen(equal_char) + strlen(argsnull_str)
                + strlen(square_bracket_char) + n_digits_arg +
                + strlen(square_bracket_char) + strlen(question_mark)
                + strlen(parenthesis_char)
                + strlen(pldotnet_GetNullableTypeName(arg_type))
                + strlen(parenthesis_char) + strlen(null_str)
                + strlen(colon_char) + argnm_size + strlen(nullable_suffix)
                + strlen(semicolon_char) + strlen(newline_char);
            break;
    }

    return total_size;
}

/*
 * Returns the size of typical C# line converting
 * a nullable C# type return to a struct return
 */
static int
GetSizeNullableFooter(Oid ret_type)
{
    int total_size = 0;

    switch (ret_type)
    {
        case INT2OID:
        case INT4OID:
        case INT8OID:
        case BOOLOID:
            total_size = strlen(resu_nullable_value)
                + strlen(resu_nullable_flag);
            break;
    }

    return total_size;
}

static bool
IsNullable(Oid type)
{
    return (type == INT2OID || type == INT4OID
       || type == INT8OID   ||  type == BOOLOID);
}

static char *
plcsharp_BuildBlockUserFuncDecl(Form_pg_proc procst, HeapTuple proc)
{
    char *block2str, *str_ptr, *source_argnm, *source_text;
    int argnm_size, i, nnames, cursize = 0, source_size, totalsize;
    bool isnull;
    const char begin_fun_decl[] = "(";
    char *func_name;
    const char comma[] = ",";
    const char end_fun_decl[] = "){";
    const char end_fun[] = "}\n";
    const char newline[] = "\n";
    const char arrbrackets[] = "[]";
    char argnm[64];
    int nargs = procst->pronargs;
    Oid rettype = procst->prorettype;
    Datum *argname, argnames, prosrc;
    Oid *argtype = procst->proargtypes.values; /* Indicates the args type */
    /* nullable related */
    char *header_nullable, *header_nullable_ptr;
    int header_size=0, cur_header_size, footer_size=0;

    /* Function name */
    func_name = NameStr(procst->proname);

    /* Source code */
    prosrc = SysCacheGetAttr(PROCOID, proc, Anum_pg_proc_prosrc, &isnull);
    source_text = DatumGetCString(DirectFunctionCall1(textout, prosrc));
    source_size = strlen(source_text);

    argnames = SysCacheGetAttr(PROCOID, proc,
                               Anum_pg_proc_proargnames, &isnull);

    if (!isnull)
        deconstruct_array(DatumGetArrayTypeP(argnames), TEXTOID,
                          -1, false, 'i', &argname, NULL, &nnames);

    /* Caculates the total amount in bytes of C# src text for 
     * the function declaration according nr of arguments 
     * their types and the function return type
     */
    if (IsNullable(rettype))
    {
        totalsize = (2 * strlen(newline))
            + strlen(pldotnet_GetNullableTypeName(rettype))
            + strlen(" ") + strlen(func_name) + strlen(begin_fun_decl);
    }
    else
    {
        totalsize = (2 * strlen(newline))
            + strlen(pldotnet_GetNetTypeName(rettype, false))
            + strlen(" ") + strlen(func_name) + strlen(begin_fun_decl);
    }

    for (i = 0; i < nargs; i++) 
    {
        source_argnm = DatumGetCString( DirectFunctionCall1(textout,
                                                            argname[i]) );
        if (IsNullable(argtype[i]))
        {
            header_size += GetSizeNullableHeader( strlen(source_argnm),
                                                  argtype[i],i );
            argnm_size = strlen(source_argnm) + strlen("_nullable");
        } 
        else
            argnm_size = strlen(source_argnm);

        if (pldotnet_IsArray(i, &func_inout_info))
        {
            totalsize += strlen( pldotnet_GetNetTypeName(
                                 func_inout_info.arrayinfo[i].typelem, false) )
                         + strlen(arrbrackets) + 1 + argnm_size;
        }
        else
        {
             /* +1 here is the space between type" "argname declaration */
            totalsize += strlen(pldotnet_GetNetTypeName(argtype[i], false)) +
                         1 + argnm_size;
        }

    }
     if (nargs > 1)
         totalsize += (nargs - 1) * strlen(comma); /* commas size */

    footer_size = GetSizeNullableFooter(rettype);

    totalsize += strlen(end_fun_decl) + header_size + source_size
        + strlen(end_fun) + footer_size + 1;

    block2str = (char *)palloc0(totalsize);

    if (IsNullable(rettype))
    {
        SNPRINTF(block2str, totalsize, "%s%s%s %s%s",newline
                   , newline, pldotnet_GetNullableTypeName(rettype)
                   , func_name, begin_fun_decl);
    }
    else
    {
        SNPRINTF(block2str, totalsize, "%s%s%s %s%s",newline
                   , newline,  pldotnet_GetNetTypeName(rettype, false)
                   , func_name, begin_fun_decl);
    }

    cursize = strlen(block2str);

    header_nullable = (char *)palloc0(header_size + 1);
    cur_header_size = strlen(header_nullable);

    for (i = 0; i < nargs; i++)
    {
        source_argnm = DatumGetCString( DirectFunctionCall1(textout,
                                                            argname[i]) );
        if (IsNullable(argtype[i]))
        {
            header_nullable_ptr = (char *) (header_nullable + cur_header_size);
            SNPRINTF(header_nullable_ptr, (header_size - cur_header_size) + 1
                       , "%s%s=%s[%d]?(%s)null:%s%s;\n"
                       , pldotnet_GetNullableTypeName(argtype[i])
                       , source_argnm, argsnull_str, i
                       , pldotnet_GetNullableTypeName(argtype[i])
                       , source_argnm, nullable_suffix);
            cur_header_size = strlen(header_nullable);
            SNPRINTF(argnm, strlen(source_argnm) + strlen("_nullable") + 1
                            , "%s_nullable", source_argnm);
        }
        else
            SNPRINTF(argnm, strlen(source_argnm) + 1, "%s", source_argnm);

        str_ptr = (char *)(block2str + cursize);
        if (pldotnet_IsArray(i, &func_inout_info))
        {
            SNPRINTF( str_ptr, totalsize - cursize, "%s%s",
            pldotnet_GetNetTypeName(func_inout_info.arrayinfo[i].typelem,
                                    false), arrbrackets );
        }
        else
        {
            SNPRINTF( str_ptr, totalsize - cursize, "%s",
                      pldotnet_GetNetTypeName(argtype[i], false) );

        }
        cursize = strlen(block2str);
        str_ptr = (char *)(block2str + cursize);
        SNPRINTF( str_ptr, totalsize - cursize, " %s", argnm);
        if (i + 1 < nargs)
        {  /* last no comma */
            cursize = strlen(block2str);
            str_ptr = (char *)(block2str + cursize);
            SNPRINTF(str_ptr, totalsize - cursize, "%s", comma);
        }

        cursize = strlen(block2str);
        bzero(argnm, sizeof(argnm));
    }

    str_ptr = (char *)(block2str + cursize);
    SNPRINTF(str_ptr, totalsize - cursize, "%s", end_fun_decl);
    cursize = strlen(block2str);

    if (header_size > 0)
    {
        str_ptr = (char *)(block2str + cursize);
        SNPRINTF(str_ptr, totalsize - cursize, "%s",header_nullable);
        cursize = strlen(block2str);
    }

    str_ptr = (char *)(block2str + cursize);
    SNPRINTF(str_ptr, totalsize - cursize, "%s%s", source_text, end_fun);
    cursize = strlen(block2str);

    if (footer_size > 0)
    {
        str_ptr = (char *)(block2str + cursize);
        SNPRINTF(str_ptr, totalsize - cursize, "%s%s"
            , resu_nullable_value, resu_nullable_flag);
    }

    return block2str;

}

/* Postgres Datum type to C# nullable type name */
static const char *
pldotnet_GetNullableTypeName(Oid id)
{
    switch (id)
    {
        case BOOLOID:
            return "bool?"; /* Nullable<System.Boolean> */
        case INT2OID:
            return "short?";/* Nullable<System.Int16> */
        case INT4OID:
            return "int?";  /* Nullable<System.Int32> */
        case INT8OID:
            return "long?"; /* Nullable<System.Int64> */
    }
    return "";
}

bool 
pldotnet_CheckArgIsArray(Datum datum, Oid oid, int narg)
{
    HeapTuple typetuple;
    Form_pg_type typeinfo;
    ArrayType *arr;
    char argName[] = " argN";
    bool isarr = false;
    pldotnet_ArgArrayInfo * parr_info;

    typetuple = SearchSysCache(TYPEOID, ObjectIdGetDatum(oid), 0, 0, 0);
    if (!HeapTupleIsValid(typetuple)) {
        elog(ERROR,
          "[pldotnet]: (CheckArgIsArray) cache lookup failed for type %u", oid);
    }
    typeinfo = (Form_pg_type) GETSTRUCT(typetuple);

    isarr = (typeinfo->typelem != 0 && typeinfo->typlen == -1);

    if (isarr)
    {
        parr_info = &(func_inout_info.arrayinfo[ narg ]);
        parr_info->ixarray = narg;
        parr_info->typlen = typeinfo->typlen;
        parr_info->typbyval = typeinfo->typbyval;
        parr_info->typtype = typeinfo->typtype;
        parr_info->typelem = typeinfo->typelem;
        parr_info->typalign = typeinfo->typalign;

        arr = DatumGetArrayTypeP(datum);
        parr_info->ndim = ARR_NDIM(arr);
        parr_info->dims = ARR_DIMS(arr);
        parr_info->nelems = ArrayGetNItems(ARR_NDIM(arr), ARR_DIMS(arr));

        /* Review for nargs > 9*/
        sprintf(argName, " arg%d", narg);
        sprintf(parr_info->csharpdecl,
"\n[MarshalAs(UnmanagedType.ByValArray,ArraySubType=UnmanagedType.%s,\
SizeConst=%d)]public %s[] %s;",
                    pldotnet_GetUnmanagedTypeName(parr_info->typelem),
                    parr_info->nelems,
                    pldotnet_GetNetTypeName(parr_info->typelem, true),
                    argName);
    }
    else
        func_inout_info.arrayinfo[ narg ].ixarray = -1;
    ReleaseSysCache(typetuple);
    return isarr;
}

static char *
pldotnet_CreateCStructLibargs(FunctionCallInfo fcinfo, Form_pg_proc procst)
{
    int i;
    char *libargs_ptr = NULL;
    char *cur_arg = NULL;
    Oid *argtype = procst->proargtypes.values;
    Oid rettype = procst->prorettype;
    /* nullable related */
    bool nullable_arg_flag = false;
    bool *argsnull_ptr;
    Datum argdatum;

    char *array_p;
    Datum array_element;
    pldotnet_ArgArrayInfo * arrinfo;
    ArrayType *arr;

    func_inout_info.typesize_args = 0;
    func_inout_info.typesize_nullflags = 0;

    for (i = 0; i < fcinfo->nargs; i++)
    {
        if (pldotnet_IsArray(i, &func_inout_info))
        {
            func_inout_info.typesize_args +=
              (func_inout_info.arrayinfo[i].nelems *
               pldotnet_GetTypeSize(func_inout_info.arrayinfo[i].typelem));
        }
        else
            func_inout_info.typesize_args += pldotnet_GetTypeSize(argtype[i]);
        if (IsNullable(argtype[i]))
            nullable_arg_flag = true;
    }

    if (nullable_arg_flag)
        func_inout_info.typesize_nullflags += sizeof(bool) * fcinfo->nargs;

    func_inout_info.typesize_nullflags += sizeof(bool);

    func_inout_info.typesize_result = pldotnet_GetTypeSize(rettype);

    libargs_ptr = (char *) palloc0(  func_inout_info.typesize_nullflags
                                   + func_inout_info.typesize_args
                                   + func_inout_info.typesize_result  );

    argsnull_ptr = (bool *) libargs_ptr;
    cur_arg = libargs_ptr + func_inout_info.typesize_nullflags;

    for (i = 0; i < fcinfo->nargs; i++)
    {
#if PG_VERSION_NUM >= 120000
        argdatum = fcinfo->args[i].value;
#else
        argdatum = fcinfo->arg[i];
#endif
        if (pldotnet_IsArray(i, &func_inout_info))
        {
            arrinfo = &(func_inout_info.arrayinfo[i]);
            arr = DatumGetArrayTypeP(argdatum);
            array_p = ARR_DATA_PTR(arr);
            if (arrinfo->ndim > 1)
                elog(ERROR, "Multidimensional array not supported.");
            for (int j = 0; j < arrinfo->nelems; j++)
            {

                array_element =
                      fetch_att(array_p, arrinfo->typbyval, arrinfo->typlen);

                pldotnet_SetScalarValue(cur_arg,
                        /* This needs to reviewed: why for bittable/simple
                           types we need to pass the value. Makes sense
                           but it seems not to be necessary/used in others pl
                           extensions. */
                                pldotnet_IsSimpleType(arrinfo->typelem) ?
                  (Datum) (*(Datum *) (array_element)) : array_element,
                                        fcinfo, j, arrinfo->typelem, NULL);
                /* Iterate array */
                array_p = att_addlength_pointer(array_p, arrinfo->typlen,
                                                array_p);
                array_p = (char *) att_align_nominal(array_p,
                                                           arrinfo->typalign);
                /* Iterate CLibargs */
                cur_arg += pldotnet_GetTypeSize(arrinfo->typelem);
            }
            continue;
        }
        else if ( !pldotnet_IsSimpleType(argtype[i]) &&
                  !pldotnet_IsTextType(argtype[i]) )
        {
            pldotnet_FillCompositeValues(cur_arg, argdatum, argtype[i],
                                                               fcinfo, procst);
        }
        else
        {
            pldotnet_SetScalarValue(cur_arg, argdatum, fcinfo, i, argtype[i],
                                    argsnull_ptr + i);
        }
        cur_arg += pldotnet_GetTypeSize(argtype[i]);
    }

    return libargs_ptr;
}

static Datum
pldotnet_GetNetResult(char * libargs, Oid rettype, FunctionCallInfo fcinfo)
{
    char * result_ptr = libargs + func_inout_info.typesize_args
                                + func_inout_info.typesize_nullflags;
    char * resultnull_ptr = libargs +
                           (func_inout_info.typesize_nullflags - sizeof(bool));

    if (!pldotnet_IsSimpleType(rettype) && !pldotnet_IsTextType(rettype))
    {
        /* TODO: review null composite values */
        fcinfo->isnull = *(bool *) (resultnull_ptr);
        if (fcinfo->isnull)
            return (Datum) 0;
        return pldotnet_CreateCompositeResult(result_ptr, rettype, fcinfo);
    }

    return
          pldotnet_GetScalarValue(result_ptr, resultnull_ptr, fcinfo, rettype);
}

PG_FUNCTION_INFO_V1(plcsharp_call_handler);
Datum plcsharp_call_handler(PG_FUNCTION_ARGS)
{
    bool istrigger;
    char *source_code, *cs_block_args_decl, *cs_block_callfunc_call,
         *cs_block_userfunc_decl;
    char *libargs;
    int source_code_size;
    HeapTuple proc;
    Form_pg_proc procst;
    Datum retval = 0;
    Oid rettype;
    char cs_block_composite_decl[256];
    cs_block_composite_decl[0] = 0;

    if (SPI_connect() != SPI_OK_CONNECT)
        elog(ERROR, "[pldotnet]: could not connect to SPI manager");
    istrigger = CALLED_AS_TRIGGER(fcinfo);
    if (istrigger)
    {
        ereport(ERROR,
              (errcode(ERRCODE_FEATURE_NOT_SUPPORTED),
               errmsg("[pldotnet]: dotnet trigger not supported")));
    }
    PG_TRY();
    {
        /* STEP 0: Creates an execution memory context for the function */
        MemoryContext oldcontext = CurrentMemoryContext;
        MemoryContext func_cxt = NULL;
        func_cxt = AllocSetContextCreate(TopMemoryContext,
                                    "PL/NET func_exec_ctx",
                                    ALLOCSET_SMALL_SIZES);
        MemoryContextSwitchTo(func_cxt);

        /*
         * STEP 1: Load HostFxr and get exported hosting functions
         */
        if (!hostfxr_loaded)
        {
            if (!pldotnet_LoadHostfxr())
                assert(0 && "Failure: pldotnet_LoadHostfxr()");
            hostfxr_loaded = true;
        }

        /* STEP 2: Build paths C# project / compiler and runner paths */
        if (!paths_defined)
        {
            plcsharp_BuildPaths();
            paths_defined = true;
        }

        /* STEP 3: Generate the function C# code from the template
         * TODO: Check if template needs to be filled again */
        proc = SearchSysCache(PROCOID
                        , ObjectIdGetDatum(fcinfo->flinfo->fn_oid), 0, 0, 0);
        if (!HeapTupleIsValid(proc))
            elog(ERROR, "[pldotnet]: cache lookup failed for function %u"
                            , (Oid) fcinfo->flinfo->fn_oid);
        procst = (Form_pg_proc) GETSTRUCT(proc);

        /* STEP 3.1: Fills the C# template source code */
        plcsharp_BuildBlockComposites(cs_block_composite_decl, fcinfo, procst);

        cs_block_args_decl = plcsharp_BuildBlockArgsDecl( fcinfo, procst );
        cs_block_callfunc_call = plcsharp_BuildBlockCallFuncCall( procst );
        cs_block_userfunc_decl = plcsharp_BuildBlockUserFuncDecl(procst, proc);

        source_code_size = strlen(cs_block_header)
                         + strlen(cs_block_composite_decl)
                         + strlen(cs_block_args_header)
                         + strlen(cs_block_args_decl)
                         + strlen(cs_block_callfunc_header)
                         + strlen(cs_block_callfunc_call)
                         + strlen(cs_block_userfunc_decl)
                         + strlen(cs_block_footer) + 1;

        source_code = palloc0(source_code_size);
        SNPRINTF(source_code, source_code_size, "%s%s%s%s%s%s%s%s",
                                                cs_block_header,
                                                cs_block_composite_decl,
                                                cs_block_args_header,
                                                cs_block_args_decl,
                                                cs_block_callfunc_header,
                                                cs_block_callfunc_call,
                                                cs_block_userfunc_decl,
                                                cs_block_footer);
        rettype = procst->prorettype;
        ReleaseSysCache(proc);

        /* STEP 4: Build the LibArgs which holds all
         * function input values accoring to .NET interop possibilites.
         * TODO: Check if CStructLibargs needs to be generated and filled
         * again */
        libargs = pldotnet_CreateCStructLibargs(fcinfo, procst);

#ifdef USE_DOTNETBUILD
        /* STEP 5: Compiles the C# code  */
        plcsharp_CompileFunctionNetBuild(source_code);

        /* STEP 6: Loads the generated Aseembly
         * TODO: Review why we need to GetNetLoadAssembly on each call */
        load_assembly_and_get_function_pointer =
                                         GetNetLoadAssembly(csharp_config_path);
         assert(load_assembly_and_get_function_pointer != nullptr &&
                                               "Failure: GetNetLoadAssembly()");
#else
        /* STEP 5: Loads the pldotnet Roslyn compiler and runner
         * TODO: Review why we need to GetNetLoadAssembly on each call */
        load_assembly_and_get_function_pointer =
                                         GetNetLoadAssembly(csharp_config_path);
         assert(load_assembly_and_get_function_pointer != nullptr &&
                                               "Failure: GetNetLoadAssembly()");
        /* STEP 6: Compiles the C# code  */
        plcsharp_CompileFunction(source_code, fcinfo);
#endif

        /* STEP 7: Executes the function */
        plcsharp_RunFunction(libargs, fcinfo);

        /* STEP 8: Collects the result from libargs*/
        retval = pldotnet_GetNetResult( libargs, rettype, fcinfo );

        /* STEP 9: previous Memory context is restored
         *
         * All palloced memory is freed by the PG memory manager.
         *
         */
        MemoryContextSwitchTo(oldcontext);
        if (func_cxt)
            MemoryContextDelete(func_cxt);
    }
    PG_CATCH();
    {
        /* Do the excption handling */
        elog(WARNING, "Exception");
        PG_RE_THROW();
    }
    PG_END_TRY();
    if (SPI_finish() != SPI_OK_FINISH)
        elog(ERROR, "[pldotnet]: could not disconnect from SPI manager");
    return retval;
}

PG_FUNCTION_INFO_V1(plcsharp_validator);
Datum plcsharp_validator(PG_FUNCTION_ARGS)
{
    /* return DotNET_validator( additional args,PG_GETARG_OID(0)); */
    if (SPI_connect() != SPI_OK_CONNECT)
        elog(ERROR, "[pldotnet]: could not connect to SPI manager");
    PG_TRY();
    {
        /* Do some dotnet checking ?? */
    }
    PG_CATCH();
    {
        /* Do the excption handling */
        PG_RE_THROW();
    }
    PG_END_TRY();
    if (SPI_finish() != SPI_OK_FINISH)
        elog(ERROR, "[pldotnet]: could not disconnect from SPI manager");
    return 0; /* VOID */
}


PG_FUNCTION_INFO_V1(plcsharp_inline_handler);
Datum plcsharp_inline_handler(PG_FUNCTION_ARGS)
{
    int source_code_size;
    char* block_inline_usercode;
    char* source_code;

    if (SPI_connect() != SPI_OK_CONNECT)
        elog(ERROR, "[plldotnet]: could not connect to SPI manager");

    PG_TRY();
    {
        /* STEP 0: Creates an execution memory context for the function */
        MemoryContext oldcontext = CurrentMemoryContext;
        MemoryContext func_cxt = NULL;
        func_cxt = AllocSetContextCreate(TopMemoryContext,
                                    "PL/NET inline_exec_ctx",
                                    ALLOCSET_SMALL_SIZES);
        MemoryContextSwitchTo(func_cxt);

        /*
         * STEP 1: Load HostFxr and get exported hosting functions
         */
        if (!hostfxr_loaded) {
            if (!pldotnet_LoadHostfxr())
                assert(0 && "Failure: pldotnet_LoadHostfxr()");
            hostfxr_loaded = true;
        }

        /* STEP 2: Build paths C# project paths */
        if (!paths_defined)
        {
            plcsharp_BuildPaths();
            paths_defined = true;
        }

        /* STEP 4: Generate the function C# code from the template
         * TODO: Check if template needs to be filled again */
        block_inline_usercode = CODEBLOCK;
        source_code_size = strlen(block_inline_header)
                         + strlen(block_inline_callfunc)
                         + strlen(block_inline_usercode)
                         + strlen(block_inline_footer) + 1;
        source_code = (char*) palloc0(source_code_size);
	    SNPRINTF(source_code, source_code_size, "%s%s%s%s",
                                                block_inline_header,
                                                block_inline_callfunc,
                                                block_inline_usercode,
                                                block_inline_footer);
#ifdef USE_DOTNETBUILD
        /* STEP 5: Compiles the C# code  */
        plcsharp_CompileFunctionNetBuild(source_code);
        /* STEP 6: Loads the User code Assembly
         * TODO: Review why we need to GetNetLoadAssembly on each call */
        load_assembly_and_get_function_pointer =
                                        GetNetLoadAssembly(csharp_config_path);
        assert(load_assembly_and_get_function_pointer != nullptr &&
                                               "Failure: GetNetLoadAssembly()");
#else
        /* STEP 5: Loads the pldotnet Roslyn compiler and runner
         * TODO: Review why we need to GetNetLoadAssembly on each call */
        load_assembly_and_get_function_pointer =
                                        GetNetLoadAssembly(csharp_config_path);
        assert(load_assembly_and_get_function_pointer != nullptr &&
                                               "Failure: GetNetLoadAssembly()");
        /* STEP 6: Compiles the C# code  */
        plcsharp_CompileFunction(source_code, fcinfo);
#endif
        /* STEP 6: runs the inline code */
        plcsharp_RunFunction(NULL, NULL);

        /* STEP 7: previous Memory context is restored
         *
         * All palloced memory is freed by the PG memory manager.
         *
         */
        MemoryContextSwitchTo(oldcontext);
        if (func_cxt)
            MemoryContextDelete(func_cxt);

    }
    PG_CATCH();
    {
        /* Exception handling */
        PG_RE_THROW();
    }
    PG_END_TRY();

    if (SPI_finish() != SPI_OK_FINISH)
        elog(ERROR, "[pldotnet]: could not disconnect from SPI manager");
    PG_RETURN_VOID();
}

static inline void
plcsharp_BuildPaths(void)
{
    const char json_path_suffix[] = "/src/csharp/PlDotNET.runtimeconfig.json";
    const char src_path_suffix[] = "/src/csharp/Lib.cs";
    const char dll_path_suffix[] = "/src/csharp/PlDotNET.dll";
    SNPRINTF(csharp_config_path,MAXPGPATH, "%s%s", root_path, json_path_suffix);
    SNPRINTF(csharp_lib_path, MAXPGPATH, "%s%s", root_path, dll_path_suffix);
    SNPRINTF(csharp_srclib_path, MAXPGPATH, "%s%s", dnldir, src_path_suffix);
}

int 
plcsharp_CompileFunctionNetBuild(char * source_code)
{
    FILE *output_file;
    int compile_resp;
    char *cmd;
    output_file = fopen(csharp_srclib_path, "w");
    if (!output_file)
    {
        fprintf(stderr, "Cannot open file: '%s'\n", csharp_srclib_path);
        exit(-1);
    }
    if (fputs(source_code, output_file) == EOF)
    {
        fprintf(stderr, "Cannot write to file: '%s'\n", csharp_srclib_path);
        exit(-1);
    }
    fclose(output_file);
    setenv("DOTNET_CLI_HOME", dnldir, 1);
    cmd = palloc0(strlen("dotnet build ")
        + strlen(dnldir) + strlen("/src/csharp > null") + 1);
    SNPRINTF(cmd, strlen("dotnet build ") + strlen(dnldir) +
                  strlen("/src/csharp > null") + 1,
                  "dotnet build %s/src/csharp > null", dnldir);
    compile_resp = system(cmd);
    assert(compile_resp != -1 && "Failure: Cannot compile C# source code");
    return 0;
}

int 
plcsharp_CompileFunction(char * src, FunctionCallInfo fcinfo)
{
    char dotnet_type[] = "PlDotNET.Engine, PlDotNET";
    char dotnet_type_method[64] = "Compile";

    pldotnet_ArgsSource args;
    args.source_code = src;
    args.func_oid = (int) fcinfo->flinfo->fn_oid;
    args.result = 1;

    return plcsharp_Run(dotnet_type, dotnet_type_method, (char *)&args,
                                                 sizeof(pldotnet_ArgsSource));
}

Datum 
plcsharp_RunFunction(char * libargs, FunctionCallInfo fcinfo)
{
    Datum retval = 0;
#ifdef USE_DOTNETBUILD
    char dotnet_type[]  = "PlDotNETUserSpace.UserClass, PlDotNETUserSpace";
    char dotnet_type_method[64] = "CallFunction";
    FILE *output_file;
#else
    char dotnet_type[] = "PlDotNET.Engine, PlDotNET";
    char dotnet_type_method[64] = "Run";
#endif

    if (libargs != NULL)  /* Regular functions */
    {
        retval = plcsharp_Run(dotnet_type, dotnet_type_method, libargs,
                 func_inout_info.typesize_nullflags +
                 func_inout_info.typesize_args +
                 func_inout_info.typesize_result);
    }
    else  /* Inlines */
        retval = plcsharp_Run(dotnet_type, dotnet_type_method, NULL, 0);

    return retval;
}

int 
plcsharp_Run(char * dotnet_type, char * dotnet_type_method, char * libargs,
                                                                 int args_size)
{
    int rc;
    component_entry_point_fn csharp_method = nullptr;

    /* Function pointer to managed delegate */
    rc = load_assembly_and_get_function_pointer(
        csharp_lib_path,
        dotnet_type,
        dotnet_type_method,
        nullptr /* delegate_type_name */,
        nullptr,
        (void**)&csharp_method);

    assert(rc == 0 && csharp_method != nullptr && \
            "Failure: load_assembly_and_get_function_pointer()");

    return csharp_method(libargs, args_size);

}

#endif
