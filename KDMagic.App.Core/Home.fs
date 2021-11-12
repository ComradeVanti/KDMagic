[<RequireQualifiedAccess>]
module KDMagic.App.Home

open Elmish

type State = unit

[<RequireQualifiedAccess>]
type Msg = unit


let initial = (), Cmd.none

let update msg state = state, Cmd.none
