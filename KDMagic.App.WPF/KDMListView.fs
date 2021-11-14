[<RequireQualifiedAccess>]
module KDMagic.App.WPF.KDMListView

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout

[<RequireQualifiedAccess>]
type Events = unit

let view kdmList =

    let viewKDM kdm = Border.create [] |> asView

    StackPanel.create [ StackPanel.orientation Orientation.Vertical
                        StackPanel.children (kdmList |> List.map viewKDM) ]
    |> asView
