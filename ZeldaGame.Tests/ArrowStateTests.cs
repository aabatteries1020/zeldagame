using NUnit.Framework;
using StoryQ;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class ArrowStateTests : DirectionableStateTestBase
    {
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
                .Given(TheObjectIsFacingInADirection, direction)
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
                .Given(TheObjectIsFacingInADirection, direction)
                    .And(TheObjectHasARelativeExpectedXAndYPositionOf, x, y)
                    .And(TheObjectIsInTheTheArrowState)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectHasMovedToTheExpectedPositionWithin, withinX, withinY)
                .ExecuteWithReport();
        }

        private void TheObjectIsInTheTheArrowState()
        {
            _state = new ArrowState(_directionable, _directionAnimationSet);
        }
    }
}
