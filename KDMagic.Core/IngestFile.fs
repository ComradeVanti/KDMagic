module KDMagic.IngestFile

open System.IO

type Path = string

type IngestError =
    | FileNotFound of Path
    | FileNotKDM

type IngestResult = Result<KDM, IngestError>

let tryIngest file =
    match KDMDoc.tryParse file with
    | Some doc -> Ok()
    | None -> Error FileNotKDM

let tryIngestFromPath path =
    async {
        if File.Exists path then
            let! file = File.ReadAllTextAsync path |> Async.AwaitTask
            return tryIngest file
        else
            return Error(FileNotFound path)
    }
