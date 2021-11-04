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

type DigitalCinemaName = { FilmTitle: FilmTitle.T; ContentType: ContentType }

type KDMFile = { DigitalCinemaName: DigitalCinemaName }

type KDM = { ContentTitle: string }
