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
    public class SwordStateTests
    {
        private IDirectionable _directionable;
        private IDirectionAnimationSet _directionAnimationSet;
        private IAnimation _animation;
        private object _endingState;

        [SetUp]
        public void SetUp()
        {
            _animation = MockRepository.GenerateMock<IAnimation>();
            _directionAnimationSet = MockRepository.GenerateStub<IDirectionAnimationSet>();
            _directionAnimationSet.Stub(e => e[Arg<Direction>.Is.Anything]).Return(_animation);

            _directionable = MockRepository.GenerateStub<IDirectionable>();

            _endingState = new object();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void GivenWeHaveAnObjectWhenASwordStateHasBeenCreatedThenSetTheCorrectAnimation(Direction direction)
        {
            _directionable.Direction = direction;

            var swordState = new SwordState(_directionable, _directionAnimationSet, _endingState);

            _directionAnimationSet.AssertWasCalled(e => e[direction]);
            Assert.That(_directionable.Animation, Is.EqualTo(_animation));
        }

        [Test]
        public void GivenWeHaveASwordStateWithACompleteAnimationWhenAdvanceLogicIsCalledThenSetTheEndingState()
        {
            _directionable.Direction = Direction.Down;

            var swordState = new SwordState(_directionable, _directionAnimationSet, _endingState);

            _animation.Stub(e => e.IsComplete).Return(true);

            swordState.AdvanceLogic();

            Assert.That(_directionable.State, Is.EqualTo(_endingState));
        }
    }
}
