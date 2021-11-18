[<RequireQualifiedAccess>]
module KDMagic.App.FileListComp

open Elmish
open KDMagic.App
open ImportFiles

type State =
    | Importing
    | Imported of FileImportResult list
    | ImportFailed of ImportError

[<RequireQualifiedAccess>]
type Msg =
    | FilesImported of FileImportResult list
    | ImportError of ImportError


let private makeLoadCommand settings =
    Cmd.OfAsync.resultOp
        importAllFromPath
        settings.ImportFolderPath
        Msg.FilesImported
        Msg.ImportError

let makeInitial settings = Importing, (makeLoadCommand settings), None

let update msg state =

    let noChanges = state, Cmd.none, None

    match state, msg with
    | Importing, Msg.FilesImported results -> Imported results, Cmd.none, None
    | Importing, Msg.ImportError error -> ImportFailed error, Cmd.none, None
    | _, Msg.FilesImported _ -> noChanges
    | _, Msg.ImportError _ -> noChanges
