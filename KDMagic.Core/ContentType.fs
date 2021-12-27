[<Microsoft.FSharp.Core.RequireQualifiedAccess>]
module KDMagic.ContentType

let str =
    function
    | FTR -> "FTR"
    | TLR -> "TLR"
    | TSR -> "TSR"
    | TST -> "TST"
    | RTG_F -> "RTG-F"
    | RTG_T -> "RTG-T"
    | ADV -> "ADV"
    | SHR -> "SHR"
    | XSN -> "XSN"
    | PSA -> "PSA"
    | POL -> "POL"
    | CLP -> "CLP"
    | PRO -> "PRO"
    | STR -> "STR"
    | EPS -> "EPS"
    | HLT -> "HLT"
    | EVT -> "EVT"
