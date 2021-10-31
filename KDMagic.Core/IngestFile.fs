module KDMagic.IngestFile

open System.IO

type Path = string

type IngestError =
    | FileNotFound of Path
    | FileNotKDM
    | InvalidDigitalCinemaName of string

type IngestResult = Result<KDM, IngestError>


let private tryParseDigitalCinemaName (contentTitleText: string) =
    let fields = contentTitleText.Split '_'

    let tryGetField index = fields |> Array.tryItem index

    opt {
        let! filmName = tryGetField 0

        return { FilmTitle = filmName }
    }

let tryIngest file =
    match KDMDoc.tryParse file with
    | Some doc ->
        let contentTitleText =
            doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentTitleText

        res {
            let! digitalCinemaName =
                contentTitleText
                |> tryParseDigitalCinemaName
                |> asResult (InvalidDigitalCinemaName contentTitleText)

            return { DigitalCinemaName = digitalCinemaName }
        }
    | None -> Error FileNotKDM

let tryIngestFromPath path =
    async {
        if File.Exists path then
            let! file = File.ReadAllTextAsync path |> Async.AwaitTask
            return tryIngest file
        else
            return Error(FileNotFound path)
    }
