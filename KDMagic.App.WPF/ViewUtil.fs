[<AutoOpen>]
module KDMagic.App.WPF.ViewUtil

open Avalonia.FuncUI.Types

let asView (view: IView<_>) : IView = upcast view
