[<AutoOpen>]
module KDMagic.ResultUtil

type ResultBuilder() =

    member this.Bind(x, f) =
        match x with
        | Ok ok -> f ok
        | Error error -> Error error

    member this.Return(x) = Ok x

    member this.ReturnFrom(x) = x

let res = ResultBuilder()
