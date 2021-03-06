using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using HEX.StateSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HEX.GameSystem
{
        public class StartMenuState : GameStateBase
        {
            GameObject _menu;
            Button _button;
            public StartMenuState(StateMachine<GameStateBase> stateMachine, GameObject menu, Button button) : base(stateMachine)
            {
            _menu = menu;
            _button = button;
            }

            public override void OnEnter()
            {
                _menu.SetActive(true);

            }

            public override void OnExit()
            {
                _menu.SetActive(false);
                //base.OnExit();
            }


    }
}
