using HEX.Additional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.StateSystem
{
    public class StateMachine<TGameState> where TGameState : class, IGameState<TGameState>
    {
        private Dictionary<string, TGameState> _gameStates = new Dictionary<string, TGameState>();

        public TGameState CurrentState { get; internal set; }

        public void SetStartState(string name)
        {
            CurrentState = _gameStates[name];
        }

        public void AddState(string name, TGameState gameState)
        {
            gameState.StateMachine = this;
            _gameStates.Add(name, gameState);
        }

        public void MoveToState(string name)
        {
            CurrentState?.OnExit();

            CurrentState = _gameStates[name];
            CurrentState.OnEnter();
        }
    }
}
