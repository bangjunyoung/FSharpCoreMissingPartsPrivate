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

type SortOrder =
    | Ascending
    | Descending
    | StrictAscending
    | StrictDescending

module Seq =
    let iterate f x =
        let rec loop x = seq {
            yield x
            yield! loop (f x)
        }
        loop x

    let foldWhileSome folder state (source: seq<_>) =
        use en = source.GetEnumerator()

        let rec loop state =
            if en.MoveNext() then
                match folder state en.Current with
                | None -> state
                | Some state' -> loop state'
            else
                state

        loop state

    let isOrderedWith comparer source =
        if source |> Seq.truncate 2 |> Seq.length <= 1 then
            true
        else
            ((true, Seq.head source), Seq.tail source)
            ||> foldWhileSome
                (fun (sorted, prev) current ->
                    if sorted
                    then Some (comparer prev current <= 0, current)
                    else None)
            |> fst

    let isOrdered order source =
        let reverseCompare a b = compare b a
        let strictCompare a b =
            match sign <| compare a b with
            | -1 -> -1
            | _ -> 1
        let strictReverseCompare a b =
            match sign <| compare a b with
            | 1 -> -1
            | _ -> 1

        match order with
        | Ascending -> isOrderedWith compare source
        | Descending -> isOrderedWith reverseCompare source
        | StrictAscending -> isOrderedWith strictCompare source
        | StrictDescending -> isOrderedWith strictReverseCompare source
