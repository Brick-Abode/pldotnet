// <copyright file="Enums.cs" company="Brick Abode">
//
// PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as
//                      procedural languages (PL)
//
//
// Copyright (c) 2023 Brick Abode
//
// This code is subject to the terms of the PostgreSQL License.
// The full text of the license can be found in the LICENSE file
// at the top level of the pldotnet repository.
//
// </copyright>

using System;

namespace PlDotNET.Common
{
    /// <summary>
    /// Represents the supported .NET programming languages in the PlDotNET context.
    /// </summary>
    public enum DotNETLanguage : ushort
    {
        /// <summary>
        /// Represents the C# language.
        /// </summary>
        CSharp,

        /// <summary>
        /// Represents the F# language.
        /// </summary>
        FSharp,

        /// <summary>
        /// Represents the Visual Basic language.
        /// </summary>
        VisualBasic,
    }

    /// <summary>
    /// Defines the modes of function calls in the PlDotNET system.
    /// </summary>
    public enum CallMode : int
    {
        /// <summary>
        /// Indicates a normal, non-set-returning function (SRF) call.
        /// </summary>
        [System.ComponentModel.Description("CALL_NORMAL")]
        Normal = 1,

        /// <summary>
        /// Indicates the first call to a set-returning function (SRF); this is used to create and cache the function.
        /// </summary>
        [System.ComponentModel.Description("CALL_SRF_FIRST")]
        SrfFirst = 2,

        /// <summary>
        /// Indicates a subsequent call to a set-returning function (SRF).
        /// </summary>
        [System.ComponentModel.Description("CALL_SRF_NEXT")]
        SrfNext = 3,

        /// <summary>
        /// Indicates that the set-returning function (SRF) is done and can be removed from the cache.
        /// </summary>
        [System.ComponentModel.Description("CALL_SRF_CLEANUP")]
        SrfCleanup = 4,

        /// <summary>
        /// Indicates that is calling a trigger.
        /// </summary>
        [System.ComponentModel.Description("CALL_TRIGGER")]
        Trigger = 5,
    }

    /// <summary>
    /// Defines the modes of return values from functions in the PlDotNET system.
    /// </summary>
    public enum ReturnMode : int
    {
        /// <summary>
        /// Indicates that an error was encountered during the function execution.
        /// </summary>
        [System.ComponentModel.Description("RETURN_ERROR")]
        Error = 0,

        /// <summary>
        /// Indicates a normal return from a non-set-returning function call.
        /// </summary>
        [System.ComponentModel.Description("RETURN_NORMAL")]
        Normal = 1,

        /// <summary>
        /// Indicates a return from a set-returning function (SRF) with more values to follow.
        /// </summary>
        [System.ComponentModel.Description("RETURN_SRF_NEXT")]
        SrfNext = 2,

        /// <summary>
        /// Indicates that a set-returning function (SRF) has no more values to return.
        /// </summary>
        [System.ComponentModel.Description("RETURN_SRF_DONE")]
        SrfDone = 3,

        /// <summary>
        /// Indicates that the trigger event should be abort.
        /// </summary>
        [System.ComponentModel.Description("RETURN_TRIGGER_SKIP")]
        TriggerSkip = 4,

        /// <summary>
        /// Indicates that the row has been modified.
        /// </summary>
        [System.ComponentModel.Description("RETURN_TRIGGER_MODIFY")]
        TriggerModify = 5,
    }
}
