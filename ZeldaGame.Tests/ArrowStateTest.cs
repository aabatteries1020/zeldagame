using Microsoft.Xna.Framework;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class ArrowStateTests
    {
        private IDirectionable _directionable;

        [SetUp]
        public void SetUp()
        {
            _directionable = MockRepository.GenerateStub<IDirectionable>();
        }

        [TestCase(Direction.Down, 0f, 1f, 0f, 0.1f)]
        [TestCase(Direction.Up, 0f, -1f, 0f, 0.1f)]
        [TestCase(Direction.Left, -1f, 0f, 0.1f, 0f)]
        [TestCase(Direction.Right, 1f, 0f, 0.1f, 0f)]
        public void GivenWeAreInTheArrowStateWhenAdvanceLogicIsCalledThenMoveTheArrowInTheSpecifiedDirection(Direction direction, float x, float y, float withinX, float withinY)
        {
            _directionable.Position = new Vector2();
            _directionable.Direction = direction;

            var expectedPosition = new Vector2(_directionable.Position.X + x, _directionable.Position.Y + y);

            var arrowState = new ArrowState(_directionable);

            arrowState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(withinX));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(withinY));
        }
    }
}
