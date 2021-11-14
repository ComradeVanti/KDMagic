[<AutoOpen>]
module KDMagic.OptionBuilder

type OptionBuilder() =

    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = Some x

    member this.ReturnFrom(x) = x

let opt = OptionBuilder()
