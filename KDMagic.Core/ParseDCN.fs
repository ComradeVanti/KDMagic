module KDMagic.ParseDCN

open System
open Microsoft.FSharp.Reflection

[<RequireQualifiedAccess>]
type DCNParsingError =
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


let private captionTags =
    [ ("OCAP", Open Rendered)
      ("ocap", Open Burned)
      ("CCAP", Closed Rendered)
      ("ccap", Closed Burned) ]

let private audioFormatTags =
    [ ("51", Surround51)
      ("71", Surround71)
      ("10", Mono)
      ("20", Stereo)
      ("21", StereoSub)
      ("MOS", MOS) ]

let private resolutionTags = [ ("2K", TwoK); ("4K", FourK); ("8K", EightK) ]

let private tryParseUnion<'a> (s: string) =
    FSharpType.GetUnionCases typeof<'a>
    |> Array.tryFind (fun case -> case.Name = s)
    |> Option.map (fun case -> FSharpValue.MakeUnion(case, [||]) :?> 'a)

let private tryParseInt s =
    try
        s |> int |> Some
    with
    | :? FormatException -> None

let private tryParseSubfield index parser ctt =
    ctt
    |> CTT.tryGetSubfield index
    |> function
        | Some field -> parser field
        | None -> Error(DCNParsingError.SubfieldNotFound index)

let private tryParseField index parser ctt =
    ctt
    |> CTT.tryGetField index
    |> function
        | Some field -> parser field
        | None -> Error(DCNParsingError.FieldNotFound index)

let private tryParseFieldContent index parser ctt =
    ctt |> tryParseField index (Field.content >> parser)

let private tryParseSubfieldContent index parser ctt =
    ctt |> tryParseSubfield index (Subfield.content >> parser)

let private tryParseFilmTitle ctt =

    let parser content =
        content
        |> FilmTitle.tryMake
        |> Option.asResult (DCNParsingError.InvalidFilmTitle content)

    ctt |> tryParseFieldContent 0 parser

let private tryParseContentType ctt =

    let parser field =
        if field |> Field.contains "RTG-T" then
            Ok RTG_T
        elif field |> Field.contains "RTG-F" then
            Ok RTG_F
        else
            field
            |> Field.tryGetSubfield 0
            |> Option.map Subfield.content
            |> Option.bind tryParseUnion<ContentType>
            |> Option.asResult (
                DCNParsingError.InvalidContentType(field |> Field.content)
            )

    ctt |> tryParseField 1 parser

let private tryParseVersionNumber ctt =

    let isNumber s = s |> tryParseInt |> Option.isSome

    let parser field =
        field
        |> Field.tryFindSubfield (Subfield.content >> isNumber)
        |> Option.bind (Subfield.content >> tryParseInt)
        |> Option.bind VersionNumber.tryMake
        |> Ok

    ctt |> tryParseField 1 parser

let private tryParseDimension ctt =

    let parser field =
        if field |> Field.contains "3D" then ThreeD else TwoD
        |> Ok

    ctt |> tryParseField 1 parser

let private tryParseProjectorAspect ctt =

    let parser content =
        content
        |> tryParseUnion<ProjectorAspect>
        |> Option.asResult (DCNParsingError.InvalidProjectorAspect content)

    ctt |> tryParseSubfieldContent (2, 0) parser

let private tryParseAudioLanguage ctt =

    let parser content =
        content
        |> Language.tryMake
        |> Option.asResult (DCNParsingError.InvalidAudioLanguage content)

    ctt |> tryParseSubfieldContent (3, 0) parser

let private tryParseSubtitleLanguage ctt =

    let parser content =
        if content = "XX" then None else content |> Language.tryMake
        |> Ok

    ctt |> tryParseSubfieldContent (3, 1) parser

let private tryParseCaptions ctt =

    let parser field = field |> Field.tryMatchTag captionTags |> Ok

    ctt |> tryParseField 3 parser

let private tryParseAudioFormat ctt =

    let parser subfield =
        subfield
        |> Subfield.tryMatchTag audioFormatTags
        |> Option.asResult (
            DCNParsingError.InvalidAudioFormat(subfield |> Subfield.content)
        )

    ctt |> tryParseSubfield (5, 0) parser

let private tryParseResolution ctt =

    let parser field =
        field
        |> Field.tryMatchTag resolutionTags
        |> Option.asResult (
            DCNParsingError.InvalidResolution(field |> Field.content)
        )

    ctt |> tryParseField 6 parser

let private tryParsePackageType ctt =
    let parser field =
        let content = field |> Field.content

        content
        |> tryParseUnion<PackageType>
        |> Option.asResult (DCNParsingError.InvalidPackageName content)

    ctt |> tryParseField 11 parser

let tryParse ctt =
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
