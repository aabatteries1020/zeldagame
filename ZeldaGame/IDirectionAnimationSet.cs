using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public interface IDirectionAnimationSet
    {
        IAnimation this[Direction direction]
        {
            get;
        }
    }
}
