using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public class BowFiringState
    {
        private IArrowFactory _arrowFactory;
        private IDirectionable _directionable;

        public BowFiringState(IArrowFactory arrowFactory, IDirectionable directionable)
        {
            _arrowFactory = arrowFactory;
            _directionable = directionable;

            _arrowFactory.CreateArrow(_directionable.Position, _directionable.Direction);
        }
    }
}
