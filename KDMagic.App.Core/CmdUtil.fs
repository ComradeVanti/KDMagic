[<RequireQualifiedAccess>]
module KDMagic.App.Cmd

open Elmish

module OfAsync =

    let resultOp op arg succToMsg errToMsg =

        let mapResult =
            function
            | Ok succ -> succ |> succToMsg
            | Error err -> err |> errToMsg

        Cmd.OfAsync.perform op arg mapResult
