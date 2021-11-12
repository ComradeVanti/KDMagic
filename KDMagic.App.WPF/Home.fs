[<RequireQualifiedAccess>]
module KDMagic.App.WPF.Home

open KDMagic.App
open Avalonia.FuncUI.DSL

let view (state: Home.State) dispatch =
    Border.create []
