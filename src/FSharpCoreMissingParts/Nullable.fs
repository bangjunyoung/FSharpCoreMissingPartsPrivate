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

namespace FSharpCoreMissingParts.Nullable

[<Struct>]
type FSharpNullable<'T when 'T : struct> =
    | Value of 'T
    | Null

    member this.HasValue =
        match this with
        | Null -> false
        | Value _ -> true

    static member inline op_Explicit(this) : 'T =
        match this with
        | Null -> raise <| System.InvalidOperationException(
                               "Nullable object must have a value.")
        | Value x -> x

    static member inline (|??|) (lhs, rhs) =
        match lhs with
        | Null -> rhs
        | Value x -> x

module FSharpNullable =
    let bind f x =
        match x with
        | Null -> Null
        | Value x' -> f x'

type FSharpNullableBuilder() =
    member __.Bind(x, f) = x |> FSharpNullable.bind f
    member __.Return(x) = Value x

[<AutoOpen>]
module FSharpNullableComputationExpression =
    let nullable = FSharpNullableBuilder()

[<AutoOpen>]
module private FSharpNullableOps =
    let inline add (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x + y
        }

    let inline subtract (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x - y
        }

    let inline multiply (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x * y
        }

    let inline divide (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x / y
        }

type FSharpNullable<'T when 'T : struct> with
    static member inline ( + ) (lhs, rhs) = add lhs rhs
    static member inline (.+.) (lhs, rhs) = add lhs rhs
    static member inline ( +.) (lhs, rhs) = add (Value lhs) rhs
    static member inline (.+ ) (lhs, rhs) = add lhs (Value rhs)

    static member inline ( - ) (lhs, rhs) = subtract lhs rhs
    static member inline (.-.) (lhs, rhs) = subtract lhs rhs
    static member inline ( -.) (lhs, rhs) = subtract (Value lhs) rhs
    static member inline (.- ) (lhs, rhs) = subtract lhs (Value rhs)

    static member inline ( * ) (lhs, rhs) = multiply lhs rhs
    static member inline (.*.) (lhs, rhs) = multiply lhs rhs
    static member inline ( *.) (lhs, rhs) = multiply (Value lhs) rhs
    static member inline (.* ) (lhs, rhs) = multiply lhs (Value rhs)

    static member inline ( / ) (lhs, rhs) = divide lhs rhs
    static member inline (./.) (lhs, rhs) = divide lhs rhs
    static member inline ( /.) (lhs, rhs) = divide (Value lhs) rhs
    static member inline (./ ) (lhs, rhs) = divide lhs (Value rhs)
