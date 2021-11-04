module KDMagic.IngestFile

open System.IO

type Path = string

type IngestError =
    | FileNotFound of Path
    | FileNotKDM
    | InvalidDigitalCinemaName of string

let private tryParseDigitalCinemaName (contentTitleText: string) =
    let fields = contentTitleText.Split '_'

    let tryGetField index = fields |> Array.tryItem index

    opt {
        let! filmName = tryGetField 0

        return { FilmTitle = FilmTitle.make filmName }
    }

let tryIngestFromFile kdmFile =
    Ok
        {
            ContentTitle = FilmTitle.value kdmFile.DigitalCinemaName.FilmTitle
        }

let tryIngestFromXml xml =
    match KDMDoc.tryParse xml with
    | Some doc ->
        let contentTitleText =
            doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentTitleText

        res {
            let! digitalCinemaName =
                contentTitleText
                |> tryParseDigitalCinemaName
                |> asResult (InvalidDigitalCinemaName contentTitleText)

            let file = { DigitalCinemaName = digitalCinemaName }
            return! tryIngestFromFile file
        }
    | None -> Error FileNotKDM

let tryIngestFromPath path =
    async {
        if File.Exists path then
            let! xml = File.ReadAllTextAsync path |> Async.AwaitTask
            return tryIngestFromXml xml
        else
            return Error(FileNotFound path)
    }
