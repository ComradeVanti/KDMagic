module KDMagic.App.ImportFiles

open KDMagic
open KDMagic.App
open KDMagic.ParseKdm


[<RequireQualifiedAccess>]
type ImportError = Directory of Directory.OpenError

[<RequireQualifiedAccess>]
type FileImportError =
    | InvalidKdm of KdmParseError
    | ReadError of File.ReadError

type FileImportResult = Result<ImportedFile, FileImportError>

type ImportResult = Result<FileImportResult list, ImportError>


let private validKdmFile path kdm : ImportedFile =
    path, FileContent.ValidKdm kdm

let private otherFile path : ImportedFile = path, FileContent.Other

let private tryIngestFromPath path : Async<FileImportResult> =
    async {
        return!
            File.tryRead path
            |> Async.map
                (function
                | Ok content ->
                    match tryParseXml content with
                    | Ok kdm -> Ok(validKdmFile path kdm)
                    | Error parseError ->
                        match parseError with
                        | KdmParseError.FileNotKDM -> Ok(otherFile path)
                        | error -> Error(FileImportError.InvalidKdm error)
                | Error error -> Error(FileImportError.ReadError error))
    }

let importAllFromPath importDirectory : Async<ImportResult> =
    async {
        match Directory.tryGetFilePaths importDirectory with
        | Ok paths ->
            return!
                paths
                |> List.map tryIngestFromPath
                |> Async.merge
                |> Async.map Ok
        | Error error -> return Error(ImportError.Directory error)
    }
