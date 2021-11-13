[<RequireQualifiedAccess>]
module KDMagic.App.Json

open System
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

let tryParse<'a> text =

    let mutable hadError = false

    let errorHandler = EventHandler<ErrorEventArgs>(fun _ _ -> hadError <- true)

    let settings = JsonSerializerSettings(Error = errorHandler)

    let parsed = JsonConvert.DeserializeObject<'a>(text, settings)

    if hadError then Some parsed else None

let stringify item =
    JsonConvert.SerializeObject item