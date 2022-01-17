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
    class MoveManager
    {
        //fields
        private Board<Position, ICharacter> _board;
        private Grid<Position> _grid;
        private Position _hex;
        private MultiValueDictionary<CardType, MoveBase> _cardMoves = new MultiValueDictionary<CardType, MoveBase>();
        private MultiValueDictionary<CardType, MoveBase> _adjustedMoves = new MultiValueDictionary<CardType, MoveBase>();
        private List<Position> _validPositions = new List<Position>();
        private List<Position> _isolatedPositions = new List<Position>();

        //constructor
        public MoveManager(Board<Position, ICharacter> board, Grid<Position> grid)
        {
            _board = board;
            _grid = grid;

            InitializeMoves();
        }

        public List<Position> ValidPositions(ICard card, ICharacter piece, Position position)
        {
            _hex = position;
           _validPositions = _cardMoves[card.Type]
                  .Where(move => move.CanExecute(_board, _grid, piece, card, position))
                .SelectMany(move => move.Positions(_board, _grid, piece, card, position))
                .ToList();
            return _validPositions;
        }

        public List<Position> IsolatedPositions(ICard card, ICharacter piece, Position position)
        {
            _hex = position;
            _isolatedPositions = _adjustedMoves[card.Type]
                 .Where(move => move.CanExecute(_board, _grid, piece, card, position))
                .SelectMany(move => move.IsolatedPositions(_board, _grid, piece, card, position))
                .ToList();
            return _isolatedPositions;
        }

        public void Move(ICard card, ICharacter player, Position position)
        {
            _hex = position;
            var move = _adjustedMoves[card.Type]
                 .Where(move => move.CanExecute(_board, _grid, player, card, position))
                 .FirstOrDefault(move => move.Positions(_board, _grid, player, card, position).Contains(position));

            if (move != null)
                move.Execute(_board, _grid, player, position);

        }

        private void InitializeMoves()
        {
            //teleport card
            _cardMoves.Add(CardType.Teleport, new TeleportAction());

            //Knockback card
            _cardMoves.Add(CardType.Knockback, new KnockbackAction());

            //Swing card
            _cardMoves.Add(CardType.Swing, new SwingAction());

            //Swipe card
            _cardMoves.Add(CardType.Swipe, new SwipeAction());

            //Bomb card
            _cardMoves.Add(CardType.Bomb, new BombAction());

            //isolated positions

            #region assign isolated positions
            //teleport card
            _adjustedMoves.Add(CardType.Teleport, new TeleportAction());
            //knockback card
            _adjustedMoves.Add(CardType.Knockback, new KnockbackAction());
            //swing card
            _adjustedMoves.Add(CardType.Swing, new SwingAction());
            //swipe card
            _adjustedMoves.Add(CardType.Swipe, new SwipeAction());
            //bomb card
            _adjustedMoves.Add(CardType.Bomb, new BombAction());

            #endregion
        }

    }
}
