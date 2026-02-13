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

module Array =

    ///
    /// <summary>Performs a binary search within the specified array using an external comparer function.</summary>
    ///
    /// <param name="comparer">A function that compares two values of the array's element type.</param>
    /// <param name="value">The value to locate.</param>
    ///
    /// <returns>The index of the value if found; otherwise, <c>None</c>.</returns>
    ///
    let tryBinarySearchWith comparer (value: 'a) (source: 'a[]) =
        let rec loop lo hi =
            if lo > hi then None
            else
                let mid = lo + (hi - lo) / 2
                match sign <| comparer value source.[mid] with
                | 0 -> Some mid
                | 1 -> loop (mid + 1) hi
                | _ -> loop lo (mid - 1)

        loop 0 (source.Length - 1)

    ///
    /// <summary>Performs a binary search within the specified array.</summary>
    ///
    /// <param name="value">The value to locate.</param>
    ///
    /// <returns>The index of the value if found; otherwise, <c>None</c>.</returns>
    ///
    let tryBinarySearch value source =
        source |> tryBinarySearchWith compare value
