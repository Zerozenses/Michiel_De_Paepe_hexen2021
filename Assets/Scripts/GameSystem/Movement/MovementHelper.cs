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
    public class MovementHelper
    {
        public delegate bool Validator(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position);

        private Board<Position, ICharacter> _board;
        private Grid<Position> _grid;
        private ICharacter _piece;
        private ICard _card;
        private Position _tilePosition;

        private List<Position> _validPositions = new List<Position>();
        private List<Position> _isolatedPositions = new List<Position>();
        //private Vector2[] _directions = new Vector2[6];

        public List<(int x, int y)> _offset = new List<(int x, int y)>();

        public MovementHelper(Board<Position, ICharacter> board, Grid<Position> grid, Position tile)
        {
            _board = board;
            _grid = grid;
            _tilePosition = tile;

            #region offsets
            _offset.Add((1, 0));
            _offset.Add((0, 1));
            _offset.Add((-1, 1));
            _offset.Add((-1, 0));
            _offset.Add((0, -1));
            _offset.Add((1, -1));
            #endregion
        }

        #region normal movements
        public MovementHelper NorthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, 1, numTiles, validators);

        public MovementHelper East(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, 0, numTiles, validators);

        public MovementHelper SouthEast(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, -1, numTiles, validators);

        public MovementHelper SouthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, -1, numTiles, validators);

        public MovementHelper West(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 0, numTiles, validators);

        public MovementHelper NorthWest(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 1, numTiles, validators);

        #endregion

        #region isolated movements
        public MovementHelper IsolatedNorthEast(Position position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 0, 1, numTiles, validators);

        public MovementHelper IsolatedEast(Position position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 1, 0, numTiles, validators);

        public MovementHelper IsolatedSouthEast(Position position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 1, -1, numTiles, validators);

        public MovementHelper IsolatedSouthWest(Position position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 0, -1, numTiles, validators);

        public MovementHelper IsolatedWest(Position positon, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(positon, -1, 0, numTiles, validators);

        public MovementHelper IsolatedNorthWest(Position position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, -1, 1, numTiles, validators);
        #endregion

        #region direction movements
        public MovementHelper Direction0(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[0].x, (int)_directions[0].y, 0, numTiles, validators);

        public MovementHelper Direction1(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[1].x, (int)_directions[1].y, 1, numTiles, validators);

        public MovementHelper Direction2(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[2].x, (int)_directions[2].y, 2, numTiles, validators);

        public MovementHelper Direction3(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[3].x, (int)_directions[3].y, 3, numTiles, validators);
        public MovementHelper Direction4(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[4].x, (int)_directions[4].y, 4, numTiles, validators);

        public MovementHelper Direction5(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[5].x, (int)_directions[5].y, 5, numTiles, validators);

        public Vector2 GetNextDirectionDown(int currentDirection)
        {
            if (currentDirection - 1 < 0)
            {
                return _directions[5];
            }

            else return _directions[currentDirection - 1];
        }

        public Vector2 GetNextDirectionUp(int currentDirection)
        {
            if (currentDirection + 1 > 5)
            {
                return _directions[0];
            }

            else return _directions[currentDirection + 1];
        }

        public Vector2[] _directions =
            new Vector2[6]
            {
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,-1),
                new Vector2(0,-1),
                new Vector2(-1,0),
                new Vector2(-1,1)
            };


        #endregion

        #region bomb movements
        public MovementHelper BombDirection0(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[0].x, (int)_directions[0].y, 0, _tilePosition, numTiles, validators);

        public MovementHelper BombDirection1(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[1].x, (int)_directions[1].y, 1, _tilePosition, numTiles, validators);

        public MovementHelper BombDirection2(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[2].x, (int)_directions[2].y, 2, _tilePosition, numTiles, validators);

        public MovementHelper BombDirection3(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[3].x, (int)_directions[3].y, 3,  _tilePosition, numTiles, validators);
        public MovementHelper BombDirection4(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[4].x, (int)_directions[4].y, 4, _tilePosition, numTiles, validators);

        public MovementHelper BombDirection5(int numTiles = int.MaxValue, params Validator[] validators)
            => MoveDirections((int)_directions[5].x, (int)_directions[5].y, 5, _tilePosition, numTiles, validators);

        public Vector2 GetBombNextDirectionDown(int currentDirection)
        {
            if (currentDirection - 1 < 0)
            {
                return _bombDirections[5];
            }

            else return _bombDirections[currentDirection - 1];
        }

        public Vector2 GetNextBombDirectionUp(int currentDirection)
        {
            if (currentDirection + 1 > 5)
            {
                return _bombDirections[0];
            }

            else return _bombDirections[currentDirection + 1];
        }

        public Vector2[] _bombDirections =
            new Vector2[6]
            {
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,-1),
                new Vector2(0,-1),
                new Vector2(-1,0),
                new Vector2(-1,1)
            };
        #endregion

        public MovementHelper Warp()
        {
            _validPositions.Add(_tilePosition);
            return this;
        }

        public MovementHelper Move(int xOffset, int yOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {

            if (!_board.TryGetPositionOf(_board.Player, out var position))
                return this;

            if (!_grid.TryGetCoordinateAt(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            int step = 0;
            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _board.Player, position));
                if (!isOk)
                    return this;

                var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                _validPositions.Add(nextPosition);

                nextXCoordinate += xOffset;
                nextYCoordinate += yOffset;

                hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out nextPosition);

                step++;
            }
            return this;
        }

        public MovementHelper IsolatedMove(Position position, int xOffset, int yOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {
            var tempList = new List<Position>();

            if (!_board.TryGetPositionOf(_board.Player, out var playerposition))
                return this;

            if (!_grid.TryGetCoordinateAt(playerposition, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            int step = 0;
            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _board.Player, position));
                if (!isOk)
                    return this;

                //var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                tempList.Add(nextPosition);

                nextXCoordinate += xOffset;
                nextYCoordinate += yOffset;

                hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out nextPosition);

                step++;
            }

            if (tempList.Contains(position))
            {
                //_isolatedPositions.Equals(validPositions);
                _isolatedPositions = tempList;
            }

            return this;
        }

        public MovementHelper MoveDirections(int xOffset, int yOffset, int directionNumber, int numTiles = int.MaxValue, params Validator[] validators)
        {
            var directionList = new List<Position>();
            if (!_board.TryGetPositionOf(_board.Player, out var position))
                return this;

            if (!_grid.TryGetCoordinateAt(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            var isOk = validators.All((v) => v(_board, _grid, _board.Player, position));
            if (!isOk)
                return this;

            if (nextPosition.Equals(_tilePosition))
            {
                directionList.Add(nextPosition);

                //up position
                Vector2 upPosition = GetNextDirectionUp(directionNumber);
                var upX = coordinate.x + (int)upPosition.x;
                var upY = coordinate.y + (int)upPosition.y;
                if (_grid.TryGetPositionAt(upX, upY, out var upperPosition))
                {
                    directionList.Add(upperPosition);
                }
                else
                {
                    //if(upperPosition.Equals()
                    //directionList.Add(upperPosition);
                }


                //down position
                Vector2 downPosition = GetNextDirectionDown(directionNumber);
                var downX = coordinate.x + (int)downPosition.x;
                var downY = coordinate.y + (int)downPosition.y;
                if (_grid.TryGetPositionAt(downX, downY, out var lowerPosition))
                {
                    directionList.Add(lowerPosition);
                }
                else
                {
                    //directionList.Add(lowerPosition);
                }


                //check valid positions
                _isolatedPositions = directionList;
            }

            return this;

        }

        public MovementHelper MoveDirections(int xOffset, int yOffset, int directionNumber, Position pos, int numTiles = int.MaxValue, params Validator[] validators)
        {

            Position position = _tilePosition;


            if (!_grid.TryGetCoordinateAt(position, out var coordinate))
                return this;

            var nextXCoordinate = coordinate.x + xOffset;
            var nextYCoordinate = coordinate.y + yOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out var nextPosition);
            int step = 0;
            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _board.Player, position));
                if (!isOk)
                    return this;

                //var hasPiece = _board.TryGetPieceAt(nextPosition, out var nextPiece);
                _validPositions.Add(nextPosition);

                nextXCoordinate += xOffset;
                nextYCoordinate += yOffset;

                hasNextPosition = _grid.TryGetPositionAt(nextXCoordinate, nextYCoordinate, out nextPosition);

                step++;
            }
            return this;

        }

        public List<Position> CollectValidPositions()
        {
            return _validPositions;
        }

        public List<Position> CollectIsolatedPositions()
        {
            return _isolatedPositions;
        }

        public static bool IsEmptyTile(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
            => !board.TryGetPieceAt(position, out _);

        public static bool HasEnemyPiece(Board<Position, ICharacter> board, Grid<Position> grid, ICharacter piece, Position position)
        {
            if (!board.TryGetPieceAt(position, out var enemyPiece))
                return false;
                
            return enemyPiece.CharacterType != CharacterType.Player;
        }


    }
}
