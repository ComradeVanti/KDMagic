module KDMagic.App.Settings

open KDMagic.App.AppData

type Settings = { KDMFolderPath: string }

[<RequireQualifiedAccess>]
type SettingsLoadError = | CouldNotParse

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
                | None -> Error SettingsLoadError.CouldNotParse
            | Error error ->
                match error with
                | AppDataError.FileNotFound _ -> Ok defaultSettings
    }

let save (settings: Settings) =
    let json = Json.stringify settings
    writeAppFile settingsFileName json
