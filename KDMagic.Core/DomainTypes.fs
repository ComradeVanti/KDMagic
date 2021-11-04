[<AutoOpen>]
module KDMagic.DomainTypes

type ContentType =
    | FTR
    | TLR
    | TSR
    | TST
    | RTG_F
    | RTG_T
    | ADV
    | SHR
    | XSN
    | PSA
    | POL
    | CLP
    | PRO
    | STR
    | EPS
    | HLT
    | EVT

type Dimension =
    | TwoD
    | ThreeD

type ProjectorAspect =
    | F
    | S
    | C

type CaptionDisplayMode =
    | Burned
    | Rendered

type Captions =
    | Open of CaptionDisplayMode
    | Closed of CaptionDisplayMode

type AudioFormat =
    | MOS
    | Mono
    | Stereo
    | StereoSub
    | Surround51
    | Surround71

type Resolution =
    | TwoK
    | FourK
    | EightK

type PackageType =
    | OV
    | VF

type DigitalCinemaName =
    {
        FilmTitle: FilmTitle.T
        ContentType: ContentType
        VersionNumber: VersionNumber.T
        Dimension: Dimension
        ProjectorAspect: ProjectorAspect
        AudioLanguage: Language.T
        SubtitleLanguage: Language.T
        Captions: Captions option
        AudioFormat: AudioFormat
        Resolution: Resolution
        PackageType: PackageType
    }

type KDMFile = { DigitalCinemaName: DigitalCinemaName }

type KDM = { ContentTitle: string }
