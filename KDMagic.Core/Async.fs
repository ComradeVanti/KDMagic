[<RequireQualifiedAccess>]
module KDMagic.Async


let map mapper task =
    async {
        let! res = task
        return mapper res
    }

let merge tasks = tasks |> Async.Parallel |> (map Array.toList)
