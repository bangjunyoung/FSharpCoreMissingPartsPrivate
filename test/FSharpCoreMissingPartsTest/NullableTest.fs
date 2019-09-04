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

module FSharpCoreMissingParts.NullableTest

open NUnit.Framework

let validTestParameters =
    [
        (fun () -> Value 19 + Value 23), Value 42
        (fun () -> 19 + Value 23), Value 42
        (fun () -> Value 19 + 23), Value 42
        (fun () -> Value 19 - Value 23), Value -4
        (fun () -> 19 - Value 23), Value -4
        (fun () -> Value 19 - 23), Value -4
        (fun () -> Value 19 * Value 23), Value 437
        (fun () -> 19 * Value 23), Value 437
        (fun () -> Value 19 * 23), Value 437
        (fun () -> Value 437 / Value 23), Value 19
        (fun () -> 437 / Value 19), Value 23
        (fun () -> Value 437 / 23), Value 19
    ]
    |> List.map (fun (expr, expected) ->
        TestCaseData(expr).Returns(expected))

[<TestCaseSource("validTestParameters")>]
let ``operation with valid operands`` (expr: unit -> FSharpNullable<int>) =
    expr ()

let nullTestParameters =
    [
        (fun () -> Value 19 + Null), Null
        (fun () -> Null + 23), Null
        (fun () -> Value 19 + Value 23 + Null), Null
    ]
    |> List.map (fun (expr, expected) ->
        TestCaseData(expr).Returns(expected))

[<TestCaseSource("nullTestParameters")>]
let ``operation evaluates to Null if any operand is Null`` (expr: unit -> FSharpNullable<int>) =
    expr ()

[<Test>]
let ``type conversion to valid types`` () =
    Assert.That(int <| Value 42, Is.EqualTo 42)
    Assert.That(int64 <| Value 42L, Is.EqualTo 42L)
    Assert.That(float <| Value 42., Is.EqualTo 42.)
    Assert.That(byte <| Value 42uy, Is.EqualTo 42uy)
    Assert.That(sbyte <| Value 42y, Is.EqualTo 42y)

[<Test>]
let ``Null-coalescing operator`` () =
    Assert.That(Value 19 + Value 23 <??> 84, Is.EqualTo 42)
    Assert.That(Value 19 + Null <??> 42, Is.EqualTo 42)
