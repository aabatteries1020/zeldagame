using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame.Tests
{
    class ArrowState
    {
        private IDirectionable _directionable;
        private MovingState _state;

        private class Controller : IControllable
        {
            private Direction _direction;

            public Controller(Direction direction)
            {
                _direction = direction;
            }

            public bool MoveLeft
            {
                get { return _direction == Direction.Left; }
            }

            public bool MoveRight
            {
                get { return _direction == Direction.Right; }
            }

            public bool MoveDown
            {
                get { return _direction == Direction.Down; }
            }

            public bool MoveUp
            {
                get { return _direction == Direction.Up; }
            }
        }

        public ArrowState(IDirectionable directionable)
        {
            _directionable = directionable;
            _state = new MovingState(directionable, new Controller(directionable.Direction), null, 1.0f);
        }

        public void AdvanceLogic()
        {
            _state.AdvanceLogic();
        }
    }
}
