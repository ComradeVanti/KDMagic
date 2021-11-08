module KDMagic.KDMXMLGen

open System
open FsCheck
open ContentTitleTextGen

let private baseDate = DateTime(2000, 1, 1)

let private template =
    """<?xml version="1.0" encoding="UTF-8" standalone="no" ?>
    <DCinemaSecurityMessage xmlns:dsig="http://www.w3.org/2000/09/xmldsig#"
                            xmlns:enc="http://www.w3.org/2001/04/xmlenc#"
                            xmlns="http://www.smpte-ra.org/schemas/430-3/2006/ETM">
        <AuthenticatedPublic>
            <RequiredExtensions>
                <KDMRequiredExtensions xmlns="http://www.smpte-ra.org/schemas/430-1/2006/KDM">
                    <ContentTitleText>[CTT]</ContentTitleText>
                    <ContentKeysNotValidBefore>[Start]</ContentKeysNotValidBefore>
                    <ContentKeysNotValidAfter>[End]</ContentKeysNotValidAfter>
                </KDMRequiredExtensions>
            </RequiredExtensions>
        </AuthenticatedPublic>
    </DCinemaSecurityMessage>"""

let private genDateAfter (start: DateTime) =
    Gen.choose (1, 3600) |> Gen.map (float >> start.AddDays)

let private formatDate (date: DateTime) =
    date.ToString "yyyy-MM-ddThh:mm:ss+00:00"

let genKDMXML =
    gen {
        let! contentTitleText = genContentTitleText
        let! startDate = genDateAfter baseDate
        let! endDate = genDateAfter startDate

        return
            template
                .Replace("[CTT]", contentTitleText)
                .Replace("[Start]", startDate |> formatDate)
                .Replace("[End]", endDate |> formatDate)
    }

type ValidKDMXML = ValidKDMXML of string

type ArbKDMXML =
    static member Valid = genKDMXML |> asArbOf ValidKDMXML
