[<RequireQualifiedAccess>]
module KDMagic.Seq

let countItem x = Seq.filter ((=) x) >> Seq.length
