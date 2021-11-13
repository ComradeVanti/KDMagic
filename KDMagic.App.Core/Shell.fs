[<RequireQualifiedAccess>]
module KDMagic.App.Shell

open Elmish

[<RequireQualifiedAccess>]
type State =
    | Home of HomePage.State
    | Settings of SettingsPage.State

[<RequireQualifiedAccess>]
type Msg =
    | OpenSettings
    | CloseSettings
    | Home of HomePage.Msg
    | Settings of SettingsPage.Msg


let private wrapHome homeOutput =

    let wrapEmit =
        function
        | HomePage.Emit.OpenSettings -> Cmd.ofMsg Msg.OpenSettings

    homeOutput |> wrapChild State.Home Msg.Home wrapEmit

let private wrapSettings settingsOutput =

    let wrapEmit =
        function
        | SettingsPage.Emit.CloseSettings -> Cmd.ofMsg Msg.CloseSettings

    settingsOutput
    |> wrapChild State.Settings Msg.Settings wrapEmit


let openSettings () = SettingsPage.initial () |> wrapSettings


let initial = HomePage.initial |> wrapHome

let update msg state =
    match msg, state with
    | Msg.Home homeMsg, State.Home homeState ->
        homeState |> HomePage.update homeMsg |> wrapHome
    | Msg.Settings settingsMsg, State.Settings settingsState ->
        settingsState
        |> SettingsPage.update settingsMsg
        |> wrapSettings
    | Msg.OpenSettings, State.Home _ -> openSettings ()
    | Msg.Home _, _ -> state, Cmd.none
    | Msg.Settings _, _ -> state, Cmd.none
    | Msg.OpenSettings, _ -> state, Cmd.none
