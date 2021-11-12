module KDMagic.App.HomePageTests

open FsCheck.Xunit

[<Property>]
let ``Clicking the settings button opens the settings`` state =
    let _, _, emit = state |> HomePage.update HomePage.Msg.SettingsButtonPressed
    emit = Some HomePage.Emit.OpenSettings
