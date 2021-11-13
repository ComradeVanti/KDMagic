[<RequireQualifiedAccess>]
module KDMagic.App.WPF.SettingsPage

open Avalonia.Controls
open KDMagic.App
open Avalonia.FuncUI.DSL
open KDMagic.App.Settings

let view (state: SettingsPage.State) dispatch =

    let loadingScreen = TextBlock.create [ TextBlock.text "Loading" ]

    let viewError error =
        let errorMsg =
            match error with
            | SettingsError.CouldNotParse -> "Could not parse settings"

        TextBlock.create [ TextBlock.text errorMsg ]

    let viewSettingsEditor settings =
        TextBlock.create [ TextBlock.text "Settings" ]

    match state with
    | SettingsPage.State.Unloaded -> loadingScreen
    | SettingsPage.State.Loaded settings -> viewSettingsEditor settings
    | SettingsPage.State.LoadError error -> viewError error
    |> asView
