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

        bool MoveRight { get; set; }

        bool MoveDown { get; set; }

        bool MoveUp { get; set; }
    }
}
