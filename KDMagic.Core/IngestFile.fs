module KDMagic.IngestFile

open System.IO

type Path = string

type IngestError = FileNotFound of Path

type IngestResult = Result<KDM, IngestError>

let tryIngest file = ()

let tryIngestFromPath path =
    async {
        if File.Exists path then
            let! file = File.ReadAllTextAsync path |> Async.AwaitTask
            return Ok(tryIngest file)
        else
            return Error(FileNotFound path)
    }
