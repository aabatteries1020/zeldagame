using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public class ArrowState
    {
        private MovingState _state;

        private class Controller : IControllable
        {
            public Controller(Direction direction)
            {
                Direction = direction;
            }

            public Direction Direction
            {
                get;
                private set;
            }
        }

        public ArrowState(IDirectionable directionable, IDirectionAnimationSet directionAnimationSet)
        {
            _state = new MovingState(directionable, directionAnimationSet, new Controller(directionable.Direction), directionAnimationSet, 1.0f);
        }

        public void AdvanceLogic()
        {
            _state.AdvanceLogic();
        }
    }
}
