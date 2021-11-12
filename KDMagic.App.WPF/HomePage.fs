[<RequireQualifiedAccess>]
module KDMagic.App.WPF.HomePage

open KDMagic.App
open Avalonia.FuncUI.DSL

let view (state: HomePage.State) dispatch =
    Border.create []
