using HEX.CardSystem;
using HEX.BoardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HEX.GameSystem
{
    class SwipeAction : MoveBase
    {
        public ICard _card;
        public override bool CanExecute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            _card = card;
            return true;
        }

        public override void Execute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
        {
            foreach (var hex in IsolatedPositions(board, grid, piece, _card, position))
            {
                if(board.TryGetPieceAt(hex, out var enemyPiece))
                {
                    board.Hit(enemyPiece);
                }
            }
        }

        public override List<Position> Positions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
                    .West()
                    .NorthWest()
                    .SouthWest()
                    .East()
                    .NorthEast()
                    .SouthEast();

            return movementHelper.CollectValidPositions();
        }

        public override List<Position> IsolatedPositions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
               .IsolatedNorthEast(position, 10)
               .IsolatedNorthWest(position, 10)
               .IsolatedSouthEast(position, 10)
               .IsolatedSouthWest(position, 10)
               .IsolatedWest(position, 10)
               .IsolatedEast(position, 10);

            return movementHelper.CollectIsolatedPositions();

        }

    }
}
