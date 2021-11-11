[<RequireQualifiedAccess>]
module KDMagic.App.Shell

open Elmish

[<RequireQualifiedAccess>]
type State = Home of Home.State

[<RequireQualifiedAccess>]
type Msg = Home of Home.Msg


let private wrapHome (state, cmd) =
    (State.Home state), (cmd |> Cmd.map Msg.Home)

let initial = Home.initial |> wrapHome

let update msg state =
    match msg, state with
    | Msg.Home homeMsg, State.Home homeState ->
        homeState |> Home.update homeMsg |> wrapHome
