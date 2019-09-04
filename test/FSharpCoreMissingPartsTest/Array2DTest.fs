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

module FSharpCoreMissingParts.Array2DTest

open NUnit.Framework

let ofArrayTestParameters =
    [
        [||], 0, 0, array2D [| [||] |]
        [|1|], 1, 1, array2D [| [|1|] |]
        [|1 .. 6|], 3, 2, array2D [| [|1; 2|]; [|3; 4|]; [|5; 6|] |]
        [|1 .. 6|], 2, 3, array2D [| [|1; 2; 3|]; [|4; 5; 6|] |]
    ]
    |> List.map (fun (source, nrows, ncols, expected) ->
        TestCaseData(source, nrows, ncols).Returns(expected))

[<TestCaseSource("ofArrayTestParameters")>]
let ``ofArray with valid arguments`` (source: int[]) nrows ncols =
    source |> Array2D.ofArray nrows ncols

let toArrayTestParameters =
    [
        array2D [| [||] |], [||]
        array2D [| [|1|] |], [|1|]
        array2D [| [|1; 2|]; [|3; 4|]; [|5; 6|] |], [|1 .. 6|]
        array2D [| [|1; 2; 3|]; [|4; 5; 6|] |], [|1 .. 6|]
    ]
    |> List.map (fun (source, expected) ->
        TestCaseData(source).Returns(expected))

[<TestCaseSource("toArrayTestParameters")>]
let ``toArray with valid arguments`` (source: int[,]) =
    source |> Array2D.toArray
