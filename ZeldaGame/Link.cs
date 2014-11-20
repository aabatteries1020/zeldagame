using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public class Link
    {
        private IControllable _controllable;
        private Vector2 _position;
        private const float Speed = 2;

        public Link(IControllable controllable)
        {
            _controllable = controllable;
        }

        private enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        };

        private Direction _direction;

        public void AdvanceLogic()
        {
            var newDirection = Direction.None;
            var speed = Speed;

            if (
                (_controllable.MoveDown || _controllable.MoveUp) &&
                !(_controllable.MoveDown && _controllable.MoveUp) &&
                (_controllable.MoveLeft || _controllable.MoveRight) &&
                !(_controllable.MoveLeft && _controllable.MoveRight))
            {
                speed *= 0.7f; 
            }

            if (_controllable.MoveDown && _direction != Direction.Up)
            {
                newDirection = Direction.Down;
                _position.Y += speed;
            }

            if (_controllable.MoveUp && _direction != Direction.Down)
            {
                newDirection = Direction.Up;
                _position.Y -= speed;
            }

            if (_controllable.MoveLeft && _direction != Direction.Right)
            {
                newDirection = Direction.Left;
                _position.X -= speed;
            }

            if (_controllable.MoveRight && _direction != Direction.Left)
            {
                newDirection = Direction.Right;
                _position.X += speed;
            }

            _direction = newDirection;
        }

        public Vector2 Position
        {
            get { return _position; }
        }
    }
}
