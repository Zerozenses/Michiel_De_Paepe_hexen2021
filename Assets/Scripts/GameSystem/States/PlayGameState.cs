using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using HEX.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.GameSystem
{
    public class PlayGameState : GameStateBase
    {
        public const string Name = "PlayGame";

        private Player Player;

        private GameObject _deck;


        public PlayGameState(StateMachine<GameStateBase> stateMachine, GameObject deck, ICharacter Player): base(stateMachine)
        {
            
        }

        public override void OnEnter()
        {
            _deck.SetActive(true);
            
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Select(ICard card)
        {
            base.Select(card);
        }

    }
}
