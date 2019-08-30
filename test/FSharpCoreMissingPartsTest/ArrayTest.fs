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

module FSharpCoreMissingParts.ArrayTest

open NUnit.Framework

let tryBinarySearchTestParameters =
    [
        [||], 42, None
        [|1 .. 50|], 51, None
        [|1 .. 50|], 42, Some 41
    ]
    |> List.map (fun (source, value, expected) ->
        TestCaseData(source, value).Returns(expected))

[<TestCaseSource("tryBinarySearchTestParameters")>]
let ``tryBinarySearch returns expected result`` (source: int[]) value =
    source |> Array.tryBinarySearch value

let tryBinarySearchWithTestParameters =
    [
        [||], 42, None
        [|50 .. -1 .. 1|], 51, None
        [|50 .. -1 .. 1|], 42, Some 8
    ]
    |> List.map (fun (source, value, expected) ->
        TestCaseData(source, value).Returns(expected))

[<TestCaseSource("tryBinarySearchWithTestParameters")>]
let ``tryBinarySearchWith returns expected result`` (source: int[]) value =
    let reverseCompare a b = compare b a
    source |> Array.tryBinarySearchWith reverseCompare value
