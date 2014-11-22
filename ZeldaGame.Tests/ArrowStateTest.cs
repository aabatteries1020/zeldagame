using Microsoft.Xna.Framework;
using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;
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
        private IDirectionAnimationSet _directionAnimationSet;
        private IAnimation _animation;
        private ArrowState _arrowState;
        private Vector2 _expectedPosition;

        [SetUp]
        public void SetUp()
        {
            _directionable = MockRepository.GenerateStub<IDirectionable>();
            _animation = MockRepository.GenerateMock<IAnimation>();
            _directionAnimationSet = MockRepository.GenerateStub<IDirectionAnimationSet>();
            _directionAnimationSet.Stub(e => e[Arg<Direction>.Is.Anything]).Return(_animation);
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void CreationTest(Direction direction)
        {
            new Story("Entering an arrow's state").Tag("arrow")
                .InOrderTo("See the object facing in the right direction in the arrow state")
                .AsA("Gamer")
                .IWant("Its animation to be set to the direction it's facing")
                .WithScenario("Object is entering the arrow state")
                .Given(AnObjectIsFacingInADirection, direction)
                .When(TheObjectIsInTheTheArrowState)
                .Then(TheObjectsAnimationIsCorrectForItsDirection, direction)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Down, 0f, 1f, 0f, 0.1f)]
        [TestCase(Direction.Up, 0f, -1f, 0f, 0.1f)]
        [TestCase(Direction.Left, -1f, 0f, 0.1f, 0f)]
        [TestCase(Direction.Right, 1f, 0f, 0.1f, 0f)]
        public void MovingTest(Direction direction, float x, float y, float withinX, float withinY)
        {
            new Story("Moving the object in the arrow state").Tag("arrow")
                .InOrderTo("Make use of an object in the arrow state")
                .AsA("Gamer")
                .IWant("It to move across the screen like an arrow")
                .WithScenario("Object is in the arrow state")
                .Given(AnObjectIsFacingInADirection, direction)
                    .And(ItHasARelativeExpectedXAndYPositionOf, x, y)
                    .And(TheObjectIsInTheTheArrowState)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectInTheArrowStateHasMovedToTheExpectedPositionWithin, withinX, withinY)
                .ExecuteWithReport();
        }

        private void AnObjectIsFacingInADirection(Direction direction)
        {
            _directionable.Direction = direction;
        }

        private void TheObjectsAnimationIsCorrectForItsDirection(Direction direction)
        {
            _directionAnimationSet.AssertWasCalled(e => e[direction]);
            Assert.That(_directionable.Animation, Is.EqualTo(_animation));
        }

        private void TheObjectInTheArrowStateHasMovedToTheExpectedPositionWithin(float withinX, float withinY)
        {
            Assert.That(_directionable.Position.X, Is.EqualTo(_expectedPosition.X).Within(withinX));
            Assert.That(_directionable.Position.Y, Is.EqualTo(_expectedPosition.Y).Within(withinY));
        }

        private void AdvanceLogicHasBeenCalled()
        {
            _arrowState.AdvanceLogic();
        }

        private void TheObjectIsInTheTheArrowState()
        {
            _arrowState = new ArrowState(_directionable, _directionAnimationSet);
        }

        private void ItHasARelativeExpectedXAndYPositionOf(float x, float y)
        {
            _expectedPosition = new Vector2(_directionable.Position.X + x, _directionable.Position.Y + y);
        }
    }
}
