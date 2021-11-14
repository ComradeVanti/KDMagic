[<RequireQualifiedAccess>]
module KDMagic.App.KdmIO

open System.IO
open KDMagic
open KDMagic.IngestFile

[<RequireQualifiedAccess>]
type DirectoryError =
    | NoAccess
    | NotFound
    | InvalidPath

[<RequireQualifiedAccess>]
type LoadError =
    | SettingsLoad of SettingsIO.LoadError
    | Directory of DirectoryError

let private tryGetKdmPaths folderPath =
    try
        Ok(Directory.GetFiles folderPath |> Array.toList)
    with
    | :? System.UnauthorizedAccessException -> Error DirectoryError.NoAccess
    | :? DirectoryNotFoundException -> Error DirectoryError.NotFound
    | _ -> Error DirectoryError.InvalidPath

let private tryLoadKdmsFromDirectory folderPath =
    async {
        match tryGetKdmPaths folderPath with
        | Ok paths ->
            return!
                paths
                |> List.map tryIngestFromPath
                |> Async.Parallel
                |> Async.map (Array.toList >> Ok)
        | Error error -> return Error(LoadError.Directory error)
    }

let tryLoadKdms () =
    async {
        let! settingRes = SettingsIO.tryLoad ()

        match settingRes with
        | Ok settings -> return! tryLoadKdmsFromDirectory settings.KDMFolderPath
        | Error error -> return Error(LoadError.SettingsLoad error)
    }
