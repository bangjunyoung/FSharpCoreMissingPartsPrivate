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
        TestCaseData(source, windowSize).Returns(expected).SetName($"windowed {windowSize} \"{source}\""))

[<TestCaseSource(nameof windowedTestParamters)>]
let windowedTest (source: string) size =
    source
    |> Mem.ofString
    |> Mem.windowed size
    |> Seq.map string

[<Test>]
let ``windowed throws ArgumentException if windowSize <= 0`` () =
    Assert.That(Func<_>(fun () -> "123".AsMemory() |> Mem.windowed 0),
        Throws.TypeOf<ArgumentException>())

let forallTestParameters =
    [
        let isEven n = int n % 2 = 0
        let isOdd n = int n % 2 <> 0

        "",      isEven, nameof isEven, true
        "02468", isEven, nameof isEven, true
        "02468", isOdd, nameof isOdd, false
        "13579", isEven, nameof isEven, false
        "13579", isOdd, nameof isOdd, true
    ]
    |> List.map (fun (source, (predicate: char -> bool), predicateName, expected) ->
        TestCaseData(source, predicate).Returns(expected).SetName($"forall {predicateName} \"{source}\""))

[<TestCaseSource(nameof forallTestParameters)>]
let forallTest source predicate =
    source |> Mem.ofString |> Mem.forall predicate

let forall2TestParameters =
    [
        "", "", true
        "abc", "abc", true
        "123", "1234", true
        "abc", "abd", false
        "abc", "xyz", false
    ]
    |> List.map (fun (str1, str2, expected) ->
        TestCaseData(str1, str2).Returns(expected).SetName($"forall2 (=) \"%s{str1}\" \"%s{str2}\""))

[<TestCaseSource(nameof forall2TestParameters)>]
let forall2Test str1 str2 =
    let m1 = Mem.ofString str1
    let m2 = Mem.ofString str2

    (m1, m2) ||> Mem.forall2 (=)

let forall2Test2Parameters2 =
    [
        [||], [||], true
        [|1; 2; 3|], [|1; 2; 3|], true
        [|1; 2; 3|], [|1; 2; 3; 4|], true
        [|1; 2; 3|], [|1; 2; 4|], false
        [|1; 2; 3|], [|3; 2; 1|], false
    ]
    |> List.map (fun (arr1, arr2, expected) ->
        TestCaseData(arr1, arr2).Returns(expected).SetName($"""forall2 (=) %A{arr1} %A{arr2}"""))

[<TestCaseSource(nameof forall2Test2Parameters2)>]
let forall2Test2 arr1 arr2 =
    let m1 = Mem.ofArray arr1
    let m2 = Mem.ofArray arr2

    (m1, m2) ||> Mem.forall2 (=)
