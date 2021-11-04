namespace KDMagic

open FsCheck.Xunit
open KDMagic.VersionNumberGen

[<Properties(Arbitrary = [| typeof<ArbVersionNumbers> |])>]
module VersionNumberGenTests =

    [<Property>]
    let ``Generated version-numbers are valid`` (ValidVersionNumber number) =
        number |> VersionNumber.value |> VersionNumber.isValid
