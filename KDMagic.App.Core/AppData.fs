module KDMagic.App.AppData

open System
open System.IO

type AppDataError = FileNotFound of string

let private localAppDataPath =
    Environment.GetFolderPath Environment.SpecialFolder.LocalApplicationData

let private kDMagicDataPath = Path.Combine(localAppDataPath, "KDMagic")

let private appFolderExists () = Directory.Exists kDMagicDataPath

let private createAppFolder () =
    Directory.CreateDirectory kDMagicDataPath |> ignore

let tryReadAppFile subPath =
    async {
        if not <| (appFolderExists ()) then createAppFolder ()
        let filePath = Path.Combine(kDMagicDataPath, subPath)

        if File.Exists filePath then
            let! content = File.ReadAllTextAsync filePath |> Async.AwaitTask
            return Ok content
        else
            return Error(FileNotFound filePath)
    }
