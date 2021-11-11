module KDMagic.App.Settings

open KDMagic.App.AppData

type Settings = { KDMFolderPath: string }

type SettingsError =
    | IO of AppDataError
    | CouldNotParse

let private settingsFileName = "settings.json"

let tryLoad () =
    async {
        let! res = tryReadAppFile settingsFileName

        return
            match res with
            | Ok content ->
                match content |> Json.tryParse<Settings> with
                | Some settings -> Ok settings
                | None -> Error CouldNotParse
            | Error error -> Error(IO error)
    }
