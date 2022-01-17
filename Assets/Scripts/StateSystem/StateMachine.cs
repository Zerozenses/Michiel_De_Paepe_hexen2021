using HEX.Additional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.StateSystem
{
    public class StateMachine<TGameState> where TGameState : class, IGameState<TGameState>
    {
        private Dictionary<GameStates, TGameState> _gameStates = new Dictionary<GameStates, TGameState>();

        public TGameState CurrentState { get; internal set; }

        public void SetStartState(GameStates name)
        {
            CurrentState = _gameStates[name];
        }

        public void Register(GameStates name, TGameState gameState)
        {
            gameState.StateMachine = this;
            _gameStates.Add(name, gameState);
        }

        public void MoveToState(GameStates name)
        {
            CurrentState?.OnExit();

            CurrentState = _gameStates[name];
            CurrentState.OnEnter();
        }
    }
}
