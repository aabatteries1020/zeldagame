using NUnit.Framework;
using StoryQ;
using ZeldaGame.States;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class StandingStateTests : DirectionableStateTestBase
    {
        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void AnimationEndingTest(Direction direction)
        {
            new Story("Entering the standing state").Tag("standing")
                .InOrderTo("See the object facing in the right direction in the standing state")
                .AsA("Gamer")
                .IWant("Its animation to be set to the direction it's facing")
                .WithScenario("Object is entering the standing state")
                .Given(TheObjectIsFacingInADirection, direction)
                .When(TheObjectIsInTheStandingState)
                .Then(TheObjectsAnimationIsCorrectForItsDirection, direction)
                .ExecuteWithReport();
        }

        private void TheObjectIsInTheStandingState()
        {
            _state = new StandingState(_directionable, _directionAnimationSet);
        }
    }
}
