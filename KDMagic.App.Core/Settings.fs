module KDMagic.App.Settings

open KDMagic.App.AppData

type Settings = { KDMFolderPath: string }

[<RequireQualifiedAccess>]
type SettingsError = | CouldNotParse

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
                | None -> Error SettingsError.CouldNotParse
            | Error error ->
                match error with
                | AppDataError.FileNotFound _ -> Ok defaultSettings
    }
