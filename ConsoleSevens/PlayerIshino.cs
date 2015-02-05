using System;
using System.Collections.Generic;
using System.Linq;
using Sevens.Interfaces;

namespace WpfSevens
{
    public class PlayerIshino : IPlayer
	{
        const int 最大のパスの回数 = 3;

        int パスの回数 { get; set; }

        bool パス可能
        {
            get { return パスの回数 < 最大のパスの回数; }
        }

        bool パス()
        {
            if (パス可能) {
                パスの回数++;
                return true;
            }
            return false;
        }

        //Random random = new Random();

        public string GetPalyerName()
        {
            return "石野 光仁";
        }

        public string GetPalyerImageName()
        {
            return "ishino.png";
        }

        public ICard GetPutCard(IList<ICard> 手札, IList<ICard> 場札)
        {
            var 出す札 = 小島.戦略その1.出す札(手札, 場札, パス可能);
            if (出す札 == null)
                パス();
            return 出す札;
            
            //var cards = Table.GetPutPossibleCards(playerCards, putCards);

            //if (cards.Count == 0)
            //    return null;
            //else
            //    return cards[random.Next(cards.Count)];
        }
    }
}
