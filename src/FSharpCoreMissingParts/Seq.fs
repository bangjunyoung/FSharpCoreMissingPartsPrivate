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

    ///
    /// <summary>
    /// Generates an infinite sequence by repeatedly applying a function to an initial value.
    /// Each element is the result of applying the function to the previous element.
    /// </summary>
    ///
    /// <param name="f">The function to apply.</param>
    /// <param name="x">The initial value.</param>
    ///
    /// <returns>An infinite sequence of values.</returns>
    ///
    let iterate f x =
        let rec loop x = seq {
            yield x
            yield! loop (f x)
        }
        loop x

    ///
    /// <summary>
    /// Similar to <c>Seq.fold</c>, but allows early termination.
    /// </summary>
    ///
    /// <remarks>
    /// The folding process continues as long as the folder function returns <c>Some</c>.
    /// If it returns <c>None</c>, the sequence processing stops immediately and returns the last state.
    /// </remarks>
    ///
    /// <param name="folder">A function that takes the current state and an element, returning <c>Some newState</c> to continue or <c>None</c> to stop.</param>
    /// <param name="state">The initial state.</param>
    /// <param name="source">The input sequence to fold over.</param>
    ///
    /// <returns>The final state after folding or early termination.</returns>
    ///
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

    ///
    /// <summary>
    /// Checks if a sequence is sorted according to the provided comparer function.
    /// </summary>
    ///
    /// <param name="comparer">A comparison function that takes two elements and returns a negative number if the first is less than the second, zero if they are equal, and a positive number if the first is greater than the second.</param>
    /// <param name="source">The input sequence to check.</param>
    ///
    /// <returns><c>true</c> if the sequence is sorted according to the comparer; <c>false</c> otherwise.</returns>
    ///
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

    ///
    /// <summary>
    /// Checks if a sequence is sorted according to the specified <c>SortOrder</c>, which is
    /// one of <c>Ascending</c>, <c>Descending</c>, <c>StrictAscending</c>, or <c>StrictDescending</c>.
    /// </summary>
    ///
    /// <param name="order">The desired sort order.</param>
    /// <param name="source">The input sequence to check.</param>
    ///
    /// <returns><c>true</c> if the sequence is sorted according to the specified order;
    /// <c>false</c> otherwise.</returns>
    ///
    let isOrdered order source =
        let reverseCompare a b = compare b a
        let strictCompare a b =
            match sign (compare a b) with
            | -1 -> -1
            | _ -> 1
        let strictReverseCompare a b =
            match sign (compare a b) with
            | 1 -> -1
            | _ -> 1

        match order with
        | Ascending -> isOrderedWith compare source
        | Descending -> isOrderedWith reverseCompare source
        | StrictAscending -> isOrderedWith strictCompare source
        | StrictDescending -> isOrderedWith strictReverseCompare source
