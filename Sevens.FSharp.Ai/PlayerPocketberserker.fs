namespace Sevens.FSharp.Ai

open Sevens.Interfaces

type PlayerPocketberserker() =
  interface IPlayer with
    member __.GetPalyerName() = "もみあげ"
    member __.GetPalyerImageName() = "pocketberserker.png"
    member __.GetPutCard(playerCards, putCards) =
      null
