module KDMagic.ParseKdm

open System
open KDMagic

type Path = string

[<RequireQualifiedAccess>]
type KdmParseError =
    | FileNotKDM
    | InvalidDigitalCinemaName of string * DigitalCinemaName.ParsingError
    | InvalidNotValidBefore of string
    | InvalidNotValidAfter of string

let private tryParseDateTime s =
    try
        s |> DateTime.Parse |> Some
    with
    | :? FormatException -> None

let tryParseXml xml =
    match KDMDoc.tryParse xml with
    | Ok doc ->
        res {
            let contentTitleText =
                doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentTitleText.Trim
                    ()

            let notValidBefore =
                doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentKeysNotValidBefore

            let notValidAfter =
                doc.AuthenticatedPublic.RequiredExtensions.KdmRequiredExtensions.ContentKeysNotValidAfter

            let! digitalCinemaName =
                contentTitleText
                |> DigitalCinemaName.tryParse
                |> Result.mapError
                    (fun error ->
                        KdmParseError.InvalidDigitalCinemaName(
                            contentTitleText,
                            error
                        ))

            let! validFrom =
                tryParseDateTime notValidBefore
                |> Option.asResult (
                    KdmParseError.InvalidNotValidBefore notValidBefore
                )

            let! validUntil =
                tryParseDateTime notValidAfter
                |> Option.asResult (
                    KdmParseError.InvalidNotValidAfter notValidAfter
                )

            return
                { ContentInfo = digitalCinemaName
                  ValidFrom = validFrom
                  ValidUntil = validUntil }
        }
    | Error _ -> Error KdmParseError.FileNotKDM
