namespace KDMagic

open System

type Subfield = SubfieldContent of string

type Field = Subfields of Subfield list

type CTT = Fields of Field list


[<RequireQualifiedAccess>]
module Subfield =

    let content (SubfieldContent content) = content

    let tryMatchTag (tags: (string * _) list) subfield =
        subfield
        |> content
        |> (fun c -> tags |> List.filter (fst >> c.Contains))
        |> List.map snd
        |> List.tryHead

[<RequireQualifiedAccess>]
module Field =

    let subfields (Subfields subfields) = subfields

    let tryGetSubfield index field = field |> subfields |> List.tryItem index

    let content field =
        String.Join("-", field |> subfields |> List.map Subfield.content)

    let contains (text: string) field = (field |> content).Contains text

    let tryFindSubfield matcher field =
        field |> subfields |> List.tryFind matcher

    let tryMatchTag (tags: (string * _) list) field =
        field
        |> content
        |> (fun c -> tags |> List.filter (fst >> c.Contains))
        |> List.map snd
        |> List.tryHead


[<RequireQualifiedAccess>]
module CTT =

    let make s =

        let splitIntoSubfields (field: string) =
            field.Split "-" |> Array.toList |> List.map SubfieldContent

        let splitIntoFields (ctt: string) =
            ctt.Split "_"
            |> Array.toList
            |> List.map (splitIntoSubfields >> Subfields)

        s |> splitIntoFields |> Fields

    let fields (Fields fields) = fields

    let fieldCount ctt = ctt |> fields |> List.length

    let tryGetField index ctt = ctt |> fields |> List.tryItem index

    let tryGetSubfield (fieldIndex, subfieldIndex) ctt =
        ctt
        |> tryGetField fieldIndex
        |> Option.bind (Field.tryGetSubfield subfieldIndex)

    let content ctt =
        String.Join("_", ctt |> fields |> List.map (Field.content))
