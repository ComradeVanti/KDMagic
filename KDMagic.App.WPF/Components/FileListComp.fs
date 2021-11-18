[<RequireQualifiedAccess>]
module KDMagic.App.WPF.FileListComp

open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.Layout
open KDMagic.App
open Avalonia.FuncUI.DSL
open KDMagic.App.ImportFiles

[<RequireQualifiedAccess>]
type Events = unit

let private loadingScreen =
    TextBlock.create [ TextBlock.text "Importing files" ]
    |> asView

let private viewResults results dispatch =

    let viewResult result =

        let text =
            match result with
            | Ok (path, content) ->
                match content with
                | ValidKdm kmd -> "Kdm"
                | Other -> "Other"
            | Error error ->
                match error with
                | FileImportError.InvalidKdm parseError -> "Invalid kdm"
                | FileImportError.ReadError readError -> "Read error"

        TextBlock.create [ TextBlock.text text ] |> asView

    StackPanel.create [ StackPanel.children (results |> List.map viewResult)
                        StackPanel.orientation Orientation.Vertical
                        StackPanel.verticalScrollBarVisibility
                            ScrollBarVisibility.Visible ]
    |> asView

let private viewImportError error =

    let errorText =
        match error with
        | ImportFiles.ImportError.Directory directoryError ->
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
