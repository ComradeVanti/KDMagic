[<RequireQualifiedAccess>]
module KDMagic.App.SettingsIO

open KDMagic.App.AppData

[<RequireQualifiedAccess>]
type LoadError = | CouldNotParse

let private settingsFileName = "settings.json"

let private defaultSettings = { KDMFolderPath = "" }

let tryLoad () =
    async {
        let! res = tryReadAppFile settingsFileName

        return
            match res with
            | Ok content ->
                match content |> Json.tryParse<Settings> with
                | Some settings -> Ok settings
                | None -> Error LoadError.CouldNotParse
            | Error error ->
                match error with
                | AppDataError.FileNotFound _ -> Ok defaultSettings
    }

let save (settings: Settings) =
    let json = Json.stringify settings
    writeAppFile settingsFileName json
