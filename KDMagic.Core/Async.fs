[<RequireQualifiedAccess>]
module KDMagic.Async

let map mapper a =
    async {
        let! res = a
        return mapper res
    }
