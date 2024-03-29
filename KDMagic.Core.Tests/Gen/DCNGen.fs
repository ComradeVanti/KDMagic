﻿module KDMagic.DCNGen

open FsCheck
open KDMagic.FilmTitleGen
open KDMagic.VersionNumberGen
open KDMagic.LanguageGen

let genDCN =
    gen {
        let! filmTitle = genFilmTitle
        let! contentType = Arb.generate<ContentType>
        let! versionNumber = Gen.optionOf genVersionNumber
        let! dimension = Arb.generate<Dimension>
        let! projectorAspect = Arb.generate<ProjectorAspect>
        let! audioLanguage = genLanguage
        let! subTitleLanguage = Gen.optionOf genLanguage
        let! captions = Gen.optionOf Arb.generate<Captions>
        let! audioFormat = Arb.generate<AudioFormat>
        let! resolution = Arb.generate<Resolution>
        let! packageType = Arb.generate<PackageType>

        return
            { FilmTitle = filmTitle
              ContentType = contentType
              VersionNumber = versionNumber
              Dimension = dimension
              ProjectorAspect = projectorAspect
              AudioLanguage = audioLanguage
              SubtitleLanguage = subTitleLanguage
              Captions = captions
              AudioFormat = audioFormat
              Resolution = resolution
              PackageType = packageType }
    }

type ValidDCN = ValidDCN of DCN

type ArbDCNs =
    static member Valid = genDCN |> asArbOf ValidDCN
