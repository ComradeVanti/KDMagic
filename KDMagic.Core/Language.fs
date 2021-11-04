[<RequireQualifiedAccess>]
module KDMagic.Language

type T = private T of string

let isValid language =
    let lenght = language |> String.length
    lenght >= 2 && lenght <= 3

let make language = T language

let tryMake language = if language |> isValid then Some(make language) else None

let value (T language) = language
