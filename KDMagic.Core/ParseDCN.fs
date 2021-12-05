module KDMagic.ParseDCN

open System.Text.RegularExpressions
open KDMagic
open KDMagic.ParsingUtil

[<RequireQualifiedAccess>]
type DCNParsingError =
    | InvalidCTT of string
    | MissingRequiredItem of string
    | CouldNotParse of string


let dcnRegex =
    Regex
        @"(?'FilmTitle'^[^_]+)_(?'ContentType'[A-Z]+|-T|-F)-?(?'VersionNumber'\d+)?(?:-[^_-]+)*-?(?<!3D)(?'Dimension'3D)?(?:-[^_-]+)*_(?'Aspect'[FSC])[^_]*_(?'AudioLang'[a-zA-Z]+)-(?'SubLang'[a-zA-Z]+)-?(?'Captions'CCAP|ccap|OCAP|ocap)?.*_(?:[^_]*-)?(?'AudioFormat'51|71|10|20|21|MOS|IAB)(?:-[^_]*)?_(?'Resolution'2K|4K)_.*_(?'PackageType'OV|VF).*$"

let private tryGetNamedGroup (name: string) (regMatch: Match) =
    let text = regMatch.Groups.[name].Value
    if text = "" then None else Some text

let private tryFind name regMatch =
    regMatch
    |> tryGetNamedGroup name
    |> Option.asResult (DCNParsingError.MissingRequiredItem name)

let private parsedWith parser =

    let tryParse s =
        s
        |> parser
        |> Option.asResult (DCNParsingError.CouldNotParse s)

    Result.bind tryParse

let optional =
    function
    | Ok item -> Ok(Some item)
    | Error e ->
        match e with
        | DCNParsingError.MissingRequiredItem _ -> Ok None
        | _ -> Error e

let isMissing value =
    function
    | Ok item -> Ok item
    | Error e ->
        match e with
        | DCNParsingError.MissingRequiredItem _ -> Ok value
        | _ -> Error e


let private tryMatchUnion options s =
    options
    |> List.filter (fst >> (=) s)
    |> List.map snd
    |> List.tryHead

let private tryParseContentType s =
    s
    |> tryMatchUnion [ ("FTR", FTR)
                       ("TLR", TLR)
                       ("TSR", TSR)
                       ("RTG-T", RTG_T)
                       ("RTG-F", RTG_F)
                       ("ADV", ADV)
                       ("SHR", SHR)
                       ("XSN", XSN)
                       ("PSA", PSA)
                       ("POL", POL)
                       ("CLP", CLP)
                       ("PRO", PRO)
                       ("STR", STR)
                       ("EPS", EPS)
                       ("HLT", HLT)
                       ("EVT", EVT) ]

let private tryParseCaptions s =
    s
    |> tryMatchUnion [ ("OCAP", Open Rendered)
                       ("ocap", Open Burned)
                       ("CCAP", Closed Rendered)
                       ("ccap", Closed Burned) ]

let private tryParseAudioFormat s =
    s
    |> tryMatchUnion [ ("51", Surround51)
                       ("71", Surround71)
                       ("10", Mono)
                       ("20", Stereo)
                       ("21", StereoSub)
                       ("MOS", MOS) ]

let private tryParseResolution s =
    s
    |> tryMatchUnion [ ("2K", TwoK)
                       ("4K", FourK)
                       ("8K", EightK) ]

let private tryParseAspect s =
    s |> tryMatchUnion [ ("F", F); ("S", S); ("C", C) ]

let private tryParsePackageType s =
    s |> tryMatchUnion [ ("OV", OV); ("VF", VF) ]


let private tryMatchFilmTitle regMatch =
    regMatch
    |> tryFind "FilmTitle"
    |> parsedWith FilmTitle.tryMake

let private tryMatchContentType regMatch =
    regMatch
    |> tryFind "ContentType"
    |> parsedWith tryParseContentType

let private tryMatchVersionNumber regMatch =
    regMatch
    |> tryFind "VersionNumber"
    |> parsedWith (tryParseInt >> Option.bind VersionNumber.tryMake)
    |> optional

let private tryMatchDimension regMatch =
    regMatch
    |> tryFind "Dimension"
    |> parsedWith (fun _ -> Some ThreeD)
    |> isMissing TwoD

let private tryMatchAspect regMatch =
    regMatch |> tryFind "Aspect" |> parsedWith tryParseAspect

let private tryMatchAudioLang regMatch =
    regMatch
    |> tryFind "AudioLang"
    |> parsedWith Language.tryMake

let private tryMatchSubLang regMatch =
    regMatch
    |> tryFind "SubLang"
    |> parsedWith
        (fun s ->
            if s = "XX" then
                Some None
            else
                s |> Language.tryMake |> Option.map Some)

let private tryMatchCaptions regMatch =
    regMatch
    |> tryFind "Captions"
    |> parsedWith tryParseCaptions
    |> optional

let private tryMatchAudioFormat regMatch =
    regMatch
    |> tryFind "AudioFormat"
    |> parsedWith tryParseAudioFormat

let private tryMatchResolution regMatch =
    regMatch
    |> tryFind "Resolution"
    |> parsedWith tryParseResolution

let private tryMatchPackageType regMatch =
    regMatch
    |> tryFind "PackageType"
    |> parsedWith tryParsePackageType


let tryParse ctt =
    let content = ctt |> CTT.content
    let regMatch = content |> dcnRegex.Match

    if regMatch.Success then
        res {
            let! filmTitle = regMatch |> tryMatchFilmTitle
            let! contentType = regMatch |> tryMatchContentType
            let! versionNumber = regMatch |> tryMatchVersionNumber
            let! dimension = regMatch |> tryMatchDimension
            let! projectorAspect = regMatch |> tryMatchAspect
            let! audioLanguage = regMatch |> tryMatchAudioLang
            let! subtitleLanguage = regMatch |> tryMatchSubLang
            let! captions = regMatch |> tryMatchCaptions
            let! audioFormat = regMatch |> tryMatchAudioFormat
            let! resolution = regMatch |> tryMatchResolution
            let! packageType = regMatch |> tryMatchPackageType

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
    else
        Error(DCNParsingError.InvalidCTT content)
