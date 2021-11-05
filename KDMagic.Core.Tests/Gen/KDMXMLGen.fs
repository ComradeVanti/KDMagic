module KDMagic.KDMXMLGen

open FsCheck
open ContentTitleTextGen

let private template =
    """<?xml version="1.0" encoding="UTF-8" standalone="no" ?>
    <DCinemaSecurityMessage xmlns:dsig="http://www.w3.org/2000/09/xmldsig#"
                            xmlns:enc="http://www.w3.org/2001/04/xmlenc#"
                            xmlns="http://www.smpte-ra.org/schemas/430-3/2006/ETM">
        <AuthenticatedPublic>
            <RequiredExtensions>
                <KDMRequiredExtensions xmlns="http://www.smpte-ra.org/schemas/430-1/2006/KDM">
                    <ContentTitleText>[CTT]</ContentTitleText>
                </KDMRequiredExtensions>
            </RequiredExtensions>
        </AuthenticatedPublic>
    </DCinemaSecurityMessage>"""

let genKDMXML =
    gen {
        let! contentTitleText = genContentTitleText
        return template.Replace("[CTT]", contentTitleText)
    }

type ValidKDMXML = ValidKDMXML of string

type ArbKDMXML =
    static member Valid = genKDMXML |> asArbOf ValidKDMXML
