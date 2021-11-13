module KDMagic.App.SettingsPageTests

open FsCheck
open FsCheck.Xunit
open KDMagic.App

[<Property>]
let ``Loading settings brings the page into the loaded state`` settings =
    let msg = SettingsPage.Msg.SettingsLoaded settings

    match SettingsPage.State.Unloaded |> SettingsPage.update msg with
    | SettingsPage.State.Loaded _, _, _ -> true
    | _ -> false

[<Property>]
let ``Loading settings when they are already loaded, does nothing``
    oldSettings
    settings
    =
    oldSettings <> settings
    ==> lazy
        (let state = SettingsPage.State.Loaded oldSettings
         let msg = SettingsPage.Msg.SettingsLoaded settings

         match state |> SettingsPage.update msg with
         | SettingsPage.State.Loaded newSettings, _, _ ->
             newSettings = oldSettings
         | _ -> false)
