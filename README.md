# FSharpCoreMissingParts

FSharpCoreMissingParts is a collection of various useful functions intended to fill missing parts of the F# Core library.

## Installation

You can install FSharpCoreMissingParts via NuGet:

```powershell
Install-Package FSharpCoreMissingParts
```

Or via .NET CLI:
```powershell
dotnet add package FSharpCoreMissingParts
```

## Usage

To use FSharpCoreMissingParts in your F# project:

```fsharp
open FSharpCoreMissingParts
```

If you want to use flexible numeric casting:

```fsharp
open FSharpCoreMissingParts.Casting
```

## Array

### `tryBinarySearch`

Performs a binary search within the specified array.  The array must be sorted, otherwise the result can be wrong.

```fsharp
> [|1 .. 100|] |> Array.tryBinarySearch 42;;
val it : int option = Some 41
```

### `tryBinarySearchWith`

Performs a binary search within the specified array using an external compare function.  The array must be sorted, otherwise the result can be wrong.

```fsharp
> let reverseCompare a b = compare b a
[|100 .. -1 .. 1|] |> Array.tryBinarySearchWith reverseCompare 42;;
val it : int option = Some 58
```

## Array2D

### `ofArray`

Create a 2D array from a 1D array by specifying dimensions (rows and columns).

```fsharp
> let nrows, ncolumns = 3, 2;;
val nrows: int = 3
val ncolumns: int = 2

> let arr2d = [|1 .. 6|] |> Array2D.ofArray nrows ncolumns;;
val arr2d : int [,] = [[1; 2]
                       [3; 4]
                       [5; 6]]
```

### `toArray`

Flattens a 2D array back into a 1D array.

```fsharp
> arr2d |> Array2D.toArray;;
val it : int [] = [|1; 2; 3; 4; 5; 6|]
```

## Casting

### `^>` operator

A type-safe cast operator that performs flexible numeric conversions.

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

Creates an immutable circular list where the last element points back to the first.

```fsharp
> let head = CircularList.ofList [1; 2];;
val head : CircularListNode<int>

> head |> CircularList.value;;
val it : int = 1

> head |> CircularList.next |> CircularList.value;;
val it : int = 2

> head |> CircularList.next |> CircularList.next |> CircularList.value;;
val it : int = 1
```

### `value`

Returns the actual data stored in the current node.

### `next`

Returns the next node in the list. Since the list is circular, calling `next` on the last element will seamlessly point back to the first element, allowing for infinite traversal without encountering `null` or `None`.

## List

### `pairwiseWrapped`

A wrap-around version of `List.pairwise` that pairs the last element with the first.

```fsharp
> [1] |> List.pairwiseWrapped;;
val it : (int * int) list = [(1, 1)]

> [1 .. 3] |> List.pairwiseWrapped;;
val it : (int * int) list = [(1, 2); (2, 3); (3, 1)]
```

### `crossMap`

Applies a mapper function to the Cartesian product of two lists.  Equivalent to `allPairs` + `map`.

```fsharp
> List.crossMap (+) [1; 2] [10; 20];;
val it: int list = [11; 21; 12; 22]
```

## Mem

A functional style wrapper for `ReadOnlyMemory<'T>`, allowing zero-allocation slicing and iteration.

### `ofArray`

Wraps an entire array into a `ReadOnlyMemory<'T>`. This provides a view of the array without copying its elements.

### `ofArraySlice`

Creates a `ReadOnlyMemory<'T>` from a specific range of an array. It takes a start index and a length, allowing you to work with a sub-section of the array efficiently.

### `ofString`

Converts a string into a `ReadOnlyMemory<char>`. This is useful for performing high-performance string operations without additional allocations.

### `ofStringSlice`

Creates a `ReadOnlyMemory<char>` from a sub-string based on the start index and length. Unlike `String.Substring`, this operation does not allocate a new string on the heap.

### `windowed`

Yields a sequence of overlapping slices (windows) of the specified size.

```fsharp
> "Hello" |> Mem.ofString |> Mem.windowed 3 |> Seq.map string
val it : seq<string> = seq ["Hel"; "ell"; "llo"]
```

### `forall`

Returns true if all elements satisfy the given predicate.

```fsharp
> let mem = Mem.ofArray [|1; 2; 3; 4; 5|];;
val mem : ReadOnlyMemory<int> = 1 2 3 4 5

> Mem.forall (fun x -> x > 0) mem;;
val it : bool = true
```

### `forall2`

Returns true if all elements satisfy the given predicate.  The predicate takes two arguments, one from each memory block.

```fsharp
> let mem = Mem.ofArray [|1; 2; 3; 4; 5|];;
val mem : ReadOnlyMemory<int> = 1 2 3 4 5

> let mem2 = Mem.ofArray [|5; 4; 3; 2; 1|];;
val mem2 : ReadOnlyMemory<int> = 5 4 3 2 1

> Mem.forall2 (fun x y -> x + y = 6) mem mem2;;
val it : bool = true
```

## NaturalSort

Provides "human-friendly" sorting that handles numbers within strings logically.

### `sort`

```fsharp
> ["z10"; "z"; "z1"; "z012"; "zz"; "z21"; "z2"] |> NaturalSort.sort;;
val it : string list = ["z"; "z1"; "z2"; "z10"; "z012"; "z21"; "zz"]
```

## Seq

### `iterate`

Generates an infinite sequence by repeatedly applying a function to an initial value. Each element is the result of applying the function to the previous element.

```fsharp
> Seq.iterate (fun x -> x * 2) 1 |> Seq.take 5 |> Seq.toList;;
val it : int list = [1; 2; 4; 8; 16]
```

### `foldWhileSome`

Similar to `Seq.fold`, but allows early termination. The folding process continues as long as the folder function returns `Some`. If it returns `None`, the sequence processing stops immediately and returns the last state.

```fsharp
> let folder state x =
    if x < 5
    then Some (state + x)
    else None;;
val folder : state:int -> x:int -> int option

> [1 .. 10] |> Seq.foldSome folder 0;;
val it : int = 10
```

### `isOrdered`

Checks if a sequence is sorted according to the specified `SortOrder`, which is one of `Ascending`, `Descending`, `StrictAscending`, or `StrictDescending`.

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

Shortens a string to the specified length. The last character is replaced with an ellipsis (U+2026).

```fsharp
> "abcde" |> String.ellipsize 3;;
val it : string = "ab…"
```

### `suffixes`

Generates all suffixes of the string, from longest to shortest.

```fsharp
> "bjy" |> String.suffixes;;
val it : string seq = seq ["bjy"; "jy"; "y"]
```
