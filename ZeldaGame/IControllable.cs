using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame
{
    public interface IControllable
    {
        Direction Direction { get; }

        EquipmentSlots Slot { get; }
    }
}
