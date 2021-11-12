[<RequireQualifiedAccess>]
module KDMagic.App.HomePage

open Elmish

type State = unit

[<RequireQualifiedAccess>]
type Msg = | SettingsButtonPressed

[<RequireQualifiedAccess>]
type Emit = | OpenSettings


let initial = (), Cmd.none, None

let update msg (state: State) =
    match msg with
    | Msg.SettingsButtonPressed -> state, Cmd.none, Some Emit.OpenSettings
