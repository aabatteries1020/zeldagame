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
    public class MovingStateTests
    {
        private const float OneStep = 2;
        private const float TwoSteps = OneStep * 2;
        private const float DiagonalStep = OneStep * 0.7071067811865475f;

        private IControllable _controllable;
        private IDirectionable _directionable;
        private MovingState _movingState;
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
            _controllable = MockRepository.GenerateMock<IControllable>();
            _movingState = null;
            _endingState = new object();
        }

        private void CreateMovingState()
        {
            if(_movingState != null)
                return;

            _movingState = new MovingState(_directionable, _directionAnimationSet, _controllable,  _endingState, OneStep);
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void GivenADirectionIsSpecifiedWhenAMovingStateIsCreatedTheSetTheDirection(Direction direction)
        {
            _controllable.Stub(e => e.Direction).Return(direction);

            CreateMovingState();

            Assert.AreEqual(direction, _directionable.Direction);
            _directionAnimationSet.AssertWasCalled(e => e[direction]);
            Assert.That(_directionable.Animation, Is.EqualTo(_animation));
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void GivenADirectionIsSpecifiedOnCreationAndReleasedBeforeAdvancingLogicThenEndTheState(Direction direction)
        {
            _controllable.Stub(e => e.Direction).Return(direction).Repeat.Once();

            CreateMovingState();

            _movingState.AdvanceLogic();

            Assert.AreEqual(_endingState, _directionable.State);
        }

        [Test]
        public void GivenNoDirectionsAreSpecifiedWhenAdvanceLogicIsCalledThenDontMove()
        {
            CreateMovingState();
            
            var expectedPosition = _directionable.Position;

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position, Is.EqualTo(expectedPosition));
        }

        [TestCase(Direction.Left | Direction.Down, -DiagonalStep, DiagonalStep)]
        [TestCase(Direction.Left | Direction.Up, -DiagonalStep, -DiagonalStep)]
        [TestCase(Direction.Right | Direction.Down, DiagonalStep, DiagonalStep)]
        [TestCase(Direction.Right | Direction.Up, DiagonalStep, -DiagonalStep)]
        public void GivenADiagonalDirectionIsSpecifiedWhenAdvanceLogicIsCalledThenMoveInThatDirection(Direction direction, float expectedX, float expectedY)
        {
            _controllable.Stub(e => e.Direction).Return(direction);

            CreateMovingState();
            
            var expectedPosition = new Vector2(_directionable.Position.X + expectedX, _directionable.Position.Y + expectedY);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [TestCase(Direction.Left, Direction.Right, -TwoSteps, 0, 0.1f, 0)]
        [TestCase(Direction.Up, Direction.Down, 0, -TwoSteps, 0, 0.1f)]
        [TestCase(Direction.Right, Direction.Left, TwoSteps, 0, 0.1f, 0)]
        [TestCase(Direction.Down, Direction.Up, 0, TwoSteps, 0, 0.1f)]
        public void GivenADirectionWasSpecifiedAndAdvanceLogicHasBeenCalledWhenTheOppositeDirectionIsSpecifiedAndAdvanceLogicIsCalledAgainThenContinueMovingInTheOriginalDirection(Direction originalDirection, Direction nextDirection, float expectedX, float expectedY, float withinX, float withinY)
        {
            _controllable.Stub(e => e.Direction).Return(originalDirection);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X + expectedX, _directionable.Position.Y + expectedY);

            _movingState.AdvanceLogic();

            _controllable.BackToRecord();
            _controllable.Replay();

            _controllable.Stub(e => e.Direction).Return(originalDirection | nextDirection);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(withinX));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(withinY));
        }

        [TestCase(Direction.Left, -OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Up, 0, -OneStep, 0, 0.1f)]
        [TestCase(Direction.Right, OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Down, 0, OneStep, 0, 0.1f)]
        public void GivenOnlyOneDirectionIsSpecifiedWhenAdvanceLogicIsCalledThenMoveInThatDirection(Direction direction, float expectedX, float expectedY, float withinX, float withinY)
        {
            _controllable.Stub(e => e.Direction).Return(direction);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X + expectedX, _directionable.Position.Y + expectedY);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(withinX));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(withinY));
        }

        [TestCase(Direction.Left, Direction.Up | Direction.Down, -OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Right, Direction.Up | Direction.Down, OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Up, Direction.Left | Direction.Right, 0, -OneStep, 0, 0.1f)]
        [TestCase(Direction.Down, Direction.Left | Direction.Right, 0, OneStep, 0, 0.1f)]
        public void GivenOnlyOneDirectionIsSpecifiedAndThenBothDirectionsOnTheOtherAxisAreSpecifiedWhenAdvanceLogicIsCalledThenOnlyMoveInTheFirstDirectionSpecified(Direction originalDirection, Direction nextDirection, float expectedX, float expectedY, float withinX, float withinY)
        {
            _controllable.Stub(e => e.Direction).Return(originalDirection).Repeat.Once();

            CreateMovingState();

            _controllable.Stub(e => e.Direction).Return(originalDirection | nextDirection);

            GivenOnlyOneDirectionIsSpecifiedWhenAdvanceLogicIsCalledThenMoveInThatDirection(originalDirection, expectedX, expectedY, withinX, withinY);
        }
    }
}
