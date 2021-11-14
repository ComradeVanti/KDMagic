[<RequireQualifiedAccess>]
module KDMagic.Option

let asResult error opt =
    match opt with
    | Some value -> Ok value
    | None -> Error error
