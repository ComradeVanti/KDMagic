module KDMagic.LanguageGen

open FsCheck

let private languages = [ "DE"; "EN"; "FR" ]

let genLanguage = Gen.elements languages |> Gen.map Language.make

type ValidLanguage = ValidLanguage of Language.T

type ArbLanguages =
    static member Valid = genLanguage |> asArbOf ValidLanguage
