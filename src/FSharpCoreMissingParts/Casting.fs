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

[<AutoOpen>]
module Casting =
    type BigIntCast = BigIntCast with
        static member inline (=>) (BigIntCast, x: int) = bigint x
        static member inline (=>) (BigIntCast, x: uint32) = bigint x
        static member inline (=>) (BigIntCast, x: int64) = bigint x
        static member inline (=>) (BigIntCast, x: uint64) = bigint x
        static member inline (=>) (BigIntCast, x: bigint) = x
        static member inline (=>) (BigIntCast, x: single) = bigint x
        static member inline (=>) (BigIntCast, x: double) = bigint x
        static member inline (=>) (BigIntCast, x: decimal) = bigint x
        static member inline (=>) (BigIntCast, x: byte[]) = bigint x

    type NumericCast = NumericCast with
        static member inline (=>) (NumericCast, _: sbyte) = sbyte
        static member inline (=>) (NumericCast, _: byte) = byte
        static member inline (=>) (NumericCast, _: int16) = int16
        static member inline (=>) (NumericCast, _: uint16) = uint16
        static member inline (=>) (NumericCast, _: int) = int
        static member inline (=>) (NumericCast, _: uint32) = uint32
        static member inline (=>) (NumericCast, _: int64) = int64
        static member inline (=>) (NumericCast, _: uint64) = int64
        static member inline (=>) (NumericCast, _: nativeint) = nativeint
        static member inline (=>) (NumericCast, _: unativeint) = unativeint
        static member inline (=>) (NumericCast, _: single) = single
        static member inline (=>) (NumericCast, _: double) = double
        static member inline (=>) (NumericCast, _: decimal) = decimal
        static member inline (=>) (NumericCast, _: bigint) = (=>) BigIntCast

    let inline (^>) x t = (NumericCast => t) x
