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

module FSharpCoreMissingParts.CircularListTests

open NUnit.Framework

let valueTestParameters =
    let boxedList: obj list = [0; 1; 2]
    let head = CircularList.ofList boxedList

    [
        (fun () -> head |> CircularList.value), boxedList[0]
        (fun () -> head |> CircularList.next |> CircularList.value), boxedList[1]
        (fun () -> head |> CircularList.next |> CircularList.next |> CircularList.value), boxedList[2]
        (fun () -> head |> CircularList.next |> CircularList.next |> CircularList.next |> CircularList.value), boxedList[0]

        (fun () -> head.Value), boxedList[0]
        (fun () -> head.Next.Value), boxedList[1]
        (fun () -> head.Next.Next.Value), boxedList[2]
        (fun () -> head.Next.Next.Next.Value), boxedList[0]
    ]
    |> List.mapi (fun i (expr, expected) ->
        TestCaseData(expr).Returns(expected).SetName($"[{i}] value"))

[<TestCaseSource(nameof valueTestParameters)>]
let valueTest (f: unit -> obj) =
    f ()

[<Test>]
let ``ofList throws ArgumentException if an empty list is specified`` () =
    Assert.That((fun () -> CircularList.ofList [] |> ignore),
        Throws.TypeOf<System.ArgumentException>())
