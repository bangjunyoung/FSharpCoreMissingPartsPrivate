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

module FSharpCoreMissingParts.ListTests

open NUnit.Framework

let pairwiseWrappedTestParameters =
    [
        [], []
        [1], [1, 1]
        [1; 2; 3], [1, 2; 2, 3; 3, 1]
    ]
    |> List.map (fun (source, expected) ->
        TestCaseData(source).Returns(expected).SetName($"pairwiseWrapped %A{source}"))

[<TestCaseSource(nameof pairwiseWrappedTestParameters)>]
let pairwiseWrappedTest source =
    source |> List.pairwiseWrapped

let crossMapTestParameters =
    [
        (+), "op_Addition", [1; 2], [10; 20], [11; 21; 12; 22]
        (fun x y -> x * y), "fun x y -> x * y", [1; 2], [10; 20], [10; 20; 20; 40]
    ]
    |> List.map (fun (mapping, mappingDispname, source1, source2, expected) ->
        let args = [| box mapping; box source1; box source2 |]
        TestCaseData(args).Returns(expected)
            .SetName($"crossMap ({mappingDispname}) %A{source1} %A{source2}"))

[<TestCaseSource(nameof crossMapTestParameters)>]
let crossMapTest (mapping: 'a -> 'a -> 'a) source1 source2 =
    (source1, source2) ||> List.crossMap mapping

