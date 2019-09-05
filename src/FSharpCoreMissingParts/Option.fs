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

namespace FSharpCoreMissingParts.Option

type OptionBuilder() =
    member __.Bind(x, f) = x |> Option.bind f
    member __.Return(x) = Some x

[<AutoOpen>]
module OptionOps =
    let optional = OptionBuilder()

    let inline ( <??> ) lhs rhs =
        match lhs with
        | None -> rhs
        | Some x -> x

    let inline ( .+. ) (a: Option<'T>) (b: Option<'T>) : Option<'T> =
        optional {
            let! x = a
            let! y = b
            return x + y
        }

    let inline ( .+ ) (a: Option<'T>) (b: 'T) : Option<'T> =
        optional {
            let! x = a
            return x + b
        }

    let inline ( +. ) (a: 'T) (b: Option<'T>) : Option<'T> =
        optional {
            let! y = b
            return a + y
        }

    let inline ( .-. ) (a: Option<'T>) (b: Option<'T>) : Option<'T> =
        optional {
            let! x = a
            let! y = b
            return x - y
        }

    let inline ( .- ) (a: Option<'T>) (b: 'T) : Option<'T> =
        optional {
            let! x = a
            return x - b
        }

    let inline ( -. ) (a: 'T) (b: Option<'T>) : Option<'T> =
        optional {
            let! y = b
            return a - y
        }

    let inline ( .*. ) (a: Option<'T>) (b: Option<'T>) : Option<'T> =
        optional {
            let! x = a
            let! y = b
            return x * y
        }

    let inline ( .* ) (a: Option<'T>) (b: 'T) : Option<'T> =
        optional {
            let! x = a
            return x * b
        }

    let inline ( *. ) (a: 'T) (b: Option<'T>) : Option<'T> =
        optional {
            let! y = b
            return a * y
        }

    let inline ( ./. ) (a: Option<'T>) (b: Option<'T>) : Option<'T> =
        optional {
            let! x = a
            let! y = b
            return x / y
        }

    let inline ( ./ ) (a: Option<'T>) (b: 'T) : Option<'T> =
        optional {
            let! x = a
            return x / b
        }

    let inline ( /. ) (a: 'T) (b: Option<'T>) : Option<'T> =
        optional {
            let! y = b
            return a / y
        }
