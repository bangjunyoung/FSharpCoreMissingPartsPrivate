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

module FSharpCoreMissingParts.StringTests

open System
open NUnit.Framework

let ofSeqTestParameters =
    [
        Seq.empty, ""
        seq [], ""
        seq [||], ""
        seq [|'a'|], "a"
        seq ['b'], "b"
        seq { yield 'c'}, "c"
        seq ['d'; 'e'], "de"
        seq [|'f'; 'g'; 'h'|], "fgh"
    ]
    |> Seq.map (fun (source, expected) ->
        TestCaseData(source).Returns(expected))

[<TestCaseSource(nameof ofSeqTestParameters)>]
let ``ofSeq with valid arguments`` source =
    source |> String.ofSeq

let ellipsizeTestParameters =
    [
        0, "", ""
        1, "1", "1"
        1, "12", "…"
        0, "123", ""
        1, "123", "…"
        2, "123", "1…"
        3, "123", "123"
        4, "123", "123"
    ]
    |> Seq.map (fun (length, source, expected) ->
        TestCaseData(length, source).Returns(expected))

[<TestCaseSource(nameof ellipsizeTestParameters)>]
let ``ellipsize with valid arguments`` length source =
    source |> String.ellipsize length

[<Test>]
let ``ellipsize throws ArgumentException if length < 0`` () =
    Assert.That(Func<_>(fun () -> "123" |> String.ellipsize (-1)),
        Throws.TypeOf<ArgumentException>())
