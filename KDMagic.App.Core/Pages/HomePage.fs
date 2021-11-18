[<RequireQualifiedAccess>]
module KDMagic.App.HomePage

open Elmish
open KDMagic.App

type State =
    | Loading
    | Loaded of Settings * FileListComp.State
    | LoadFailed of SettingsIO.LoadError

[<RequireQualifiedAccess>]
type Msg =
    | SettingsButtonPressed
    | FileListCompMsg of FileListComp.Msg
    | SettingsLoaded of Settings
    | SettingsLoadFailed of SettingsIO.LoadError

[<RequireQualifiedAccess>]
type Emit = | OpenSettings


let private loadCommand =
    Cmd.OfAsync.resultOp
        SettingsIO.tryLoad
        ()
        Msg.SettingsLoaded
        Msg.SettingsLoadFailed

let private wrapFileList wrapState output =
    let wrapEmit _ = failwith "Not handled!"

    output |> wrapChild wrapState Msg.FileListCompMsg wrapEmit


let initial = Loading, loadCommand, None

let update msg state =

    let noChanges = state, Cmd.none, None

    match state, msg with
    | _, Msg.SettingsButtonPressed -> state, Cmd.none, Some Emit.OpenSettings
    | State.Loaded (settings, fileListState), Msg.FileListCompMsg fileListMsg ->
        let wrapState state = Loaded(settings, state)

        let state, cmd =
            (FileListComp.update fileListMsg fileListState)
            |> wrapFileList wrapState

        state, cmd, None
    | State.Loading, Msg.SettingsLoaded settings ->
        let wrapState state = Loaded(settings, state)

        let state, cmd =
            (FileListComp.makeInitial settings)
            |> wrapFileList wrapState

        state, cmd, None

    | State.Loading, Msg.SettingsLoadFailed error ->
        State.LoadFailed error, Cmd.none, None
    | State.Loading, _ -> noChanges
    | State.Loaded _, _ -> noChanges
    | State.LoadFailed _, _ -> noChanges
