[<RequireQualifiedAccess>]
module KDMagic.App.File

open System
open System.IO
open System.Security
open KDMagic

[<RequireQualifiedAccess>]
type ReadError =
    | NotFound of string
    | InvalidPath of string
    | NoAccess of string
    | Unknown of exn

let tryRead path =
    async {
        try
            let! content = File.ReadAllTextAsync path |> Async.AwaitTask
            return Ok content
        with
        | :? ArgumentNullException -> return Error(ReadError.InvalidPath path)
        | :? PathTooLongException -> return Error(ReadError.InvalidPath path)
        | :? DirectoryNotFoundException -> return Error(ReadError.NotFound path)
        | :? UnauthorizedAccessException ->
            return Error(ReadError.NoAccess path)
        | :? FileNotFoundException -> return Error(ReadError.NotFound path)
        | :? NotSupportedException -> return Error(ReadError.InvalidPath path)
        | :? SecurityException -> return Error(ReadError.NoAccess path)
        | e -> return Error(ReadError.Unknown e)
    }
