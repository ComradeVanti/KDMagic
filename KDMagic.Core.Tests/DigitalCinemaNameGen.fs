module KDMagic.DigitalCinemaNameGen

open FsCheck
open KDMagic.DomainTypes
open FilmTitleGen

let genDigitalCinemaNameGen =
    gen {
        let! filmTitle = genFilmTitle
        return { FilmTitle = filmTitle }
    }

type ValidDigitalCinemaName = ValidDigitalCinemaName of DigitalCinemaName

type ArbDigitalCinemaNames =
    static member Valid =
        genDigitalCinemaNameGen |> asArbOf ValidDigitalCinemaName
