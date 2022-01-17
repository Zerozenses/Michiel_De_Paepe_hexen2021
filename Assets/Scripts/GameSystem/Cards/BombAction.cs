using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace HEX.GameSystem
{
    class BombAction: MoveBase
    {
        ICard _card;
        public override bool CanExecute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            _card = card;
            //return base.CanExecute(board, grid, piece, card);
            return true;
        }

        public override void Execute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
        {
            foreach (var hex in IsolatedPositions(board, grid, piece, _card, position))
            {
                if (board.TryGetPieceAt(hex, out var enemyPiece))
                {
                    board.Hit(enemyPiece);
                }
                Debug.Log("Boom");
            }
        }

        public override List<Position> Positions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
                .Warp()
                .BombDirection0(1)
                .BombDirection1(1)
                .BombDirection2(1)
                .BombDirection3(1)
                .BombDirection4(1)
                .BombDirection5(1);
            return movementHelper.CollectValidPositions();
        }

        public override List<Position> IsolatedPositions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
                .Warp()
                .BombDirection0(1)
                .BombDirection1(1)
                .BombDirection2(1)
                .BombDirection3(1)
                .BombDirection4(1)
                .BombDirection5(1);
            return movementHelper.CollectValidPositions();
        }
    }
}
