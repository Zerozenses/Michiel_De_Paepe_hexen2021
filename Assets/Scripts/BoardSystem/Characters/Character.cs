using HEX.Additional;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HEX.BoardSystem
{
    public class CharacterEventArgs<Position> : EventArgs
    {
        public Position Pos { get; }
        public CharacterEventArgs(Position position)
        {
            Pos = position;
        }
    }

    public interface ICharacter
    {
        int CharacterID { get; }
        public CharacterType CharacterType { get; }
        public Position Position { get; }
        public void Hitting();
    }

    public class Character: MonoBehaviour, ICharacter
    {
        public event EventHandler<CharacterEventArgs<Hex>> Hit;
        public event EventHandler<CharacterEventArgs<Hex>> Teleported;
        public event EventHandler<CharacterEventArgs<Hex>> Pushback;
        public event EventHandler<CharacterEventArgs<Hex>> Die;

        public int CharacterID { get; set; }
        public CharacterType CharacterType { get; set; }

        public Position Position { get; set; }

        public void Teleport(Hex pos)
        {
            OnTeleported(new CharacterEventArgs<Hex>(pos));
        }

        public void Pushing(Hex toPos)
        {
            OnPushed(new CharacterEventArgs<Hex>(toPos));
        }

        public void Hitting()
        {
            Destroy(gameObject);
        }

        protected virtual void OnTeleported(CharacterEventArgs<Hex> e)
        {
            var handler = Teleported;
            handler?.Invoke(this, e);
        }

        protected virtual void OnPushed(CharacterEventArgs<Hex> e)
        {
            var handler = Pushback;
            handler?.Invoke(this, e);
        }

        protected virtual void OnImpact(CharacterEventArgs<Hex> e)
        {
            var handler = Hit;
            handler?.Invoke(this, e);
        }
    }
}
