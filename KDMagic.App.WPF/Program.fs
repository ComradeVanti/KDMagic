﻿namespace KDMagic.App.WPF

open Elmish
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.Components.Hosts
open KDMagic.App
open KDMagic.App.WPF

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "KDMagic"
        base.Width <- 600.0
        base.Height <- 400.0

        Elmish.Program.mkProgram
            (fun _ -> Shell.initial)
            Shell.update
            Shell.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run


type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Default/DefaultTheme.xaml"

        this.Styles.Load
            "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string []) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
