[<RequireQualifiedAccess>]
module KDMagic.App.SettingsPage

open Elmish
open KDMagic.App.Settings

type State =
    | Unloaded
    | Loaded of Settings
    | LoadError of SettingsError

[<RequireQualifiedAccess>]
type Msg =
    | SettingsLoaded of Settings
    | LoadError of SettingsError
    | SettingsChanged of Settings

[<RequireQualifiedAccess>]
type Emit = unit


let initial () =

    let loadSettings =
        cmdOfAsyncResult tryLoad () Msg.SettingsLoaded Msg.LoadError

    Unloaded, loadSettings, None

let update msg state =
    match state, msg with
    | Unloaded, Msg.SettingsLoaded settings -> Loaded settings, Cmd.none, None
    | Unloaded, Msg.LoadError error -> LoadError error, Cmd.none, None
    | Loaded _, Msg.SettingsChanged settings -> Loaded settings, Cmd.none, None
    | _, Msg.SettingsLoaded _ -> state, Cmd.none, None
    | _, Msg.LoadError _ -> state, Cmd.none, None
    | _, Msg.SettingsChanged _ -> state, Cmd.none, None
