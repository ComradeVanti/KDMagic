[<RequireQualifiedAccess>]
module KDMagic.App.SettingsPage

open Elmish
open KDMagic.App.Settings

type State =
    | Unloaded
    | Loaded of Settings
    | LoadError of SettingsLoadError

[<RequireQualifiedAccess>]
type Msg =
    | SettingsLoaded of Settings
    | LoadError of SettingsLoadError
    | SettingsChanged of Settings
    | Save
    | SettingsSaved
    | Exit

[<RequireQualifiedAccess>]
type Emit = | CloseSettings

let private loadCommand =
    cmdOfAsyncResult tryLoad () Msg.SettingsLoaded Msg.LoadError

let private makeSaveCommand settings =
    Cmd.OfAsync.perform save settings (fun _ -> Msg.SettingsSaved)

let initial () = Unloaded, loadCommand, None

let update msg state =

    let closePage = state, Cmd.none, Some Emit.CloseSettings
    
    let noChanges = state, Cmd.none, None

    match state, msg with
    | Unloaded, Msg.SettingsLoaded settings -> Loaded settings, Cmd.none, None
    | Unloaded, Msg.LoadError error -> LoadError error, Cmd.none, None
    | Loaded _, Msg.SettingsChanged settings -> Loaded settings, Cmd.none, None
    | Loaded settings, Msg.Save -> state, makeSaveCommand settings, None
    | Loaded _, Msg.SettingsSaved -> closePage
    | _, Msg.Exit -> closePage
    | _, Msg.SettingsLoaded _ -> noChanges
    | _, Msg.LoadError _ -> noChanges
    | _, Msg.SettingsChanged _ -> noChanges
    | _, Msg.Save -> noChanges
    | _, Msg.SettingsSaved -> noChanges
