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

module List =

    ///
    /// <summary>
    /// A wrap-around version of <c>List.pairwise</c> that pairs the last element with the first.
    /// </summary>
    ///
    /// <param name="source">The input list.</param>
    ///
    /// <returns>A list of pairs where each pair consists of consecutive elements,
    /// with the last paired with the first.</returns>
    ///
    let pairwiseWrapped source =
        match source with
        | [] -> []
        | head :: _ ->
            let rec loop = function
                | [] -> []
                | [x] -> [x, head]
                | x :: y :: rest -> (x, y) :: loop (y :: rest)

            loop source

    ///
    /// <summary>
    /// Applies a mapper function to the Cartesian product of two lists.
    /// </summary>
    ///
    /// <remarks>
    /// This function generates all possible pairs of elements from the two input lists
    /// and applies the provided mapper function to each pair, collecting the results
    /// into a new list. Equivalent to <c>List.allPairs</c> followed by <c>List.map</c>.
    /// </remarks>
    ///
    /// <param name="mapper">A function that takes an element from the first list and an element from the second list, returning a mapped value.</param>
    /// <param name="source1">The first input list.</param>
    /// <param name="source2">The second input list.</param>
    ///
    /// <returns>A list containing the results of applying the mapper function
    /// to all combinations of elements from the two input lists.</returns>
    ///
    let crossMap mapper source1 source2 =
        source1 |> List.collect (fun x ->
            source2 |> List.map (fun y ->
                mapper x y))
