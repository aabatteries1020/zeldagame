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
        private object _endingState;

        [SetUp]
        public void SetUp()
        {
            _directionable = MockRepository.GenerateStub<IDirectionable>();
            _controllable = MockRepository.GenerateMock<IControllable>();
            _movingState = null;
            _endingState = new object();
        }

        private void CreateMovingState()
        {
            if(_movingState != null)
                return;

            _movingState = new MovingState(_directionable, _controllable, _endingState, OneStep);
        }

        [Test]
        public void GivenTheDownKeyIsPressedOnCreationSetTheDirectionToDown()
        {
            _controllable.Stub(e => e.MoveDown).Return(true);

            CreateMovingState();

            Assert.AreEqual(Direction.Down, _directionable.Direction);
        }

        [Test]
        public void GivenTheDownKeyIsPressedOnCreationAndReleasedBeforeAdvancingLogicThenResetTheState()
        {
            _controllable.Stub(e => e.MoveDown).Return(true).Repeat.Once();

            CreateMovingState();

            _movingState.AdvanceLogic();

            Assert.AreEqual(_endingState, _directionable.State);
        }

        [Test]
        public void GivenTheUpKeyIsPressedOnCreationSetTheDirectionToUp()
        {
            _controllable.Stub(e => e.MoveUp).Return(true);

            CreateMovingState();

            Assert.AreEqual(Direction.Up, _directionable.Direction);
        }

        [Test]
        public void GivenTheLeftKeyIsPressedOnCreationSetTheDirectionToLeft()
        {
            _controllable.Stub(e => e.MoveLeft).Return(true);

            CreateMovingState();

            Assert.AreEqual(Direction.Left, _directionable.Direction);
        }

        [Test]
        public void GivenTheRightKeyIsPressedOnCreationSetTheDirectionToRight()
        {
            _controllable.Stub(e => e.MoveRight).Return(true);

            CreateMovingState();

            Assert.AreEqual(Direction.Right, _directionable.Direction);
        }

        [Test]
        public void GivenNoKeysArePressedWhenAdvanceLogicIsCalledThenDontMove()
        {
            CreateMovingState();
            
            var expectedPosition = _directionable.Position;

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void GivenOnlyTheLeftKeyIsPressedWhenAdvanceLogicIsCalledThenMoveLeft()
        {
            _controllable.Stub(e => e.MoveLeft).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X - OneStep, _directionable.Position.Y);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenTheLeftAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUpAndLeft()
        {
            _controllable.Stub(e => e.MoveLeft).Return(true);

            CreateMovingState();
            
            _controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(_directionable.Position.X - DiagonalStep, _directionable.Position.Y - DiagonalStep);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheLeftAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDownAndLeft()
        {
            _controllable.Stub(e => e.MoveLeft).Return(true);

            CreateMovingState();
            
            _controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(_directionable.Position.X - DiagonalStep, _directionable.Position.Y + DiagonalStep);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheRightAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUpAndRight()
        {
            _controllable.Stub(e => e.MoveRight).Return(true);

            CreateMovingState();
            
            _controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(_directionable.Position.X + DiagonalStep, _directionable.Position.Y - DiagonalStep);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheRightAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDownAndRight()
        {
            _controllable.Stub(e => e.MoveRight).Return(true);

            CreateMovingState();
            
            _controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(_directionable.Position.X + DiagonalStep, _directionable.Position.Y + DiagonalStep);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheLeftKeyWasPreviouslyPressedAndRightKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingLeft()
        {
            _controllable.Stub(e => e.MoveLeft).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X - TwoSteps, _directionable.Position.Y);

            _movingState.AdvanceLogic();

            _controllable.Stub(e => e.MoveRight).Return(true);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenTheRightKeyWasPreviouslyPressedAndLeftKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingRight()
        {
            _controllable.Stub(e => e.MoveRight).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X + TwoSteps, _directionable.Position.Y);

            _movingState.AdvanceLogic();

            _controllable.Stub(e => e.MoveLeft).Return(true);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenTheUpKeyWasPreviouslyPressedAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingUp()
        {
            _controllable.Stub(e => e.MoveUp).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X, _directionable.Position.Y - TwoSteps);

            _movingState.AdvanceLogic();

            _controllable.Stub(e => e.MoveDown).Return(true);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenTheDownKeyWasPreviouslyPressedAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingDown()
        {
            _controllable.Stub(e => e.MoveDown).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X, _directionable.Position.Y + TwoSteps);

            _movingState.AdvanceLogic();

            _controllable.Stub(e => e.MoveUp).Return(true);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenOnlyTheRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight()
        {
            _controllable.Stub(e => e.MoveRight).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X + OneStep, _directionable.Position.Y);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenOnlyTheUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp()
        {
            _controllable.Stub(e => e.MoveUp).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X, _directionable.Position.Y - OneStep);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenOnlyTheDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown()
        {
            _controllable.Stub(e => e.MoveDown).Return(true);

            CreateMovingState();
            var expectedPosition = new Vector2(_directionable.Position.X, _directionable.Position.Y + OneStep);

            _movingState.AdvanceLogic();

            Assert.That(_directionable.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(_directionable.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenTheUpAndDownAndLeftKeyIsPressedWhenAdvanceLogicIsCalledThenMoveLeft()
        {
            _controllable.Stub(e => e.MoveLeft).Return(true);

            CreateMovingState();

            _controllable.Stub(e => e.MoveDown).Return(true);
            _controllable.Stub(e => e.MoveUp).Return(true);

            GivenOnlyTheLeftKeyIsPressedWhenAdvanceLogicIsCalledThenMoveLeft();
        }

        [Test]
        public void GivenTheUpAndDownAndRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight()
        {
            _controllable.Stub(e => e.MoveRight).Return(true);

            CreateMovingState();

            _controllable.Stub(e => e.MoveDown).Return(true);
            _controllable.Stub(e => e.MoveUp).Return(true);
            
            GivenOnlyTheRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight();
        }

        [Test]
        public void GivenTheLeftAndRightAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp()
        {
            _controllable.Stub(e => e.MoveUp).Return(true);

            CreateMovingState();

            _controllable.Stub(e => e.MoveLeft).Return(true);
            _controllable.Stub(e => e.MoveRight).Return(true);

            GivenOnlyTheUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp();
        }

        [Test]
        public void GivenTheLeftAndRightAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown()
        {
            _controllable.Stub(e => e.MoveDown).Return(true);

            CreateMovingState();

            _controllable.Stub(e => e.MoveLeft).Return(true);
            _controllable.Stub(e => e.MoveRight).Return(true);

            GivenOnlyTheDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown();
        }
    }
}
