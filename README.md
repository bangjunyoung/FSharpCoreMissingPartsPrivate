# FSharpCoreMissingParts

A library and collection of various useful functions intended to fill missing parts of the F# Core library.

## Casting

### `^>` operator

```fsharp
let inline sqrtn n = (n |> float |> sqrt) ^> n

> sqrtn 25;;
val it : int = 5

> sqrtn 25I;;
val it : bigint = 5

> sqrtn 25.;;
val it : double = 5.0
```

## Seq

### `foldSome`

### `isOrderedAscending`

```fsharp
> [1 .. 9] |> Seq.isOrderedAscending;;
val it : bool = true
```

### `isOrderedDescending`

```fsharp
> [3; 2; 1] |> Seq.isOrderedDescending;;
val it : bool = true
```

## List

### `pairwiseCyclic`

```fsharp
> [1 .. 3] |> List.pairwiseCyclic;;
val it : (int * int) list = [(1, 2); (2, 3); (3, 1)]
```

## Array

### `tryBinarySearch`

```fsharp
> [|1 .. 100|] |> Array.tryBinarySearch 42;;
val it : int option = Some 41
```

### `tryBinarySearchWith`

```fsharp
> let reverseCompare a b = compare b a
[|100 .. -1 .. 1|] |> Array.tryBinarySearchWith reverseCompare 42;;
val it : int option = Some 58
```

## Array2D

### `ofArray`

### `toArray`

## String

### `ofSeq`

```fsharp
> ['a' .. 'f'] |> String.ofSeq;;
val it : string = "abcdef"
```

## Cycle

### `value`

### `next`
