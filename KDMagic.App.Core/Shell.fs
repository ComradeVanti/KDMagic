[<RequireQualifiedAccess>]
module KDMagic.App.Shell

open Elmish

[<RequireQualifiedAccess>]
type State = Home of HomePage.State

[<RequireQualifiedAccess>]
type Msg =
    | OpenSettings
    | Home of HomePage.Msg


let private wrapHome (state, cmd, emit) =

    let wrapEmit =
        function
        | HomePage.Emit.OpenSettings -> Cmd.ofMsg Msg.OpenSettings

    let wrappedCmd = cmd |> Cmd.map Msg.Home |> addEmit emit wrapEmit

    (State.Home state), wrappedCmd

let initial = HomePage.initial |> wrapHome

let update msg state =
    match msg, state with
    | Msg.Home homeMsg, State.Home homeState ->
        homeState |> HomePage.update homeMsg |> wrapHome
    | Msg.OpenSettings, _ -> failwith "Settings-page not implemented"
