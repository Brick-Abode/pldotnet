// <copyright file="Program.fs" company="Brick Abode">
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

namespace TestFSharpDLLFunctions

open System
open System.Collections
open System.Net
open System.Net.NetworkInformation
open System.Text
open NpgsqlTypes

#nowarn "3391"

type TestFSharpClass() =
    static member sum2SmallIntFSharp (a: int16) (b: int16) : Nullable<int16> =
        Nullable (a+b)

    static member mult2IntFSharp (a: Nullable<int>) (b: Nullable<int>) : Nullable<int> =
        match (a.HasValue, b.HasValue) with
        | (false, false) -> System.Nullable()
        | (true, false) -> a.Value
        | (false, true) -> b.Value
        | (true, true) -> Nullable (a.Value*b.Value)

    static member createDateFSharp (year: Nullable<int>) (month: Nullable<int>) (day: Nullable<int>) : Nullable<DateOnly> =
        match (year.HasValue, month.HasValue, day.HasValue) with
        | (true, true, true) -> new DateOnly(year.Value, month.Value, day.Value)
        | _ -> System.Nullable()

    static member addMinutes (time: Nullable<TimeOnly>) (minutes: Nullable<int>) : Nullable<TimeOnly> =
        match (time.HasValue, minutes.HasValue) with
        | (true, true) ->
            (time.Value).AddMinutes(double minutes.Value)
        | (true, false) -> time
        | _ ->
            let auxTime = new TimeOnly(0, 30, 20)
            auxTime.AddMinutes(double minutes.Value)

    static member concatenateString (a: string) (b: string) : string =
        match (a = null, b = null) with
        | (false, false) -> a + " " + b
        | (false, true) -> a
        | (true, false) -> b
        | _ -> null

    static member modifyStringArray (a: Array) (b: string) : Array =
        let dim = a.Rank
        match dim with
        | 1 ->
            a.SetValue(b, 0) |> ignore
            a
        | 2 ->
            a.SetValue(b, 0, 0) |> ignore
            a
        | 3 ->
            a.SetValue(b, 0, 0, 0) |> ignore
            a
        | _ -> a

    static member modifyTimestampArray (a: Array) (b: Nullable<DateTime>) : Array =
        let dim = a.Rank
        let newValue =
            match b.HasValue with
            | true -> b.Value
            | false -> new DateTime(2022, 11, 15, 13, 23, 45)

        match dim with
        | 1 ->
            a.SetValue(newValue, 0) |> ignore
            a
        | 2 ->
            a.SetValue(newValue, 0, 0) |> ignore
            a
        | 3 ->
            a.SetValue(newValue, 0, 0, 0) |> ignore
            a
        | _ -> a

    static member modifyFloat4ArrayFSharp (a: Array) (b: float32) : Array =
        let dim = a.Rank
        match dim with
        | 1 ->
            a.SetValue(b, 0) |> ignore
            a
        | 2 ->
            a.SetValue(b, 0, 0) |> ignore
            a
        | 3 ->
            a.SetValue(b, 0, 0, 0) |> ignore
            a
        | _ -> a

    static member modifyFloat8ArrayFSharp (a: Array) (b: Nullable<float>) : Array =
        let dim = a.Rank
        let newValue =
            match b.HasValue with
            | true -> b.Value
            | false -> 0.0
        match dim with
        | 1 ->
            a.SetValue(newValue, 0) |> ignore
            a
        | 2 ->
            a.SetValue(newValue, 0, 0) |> ignore
            a
        | 3 ->
            a.SetValue(newValue, 0, 0, 0) |> ignore
            a
        | _ -> a
