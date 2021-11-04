module KDMagic.VersionNumberTests

open FsCheck.Xunit

[<Property>]
let ``Version-numbers must be at least 1`` number =
    number |> VersionNumber.isValid = (number >= 1)
