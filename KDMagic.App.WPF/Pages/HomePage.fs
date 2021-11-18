[<RequireQualifiedAccess>]
module KDMagic.App.WPF.HomePage

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open KDMagic.App

let private loadingScreen =
    TextBlock.create [ TextBlock.text "Loading settings" ]
    |> asView

let private viewLoadError error =
    TextBlock.create [ TextBlock.text "Load error" ] |> asView

let view (state: HomePage.State) dispatch : IView =

    let onSettingsButtonClicked _ = dispatch HomePage.Msg.SettingsButtonPressed

    let settingsButton =
        Button.create [ Button.content "Settings"
                        Button.onClick onSettingsButtonClicked
                        Button.dock Dock.Top ]

    let content =
        Border.create [ Border.dock Dock.Bottom
                        Border.child (
                            match state with
                            | HomePage.State.Loading -> loadingScreen
                            | HomePage.State.LoadFailed error ->
                                viewLoadError error
                            | HomePage.State.Loaded (_, fileList) ->
                                FileListComp.view
                                    fileList
                                    (HomePage.Msg.FileListCompMsg >> dispatch)
                        ) ]
        |> asView


    DockPanel.create [ DockPanel.children [ settingsButton; content ] ]
    |> asView
