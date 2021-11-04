module KDMagic.FilmTitleGen

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
        "Pasta"
        "Quest"
        "Rent"
        "Soul"
        "Tango"
        "UFO"
        "View"
        "Wee"
        "XRay"
        "Year"
        "Zero"
    ]

let genFilmTitle =

    let genWord = Gen.elements words

    let genWordCombinator: Gen<string -> string -> string> =
        Gen.elements [ (fun s1 s2 -> $"{s1}-{s2}")
                       (fun s1 s2 -> $"{s1}{s2}") ]

    gen {
        let! words = Gen.listOfLength 2 genWord
        let! combinator = genWordCombinator

        return words |> List.reduce combinator |> FilmTitle.make
    }

type ValidFilmTitle = ValidFilmTitle of FilmTitle.T

type ArbFilmTitles =
    static member Valid = genFilmTitle |> asArbOf ValidFilmTitle
