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
    |> List.map (fun (source, windowSize, expected) ->
        TestCaseData(source, windowSize).Returns(expected)
            .SetName($"windowed {windowSize} \"{source}\" returns %A{expected}"))

[<TestCaseSource(nameof windowedTestParamters)>]
let ``windowed with valid arguments`` (source: string) size =
    source
    |> Mem.ofString
    |> Mem.windowed size
    |> Seq.map string

[<Test>]
let ``windowed throws ArgumentException if windowSize <= 0`` () =
    Assert.That(Func<_>(fun () -> "123".AsMemory() |> Mem.windowed 0),
        Throws.TypeOf<ArgumentException>())

let isEven = fun n -> int n % 2 = 0
let isOdd = fun n -> int n % 2 <> 0
let forallTestParameters =
    [
        "",      isEven, nameof isEven, true
        "02468", isEven, nameof isEven, true
        "02468", isOdd, nameof isOdd, false
        "13579", isEven, nameof isEven, false
        "13579", isOdd, nameof isOdd, true
    ]
    |> List.map (fun (source, (predicate: char -> bool), predicateName, expected) ->
        TestCaseData(source, predicate).Returns(expected)
            .SetName($"forall {predicateName} \"{source}\" returns {expected}"))

[<TestCaseSource(nameof forallTestParameters)>]
let ``forall with valid arguments`` source predicate =
    source |> Mem.ofString |> Mem.forall predicate

let forall2TestParameters =
    [
        "abc", "abc", true
        "abc", "abd", false
        "abc", "xyz", false
    ]
    |> List.map (fun (s1, s2, expected) ->
        TestCaseData(s1, s2).Returns(expected)
            .SetName($"""forall2 (=) "%s{s1}" "%s{s2}" returns {expected}"""))

[<TestCaseSource(nameof forall2TestParameters)>]
let ``forall2 with valid arguments`` s1 s2 =
    let m1 = Mem.ofString s1
    let m2 = Mem.ofString s2
    
    (m1, m2) ||> Mem.forall2 (=)

[<Test>]
let ``forall2 with two empty sources returns True`` () =
    Assert.That((Mem.ofString "", Mem.ofString "")
                ||> Mem.forall2 (=))

[<Test>]
let ``forall2 with two sources of different lengths`` () =
    Assert.That((Mem.ofString "123", Mem.ofString "1234")
                ||> Mem.forall2 (=))
    Assert.That((Mem.ofArray [|1 .. 4|], Mem.ofArray [|1 .. 3|])
                ||> Mem.forall2 (=))
