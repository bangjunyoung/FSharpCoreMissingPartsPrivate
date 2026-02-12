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

namespace FSharpCoreMissingParts

open System

type Mem<'T> = ReadOnlyMemory<'T>

module Mem =
    let ofArray (source: 'T[]) = Mem source

    let ofArraySlice startIndex length (source: 'T[]) =
        Mem(source, startIndex, length)

    let ofString (source: string) : Mem<_> = source.AsMemory()

    let ofStringSlice startIndex length (source: string) : Mem<_> =
        source.AsMemory(startIndex, length)

    let windowed windowSize (source: Mem<'T>) : Mem<'T> seq =
        if windowSize <= 0 then
            invalidArg (nameof windowSize) $"must be positive, but {windowSize} was given."

        seq {
            for i in 0 .. source.Length - windowSize do
                yield source.Slice(i, windowSize)
        }

    let forall predicate (source: Mem<'T>) =
        let mutable result = true
        let mutable en = source.Span.GetEnumerator()

        while en.MoveNext() && result do
            result <- predicate en.Current

        result

    let forall2 predicate (source1: Mem<'T1>) (source2: Mem<'T2>) =
        let mutable result = true
        let mutable en1 = source1.Span.GetEnumerator()
        let mutable en2 = source2.Span.GetEnumerator()

        while en1.MoveNext() && en2.MoveNext() && result do
            result <- predicate en1.Current en2.Current

        result
