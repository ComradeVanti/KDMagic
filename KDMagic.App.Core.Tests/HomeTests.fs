module KDMagic.App.HomeTests

open FsCheck
open FsCheck.Xunit

[<Property>]
let ``Directory-path changes on DirectoryPathChanged event`` state newPath =
    let updated =
        state
        |> Home.update (newPath |> Home.Msg.DirectoryPathChanged)
        |> fst

    updated.DirectoryPath = newPath
    |@ $"Expected \"{newPath}\" but got \"{updated.DirectoryPath}\"."
