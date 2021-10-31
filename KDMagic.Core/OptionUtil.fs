[<AutoOpen>]
module KDMagic.OptionUtil

type OptionBuilder() =

    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = Some x

let opt = OptionBuilder()

let asResult error opt =
    match opt with
    | Some value -> Ok value
    | None -> Error error
