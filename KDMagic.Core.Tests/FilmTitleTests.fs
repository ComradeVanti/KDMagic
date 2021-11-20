module KDMagic.FilmTitleTests

open FsCheck.Xunit

[<Property>]
let ``Film-titles must be between 1 and 20 characters`` title =
    let length = title |> String.length
    title |> FilmTitle.isValid = (length >= 1 && length <= 20)
