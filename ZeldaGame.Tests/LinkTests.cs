﻿using Axiom.Math;
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
        private Link link;
        IControllable controllable;

        [SetUp]
        public void SetUp()
        {
            controllable = MockRepository.GenerateStub<IControllable>();
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

            var expectedPosition = new Vector2(link.Position.x - Link.Speed, link.Position.y);

            link.AdvanceLogic();

            Assert.That(link.Position.x, Is.EqualTo(expectedPosition.x).Within(0.1));
            Assert.That(link.Position.y, Is.EqualTo(expectedPosition.y));
        }

        [Test]
        public void GivenOnlyTheRightKeyIsPressedWhenAdvanceLogicIsCalledThenMoveRight()
        {
            controllable.Stub(e => e.MoveRight).Return(true);

            var expectedPosition = new Vector2(link.Position.x + Link.Speed, link.Position.y);

            link.AdvanceLogic();

            Assert.That(link.Position.x, Is.EqualTo(expectedPosition.x).Within(0.1));
            Assert.That(link.Position.y, Is.EqualTo(expectedPosition.y));
        }

        [Test]
        public void GivenOnlyTheUpKeyIsPressedWhenAdvanceLogicIsCalledThenMoveUp()
        {
            controllable.Stub(e => e.MoveUp).Return(true);

            var expectedPosition = new Vector2(link.Position.x, link.Position.y - Link.Speed);

            link.AdvanceLogic();

            Assert.That(link.Position.x, Is.EqualTo(expectedPosition.x));
            Assert.That(link.Position.y, Is.EqualTo(expectedPosition.y).Within(0.1));
        }

        [Test]
        public void GivenOnlyTheDownKeyIsPressedWhenAdvanceLogicIsCalledThenMoveDown()
        {
            controllable.Stub(e => e.MoveDown).Return(true);

            var expectedPosition = new Vector2(link.Position.x, link.Position.y + Link.Speed);

            link.AdvanceLogic();

            Assert.That(link.Position.x, Is.EqualTo(expectedPosition.x));
            Assert.That(link.Position.y, Is.EqualTo(expectedPosition.y).Within(0.1));
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