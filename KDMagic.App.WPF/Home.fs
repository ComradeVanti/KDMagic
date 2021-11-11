[<RequireQualifiedAccess>]
module KDMagic.App.WPF.Home

open KDMagic.App
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout

let view (state: Home.State) dispatch =

    let onDirectoryPathChanged = Home.Msg.DirectoryPathChanged >> dispatch

    let pathInput =
        StackPanel.create [ StackPanel.orientation Orientation.Vertical
                            StackPanel.children [ TextBlock.create [ TextBlock.text
                                                                         "KDM directory path" ]
                                                  TextBox.create [ TextBox.text
                                                                       state.DirectoryPath
                                                                   TextBox.onTextChanged
                                                                       onDirectoryPathChanged ] ] ]

    Border.create [ Border.child pathInput ]
