[<RequireQualifiedAccess>]
module KDMagic.App.SettingsIO

open KDMagic.App.AppData

[<RequireQualifiedAccess>]
type LoadError =
    | CouldNotRead of File.ReadError
    | CouldNotParse

let private settingsFileName = "settings.json"

let private defaultSettings = { ImportFolderPath = "" }

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
                | File.ReadError.NotFound _ -> Ok defaultSettings
                | _ -> Error(LoadError.CouldNotRead error)
    }

let save (settings: Settings) =
    let json = Json.stringify settings
    writeAppFile settingsFileName json
