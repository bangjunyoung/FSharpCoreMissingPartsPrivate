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

module FSharpCoreMissingParts.MemTests

open System
open NUnit.Framework

[<Test>]
let ``windowed with an empty source returns an empty Seq`` () =
    Assert.That("" |> Mem.ofString |> Mem.windowed 1, Is.EqualTo Seq.empty)

let windowedTestParamters =
    [
        "1234", 1, [|"1"; "2"; "3"; "4"|]
        "1234", 2, [|"12"; "23"; "34"|]
        "1234", 3, [|"123"; "234"|]
        "1234", 4, [|"1234"|]
        "1234", 5, [||]
    ]
    |> List.map (fun (source, predicate, expected) ->
        TestCaseData(source, predicate).Returns(expected)
            .SetName(sprintf "{0} |> windowed {1} returns %A" expected))

[<TestCaseSource(nameof windowedTestParamters)>]
let ``windowed with valid arguments`` (source: string) size =
    source
    |> Mem.ofString
    |> Mem.windowed size
    |> Seq.map string

[<Test>]
let ``windowed with windowSize <= 0 throws ArgumentException`` () =
    Assert.That(Func<_>(fun () -> "123".AsMemory() |> Mem.windowed 0),
        Throws.TypeOf<ArgumentException>())

let forallTestParameters =
    [
        "",      (fun (x: char) -> int x % 2 = 0), true
        "02468", (fun (x: char) -> int x % 2 = 0), true
        "02468", (fun (x: char) -> int x % 2 = 1), false
        "13579", (fun (x: char) -> int x % 2 = 0), false
        "13579", (fun (x: char) -> int x % 2 = 1), true
    ]
    |> List.map (fun (source, predicate, expected) ->
        TestCaseData(source, predicate).Returns(expected))

[<TestCaseSource(nameof forallTestParameters)>]
let ``forall with various arguments`` source predicate =
    source |> Mem.ofString |> Mem.forall predicate

let forall2TestParameters =
    [
        "",     "23", None
        "1234", "23", Some 1
        "1234", "45", None
    ]
    |> List.map (fun (source, pattern, expected) ->
        TestCaseData(source, pattern).Returns(expected))

[<TestCaseSource(nameof forall2TestParameters)>]
let ``forall2 with various arguments`` source pattern =
    source
    |> Mem.ofString
    |> Mem.windowed 2
    |> Seq.tryFindIndex (fun t ->
        (t, Mem.ofString pattern)
        ||> Mem.forall2 (=))

[<Test>]
let ``forall2 with two empty sources returns true`` () =
    Assert.That((Mem.ofString "", Mem.ofString "")
                ||> Mem.forall2 (=))

[<Test>]
let ``forall2 with two sources of different lengths`` () =
    Assert.That((Mem.ofString "123", Mem.ofString "1234")
                ||> Mem.forall2 (=))
    Assert.That((Mem.ofArray [|1 .. 4|], Mem.ofArray [|1 .. 3|])
                ||> Mem.forall2 (=))
