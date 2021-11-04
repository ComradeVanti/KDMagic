[<AutoOpen>]
module KDMagic.DomainTypes


type DigitalCinemaName = { FilmTitle: FilmTitle.T }

type KDMFile = { DigitalCinemaName: DigitalCinemaName }

type KDM = { ContentTitle: string }
