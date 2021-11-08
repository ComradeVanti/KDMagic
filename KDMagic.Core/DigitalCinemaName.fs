﻿[<RequireQualifiedAccess>]
module KDMagic.DigitalCinemaName

open System
open ContentTitleText
open KDMagic

let private tryParseInt s =
    try
        s |> int |> Some
    with
    | :? FormatException -> None

let private tryFindStringMatch possibilities s =
    possibilities
    |> List.tryFind (fst >> (=) s)
    |> Option.map snd


let private tryParseFilmTitle ctt =
    ctt |> tryGetField 0 |> Option.bind FilmTitle.tryMake

let private tryParseContentType ctt =
    opt {
        let! subfield = ctt |> tryGetSubfield (1, 0)

        if subfield <> "RTG" then
            return! subfield |> tryParseUnion
        else
            let! next = ctt |> tryGetSubfield (1, 1)

            if next = "T" then return RTG_T
            elif next = "F" then return RTG_F
            else return! None
    }

let private tryParseVersionNumber ctt =
    opt {
        let! contentType = ctt |> tryGetSubfield (1, 0)
        let index = if contentType = "RTG" then 2 else 1

        return!
            ctt
            |> tryGetSubfield (1, index)
            |> Option.bind tryParseInt
            |> Option.bind VersionNumber.tryMake
    }

let private tryParseDimension ctt =
    ctt
    |> tryGetField 1
    |> Option.bind
        (fun field ->
            if field.Contains "2D" then Some TwoD
            elif field.Contains "3D" then Some ThreeD
            else None)

let private tryParseProjectorAspect ctt =
    ctt
    |> tryGetSubfield (2, 0)
    |> Option.bind tryParseUnion<ProjectorAspect>

let private tryParseAudioLanguage ctt =
    ctt |> tryGetSubfield (3, 0) |> Option.bind Language.tryMake

let private tryParseSubtitleLanguage ctt =
    ctt
    |> tryGetSubfield (3, 1)
    |> Option.bind
        (fun lang -> if lang = "XX" then None else lang |> Language.tryMake)

let private tryParseCaptions ctt =
    ctt
    |> tryGetSubfield (3, 2)
    |> Option.bind (
        tryFindStringMatch [ ("OCAP", Open Rendered)
                             ("ocap", Open Burned)
                             ("CCAP", Closed Rendered)
                             ("ccap", Closed Burned) ]
    )

let private tryParseAudioFormat ctt =
    ctt
    |> tryGetSubfield (5, 0)
    |> Option.bind (
        tryFindStringMatch [ ("51", Surround51)
                             ("71", Surround71)
                             ("10", Mono)
                             ("20", Stereo)
                             ("21", StereoSub)
                             ("MOS", MOS) ]
    )

let private tryParseResolution ctt =
    ctt
    |> tryGetField 6
    |> Option.bind (
        tryFindStringMatch [ ("2K", TwoK)
                             ("4K", FourK)
                             ("8K", EightK) ]
    )

let private tryParsePackageType ctt =
    ctt
    |> tryGetField 11
    |> Option.bind tryParseUnion<PackageType>

let tryParse ctt =
    opt {
        let! filmTitle = ctt |> tryParseFilmTitle
        let! contentType = ctt |> tryParseContentType
        let versionNumber = ctt |> tryParseVersionNumber
        let! dimension = ctt |> tryParseDimension
        let! projectorAspect = ctt |> tryParseProjectorAspect
        let! audioLanguage = ctt |> tryParseAudioLanguage
        let subtitleLanguage = ctt |> tryParseSubtitleLanguage
        let captions = ctt |> tryParseCaptions
        let! audioFormat = ctt |> tryParseAudioFormat
        let! resolution = ctt |> tryParseResolution
        let! packageType = ctt |> tryParsePackageType

        return
            {
                FilmTitle = filmTitle
                ContentType = contentType
                VersionNumber = versionNumber
                Dimension = dimension
                ProjectorAspect = projectorAspect
                AudioLanguage = audioLanguage
                SubtitleLanguage = subtitleLanguage
                Captions = captions
                AudioFormat = audioFormat
                Resolution = resolution
                PackageType = packageType
            }
    }