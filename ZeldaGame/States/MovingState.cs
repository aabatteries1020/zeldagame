using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public class MovingState : IState
    {
        private readonly IControllable _controllable;
        private readonly IDirectionable _directionable;
        private readonly object _endingState;
        private readonly float _speed;
        private readonly IDirectionAnimationSet _directionAnimationSet;

        public MovingState(IDirectionable directionable, IDirectionAnimationSet directionAnimationSet, IControllable controllable, object endingState, float speed)
        {
            _directionable = directionable;
            _directionAnimationSet = directionAnimationSet;
            _controllable = controllable;
            _endingState = endingState;
            _speed = speed;

            _directionable.Direction = _controllable.Direction;
            _directionable.Animation = _directionAnimationSet[_directionable.Direction];
        }

        public void AdvanceLogic()
        {
            var speed = _speed;

            var moveDown = _controllable.Direction.HasFlag(Direction.Down);
            var moveUp = _controllable.Direction.HasFlag(Direction.Up);
            var moveLeft = _controllable.Direction.HasFlag(Direction.Left);
            var moveRight = _controllable.Direction.HasFlag(Direction.Right);

            if (
                (moveDown || moveUp) &&
                !(moveDown && moveUp) &&
                (moveLeft || moveRight) &&
                !(moveLeft && moveRight))
            {
                speed *= 0.7f; 
            }

            var direction = _directionable.Direction;
            var position = _directionable.Position;
            var wasCalled = false;

            if (moveDown && direction != Direction.Up)
            {
                position.Y += speed;
                wasCalled = true;
            }

            if (moveUp && direction != Direction.Down)
            {
                position.Y -= speed;
                wasCalled = true;
            }

            if (moveLeft && direction != Direction.Right)
            {
                position.X -= speed;
                wasCalled = true;
            }

            if (moveRight && direction != Direction.Left)
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
