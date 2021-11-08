namespace KDMagic

open System
open FsCheck.Xunit
open KDMagic.KDMXMLGen

[<Properties(Arbitrary = [| typeof<ArbKDMXML> |])>]
module KDMXMLGenTests =

    [<Property>]
    let ``Generated xml can be parsed`` (ValidKDMXML xml) =
        match xml |> KDMDoc.tryParse with
        | Ok _ -> true
        | Error e -> raise e

    [<Property>]
    let ``Generated end-date is always after start-date`` (ValidKDMXML xml) =
        match xml |> KDMDoc.tryParse with
        | Ok doc ->
            let startDate =
                DateTime.Parse
                    doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentKeysNotValidBefore

            let endDate =
                DateTime.Parse
                    doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentKeysNotValidAfter

            endDate > startDate
        | Error _e -> false
