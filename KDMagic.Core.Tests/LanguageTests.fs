module KDMagic.LanguageTests

open FsCheck.Xunit

[<Property>]
let ``Languages must be between 2 and 3 characters`` language =
    let length = language |> String.length
    language |> Language.isValid = (length >= 2 && length <= 3)
