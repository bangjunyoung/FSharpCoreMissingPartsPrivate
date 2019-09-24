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

module FSharpCoreMissingParts.MemoryTest

open System
open NUnit.Framework

[<Test>]
let ``windowed with empty source`` () =
    Assert.That("".AsMemory() |> Memory.windowed 1, Is.EqualTo Seq.empty)

[<TestCase("12345", 1, ExpectedResult = [|"1"; "2"; "3"; "4"; "5"|])>]
[<TestCase("12345", 3, ExpectedResult = [|"123"; "234"; "345"|])>]
let ``windowed with valid arguments`` (source: string) size =
    source.AsMemory()
    |> Memory.windowed size
    |> Seq.map string

[<TestCase("12345", 0)>]
let ``windowed with invalid arguments throws ArgumentException`` (source: string) size =
    Assert.That(Func<_>(fun () -> source.AsMemory() |> Memory.windowed size),
        Throws.TypeOf<ArgumentException>())
