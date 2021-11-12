[<RequireQualifiedAccess>]
module KDMagic.App.WPF.HomePage

open Avalonia.Controls
open KDMagic.App
open Avalonia.FuncUI.DSL

let view (state: HomePage.State) dispatch =

    let onSettingsButtonClicked _ = dispatch HomePage.Msg.SettingsButtonPressed

    let settingsButton =
        Button.create [ Button.content "Settings"
                        Button.onClick onSettingsButtonClicked ]

    Border.create [ Border.child settingsButton ]
