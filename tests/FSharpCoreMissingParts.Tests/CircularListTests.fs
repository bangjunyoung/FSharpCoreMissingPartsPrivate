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
    let boxedList: obj list = [1; 2]
    let listHead = CircularList.ofList boxedList

    [
        (fun () -> listHead |> CircularList.value), boxedList[0]
        (fun () -> listHead |> CircularList.next |> CircularList.value), boxedList[1]
        (fun () -> listHead |> CircularList.next |> CircularList.next |> CircularList.value), boxedList[0]
        (fun () -> listHead |> CircularList.next |> CircularList.next |> CircularList.next |> CircularList.value), boxedList[1]

        (fun () -> listHead.Value), boxedList[0]
        (fun () -> listHead.Next.Value), boxedList[1]
        (fun () -> listHead.Next.Next.Value), boxedList[0]
        (fun () -> listHead.Next.Next.Next.Value), boxedList[1]
    ]
    |> List.map (fun (expr, expected) ->
        TestCaseData(expr).Returns(expected))

[<TestCaseSource(nameof valueTestParameters)>]
let ``value with valid arguments`` (f: unit -> obj) =
    f ()

[<Test>]
let ``ofList throws ArgumentException if an empty list is given`` () =
    Assert.That((fun () -> CircularList.ofList [] |> ignore),
        Throws.TypeOf<System.ArgumentException>())
