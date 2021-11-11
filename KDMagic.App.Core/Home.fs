[<RequireQualifiedAccess>]
module KDMagic.App.Home

open Elmish

type State = { DirectoryPath: string }

[<RequireQualifiedAccess>]
type Msg = DirectoryPathChanged of string


let private changeDirectoryPath newPath state =
    { state with DirectoryPath = newPath }


let initial = { DirectoryPath = "" }, Cmd.none

let update msg state =
    match msg with
    | Msg.DirectoryPathChanged newPath ->
        state |> changeDirectoryPath newPath, Cmd.none
