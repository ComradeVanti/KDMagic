[<AutoOpen>]
module KDMagic.TestGenUtil

open FsCheck

let asArbOf wrapper generator = generator |> Gen.map wrapper |> Arb.fromGen
