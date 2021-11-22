namespace KDMagic

open FsCheck.Xunit
open KDMagic.CTTGen

[<Properties(Arbitrary = [| typeof<ArbCTTs> |])>]
module CTTGenTests =

    [<Property>]
    let ``Texts are made of 12 fields`` (ValidCTT ctt) =
        ctt |> CTT.fieldCount = 12

    [<Property>]
    let ``Texts contain no whitespace`` (ValidCTT ctt) =

        let containsWhitespace (s: string) =
            s.Contains ' '
            || s.Contains '\n'
            || s.Contains '\t'
            || s.Contains '\r'

        ctt |> CTT.content |> (not << containsWhitespace)

    [<Property>]
    let ``The first field is a valid film-name`` (ValidCTT ctt) =
        ctt
        |> (CTT.tryGetField 0 >> Option.get)
        |> (Field.content >> FilmTitle.isValid)
