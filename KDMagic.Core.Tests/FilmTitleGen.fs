module KDMagic.Core.Tests.FilmTitleGen

open FsCheck

let private words =
    [
        "Apple"
        "Book"
        "Clown"
        "Disk"
        "Egg"
        "Face"
        "Glass"
        "House"
        "Ice"
        "Joker"
        "Key"
        "Mouse"
        "Ninja"
        "Orca"
        "Penguin"
        "Quest"
        "Rent"
        "Soul"
        "Tango"
        "UFO"
        "View"
        "Warrior"
        "XRay"
        "Year"
        "Zero"
    ]

let private genWordCount = Gen.choose (1, 3)

let private genWord = Gen.elements words

let private genWordCombinator: Gen<string -> string -> string> =
    Gen.elements [ (fun s1 s2 -> $"{s1}-{s2}")
                   (fun s1 s2 -> $"{s1}{s2}") ]

let private genFilmTitle =
    gen {
        let! count = genWordCount
        let! words = Gen.listOfLength count genWord
        let! combinator = genWordCombinator

        return words |> List.reduce combinator
    }
