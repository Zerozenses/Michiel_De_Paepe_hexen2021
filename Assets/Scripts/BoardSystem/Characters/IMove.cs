using HEX.Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEX.BoardSystem
{
    public interface IMove
    {
        List<Position> Positions(ICharacter piece);

        bool CanExecute(ICharacter piece);

        void Execute(ICharacter piece, Position toPosition);
    }
}
