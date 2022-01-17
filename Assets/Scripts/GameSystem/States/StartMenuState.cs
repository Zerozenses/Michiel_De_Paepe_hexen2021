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
            public StartMenuState(StateMachine<GameStateBase> stateMachine, GameObject menu, Button button) : base(stateMachine)
            {

            }
        }
}
