[<RequireQualifiedAccess>]
module KDMagic.App.Directory

open System.IO

[<RequireQualifiedAccess>]
type OpenError =
    | NoAccess
    | NotFound
    | InvalidPath

let tryGetFilePaths directory =
    try
        Ok(Directory.GetFiles directory |> Array.toList)
    with
    | :? System.UnauthorizedAccessException -> Error OpenError.NoAccess
    | :? DirectoryNotFoundException -> Error OpenError.NotFound
    | _ -> Error OpenError.InvalidPath
