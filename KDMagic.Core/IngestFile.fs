module KDMagic.IngestFile

open System
open System.IO

type Path = string

type IngestError =
    | FileNotFound of Path
    | FileNotKDM
    | InvalidDigitalCinemaName of string
    | InvalidNotValidBefore of string
    | InvalidNotValidAfter of string

type IngestResult = Result<KDM, IngestError>

let private tryParseDateTime s =
    try
        s |> DateTime.Parse |> Some
    with
    | :? FormatException -> None

let tryIngestFromXml xml : IngestResult =
    match KDMDoc.tryParse xml with
    | Ok doc ->
        res {
            let contentTitleText =
                doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentTitleText

            let notValidBefore =
                doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentKeysNotValidBefore

            let notValidAfter =
                doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentKeysNotValidAfter

            let! digitalCinemaName =
                contentTitleText
                |> DigitalCinemaName.tryParse
                |> Option.asResult (InvalidDigitalCinemaName contentTitleText)

            let! validFrom =
                tryParseDateTime notValidBefore
                |> Option.asResult (InvalidNotValidBefore notValidBefore)

            let! validUntil =
                tryParseDateTime notValidAfter
                |> Option.asResult (InvalidNotValidAfter notValidAfter)

            return
                { ContentInfo = digitalCinemaName
                  ValidFrom = validFrom
                  ValidUntil = validUntil }
        }
    | Error _ -> Error FileNotKDM

let tryIngestFromPath path =
    async {
        if File.Exists path then
            let! xml = File.ReadAllTextAsync path |> Async.AwaitTask
            return tryIngestFromXml xml
        else
            return Error(FileNotFound path)
    }
