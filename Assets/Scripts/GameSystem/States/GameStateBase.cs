using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using HEX.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.GameSystem
{
    public abstract class GameStateBase : IGameState<GameStateBase>
    {
        public StateMachine<GameStateBase> StateMachine { set; protected get; }
        protected GameStateBase(StateMachine<GameStateBase> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnExit() { }

        public virtual void OnEnter() { }

        public virtual void Backward() { }

        public virtual void Forward() { }

        public virtual void Select(ICard card) { }

        public virtual void Select(Position position) { }

    }
}
