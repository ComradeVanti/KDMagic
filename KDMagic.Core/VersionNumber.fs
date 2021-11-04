[<RequireQualifiedAccess>]
module KDMagic.VersionNumber

type T = private T of int

let isValid number = number >= 1

let make number = T number

let tryMake number = if number |> isValid then Some(make number) else None

let value (T number) = number
