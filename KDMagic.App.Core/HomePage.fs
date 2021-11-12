[<RequireQualifiedAccess>]
module KDMagic.App.HomePage

open Elmish

type State = unit

[<RequireQualifiedAccess>]
type Msg = unit


let initial = (), Cmd.none

let update msg state = state, Cmd.none
