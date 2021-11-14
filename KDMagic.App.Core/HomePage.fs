[<RequireQualifiedAccess>]
module KDMagic.App.HomePage

open Elmish
open KDMagic.App
open KDMagic.IngestFile

type State =
    | Loading
    | Loaded of IngestResult list
    | LoadErrored of KdmIO.LoadError

[<RequireQualifiedAccess>]
type Msg =
    | SettingsButtonPressed
    | KdmsLoaded of IngestResult list
    | KdmLoadError of KdmIO.LoadError

[<RequireQualifiedAccess>]
type Emit = | OpenSettings


let private loadCommand =
    Cmd.OfAsync.resultOp KdmIO.tryLoadKdms () Msg.KdmsLoaded Msg.KdmLoadError

let initial = Loading, loadCommand, None

let update msg state =

    let noChanges = state, Cmd.none, None

    match state, msg with
    | _, Msg.SettingsButtonPressed -> state, Cmd.none, Some Emit.OpenSettings
    | Loading, Msg.KdmsLoaded results -> Loaded results, Cmd.none, None
    | Loading, Msg.KdmLoadError error -> LoadErrored error, Cmd.none, None
    | _, Msg.KdmsLoaded _ -> noChanges
    | _, Msg.KdmLoadError _ -> noChanges
