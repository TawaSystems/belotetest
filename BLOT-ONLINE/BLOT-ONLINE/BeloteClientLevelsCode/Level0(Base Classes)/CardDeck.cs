using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
	// Класс, содержащий в себе колоду карт
	class CardsDeck
	{
		private CardList list;
		private Random random;

		// Создание колоды для игры в блот
		public CardsDeck()
		{
			list = new CardList();
			random = new Random();
			// Добавляем в список все возможные карты в колоде
			foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
			{
				if (s == CardSuit.C_NONE)
					continue;
				foreach (CardType t in Enum.GetValues(typeof(CardType)))
				{
					if (t == CardType.C_UNDEFINED)
						continue;
					list.Add(new Card(t, s));
				}
			}
		}
			
		// Взятие случайной карты из колоды
		public Card GetRandomCard()
		{
			Card c = list[random.Next(list.Count)];
			list.Remove(c);
			return c;
		}
	}
}


