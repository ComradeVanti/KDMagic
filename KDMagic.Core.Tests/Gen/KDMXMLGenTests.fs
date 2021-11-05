namespace KDMagic

open FsCheck.Xunit
open KDMagic.KDMXMLGen

[<Properties(Arbitrary = [| typeof<ArbKDMXML> |])>]
module KDMXMLGenTests =

    [<Property>]
    let ``Generated xml can be parsed`` (ValidKDMXML xml) =
        match xml |> KDMDoc.tryParse with
        | Ok _ -> true
        | Error e -> raise e
