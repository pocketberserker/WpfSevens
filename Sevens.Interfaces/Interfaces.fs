namespace Sevens.Interfaces

open System.Collections.Generic

type CardTypeEnum =
  | Spades = 0
  |Hearts = 1
  | Clubs = 2
  | Diamonds = 3

[<AllowNullLiteral>]
type ICard =
  abstract member CardType: CardTypeEnum
  abstract member CardNumber: int

type IPlayer =
  abstract member GetPalyerName: unit -> string
  abstract member GetPalyerImageName: unit -> string
  abstract member GetPutCard: IList<ICard> * IList<ICard> -> ICard
