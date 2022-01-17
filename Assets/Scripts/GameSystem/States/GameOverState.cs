using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using HEX.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HEX.GameSystem
{
    public class GameOverState : GameStateBase
    {
        GameObject _screen;

        public GameOverState(StateMachine<GameStateBase> stateMachine, GameObject screen) : base(stateMachine)
        {
            _screen = screen;
        }

        public override void OnEnter()
        {
            _screen.SetActive(true);
        }

        public override void OnExit()
        {
            //_screen.SetActive(false);
            //base.OnExit();
        }
    }
}
