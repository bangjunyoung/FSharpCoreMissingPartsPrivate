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

module Array2D =

    ///
    /// <summary>Converts a 2D array to a 1D array.</summary>
    ///
    /// <param name="source">The input 2D array.</param>
    ///
    /// <returns>A 1D array containing all elements of the 2D array in row-major order.</returns>
    ///
    let toArray (source: 'T[,]) =
        source
        |> Seq.cast<'T>
        |> Seq.toArray

    ///
    /// <summary>Creates a 2D array from a 1D array.</summary>
    ///
    /// <param name="nrows">The number of rows in the resulting 2D array.</param>
    /// <param name="ncols">The number of columns in the resulting 2D array.</param>
    /// <param name="source">The input 1D array.</param>
    ///
    /// <returns>A 2D array with the specified dimensions containing elements from the 1D array in row-major order.</returns>
    ///
    let ofArray nrows ncols source =
        if Array.length source <> nrows * ncols then
            invalidArg (nameof source) "must have a length of nrows multiplied by ncols"

        let array2D = Array2D.zeroCreate nrows ncols
        source
        |> Array.iteri (fun i elem -> array2D.[i / ncols, i % ncols] <- elem)

        array2D
