namespace KDMagic

open FsCheck.Xunit
open KDMagic.ContentTitleTextGen

[<Properties(Arbitrary = [| typeof<ArbValidContentTitleTexts> |])>]
module ContentTitleTextGenTests =

    let private fields (text: string) = text.Split("_") |> Array.toList

    let private subfields (field: string) = field.Split("-") |> Array.toList

    let private getField index text = text |> fields |> List.item index

    let private getSubfield (fieldIndex, subfieldIndex) text =
        text
        |> getField fieldIndex
        |> subfields
        |> List.item subfieldIndex

    [<Property>]
    let ``Texts are made of 12 fields`` (ValidContentTitleText text) =
        text |> fields |> List.length = 12

    [<Property>]
    let ``Texts contain no whitespace`` (ValidContentTitleText text) =
        not
        <| (text.Contains ' '
            || text.Contains '\n'
            || text.Contains '\t'
            || text.Contains '\r')

    [<Property>]
    let ``The first field is a valid film-name`` (ValidContentTitleText text) =
        text |> getField 0 |> FilmTitle.isValid

    [<Property>]
    let ``The second field has between 2 and 4 subfields``
        (ValidContentTitleText text)
        =
        let subFieldCount = text |> getField 1 |> subfields |> List.length
        subFieldCount >= 2 && subFieldCount <= 4
