[<AutoOpen>]
module KDMagic.TestUtil

open FsCheck

let (=?) other item =
    item = other |@ $"Expected \"{other}\" but got \"{item}\"."
