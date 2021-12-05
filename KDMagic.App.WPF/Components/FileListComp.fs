[<RequireQualifiedAccess>]
module KDMagic.App.WPF.FileListComp

open System.IO
open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.Layout
open KDMagic.App
open KDMagic
open Avalonia.FuncUI.DSL
open KDMagic.App.ImportFiles

[<RequireQualifiedAccess>]
type Events = unit

let private loadingScreen =
    TextBlock.create [ TextBlock.text "Importing files" ]
    |> asView

let private viewOther (path: string) =
    let fileName = Path.GetFileName path

    TextBlock.create [ TextBlock.text $"Non-Kdm file \"{fileName}\"" ]
    |> asView

let private viewReadError error =
    let fileName =
        match error with
        | File.ReadError.InvalidPath path -> Path.GetFileName path
        | File.ReadError.NoAccess path -> Path.GetFileName path
        | File.ReadError.NotFound path -> Path.GetFileName path
        | File.ReadError.Unknown _ -> "???"

    TextBlock.create [ TextBlock.text $"Could not read file \"{fileName}\"" ]
    |> asView

let private viewKdm dispatch kdm =

    let filmTitle = kdm.ContentInfo.FilmTitle |> FilmTitle.value

    TextBlock.create [ TextBlock.text filmTitle ] |> asView


let private viewResults results dispatch =

    let viewResult result =
        match result with
        | Ok (path, content) ->
            match content with
            | ValidKdm kdm -> kdm |> viewKdm dispatch
            | Other -> viewOther path
        | Error error ->
            match error with
            | FileImportError.InvalidKdm parseError ->
                TextBlock.create [ TextBlock.text "Invalid kdm" ] |> asView
            | FileImportError.ReadError readError -> viewReadError readError

    StackPanel.create [ StackPanel.children (results |> List.map viewResult)
                        StackPanel.orientation Orientation.Vertical
                        StackPanel.verticalScrollBarVisibility
                            ScrollBarVisibility.Visible ]
    |> asView

let private viewImportError error =

    let errorText =
        match error with
        | ImportError.Directory directoryError ->
            match directoryError with
            | Directory.OpenError.InvalidPath ->
                "The import-directory has an invalid path"
            | Directory.OpenError.NoAccess ->
                "No access to the import-directory"
            | Directory.OpenError.NotFound -> "Import directory not found"

    TextBlock.create [ TextBlock.text errorText ] |> asView

let view state dispatch =

    match state with
    | FileListComp.State.Importing -> loadingScreen
    | FileListComp.State.Imported results -> viewResults results dispatch
    | FileListComp.State.ImportFailed error -> viewImportError error
