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

type DigitalCinemaName =
    {
        FilmTitle: FilmTitle.T
        ContentType: ContentType
        VersionNumber: VersionNumber.T
        ProjectorAspect: ProjectorAspect
        AudioLanguage: Language.T
        SubtitleLanguage: Language.T
        Captions: Captions option
    }

type KDMFile = { DigitalCinemaName: DigitalCinemaName }

type KDM = { ContentTitle: string }
