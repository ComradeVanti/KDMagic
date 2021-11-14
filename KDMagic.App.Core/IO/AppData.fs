module KDMagic.App.AppData

open System
open System.IO

[<RequireQualifiedAccess>]
type AppDataError = FileNotFound of string

let private localAppDataPath =
    Environment.GetFolderPath Environment.SpecialFolder.LocalApplicationData

let private kDMagicDataPath = Path.Combine(localAppDataPath, "KDMagic")

let private appFolderExists () = Directory.Exists kDMagicDataPath

let private createAppFolder () =
    Directory.CreateDirectory kDMagicDataPath |> ignore

let private requireAppFolder () =
    if not <| (appFolderExists ()) then createAppFolder ()

let tryReadAppFile subPath =
    async {
        requireAppFolder ()
        let filePath = Path.Combine(kDMagicDataPath, subPath)

        if File.Exists filePath then
            let! content = File.ReadAllTextAsync filePath |> Async.AwaitTask
            return Ok content
        else
            return Error(AppDataError.FileNotFound filePath)
    }

let writeAppFile subPath content =
    requireAppFolder ()
    let filePath = Path.Combine(kDMagicDataPath, subPath)

    File.WriteAllTextAsync(filePath, content) |> Async.AwaitTask
