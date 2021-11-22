module KDMagic.ParsingUtil

open System
open Microsoft.FSharp.Reflection

let tryParseUnion<'a> (s: string) =
    FSharpType.GetUnionCases typeof<'a>
    |> Array.tryFind (fun case -> case.Name = s)
    |> Option.map (fun case -> FSharpValue.MakeUnion(case, [||]) :?> 'a)

let tryParseInt (s: string) =
    try
        s |> int |> Some
    with
    | :? FormatException -> None

let tryParseDateTime s =
    try
        s |> DateTime.Parse |> Some
    with
    | :? FormatException -> None
