module KDMagic.ContentTitleText

let fields (text: string) = text.Split("_") |> Array.toList

let fieldCount text = text |> fields |> List.length

let subfields (field: string) = field.Split("-") |> Array.toList

let subfieldCount field = field |> subfields |> List.length

let getField index text = text |> fields |> List.item index

let getSubfield (fieldIndex, subfieldIndex) text =
    text
    |> getField fieldIndex
    |> subfields
    |> List.item subfieldIndex
