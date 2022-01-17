using HEX.Additional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.StateSystem
{
    public interface IGameState<TGameState> where TGameState : class, IGameState<TGameState>
    {
        void OnExit();

        void OnEnter();

        StateMachine<TGameState> StateMachine { set; }
    }
}
