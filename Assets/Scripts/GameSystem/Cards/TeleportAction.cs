using HEX.CardSystem;
using HEX.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HEX.GameSystem
{
    //Teleport
     class TeleportAction : MoveBase
    {
        public override bool CanExecute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            //return base.CanExecute(board, grid, piece, card);
            return true;
        }

        public override void Execute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
        {
            board.Move(piece, position);
            Debug.Log(piece.Position.X + "," + piece.Position.Y + "," + piece.Position.Z + " moved to " + position.X + "," + position.Y + "," + position.Z);
        }
        public override List<Position> Positions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
               .Warp();

            return movementHelper.CollectValidPositions();

            //List<Position positions = movementHelper.CollectValidPositions
            //return new List<Position>
        }

        public override List<Position> IsolatedPositions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            MovementHelper movementHelper = new MovementHelper(board, grid, position);
            movementHelper
               .Warp();

            return movementHelper.CollectValidPositions();

            //List<Position positions = movementHelper.CollectValidPositions
            //return new List<Position>
        }


    }
}
