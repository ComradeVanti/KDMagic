module KDMagic.DigitalCinemaNameGen

open FsCheck
open KDMagic.FilmTitleGen
open KDMagic.LanguageGen
open KDMagic.VersionNumberGen

Arb.register<ArbFilmTitles> |> ignore
Arb.register<ArbVersionNumbers> |> ignore
Arb.register<ArbLanguages> |> ignore

let genDigitalCinemaName = Arb.generate<DigitalCinemaName>

type ValidDigitalCinemaName = ValidDigitalCinemaName of DigitalCinemaName

type ArbDigitalCinemaNames =
    static member Valid = genDigitalCinemaName |> asArbOf ValidDigitalCinemaName
