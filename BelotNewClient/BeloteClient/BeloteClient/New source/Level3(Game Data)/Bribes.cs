using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeloteClient
{
    public class Bribe
    {
        private Card[] cards;

        public Bribe()
        {
            cards = new Card[4];
        }

        public bool IsFull()
        {
            for (var i = 0; i < 4; i++)
            {
                if (cards[i] == null)
                    return false;
            }
            return true;
        }

        public void PutCard(Card card, int place)
        {
            cards[place - 1] = card;
        }

        public Card this[int Place]
        {
            get
            {
                if ((Place >= 0) && (Place < 4))
                    return cards[Place];
                else
                    return null;
            }
        }
    }

    public class Bribes
    {
        private List<Bribe> list;
        
        public Bribes()
        {
            list = new List<Bribe>();
        }

        private void Add()
        {
            list.Add(new Bribe());
        }

        public void NewDistribution()
        {
            list.Clear();
        }

        public void PutCard(Card card, int place)
        {
            if (list.Count == 0)
                Add();
            if (CurrentBribe.IsFull())
                Add();
            CurrentBribe.PutCard(card, place);
        }

        public Bribe CurrentBribe
        {
            get
            {
                if (list.Count > 0)
                    return list[list.Count - 1];
                else
                    return null;
            }
        }

        public Bribe PredBribe
        {
            get
            {
                if (list.Count > 1)
                    return list[list.Count - 2];
                else
                    return null;
            }
        }

        public Bribe this[int Index]
        {
            get
            {
                return list[Index];
            }
        }
    }
}
