using HEX.Additional;
using HEX.CardSystem;
using HEX.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HEX.GameSystem
{
    class KnockbackAction : MoveBase
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
                    //get coordinates player and enemy
                    grid.TryGetCoordinateAt(hex, out var enemycoordinate);
                    board.TryGetPositionOf(board.Player, out var playerPosition);
                    grid.TryGetCoordinateAt(playerPosition, out var playercoordinate);

                    //calculate distance
                    var distanceX = enemycoordinate.x - playercoordinate.x ;
                    var distanceY = enemycoordinate.y - playercoordinate.y;

                    //add distance to enemy
                    //enemycoordinate.x += distanceX;
                    //enemycoordinate.y += distanceY;
                    if (grid.TryGetPositionAt(enemycoordinate.x + distanceX,enemycoordinate.y + distanceY, out var targetPosition))
                    {
                        if (grid.Positions.ContainsValue(targetPosition))
                        {
                            board.Move(enemyPiece, targetPosition);
                        }
                    }
                    else
                    {
                        board.Hit(enemyPiece);
                    }
                    //Position enemyPosition = enemyPiece.Position;
                    //enemyPosition.X += enemycoordinate.x;
                    //enemyPosition.Y += enemycoordinate.y;

                    //board.Move(enemyPiece, enemyPosition);
                    // enemyPiece.Position = enemyPosition;

                    //if (grid.Positions.ContainsValue(targetPosition) && !targetPosition.Equals(null))
                    //{
                    //    board.Move(enemyPiece, targetPosition);
                    //}
                    //else
                    //{
                    //    board.Hit(enemyPiece);
                    //}

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