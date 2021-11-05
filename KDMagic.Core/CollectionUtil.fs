[<AutoOpen>]
module KDMagic.CollectionUtil

let countItem x = Seq.filter ((=) x) >> Seq.length
