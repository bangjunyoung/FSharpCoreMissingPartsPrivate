namespace FSharpCorePlus

module Array2D =
    let toArray (source: 'T[,]) =
        source
        |> Seq.cast<'T>
        |> Seq.toArray

    let ofArray nrows ncols source =
        if Array.length source <> nrows * ncols then
            invalidArg "source" "must have a length of nrows multiplied by ncols"

        let array2D = Array2D.zeroCreate nrows ncols
        source
        |> Array.iteri (fun i elem -> array2D.[i / ncols, i % ncols] <- elem)

        array2D
