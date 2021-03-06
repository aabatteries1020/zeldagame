﻿using Microsoft.Xna.Framework;
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
        private readonly Func<IState> _endingStateCallback;
        private readonly float _speed;
        private readonly IDirectionAnimationSet _directionAnimationSet;
        CollisionManager<BoundingBox, Rectangle> _collisionManager;

        public MovingState(CollisionManager<BoundingBox, Rectangle> collisionManager, IDirectionable directionable, IDirectionAnimationSet directionAnimationSet, IControllable controllable, Func<IState> endingStateCallback, float speed)
        {
            _collisionManager = collisionManager;
            _directionable = directionable;
            _directionAnimationSet = directionAnimationSet;
            _controllable = controllable;
            _endingStateCallback = endingStateCallback;
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
            var collisions = _collisionManager.GetCollisionsFor(_directionable);

            if(collisions != null)
            {
                var boundary = _collisionManager.CollisionDetector.CalculateBoundary(_directionable, collisions);

                if (boundary.HasValue)
                {
                    switch (direction)
                    {
                        case Direction.Down: position.Y = boundary.Value.Top - 1; break;
                        case Direction.Left: position.X = boundary.Value.Right; break;
                        case Direction.Right: position.X = boundary.Value.Left - 1; break;
                        case Direction.Up: position.Y = boundary.Value.Bottom; break;
                    }

                    _directionable.Position = position;

                    return;
                }
            }

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
                _directionable.State = _endingStateCallback();
            }

            _directionable.Position = position;
        }

        public bool CanUseItems
        {
            get { return true; }
        }
    }
}
