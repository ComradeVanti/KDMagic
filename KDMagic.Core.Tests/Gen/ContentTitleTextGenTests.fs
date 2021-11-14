namespace KDMagic

open FsCheck
open FsCheck.Xunit
open KDMagic.ContentTitleTextGen
open KDMagic.ContentTitleText

[<Properties(Arbitrary = [| typeof<ArbContentTitleTexts> |])>]
module ContentTitleTextGenTests =

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

    [<Property>]
    let ``Fields are seperated by underscores`` (ValidContentTitleText text) =
        let fieldCount = text |> fieldCount
        let underscoreCount = text |> Seq.countItem '_'
        fieldCount = underscoreCount + 1

    [<Property>]
    let ``Sub-fields are seperated by hyphens`` (ValidContentTitleText text) =

        let hasCorrectSubfieldCount field =
            let subfieldCount = field |> subfieldCount
            let hyphenCount = field |> Seq.countItem '-'
            subfieldCount = hyphenCount + 1

        let fields = text |> fields |> Gen.elements |> Arb.fromGen
        Prop.forAll fields hasCorrectSubfieldCount
