using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class BowFiringStateTests : DirectionableStateTestBase
    {
        private IArrowFactory _arrowFactory;
        private object _endingState;

        [SetUp]
        public void SetUp()
        {
            _endingState = new object();
            _arrowFactory = MockRepository.GenerateStub<IArrowFactory>();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void BowFiringTest(Direction direction)
        {
            new Story("Firing an arrow").Tag("arrow")
                .InOrderTo("Make use of the bow")
                .AsA("Gamer")
                .IWant("To be able to fire an arrow")
                .WithScenario("I am using the bow")
                .Given(AnObjectIsFacingInADirection, direction)
                .When(TheObjectIsInTheBowFiringState)
                .Then(TheAnimationHasFinished)
                    .And(TheArrowHasBeenFiredInDirection, direction)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void AnimationEndingTest(Direction direction)
        {
            new Story("Firing an arrow").Tag("arrow")
                .InOrderTo("Make use of the bow")
                .AsA("Gamer")
                .IWant("The bow firing to end")
                .WithScenario("The bow has been fired")
                .Given(AnObjectIsFacingInADirection, direction)
                    .And(TheObjectIsInTheBowFiringState)
                    .And(TheAnimationHasFinished)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectShouldEnterAnotherState, _endingState)
                    .And(TheArrowHasBeenFiredInDirection, direction)
                .ExecuteWithReport();
        }

        private void TheArrowHasBeenFiredInDirection(Direction direction)
        {
            _arrowFactory.AssertWasCalled(e => e.CreateArrow(_directionable.Position, direction));
        }

        private void TheObjectIsInTheBowFiringState()
        {
            _state = new BowFiringState(_arrowFactory, _directionable, _directionAnimationSet, _endingState);
        }
    }
}
