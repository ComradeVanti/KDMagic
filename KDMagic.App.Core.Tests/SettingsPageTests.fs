module KDMagic.App.SettingsPageTests

open FsCheck.Xunit
open KDMagic.App

[<Property>]
let ``Loading settings brings the page into the loaded state`` settings =
    let msg = SettingsPage.Msg.SettingsLoaded settings

    match SettingsPage.State.Unloaded |> SettingsPage.update msg with
    | SettingsPage.State.Loaded _, _, _ -> true
    | _ -> false

[<Property>]
let ``Load-errors bring the page into the error state`` error =
    let msg = SettingsPage.Msg.LoadError error

    match SettingsPage.State.Unloaded |> SettingsPage.update msg with
    | SettingsPage.State.LoadError _, _, _ -> true
    | _ -> false

[<Property>]
let ``After settings where saved, the page is closed`` settings =
    let state = SettingsPage.State.Loaded settings
    let msg = SettingsPage.Msg.SettingsSaved

    match state |> SettingsPage.update msg with
    | _, _, Some SettingsPage.Emit.CloseSettings -> true
    | _ -> false

[<Property>]
let ``On exiting, the page is closed`` state =
    let msg = SettingsPage.Msg.Exit

    match state |> SettingsPage.update msg with
    | _, _, Some SettingsPage.Emit.CloseSettings -> true
    | _ -> false
