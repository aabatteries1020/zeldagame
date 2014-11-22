﻿using Microsoft.Xna.Framework;
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

            var bowFiringState = new BowFiringState(_arrowFactory, _directionable, _directionAnimationSet, _endingState);

            Assert.That(_directionable.Animation, Is.EqualTo(_animation));
            _arrowFactory.AssertWasCalled(e => e.CreateArrow(_directionable.Position, direction));
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void GivenWeAreInTheBowFiringStateWhenCreatedAndTheBowFiringAnimationHasFinishedWhenAdvanceLogicIsCalledThenSetTheEndingState(Direction direction)
        {
            _directionable.Position = new Vector2();
            _directionable.Direction = direction;

            var bowFiringState = new BowFiringState(_arrowFactory, _directionable, _directionAnimationSet, _endingState);

            _animation.Stub(e => e.IsComplete).Return(true);

            bowFiringState.AdvanceLogic();

            Assert.That(_directionable.State, Is.EqualTo(_endingState));
        }
    }
}
