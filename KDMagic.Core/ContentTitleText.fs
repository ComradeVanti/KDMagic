module KDMagic.ContentTitleText

let fields (text: string) = text.Split("_") |> Array.toList

let fieldCount text = text |> fields |> List.length

let subfields (field: string) = field.Split("-") |> Array.toList

let subfieldCount field = field |> subfields |> List.length

let tryGetField index text = text |> fields |> List.tryItem index

let getField index text = text |> tryGetField index |> Option.get

let tryGetSubfield (fieldIndex, subfieldIndex) text =
    text
    |> getField fieldIndex
    |> subfields
    |> List.tryItem subfieldIndex

let getSubfield (fieldIndex, subfieldIndex) text =
    text
    |> tryGetSubfield (fieldIndex, subfieldIndex)
    |> Option.get
