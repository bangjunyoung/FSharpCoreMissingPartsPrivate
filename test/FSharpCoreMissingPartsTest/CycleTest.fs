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

let testParameters =
    let cycle = Cycle.ofList [1 .. 3]
    [
        (fun () -> cycle |> Cycle.value), 1
        (fun () -> cycle |> Cycle.next |> Cycle.value), 2
        (fun () -> cycle |> Cycle.next |> Cycle.next |> Cycle.value), 3
        (fun () -> cycle |> Cycle.next |> Cycle.next |> Cycle.next |> Cycle.value), 1
        (fun () -> cycle |> Cycle.next |> Cycle.next |> Cycle.next |> Cycle.next |> Cycle.value), 2

        (fun () -> cycle.Value), 1
        (fun () -> cycle.Next.Value), 2
        (fun () -> cycle.Next.Next.Value), 3
        (fun () -> cycle.Next.Next.Next.Value), 1
        (fun () -> cycle.Next.Next.Next.Next.Value), 2
    ]
    |> List.map (fun (value, expected) ->
        TestCaseData(value).Returns(expected))

[<TestCaseSource("testParameters")>]
let ``Cycle.value returns expected result`` (f: unit -> int) =
    f ()

[<Test>]
let ``Cycle.ofList throws ArgumentException on empty list`` () =
    Assert.Throws<System.ArgumentException>(
        fun () -> ([]: int list) |> Cycle.ofList |> ignore)
    |> ignore
