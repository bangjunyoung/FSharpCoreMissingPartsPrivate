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
        TestCaseData(source).Returns(expected)
            .SetName($"pairwiseWrapped %A{source} returns %A{expected}"))

[<TestCaseSource(nameof pairwiseWrappedTestParameters)>]
let ``pairwiseWrapped with valid arguments`` source =
    source |> List.pairwiseWrapped

[<Test>]
let ``crossMap is equivalent to allPairs followed by map`` () =
    let source1, source2 = [1 .. 10], [11 .. 20]
    let actual = (source1, source2) ||> List.crossMap (fun x y -> x + y)
    let expected = (source1, source2) ||> List.allPairs |> List.map (fun (x, y) -> x + y)

    Assert.That(actual, Is.EqualTo expected)
