module KDMagic.Core.Tests.IngestFileTests

open KDMagic
open Xunit
open IngestFile

[<Fact>]
let ``Ingesting a non-existent file returns a specific error`` () =
    async {
        let! result = tryIngestFromPath ""

        match result with
        | Error (FileNotFound _) -> true
        | _ -> false
        |> Assert.True
    }
