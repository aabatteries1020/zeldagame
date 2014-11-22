using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame
{
    public interface IControllable
    {
        bool MoveLeft { get; }

        bool MoveRight { get; }

        bool MoveDown { get; }

        bool MoveUp { get; }
    }
}
