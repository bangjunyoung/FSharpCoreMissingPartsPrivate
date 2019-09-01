//
// Copyright 2019 Bang Jun-young
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

module FSharpCoreMissingParts.CycleTest

open NUnit.Framework

let valueTestParameters =
    let boxList = [box 1; box 2]
    let cycle = Cycle.ofList boxList
    [
        (fun () -> cycle |> Cycle.value), boxList.[0]
        (fun () -> cycle |> Cycle.next |> Cycle.value), boxList.[1]
        (fun () -> cycle |> Cycle.next |> Cycle.next |> Cycle.value), boxList.[0]
        (fun () -> cycle |> Cycle.next |> Cycle.next |> Cycle.next |> Cycle.value), boxList.[1]

        (fun () -> cycle.Value), boxList.[0]
        (fun () -> cycle.Next.Value), boxList.[1]
        (fun () -> cycle.Next.Next.Value), boxList.[0]
        (fun () -> cycle.Next.Next.Next.Value), boxList.[1]
    ]
    |> List.map (fun (value, expected) ->
        TestCaseData(value).Returns(expected))

[<TestCaseSource("valueTestParameters")>]
let ``value with valid arguments`` (f: unit -> obj) =
    f ()

[<Test>]
let ``ofList throws ArgumentException if [] is given`` () =
    Assert.Throws<System.ArgumentException>(
        fun () -> [] |> Cycle.ofList |> ignore)
    |> ignore
