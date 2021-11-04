namespace KDMagic

open FsCheck.Xunit
open KDMagic.FilmTitleGen

[<Properties(Arbitrary = [| typeof<ArbFilmTitles> |])>]
module FilmTitleGenTests =

    [<Property>]
    let ``Generated film-titles are valid`` (ValidFilmTitle title) =
        title |> FilmTitle.value |> FilmTitle.isValid
