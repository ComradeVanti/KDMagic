[<AutoOpen>]
module KDMagic.App.DomainTypes

open KDMagic

type Settings = { ImportFolderPath: FilePath }

type FileContent =
    | ValidKdm of Kdm
    | Other

type ImportedFile = FilePath * FileContent
