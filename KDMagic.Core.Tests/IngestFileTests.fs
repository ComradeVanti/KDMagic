namespace KDMagic.Core.Tests

open System.IO
open FsCheck.Xunit
open KDMagic
open KDMagic.Core.Tests.TestFileGen
open global.Xunit
open FsCheck
open IngestFile

[<Properties(Arbitrary = [| typeof<ArbFilePaths> |])>]
module IngestFileTests =

    let private isFileNotFoundError =
        function
        | Error (FileNotFound _) -> true
        | _ -> false

    let private isFileNotKDMError =
        function
        | Error FileNotKDM -> true
        | _ -> false

    let private isOk =
        function
        | Ok _ -> true
        | _ -> false


    [<Property>]
    let ``Ingesting a non-existent file returns a file-not-found error`` path =
        path |> (not << File.Exists)
        ==> lazy
            (tryIngestFromPath path
             |> Async.RunSynchronously
             |> isFileNotFoundError)

    [<Property>]
    let ``Ingesting an invalid kdm file returns a file-not-kdm error`` file =
        tryIngest file |> isFileNotKDMError

    [<Property>]
    let ``Ingesting a valid file returns an ok kdm`` (TestFilePath path) =
        tryIngestFromPath path |> Async.RunSynchronously |> isOk
