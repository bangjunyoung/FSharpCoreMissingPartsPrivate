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
open System.Text

module String =

    ///
    /// <summary>
    /// Creates a string from a sequence of characters.
    /// </summary>
    ///
    /// <param name="source">The input sequence of characters.</param>
    ///
    /// <returns>The resulting string.</returns>
    ///
    let ofSeq (source: seq<char>) =
        (StringBuilder(Seq.length source), source)
        ||> Seq.fold (fun builder c -> builder.Append(c))
        |> string

    ///
    /// <summary>
    /// Truncates the string to the specified length, and replace the last character
    /// with an ellipsis (U+2026) if truncation occurs.
    /// </summary>
    ///
    /// <param name="length">The maximum length of the resulting string.</param>
    /// <param name="source">The input string to be truncated.</param>
    ///
    /// <returns>The truncated string (with an ellipsis if truncation occurs).</returns>
    ///
    let ellipsize length source =
        if length < 0 then
            invalidArg (nameof length) $"{nameof length} must be positive, but {length} is given."

        if String.length source > length then
            let ellipsis = "\u2026"
            if length >= ellipsis.Length then
                StringBuilder(length)
                    .Append(source.AsSpan(0, length - ellipsis.Length))
                    .Append(ellipsis)
                    .ToString()
            else
                source.Substring(0, length)
        else
            source

    ///
    /// <summary>
    /// Generates all suffixes of the string, from longest to shortest.
    /// </summary>
    ///
    /// <param name="str">The input string to extract suffixes from.</param>
    ///
    /// <returns>A sequence of suffixes.</returns>
    ///
    let suffixes str =
        str
        |> Seq.unfold (fun s ->
            match String.length s with
            | 0 -> None
            | _ -> Some (s, s.Substring 1))
