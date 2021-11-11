[<RequireQualifiedAccess>]
module KDMagic.App.Shell

open Elmish

type State = unit

[<RequireQualifiedAccess>]
type Msg = unit


let initial = (), Cmd.none

let update msg state = state, Cmd.none
