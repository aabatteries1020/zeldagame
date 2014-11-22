using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public class MovingState
    {
        private readonly IControllable _controllable;
        private readonly IDirectionable _directionable;
        private readonly object _endingState;
        private readonly float _speed;

        public MovingState(IDirectionable directionable, IControllable controllable, object endingState, float speed)
        {
            _directionable = directionable;
            _controllable = controllable;
            _endingState = endingState;
            _speed = speed;

            if(_controllable.MoveDown)
            {
                _directionable.Direction = Direction.Down;
            }
            else if (_controllable.MoveUp)
            {
                _directionable.Direction = Direction.Up;
            }
            else if (_controllable.MoveLeft)
            {
                _directionable.Direction = Direction.Left;
            }
            else if (_controllable.MoveRight)
            {
                _directionable.Direction = Direction.Right;
            }
        }

        public void AdvanceLogic()
        {
            var speed = _speed;

            if (
                (_controllable.MoveDown || _controllable.MoveUp) &&
                !(_controllable.MoveDown && _controllable.MoveUp) &&
                (_controllable.MoveLeft || _controllable.MoveRight) &&
                !(_controllable.MoveLeft && _controllable.MoveRight))
            {
                speed *= 0.7f; 
            }

            var direction = _directionable.Direction;
            var position = _directionable.Position;
            var wasCalled = false;

            if (_controllable.MoveDown && direction != Direction.Up)
            {
                position.Y += speed;
                wasCalled = true;
            }

            if (_controllable.MoveUp && direction != Direction.Down)
            {
                position.Y -= speed;
                wasCalled = true;
            }

            if (_controllable.MoveLeft && direction != Direction.Right)
            {
                position.X -= speed;
                wasCalled = true;
            }

            if (_controllable.MoveRight && direction != Direction.Left)
            {
                position.X += speed;
                wasCalled = true;
            }

            if(!wasCalled)
            {
                _directionable.State = _endingState;
            }

            _directionable.Position = position;
        }
    }
}
