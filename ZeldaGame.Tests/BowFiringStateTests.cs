using Microsoft.Xna.Framework;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaGame;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class BowFiringStateTests
    {
        private IArrowFactory _arrowFactory;
        private BowFiringState _bowFiringState;
        private IDirectionable _directionable;

        [SetUp]
        public void SetUp()
        {
            _directionable = MockRepository.GenerateStub<IDirectionable>();
            _arrowFactory = MockRepository.GenerateStub<IArrowFactory>();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void GivenWeAreInTheBowFiringStateWhenCreatedThenCreateAnArrowInTheSpecifiedDirection(Direction direction)
        {
            _directionable.Position = new Vector2();
            _directionable.Direction = direction;

            var bowFiringState = new BowFiringState(_arrowFactory, _directionable);

            _arrowFactory.AssertWasCalled(e => e.CreateArrow(_directionable.Position, direction));
        }
    }
}
