namespace KDMagic

open FsCheck
open FsCheck.Xunit
open KDMagic.CTTGen
open ParsingUtil

[<Properties(Arbitrary = [| typeof<ArbCTTs> |])>]
module CTTGenTests =

    let private fieldMatches fieldIndex predicate ctt =
        ctt
        |> CTT.tryGetField fieldIndex
        |> Option.map predicate
        |> Option.defaultValue false

    let private subfieldMatches index predicate ctt =
        ctt
        |> CTT.tryGetSubfield index
        |> Option.map predicate
        |> Option.defaultValue false


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
        ctt |> fieldMatches 0 (Field.content >> FilmTitle.isValid)

    [<Property>]
    let ``The content-type is in subfield (1,0) unless its a rating-tag``
        (ValidCTT ctt)
        =

        let contentTypeIsValid subfield =
            let content = subfield |> Subfield.content

            if content = "RTG" then
                true
            else
                content |> tryParseUnion<ContentType> |> Option.isSome

        ctt |> subfieldMatches (1, 0) contentTypeIsValid
       