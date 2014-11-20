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
    public class LinkTests
    {
        private const float OneStep = 2;
        private const float TwoSteps = OneStep * 2;
        private const float DiagonalStep = OneStep * 0.7071067811865475f;

        private Link link;
        private IControllable controllable;

        [SetUp]
        public void SetUp()
        {
            controllable = MockRepository.GenerateMock<IControllable>();
            link = new Link(controllable);
        }

        [Test]
        public void GivenNoLogicHasBeenExecutedThenLinkStartsAt0()
        {
            Assert.AreEqual(new Vector2(0, 0), link.Position);
        }

        [Test]
        public void GivenNoKeysArePressedWhenAdvanceLogicIsCalledThenDontMove()
        {
            var expectedPosition = link.Position;

            link.AdvanceLogic();

            Assert.That(link.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void GivenOnlyTheLeftKeyIsPressedWhenAdvanceLogicIsCalledThenMoveLeft()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);

            var expectedPosition = new Vector2(link.Position.X - OneStep, link.Position.Y);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenTheLeftAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUpAndLeft()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);
            controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(link.Position.X - DiagonalStep, link.Position.Y - DiagonalStep);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheLeftAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDownAndLeft()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);
            controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(link.Position.X - DiagonalStep, link.Position.Y + DiagonalStep);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheRightAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUpAndRight()
        {
            controllable.Stub(e => e.MoveRight).Return(true);
            controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(link.Position.X + DiagonalStep, link.Position.Y - DiagonalStep);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheRightAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDownAndRight()
        {
            controllable.Stub(e => e.MoveRight).Return(true);
            controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(link.Position.X + DiagonalStep, link.Position.Y + DiagonalStep);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.2));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.2));
        }

        [Test]
        public void GivenTheLeftKeyWasPreviouslyPressedAndRightKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingLeft()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);

            var expectedPosition = new Vector2(link.Position.X - TwoSteps, link.Position.Y);

            link.AdvanceLogic();

            controllable.Stub(e => e.MoveRight).Return(true);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenTheRightKeyWasPreviouslyPressedAndLeftKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingRight()
        {
            controllable.Stub(e => e.MoveRight).Return(true);

            var expectedPosition = new Vector2(link.Position.X + TwoSteps, link.Position.Y);

            link.AdvanceLogic();

            controllable.Stub(e => e.MoveLeft).Return(true);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenTheUpKeyWasPreviouslyPressedAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingUp()
        {
            controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(link.Position.X, link.Position.Y - TwoSteps);

            link.AdvanceLogic();

            controllable.Stub(e => e.MoveDown).Return(true);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenTheDownKeyWasPreviouslyPressedAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenContinueMovingDown()
        {
            controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(link.Position.X, link.Position.Y + TwoSteps);

            link.AdvanceLogic();

            controllable.Stub(e => e.MoveUp).Return(true);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenOnlyTheRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight()
        {
            controllable.Stub(e => e.MoveRight).Return(true);

            var expectedPosition = new Vector2(link.Position.X + OneStep, link.Position.Y);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X).Within(0.1));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y));
        }

        [Test]
        public void GivenOnlyTheUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp()
        {
            controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(link.Position.X, link.Position.Y - OneStep);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenOnlyTheDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown()
        {
            controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(link.Position.X, link.Position.Y + OneStep);

            link.AdvanceLogic();

            Assert.That(link.Position.X, Is.EqualTo(expectedPosition.X));
            Assert.That(link.Position.Y, Is.EqualTo(expectedPosition.Y).Within(0.1));
        }

        [Test]
        public void GivenTheUpKeyAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenDontMove()
        {
            controllable.Stub(e => e.MoveDown).Return(true);
            controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = link.Position;

            link.AdvanceLogic();

            Assert.That(link.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void GivenTheLeftKeyAndRightKeyIsPressedWhenAdvanceLogicIsCalledThenDontMove()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);
            controllable.Stub(e => e.MoveRight).Return(true);

            var expectedPosition = link.Position;

            link.AdvanceLogic();

            Assert.That(link.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void GivenTheUpAndDownAndLeftKeyIsPressedWhenAdvanceLogicIsCalledThenMoveLeft()
        {
            controllable.Stub(e => e.MoveDown).Return(true);
            controllable.Stub(e => e.MoveUp).Return(true);

            GivenOnlyTheLeftKeyIsPressedWhenAdvanceLogicIsCalledThenMoveLeft();
        }

        [Test]
        public void GivenTheUpAndDownAndRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight()
        {
            controllable.Stub(e => e.MoveDown).Return(true);
            controllable.Stub(e => e.MoveUp).Return(true);
            
            GivenOnlyTheRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight();
        }

        [Test]
        public void GivenTheLeftAndRightAndUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);
            controllable.Stub(e => e.MoveRight).Return(true);

            GivenOnlyTheUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp();
        }

        [Test]
        public void GivenTheLeftAndRightAndDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown()
        {
            controllable.Stub(e => e.MoveLeft).Return(true);
            controllable.Stub(e => e.MoveRight).Return(true);

            GivenOnlyTheDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown();
        }
    }
}
