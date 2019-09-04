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

[<StructuralEquality; StructuralComparison>]
[<Struct>]
type FSharpNullable<'T when 'T : struct> =
    | Value of 'T
    | Null

module FSharpNullable =
    let bind f x =
        match x with
        | Null -> Null
        | Value x' -> f x'

type FSharpNullableBuilder() =
    member __.Bind(x, f) = x |> FSharpNullable.bind f
    member __.Return(x) = Value x

[<AutoOpen>]
module FSharpNullableOps =
    let nullable = FSharpNullableBuilder()

    let inline ( .+. ) (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x + y
        }

    let inline ( .+ ) (a: FSharpNullable<'T>) (b: 'T) : FSharpNullable<'T> =
        nullable {
            let! x = a
            return x + b
        }

    let inline ( +. ) (a: 'T) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! y = b
            return a + y
        }

    let inline ( .-. ) (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x - y
        }

    let inline ( .- ) (a: FSharpNullable<'T>) (b: 'T) : FSharpNullable<'T> =
        nullable {
            let! x = a
            return x - b
        }

    let inline ( -. ) (a: 'T) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! y = b
            return a - y
        }

    let inline ( .*. ) (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x * y
        }

    let inline ( .* ) (a: FSharpNullable<'T>) (b: 'T) : FSharpNullable<'T> =
        nullable {
            let! x = a
            return x * b
        }

    let inline ( *. ) (a: 'T) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! y = b
            return a * y
        }

    let inline ( ./. ) (a: FSharpNullable<'T>) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! x = a
            let! y = b
            return x / y
        }

    let inline ( ./ ) (a: FSharpNullable<'T>) (b: 'T) : FSharpNullable<'T> =
        nullable {
            let! x = a
            return x / b
        }

    let inline ( /. ) (a: 'T) (b: FSharpNullable<'T>) : FSharpNullable<'T> =
        nullable {
            let! y = b
            return a / y
        }

type FSharpNullable<'T when 'T : struct> with
    member this.Bind(f) = this |> FSharpNullable.bind f

    member this.HasValue =
        match this with
        | Null -> false
        | Value _ -> true

    static member inline op_Explicit(this) : 'T =
        match this with
        | Null -> raise <| System.InvalidOperationException(
                               "Nullable object must have a value.")
        | Value x -> x

    static member inline ( <??> ) (lhs, rhs) =
        match lhs with
        | Null -> rhs
        | Value x -> x

    static member inline (+) (a, b) = a .+. b
    static member inline (-) (a, b) = a .-. b
    static member inline (*) (a, b) = a .*. b
    static member inline (/) (a, b) = a ./. b

type FSharpNullableAddition = FSharpNullableAddition with
    static member        (?<-) (FSharpNullableAddition, a, b) = a .+ b
    static member        (?<-) (FSharpNullableAddition, a, b) = a +. b
    static member inline (?<-) (FSharpNullableAddition, a, b) = a + b

type FSharpNullableSubtraction = FSharpNullableSubtraction with
    static member        (?<-) (FSharpNullableSubtraction, a, b) = a .- b
    static member        (?<-) (FSharpNullableSubtraction, a, b) = a -. b
    static member inline (?<-) (FSharpNullableSubtraction, a, b) = a - b

type FSharpNullableMultiply = FSharpNullableMultiply with
    static member        (?<-) (FSharpNullableMultiply, a, b) = a .* b
    static member        (?<-) (FSharpNullableMultiply, a, b) = a *. b
    static member inline (?<-) (FSharpNullableMultiply, a, b) = a * b

type FSharpNullableDivision = FSharpNullableDivision with
    static member        (?<-) (FSharpNullableDivision, a, b) = a ./ b
    static member        (?<-) (FSharpNullableDivision, a, b) = a /. b
    static member inline (?<-) (FSharpNullableDivision, a, b) = a / b

[<AutoOpen>]
module FSharpNullableExtension =
    let inline (+) a b = (?<-) FSharpNullableAddition a b
    let inline (-) a b = (?<-) FSharpNullableSubtraction a b
    let inline (*) a b = (?<-) FSharpNullableMultiply a b
    let inline (/) a b = (?<-) FSharpNullableDivision a b
