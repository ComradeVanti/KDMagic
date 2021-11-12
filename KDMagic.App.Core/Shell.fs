[<RequireQualifiedAccess>]
module KDMagic.App.Shell

open Elmish

[<RequireQualifiedAccess>]
type State = Home of HomePage.State

[<RequireQualifiedAccess>]
type Msg = Home of HomePage.Msg


let private wrapHome (state, cmd) =
    (State.Home state), (cmd |> Cmd.map Msg.Home)

let initial = HomePage.initial |> wrapHome

let update msg state =
    match msg, state with
    | Msg.Home homeMsg, State.Home homeState ->
        homeState |> HomePage.update homeMsg |> wrapHome
