namespace Sevens.FSharp.Ai

open System.Linq
open Sevens.Interfaces
open Sevens.Core

module private Option =

  let orElse (defaultValue: unit -> _ option) v =
    match v with
    | Some v -> Some v
    | None -> defaultValue ()

  let getOrElse (defaultValue: unit -> _) v =
    match v with
    | Some v -> v
    | None -> defaultValue ()

module private Card =

  type OrdSeven = | High | Low

  let terminalDistance (c: ICard) =
    if c.CardNumber > 7 then Card.END_CARD_NUMBER - c.CardNumber
    else c.CardNumber - Card.START_CARD_NUMBER

  let tryFindAdjacentTop (cs: ICard seq) =
    let next (cs: _ seq) (c: ICard) =
      if c.CardNumber > 7 then (List.ofSeq cs |> List.rev |> List.toSeq, (<), fun x -> x - 1)
      else (cs, (>), fun x -> x + 1)
    let f compare next (xs: ICard seq) (c: ICard) =
      xs |> Seq.filter (fun x -> compare x.CardNumber (next c.CardNumber))
    let rec inner compare next acc (xs: ICard seq) (c: ICard) =
      if Seq.length xs <= 1 then acc
      else
        match acc, xs |> Seq.tryFind (fun x -> next c.CardNumber = x.CardNumber) with
        | _, Some v -> inner compare next (Some v) (f compare next xs c) v
        | Some _, None -> acc
        | None, None ->
          let xs = f compare next xs c
          if Seq.isEmpty xs then None
          else inner compare next acc xs (Seq.head xs)
    let cs = cs |> Seq.sortBy (fun c -> c.CardNumber)
    if Seq.length cs > 1 then
      let cs, compare, next = next cs (Seq.head cs)
      Seq.head cs |> inner compare next None cs
    else None

  let tryFindAdjacentAndMinDistance (playerCards: ICard seq) (cs: ICard seq) =
    let filtered =
      playerCards
      |> Seq.groupBy (fun c -> (c.CardType, if c.CardNumber > 7 then High else Low))
      |> Seq.choose (snd >> tryFindAdjacentTop >> Option.bind (fun x -> Seq.tryFind ((=) x) cs))
    if Seq.isEmpty filtered then None
    else Some(Seq.minBy terminalDistance filtered)

module private Pocketberserker =

  let tryFindLastCard (putPossibleCards: ICard seq) (remainingCards: ICard seq) =
    let cs =
      putPossibleCards
      |> Seq.map (fun c ->
        let count =
          remainingCards
          |> Seq.filter (fun x ->
            if c.CardNumber > 7 then c.CardNumber < x.CardNumber
            else c.CardNumber > x.CardNumber)
          |> Seq.length
        (c, count))
    if Seq.isEmpty cs then None
    else Some(cs |> Seq.minBy snd |> fst)

type PlayerPocketberserker() =
  let passCount = ref 0
  interface IPlayer with
    member __.GetPalyerName() = "もみあげ"
    member __.GetPalyerImageName() = "pocketberserker.png"
    member __.GetPutCard(playerCards, putCards) =
      let cs = Table.GetPutPossibleCards(playerCards, putCards)
      if Seq.isEmpty cs || (!passCount < 3 && Seq.length cs = 1) then
        incr passCount
        null
      else
        let remaining = Card.GetCards().Select(fun x -> x :> ICard).Except(putCards).Except(cs).ToList()
        Card.tryFindAdjacentAndMinDistance playerCards cs
        |> Option.orElse (fun () -> Pocketberserker.tryFindLastCard cs remaining)
        |> Option.getOrElse (fun () -> Seq.minBy Card.terminalDistance cs)
