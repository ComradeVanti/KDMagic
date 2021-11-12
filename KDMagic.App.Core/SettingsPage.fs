[<RequireQualifiedAccess>]
module KDMagic.App.SettingsPage

open Elmish

type State = unit

[<RequireQualifiedAccess>]
type Msg = unit

[<RequireQualifiedAccess>]
type Emit = unit


let initial = (), Cmd.none, None

let update msg state = state, Cmd.none, None
