﻿[<AutoOpen>]
module KDMagic.App.ElmishUtil

open Elmish

let addEmit emit (mapper: _ -> Cmd<_>) (cmd: Cmd<_>) =
    [ cmd
      match emit with
      | Some msg -> msg |> mapper
      | None -> Cmd.none ]
    |> Cmd.batch

let wrapChild wrapState wrapCmd wrapEmit (state, cmd, emit) =

    let wrappedCmd = cmd |> Cmd.map wrapCmd |> addEmit emit wrapEmit

    (wrapState state), wrappedCmd
