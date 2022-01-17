using HEX.Additional;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HEX.BoardSystem
{
    public class Grid<Position>
    {
        public int Rows { get; }

        public int Columns { get; }

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
        
        public BidirectionalDictionary<(int x, int y), Position> Positions = new BidirectionalDictionary<(int, int), Position>();
        //private Dictionary<(int x, int y), Position> _positions = new Dictionary<(int, int), Position>();

        public bool TryGetPositionAt(int x, int y, out Position position)
            => Positions.TryGetValue((x, y), out position);

        public bool TryGetCoordinateAt(Position position, out (int x, int y) coordinate)
            => Positions.TryGetKey(position, out coordinate);

        public void Register(int x, int y, Position position)
        {
            Positions.Add((x, y), position);
        }
    }
}
