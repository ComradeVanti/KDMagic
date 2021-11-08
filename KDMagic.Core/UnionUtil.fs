[<AutoOpen>]
module KDMagic.UnionUtil

open Microsoft.FSharp.Reflection

let tryParseUnion<'a> (s: string) =
    FSharpType.GetUnionCases typeof<'a>
    |> Array.tryFind (fun case -> case.Name = s)
    |> Option.map (fun case -> FSharpValue.MakeUnion(case, [||]) :?> 'a)
