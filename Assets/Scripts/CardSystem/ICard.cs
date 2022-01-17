
using HEX.Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HEX.CardSystem
{
    public interface ICard
    {
        string Name { get; }
        Texture2D CardImage { get; }
        public CardType Type { get; set; }
        void Activate(bool active);
    }
}
