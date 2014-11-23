using Microsoft.Xna.Framework;
using NUnit.Framework;
using Rhino.Mocks;
using System;

namespace ZeldaGame.Tests
{
    public class DirectionableStateTestBase
    {
        protected IDirectionable _directionable;
        protected IDirectionAnimationSet _directionAnimationSet;
        protected IAnimation _animation;
        protected IState _state;
        protected Vector2 _expectedPosition;
        protected IState _endingState;
        protected Func<IState> _endingStateCallback;

        [SetUp]
        public void DirectionableSetup()
        {
            _directionable = MockRepository.GenerateStub<IDirectionable>();
            _animation = MockRepository.GenerateMock<IAnimation>();
            _directionAnimationSet = MockRepository.GenerateStub<IDirectionAnimationSet>();
            _directionAnimationSet.Stub(e => e[Arg<Direction>.Is.Anything]).Return(_animation);

            _endingState = MockRepository.GenerateStub<IState>();
            _endingStateCallback = () => _endingState;

            _state = null;
        }

        protected void TheObjectIsFacingInADirection(Direction direction)
        {
            _directionable.Direction = direction;
        }

        protected void AdvanceLogicHasBeenCalled()
        {
            _state.AdvanceLogic();
        }

        protected void TheObjectShouldEnterAnotherState(object state)
        {
            Assert.That(_directionable.State, Is.EqualTo(state));
        }

        protected void TheAnimationHasFinished()
        {
            _animation.Stub(e => e.IsComplete).Return(true);
        }

        protected void TheObjectsAnimationIsCorrectForItsDirection(Direction direction)
        {
            _directionAnimationSet.AssertWasCalled(e => e[direction]);
            Assert.That(_directionable.Animation, Is.EqualTo(_animation));
        }

        protected void TheObjectHasMovedToTheExpectedPositionWithin(float withinX, float withinY)
        {
            Assert.That(_directionable.Position.X, Is.EqualTo(_expectedPosition.X).Within(withinX));
            Assert.That(_directionable.Position.Y, Is.EqualTo(_expectedPosition.Y).Within(withinY));
        }

        protected void TheObjectHasARelativeExpectedXAndYPositionOf(float x, float y)
        {
            _expectedPosition = new Vector2(_directionable.Position.X + x, _directionable.Position.Y + y);
        }
    }
}
