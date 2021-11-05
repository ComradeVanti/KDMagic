[<RequireQualifiedAccess>]
module KDMagic.KDMDoc

open FSharp.Data

type DCinemaSecurityMessage =
    XmlProvider<"""

    <DCinemaSecurityMessage xmlns:dsig="http://www.w3.org/2000/09/xmldsig#"
                            xmlns:enc="http://www.w3.org/2001/04/xmlenc#"
                            xmlns="http://www.smpte-ra.org/schemas/430-3/2006/ETM">
        <AuthenticatedPublic>
            <RequiredExtensions>
                <KDMRequiredExtensions xmlns="http://www.smpte-ra.org/schemas/430-1/2006/KDM">
                    <ContentTitleText>Title</ContentTitleText>
                </KDMRequiredExtensions>
            </RequiredExtensions>
        </AuthenticatedPublic>
    </DCinemaSecurityMessage>

    """>

let tryParse text =
    try
        Ok(DCinemaSecurityMessage.Parse text)
    with
    | e -> Error e
