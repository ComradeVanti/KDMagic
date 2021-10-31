module KDMagic.TestFileGen

open System
open System.IO
open FsCheck

let private testFileFolderPath =
    Path.Combine(AppContext.BaseDirectory, "TestFiles")

let private testFilePaths =
    if Directory.Exists testFileFolderPath then
        Directory.GetFiles testFileFolderPath |> Array.toList
    else
        failwith "No test-files found!"

let genFilePath = Gen.elements testFilePaths

type TestFilePath = TestFilePath of string

type ArbFilePaths =
    static member Test = genFilePath |> Gen.map TestFilePath |> Arb.fromGen
