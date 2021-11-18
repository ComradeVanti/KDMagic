module KDMagic.App.AppData

open System
open System.IO

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
        return! File.tryRead filePath
    }

let writeAppFile subPath content =
    requireAppFolder ()
    let filePath = Path.Combine(kDMagicDataPath, subPath)

    File.WriteAllTextAsync(filePath, content) |> Async.AwaitTask
