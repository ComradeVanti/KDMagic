[<RequireQualifiedAccess>]
module KDMagic.FilmTitle

type T = private T of string

let isValid title =
    let lenght = title |> String.length
    lenght >= 1 && lenght <= 20

let make title = T title

let tryMake title = if title |> isValid then Some(make title) else None

let value (T title) = title