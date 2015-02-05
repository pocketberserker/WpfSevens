namespace Sevens.FSharp.Ai

open Sevens.Interfaces
open Sevens.Core

type PlayerPocketberserker() =
  let passCount = ref 0
  interface IPlayer with
    member __.GetPalyerName() = "もみあげ"
    member __.GetPalyerImageName() = "pocketberserker.png"
    member __.GetPutCard(playerCards, putCards) =
      let cs = Table.GetPutPossibleCards(playerCards, putCards)
      if Seq.isEmpty cs || !passCount < 1 then
        incr passCount
        null
      else
        Seq.head cs
