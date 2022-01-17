using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HEX.GameSystem
{
    class SwingAction : MoveBase
    {
        public ICard _card;
        public override bool CanExecute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            //return base.CanExecute(board, grid, piece, card);
            _card = card;
            return true;
        }

        public override void Execute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
        {
            foreach (var hex in IsolatedPositions(board, grid, piece, _card, position))
            {
                if (board.TryGetPieceAt(hex, out var enemyPiece))
                {
                    Debug.Log("we be swinging");
                    board.Hit(enemyPiece);
                }
            }
        }

        public override List<Position> Positions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper.West(1)
                .NorthWest(1)
                .SouthWest(1)
                .East(1)
                .NorthEast(1)
                .SouthEast(1);
            return movementHelper.CollectValidPositions();
        }

        public override List<Position> IsolatedPositions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
                .Direction0(1)
                .Direction1(1)
                .Direction2(1)
                .Direction3(1)
                .Direction4(1)
                .Direction5(1);
            return movementHelper.CollectIsolatedPositions();
        }
    }
}
