using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sevens.Interfaces;
using Sevens.Core;

namespace WpfSevens
{
    public class PlayerSuzuki : IPlayer
	{
        Random random = new Random();

        public string GetPalyerName()
        {
            return "鈴木";
        }

        public string GetPalyerImageName()
        {
            return "suzuki.png";
        }

        public ICard GetPutCard(IList<ICard> playerCards, IList<ICard> putCards)
        {
            var cards = Table.GetPutPossibleCards(playerCards, putCards);

            if (cards.Count == 0)
                return null;
            else
                return cards[random.Next(cards.Count)];
        }
    }
}
