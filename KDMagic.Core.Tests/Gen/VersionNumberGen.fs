module KDMagic.VersionNumberGen

open FsCheck

let genVersionNumber = Gen.choose (1, 100) |> Gen.map VersionNumber.make

type ValidVersionNumber = ValidVersionNumber of VersionNumber.T

type ArbVersionNumbers =
    static member Valid = genVersionNumber |> asArbOf ValidVersionNumber
