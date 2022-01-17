using HEX.Additional;
using HEX.BoardSystem;
using HEX.CardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEX.GameSystem
{
    abstract class MoveBase
    {
        public virtual bool CanExecute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position)
        {
            var validTiles = Positions(board, grid, piece, card, position);
            return validTiles.Count > 0;
        }

        public virtual void Execute(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
        {
            if (!board.TryGetPositionOf(piece, out var fromPosition))
                return;

            var hasEnemyPiece = board.TryGetPieceAt(position, out var toPiece);
            //board.TryGetPositionOf(piece, out var fromPosition);

            Action forward = () =>
            {
                if (hasEnemyPiece)
                    board.Hit(toPiece);

                board.Move(piece, position);
            };

            Action backward = () =>
            {
                board.Move(piece, fromPosition);

                if (hasEnemyPiece)
                    board.Place(toPiece, position);
            };
        }

        public abstract List<Position> Positions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position);

        public abstract List<Position> IsolatedPositions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, ICard card, Position position);
    }
}
