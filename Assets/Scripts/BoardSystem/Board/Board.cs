using HEX.Additional;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.BoardSystem
{
    public class PlacedEventArgs<TPosition, TPiece> : EventArgs
    {
        public TPosition ToPosition { get; }

        public TPiece Piece { get; }

        public PlacedEventArgs(TPosition toPosition, TPiece piece)
        {
            ToPosition = toPosition;
            Piece = piece;
        }
    }

    public class PushEventArgs<TPosition, TPiece> : EventArgs
    {
        public TPosition ToPosition { get; }
        public TPosition FromPosition { get; }
        public TPiece Piece { get; }

        public PushEventArgs(TPosition toPosition, TPosition fromPosition, TPiece piece)
        {
            ToPosition = toPosition;
            FromPosition = fromPosition;
            Piece = piece;
        }
    }

    public class HitEventArgs<TPosition, TPiece> : EventArgs
    {
        public TPosition FromPosition { get; }
        public TPiece Piece { get; }

        public HitEventArgs(TPosition fromPosition, TPiece piece)
        {
            FromPosition = fromPosition;
            Piece = piece;
        }
    }


    public class Board<TPosition, TPiece>
        where TPiece: ICharacter
    {
        //character
        public ICharacter Player;

        //event handlers
        public event EventHandler<PlacedEventArgs<TPosition, TPiece>> Placed;
        public event EventHandler<PushEventArgs<TPosition, TPiece>> Pushed;
        public event EventHandler<HitEventArgs<TPosition, TPiece>> Taken;

        private readonly BidirectionalDictionary<TPosition, TPiece> _positionPieces = new BidirectionalDictionary<TPosition, TPiece>();

        public bool Place(TPiece piece, TPosition toPosition)
        {
            if (TryGetPieceAt(toPosition, out _))
                return false;

            if (TryGetPositionOf(piece, out _))
                return false;

            if (_positionPieces.ContainsValue(piece))
                return false;
            if (_positionPieces.ContainsKey(toPosition))
                return false;

            _positionPieces.Add(toPosition, piece);
            OnPlaced(new PlacedEventArgs<TPosition, TPiece>(toPosition, piece));

            return true;
        }

        public bool Move(TPiece piece, TPosition toPosition)
        {
            if (TryGetPieceAt(toPosition, out _))
                return false;

            if (!TryGetPositionOf(piece, out var fromPosition) || !_positionPieces.Remove(piece))
                return false;

            _positionPieces.Add(toPosition, piece);
            OnMoved(new PushEventArgs<TPosition, TPiece>(toPosition, fromPosition, piece));

            return true;

        }

        public bool Hit(TPiece piece)
        {
            if (!TryGetPositionOf(piece, out var fromPosition))
                return false;

            if (!_positionPieces.Remove(piece))
                return false;

            OnHit(new HitEventArgs<TPosition, TPiece>(fromPosition, piece));
            //Destroy(piece);

            return true;

        }


        public bool TryGetPieceAt(TPosition position, out TPiece piece)
            => _positionPieces.TryGetValue(position, out piece);

        public bool TryGetPositionOf(TPiece piece, out TPosition position)
            => _positionPieces.TryGetKey(piece, out position);


        #region EventTriggers
        protected virtual void OnPlaced(PlacedEventArgs<TPosition, TPiece> eventArgs)
        {
            var handler = Placed;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnMoved(PushEventArgs<TPosition, TPiece> eventArgs)
        {
            var handler = Pushed;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnHit(HitEventArgs<TPosition, TPiece> eventArgs)
        {
            var handler = Taken;
            handler?.Invoke(this, eventArgs);
        }
        #endregion
    }
}
