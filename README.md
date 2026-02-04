# What is FSharpCoreMissingParts?

FSharpCoreMissingParts is a collection of various useful functions intended to fill missing parts of the F# Core library.

## Array

### `tryBinarySearch`

Performs a binary search within the specified array.  The array must be sorted, otherwise the result can be wrong.

```fsharp
> [|1 .. 100|] |> Array.tryBinarySearch 42;;
val it : int option = Some 41
```

### `tryBinarySearchWith`

Performs a binary search within the specified array with an external compare function.  The array must be sorted, otherwise the result can be wrong.

```fsharp
> let reverseCompare a b = compare b a
[|100 .. -1 .. 1|] |> Array.tryBinarySearchWith reverseCompare 42;;
val it : int option = Some 58
```

## Array2D

### `ofArray`

Create a 2D array from an 1D array.

```fsharp
> let arr2d = [|1 .. 6|] |> Array2D.ofArray 3 2;;
val arr2d : int [,] = [[1; 2]
                       [3; 4]
                       [5; 6]]
```

### `toArray`

```fsharp
> arr2d |> Array2D.toArray;;
val it : int [] = [|1; 2; 3; 4; 5; 6|]
```

## Casting

### `^>` operator

Type-safe cast operator.

```fsharp
let inline sqrtn n = (n |> float |> sqrt) ^> n

> sqrtn 25;;
val it : int = 5

> sqrtn 25I;;
val it : bigint = 5

> sqrtn 25.;;
val it : double = 5.0
```

## CircularList

Creates a circular singly linked list.

```fsharp
> let head = CircularList.ofList [1; 2; 3];;
val head : LinkNode<int>

> head |> CircularList.value;;
val it : int = 1

> head |> CircularList.next |> CircularList.value;;
val it : int = 2

> head |> CircularList.next |> CircularList.next |> CircularList.value;;
val it : int = 3

> head |> CircularList.next |> CircularList.next |> CircularList.next |> CircularList.value;;
val it : int = 1
```

### `value`

### `next`

## List

### `pairwiseCyclic`

```fsharp
> [1 .. 3] |> List.pairwiseCyclic;;
val it : (int * int) list = [(1, 2); (2, 3); (3, 1)]
```

### `crossMap`

Equivalent to `allPairs` + `map`.

## Mem

## NaturalSort

### `sort`

```fsharp
> ["z10"; "z"; "z1"; "z012"; "zz"; "z21"; "z2"] |> NaturalSort.sort;;
val it : ["z"; "z1"; "z2"; "z10"; "z012"; "z21"; "zz"]
```

## Nullable

## Seq

### `isOrdered`

```fsharp
> [1; 2; 2; 3] |> Seq.isOrdered Ascending;;
val it : bool = true

> [1; 2; 2; 3] |> Seq.isOrdered StrictAscending;;
val it : bool = false

> [3; 3; 2; 1] |> Seq.isOrdered Descending;;
val it : bool = true

> [3; 3; 2; 1] |> Seq.isOrdered StrictDescending;;
val it : bool = false
```

## String

### `ofSeq`

Creates a string from the specified `char` sequence.

```fsharp
> ['a' .. 'f'] |> String.ofSeq;;
val it : string = "abcdef"
```

### `ellipsize`

Shortens the specified string to the specified length. The last character is replaced with an ellipsis (U+2026).

```fsharp
> "abcde" |> String.ellipsize 3;;
val it : string = "ab…"
```
