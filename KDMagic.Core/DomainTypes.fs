[<AutoOpen>]
module KDMagic.DomainTypes


type DigitalCinemaName = { FilmTitle: string }

type KDMFile = { DigitalCinemaName: DigitalCinemaName }

type KDM = { ContentTitle: string; ContentType: ContentType }
