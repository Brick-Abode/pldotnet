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

type InoutTestsFSharpClass() =
    
    static member fsInoutAllthreeS (a: int) (b: int) : Nullable<int> * Nullable<int> = 
        Nullable(b+1), Nullable(a+b)

    static member fsInoutAllthree (a: Nullable<int>) (b: Nullable<int>) : Nullable<int> * Nullable<int> = 
        if a.HasValue && b.HasValue then
            Nullable(a.Value + 1), Nullable(a.Value + b.Value)
        else
            Nullable(), Nullable()

    static member fsInoutAllthreearray (a: int) (b: int) : Nullable<int> * Array = 
        let c = b + 1
        let arr = Array.CreateInstance(typeof<int16>, 3, 3)
        arr.SetValue((int16)a, 0, 0)
        arr.SetValue((int16)a, 1, 1)
        arr.SetValue((int16)a, 2, 2)
        Nullable(c), arr

    static member inoutMultiarg1FsS (a0: int) (a1: int) (a2: int) (a5: int) (a6: int) : Nullable<int> * Nullable<int> * Nullable<int> * Nullable<int> * Nullable<int> = 
        if a0 <> 0 then
            raise <| SystemException("Failed assertion: a0")
        if  a1 <> 1 then
            raise <| SystemException("Failed assertion: a1")
        if  a2 <> 2 then
            raise <| SystemException("Failed assertion: a2")
        if a5 <> 5 then
            raise <| SystemException("Failed assertion: a5")
        if  a6 <> 6 then
            raise <| SystemException("Failed assertion: a6")

        (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable(7))

    static member inoutMultiarg1Fs (a0: Nullable<int>) (a1: Nullable<int>) (a2: Nullable<int>) (a5: Nullable<int>) (a6: Nullable<int>) : Nullable<int> * Nullable<int> * Nullable<int> * Nullable<int> * Nullable<int> = 
        if a0.HasValue && a0.Value <> 0 then
            raise <| SystemException("Failed assertion: a0")
        if a1.HasValue && a1.Value <> 1 then
            raise <| SystemException("Failed assertion: a1")
        if a2.HasValue && a2.Value <> 2 then
            raise <| SystemException("Failed assertion: a2")
        if a5.HasValue then
            raise <| SystemException("Failed assertion: a5")
        if a6.HasValue && a6.Value <> 6 then
            raise <| SystemException("Failed assertion: a6")

        (Nullable(2), Nullable(4), Nullable(5), Nullable(6), Nullable())

    static member inoutMrray10FsS (input_array: Array) : Array = 
        let count = input_array.Length
        let output = Array.CreateInstance(typeof<obj>, count)
        for i = 0 to count - 1 do
            output.SetValue(input_array.GetValue(i), i)
        if count > 3 then
            output.SetValue(Nullable() :> obj, 3)
        output
    
    static member inoutMrray11FsS (address: PhysicalAddress) (count: int) : Array = 
        let output = Array.CreateInstance(typeof<obj>, count)
        for i = 0 to count - 1 do
            output.SetValue(address :> obj, i)
        if count > 3 then
            output.SetValue(Nullable() :> obj, 3)
        output

    static member inoutMbject10Fs (a: string) (b: string) : string = 
        a + " " + b;
    
    static member inoutMbject20Fs (a: string) (b: string) : string = 
        a + " " + b;