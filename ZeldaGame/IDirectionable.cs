using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame
{
    public interface IDirectionable
    {
        Vector2 Position
        {
            get;
            set;
        }

        Direction Direction
        {
            get;
            set;
        }

        object State { get; set; }
    }
}
