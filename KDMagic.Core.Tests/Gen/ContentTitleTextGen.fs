module KDMagic.ContentTitleTextGen

open FsCheck

let private formatVersionNumber number =
    match number with
    | Some number -> $"-{number |> VersionNumber.value}"
    | None -> ""

let private formatContentType (contentType: ContentType) =
    (contentType |> string).Replace("_", "-")

let private formatDimension dimension =
    match dimension with
    | TwoD -> "2D"
    | ThreeD -> "3D"

let private formatSubtitle subtitle =
    match subtitle with
    | Some language -> language |> Language.value |> string
    | None -> "XX"

let private formatCaptions captions =
    match captions with
    | Some (Open Rendered) -> "-OCAP"
    | Some (Open Burned) -> "-ocap"
    | Some (Closed Rendered) -> "-CCAP"
    | Some (Closed Burned) -> "-ccap"
    | _ -> ""

let private formatAudioFormat format =
    match format with
    | MOS -> "MOS"
    | Mono -> "10"
    | Stereo -> "20"
    | StereoSub -> "21"
    | Surround51 -> "51"
    | Surround71 -> "71"

let private formatResolution resolution =
    match resolution with
    | TwoK -> "2K"
    | FourK -> "4K"
    | EightK -> "8K"

let toContentTitleText dcn =
    let contentType = dcn.ContentType |> formatContentType
    let version = dcn.VersionNumber |> formatVersionNumber
    let dimension = dcn.Dimension |> formatDimension
    let audio = dcn.AudioLanguage |> Language.value
    let subtitle = dcn.SubtitleLanguage |> formatSubtitle
    let captions = dcn.Captions |> formatCaptions
    let audioFormat = dcn.AudioFormat |> formatAudioFormat
    let resolution = dcn.Resolution |> formatResolution

    [ dcn.FilmTitle |> FilmTitle.value
      $"{contentType}{version}-{dimension}"
      $"{dcn.ProjectorAspect}"
      $"{audio}-{subtitle}{captions}"
      "US-GB"
      $"{audioFormat}"
      $"{resolution}"
      "Stud"
      "20210101"
      "Yee"
      "SMPTE"
      $"{dcn.PackageType}" ]
    |> String.concat "_"

let genContentTitleText =
    DigitalCinemaNameGen.genDigitalCinemaName
    |> Gen.map toContentTitleText

type ValidContentTitleText = ValidContentTitleText of string

type ArbContentTitleTexts =
    static member Valid = genContentTitleText |> asArbOf ValidContentTitleText
