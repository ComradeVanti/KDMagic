[<RequireQualifiedAccess>]
module KDMagic.DigitalCinemaName

open System
open ContentTitleText
open KDMagic
open Microsoft.FSharp.Reflection

[<RequireQualifiedAccess>]
type ParsingError =
    | FieldNotFound of int
    | SubfieldNotFound of (int * int)
    | InvalidFilmTitle of string
    | InvalidContentType of string
    | InvalidProjectorAspect of string
    | InvalidAudioLanguage of string
    | InvalidCaptions of string
    | InvalidAudioFormat of string
    | InvalidResolution of string
    | InvalidPackageName of string

type ParsingResult = Result<DigitalCinemaName, ParsingError>


let private tryParseSubfield index parser ctt =
    ctt
    |> tryGetSubfield index
    |> function
        | Some field -> parser field
        | None -> Error(ParsingError.SubfieldNotFound index)

let private tryParseField index parser ctt =
    ctt
    |> tryGetField index
    |> function
        | Some field -> parser field
        | None -> Error(ParsingError.FieldNotFound index)

let private tryParseUnion<'a> (s: string) =
    FSharpType.GetUnionCases typeof<'a>
    |> Array.tryFind (fun case -> case.Name = s)
    |> Option.map (fun case -> FSharpValue.MakeUnion(case, [||]) :?> 'a)

let private tryParseInt s =
    try
        s |> int |> Some
    with
    | :? FormatException -> None

let private tryFindStringMatchExact possibilities s =
    possibilities
    |> List.tryFind (fst >> (=) s)
    |> Option.map snd

let private tryFindStringMatchContains
    (possibilities: (string * _) list)
    (s: string)
    =
    possibilities
    |> List.tryFind (fst >> s.Contains)
    |> Option.map snd


let private tryParseFilmTitle ctt =

    let parser field =
        field
        |> FilmTitle.tryMake
        |> Option.asResult (ParsingError.InvalidFilmTitle field)

    ctt |> tryParseField 0 parser

let private tryParseContentType ctt =
    res {
        let! subfield =
            ctt
            |> tryGetSubfield (1, 0)
            |> Option.asResult (ParsingError.SubfieldNotFound(1, 0))

        if subfield <> "RTG" then
            return!
                subfield
                |> tryParseUnion
                |> Option.asResult (ParsingError.InvalidContentType subfield)
        else
            let! next =
                ctt
                |> tryGetSubfield (1, 1)
                |> Option.asResult (ParsingError.SubfieldNotFound(1, 1))

            if next = "T" then return RTG_T
            elif next = "F" then return RTG_F
            else return! Error(ParsingError.InvalidContentType subfield)
    }

let private tryParseVersionNumber ctt =

    let parser contentType =
        let index = if contentType = "RTG" then 2 else 1

        let innerParser subfield =
            subfield
            |> tryParseInt
            |> Option.bind VersionNumber.tryMake
            |> Ok

        ctt |> tryParseSubfield (1, index) innerParser

    ctt |> tryParseSubfield (1, 0) parser

let private tryParseDimension ctt =

    let parser (field: string) =
        if field.Contains "3D" then ThreeD else TwoD
        |> Ok

    ctt |> tryParseField 1 parser

let private tryParseProjectorAspect ctt =

    let parser subfield =
        subfield
        |> tryParseUnion<ProjectorAspect>
        |> Option.asResult (ParsingError.InvalidProjectorAspect subfield)

    ctt |> tryParseSubfield (2, 0) parser

let private tryParseAudioLanguage ctt =

    let parser subfield =
        subfield
        |> Language.tryMake
        |> Option.asResult (ParsingError.InvalidAudioLanguage subfield)

    ctt |> tryParseSubfield (3, 0) parser

let private tryParseSubtitleLanguage ctt =

    let parser subfield =
        if subfield = "XX" then
            None
        else
            subfield |> Language.tryMake
        |> Ok

    ctt |> tryParseSubfield (3, 1) parser

let private tryParseCaptions ctt =

    ctt
    |> tryGetField 3
    |> Option.bind (
        tryFindStringMatchContains [ ("OCAP", Open Rendered)
                                     ("ocap", Open Burned)
                                     ("CCAP", Closed Rendered)
                                     ("ccap", Closed Burned) ]
    )
    |> Ok

let private tryParseAudioFormat ctt =

    let parser subfield =
        subfield
        |> tryFindStringMatchContains [ ("51", Surround51)
                                        ("71", Surround71)
                                        ("10", Mono)
                                        ("20", Stereo)
                                        ("21", StereoSub)
                                        ("MOS", MOS) ]
        |> Option.asResult (ParsingError.InvalidAudioFormat subfield)

    ctt |> tryParseSubfield (5, 0) parser

let private tryParseResolution ctt =

    let parser field =
        field
        |> tryFindStringMatchExact [ ("2K", TwoK)
                                     ("4K", FourK)
                                     ("8K", EightK) ]
        |> Option.asResult (ParsingError.InvalidResolution field)

    ctt |> tryParseField 6 parser

let private tryParsePackageType ctt =
    let parser field =
        field
        |> tryParseUnion<PackageType>
        |> Option.asResult (ParsingError.InvalidPackageName field)

    ctt |> tryParseField 11 parser

let tryParse ctt : ParsingResult =
    res {
        let! filmTitle = ctt |> tryParseFilmTitle
        let! contentType = ctt |> tryParseContentType
        let! versionNumber = ctt |> tryParseVersionNumber
        let! dimension = ctt |> tryParseDimension
        let! projectorAspect = ctt |> tryParseProjectorAspect
        let! audioLanguage = ctt |> tryParseAudioLanguage
        let! subtitleLanguage = ctt |> tryParseSubtitleLanguage
        let! captions = ctt |> tryParseCaptions
        let! audioFormat = ctt |> tryParseAudioFormat
        let! resolution = ctt |> tryParseResolution
        let! packageType = ctt |> tryParsePackageType

        return
            { FilmTitle = filmTitle
              ContentType = contentType
              VersionNumber = versionNumber
              Dimension = dimension
              ProjectorAspect = projectorAspect
              AudioLanguage = audioLanguage
              SubtitleLanguage = subtitleLanguage
              Captions = captions
              AudioFormat = audioFormat
              Resolution = resolution
              PackageType = packageType }
    }
