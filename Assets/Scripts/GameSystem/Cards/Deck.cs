using HEX.BoardSystem;
using HEX.CardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HEX.GameSystem
{

    public class Deck<CardType>
        where CardType : ICard
    {
        //private List<GameObject> _cardDeckImages;
        [SerializeField]
        private List<CardType> _cardDeckList = new List<CardType>();
        private System.Random _random = new System.Random();
        private  int _size = 5;
        private ICharacter piece = null;
        //private CardType UsedCard = null;

        public List<CardType> CardDeckList => _cardDeckList;

        // Start is called before the first frame update
        void Start()
        {
           
            //FillDeck();
        }

        // Update is called once per frame
        public void RegisterCard(CardType card)
        {
            card.Activate(false);
            _cardDeckList.Add(card);
        }

        public void FillDeck()
        {
            if (_cardDeckList.Count > _size-1) //don't add more after limit reached
            {
                for (int i = 0; i < _size; i++)
                    if (_cardDeckList.Count > -1)
                        ActivateCard(_cardDeckList[i]);
            }
        }

        public void ActivateCard(CardType card)
        {
            card.Activate(true);
        }

        public void DeactivateCard(CardType card)
        {
            card.Activate(false);
        }

        public void PlayCard(CardType card, Position position)
        {
            _cardDeckList.Remove(card);
            FillDeck();
            //return 
            //card.Activate(true);
        }


    }
}
