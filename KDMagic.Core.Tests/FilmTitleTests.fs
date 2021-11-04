module KDMagic.FilmTitleTests

open FsCheck.Xunit

[<Property>]
let ``Film-titles must be between 1 and 14 characters`` title =
    let length = title |> String.length
    title |> FilmTitle.isValid = (length >= 1 && length <= 14)
