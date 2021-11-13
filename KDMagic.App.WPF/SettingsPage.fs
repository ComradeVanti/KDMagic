[<RequireQualifiedAccess>]
module KDMagic.App.WPF.SettingsPage

open Avalonia.Controls
open Avalonia.Layout
open KDMagic.App
open Avalonia.FuncUI.DSL
open KDMagic.App.Settings

let private loadingScreen =
    TextBlock.create [ TextBlock.text "Loading" ] |> asView

let private viewError error =
    let errorMsg =
        match error with
        | SettingsError.CouldNotParse -> "Could not parse settings"

    TextBlock.create [ TextBlock.text errorMsg ] |> asView

let private viewSettings settings dispatch =

    let onKDMFolderPathChanged newPath =
        { settings with KDMFolderPath = newPath }
        |> SettingsPage.Msg.SettingsChanged
        |> dispatch

    let folderPathEditor =
        StackPanel.create [ StackPanel.orientation Orientation.Vertical
                            StackPanel.children [ TextBlock.create [ TextBlock.text
                                                                         "KDM-folder path" ]
                                                  TextBox.create [ TextBox.text
                                                                       settings.KDMFolderPath
                                                                   TextBox.onTextChanged
                                                                       onKDMFolderPathChanged ] ] ]

    StackPanel.create [ StackPanel.orientation Orientation.Vertical
                        StackPanel.children [ folderPathEditor ] ]
    |> asView

let view (state: SettingsPage.State) dispatch =
    match state with
    | SettingsPage.State.Unloaded -> loadingScreen
    | SettingsPage.State.Loaded settings -> viewSettings settings dispatch
    | SettingsPage.State.LoadError error -> viewError error
