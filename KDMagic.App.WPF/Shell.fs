[<RequireQualifiedAccess>]
module KDMagic.App.WPF.Shell

open KDMagic.App

let view state dispatch =
    match state with
    | Shell.State.Home homeState ->
        HomePage.view homeState (Shell.Msg.Home >> dispatch)
