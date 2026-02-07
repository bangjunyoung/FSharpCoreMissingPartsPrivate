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

module FSharpCoreMissingParts.SeqTests

open NUnit.Framework

let isOrderedTestParameters =
    [
        Ascending, [], true
        Ascending, [1], true
        Ascending, [1; 1], true
        Ascending, [1; 2; 3], true
        Ascending, [1; 3; 2], false
        Descending, [], true
        Descending, [1], true
        Descending, [1; 1], true
        Descending, [3; 1; 2], false
        Descending, [3; 2; 1], true
        StrictAscending, [], true
        StrictAscending, [1], true
        StrictAscending, [1; 1], false
        StrictAscending, [1; 2; 3], true
        StrictAscending, [1; 3; 2], false
        StrictDescending, [], true
        StrictDescending, [1], true
        StrictDescending, [1; 1], false
        StrictDescending, [3; 1; 2], false
        StrictDescending, [3; 2; 1], true
    ]
    |> List.map (fun (order, source, expected) ->
        TestCaseData(order, source).Returns(expected)
            .SetName($"isOrdered {order} %A{source} returns {expected}"))

[<TestCaseSource(nameof isOrderedTestParameters)>]
let ``isOrdered with valid arguments`` order source =
    Seq.isOrdered order source
