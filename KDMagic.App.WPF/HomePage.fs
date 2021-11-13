[<RequireQualifiedAccess>]
module KDMagic.App.WPF.HomePage

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open KDMagic.App

let view (state: HomePage.State) dispatch : IView =

    let onSettingsButtonClicked _ = dispatch HomePage.Msg.SettingsButtonPressed

    let settingsButton =
        Button.create [ Button.content "Settings"
                        Button.onClick onSettingsButtonClicked ]

    Border.create [ Border.child settingsButton ] |> asView
