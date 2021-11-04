namespace KDMagic

open FsCheck.Xunit
open KDMagic.LanguageGen

[<Properties(Arbitrary = [| typeof<ArbLanguages> |])>]
module LanguageGenTests =

    [<Property>]
    let ``Generated languages are valid`` (ValidLanguage language) =
        language |> Language.value |> Language.isValid
