namespace KDMagic

open FsCheck
open FsCheck.Xunit
open KDMagic.CTTGen
open KDMagic.DCNGen

[<Properties(Arbitrary = [| typeof<ArbDCNs> |])>]
module ParseDCNTests =

    [<Property>]
    let ``Digital cinema-names are parsed correctly``
        (ValidDCN original)
        =
        let ctt = original |> toContentTitleText

        let matchesOriginal parsed =

            let compare selector label =
                let first = original |> selector
                let second = parsed |> selector

                (first = second)
                |@ $"{label} parsed incorrectly (Wanted {first}, but got {second})"

            compare (fun it -> it.FilmTitle) "Film-title"
            .&. compare (fun it -> it.ContentType) "Content-type"
            .&. compare (fun it -> it.VersionNumber) "Version-number"
            .&. compare (fun it -> it.Dimension) "Dimension"
            .&. compare (fun it -> it.ProjectorAspect) "Projector-aspect"
            .&. compare (fun it -> it.AudioLanguage) "Audio-language"
            .&. compare (fun it -> it.SubtitleLanguage) "Subtitle-language"
            .&. compare (fun it -> it.Captions) "Captions"
            .&. compare (fun it -> it.AudioFormat) "Audio-format"
            .&. compare (fun it -> it.Resolution) "Resolution"
            .&. compare (fun it -> it.PackageType) "Package-type"
            .&. (parsed = original)
            |@ "An unknown property did not match"

        match ctt |> ParseDCN.tryParse with
        | Ok parsed -> parsed |> matchesOriginal
        | Error e -> false |@ $"Could not parse. Error: {e}"
        |@ $"CTT: {ctt}"
