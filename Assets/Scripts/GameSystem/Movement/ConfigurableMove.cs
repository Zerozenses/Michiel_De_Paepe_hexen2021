using HEX.BoardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEX.CardSystem;

namespace HEX.GameSystem
{
    class ConfigurableMove : MoveBase
    {
        public delegate List<Position> PositionCollector(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter character, ICard card, Position position);

        private PositionCollector _positionCollector;

        public ConfigurableMove(Board<Position, ICharacter> board, Grid<Position> grid, Position tile, PositionCollector positionCollector) 
        {
            _positionCollector = positionCollector;
        }

        public override List<Position> Positions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter character, ICard card, Position position)
         => _positionCollector(board, grid, character, card, position);

        public override List<Position> IsolatedPositions(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter character, ICard card, Position position)
         => _positionCollector(board, grid, character, card, position);
    }
}
