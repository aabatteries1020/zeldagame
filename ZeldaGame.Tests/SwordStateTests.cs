using NUnit.Framework;
using StoryQ;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class SwordStateTests : DirectionableStateTestBase
    {
        private object _endingState;

        [SetUp]
        public void SetUp()
        {
            _endingState = new object();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void AnimationEndingTest(Direction direction)
        {
            new Story("Entering the sword state").Tag("sword")
                .InOrderTo("See the object facing in the right direction in the sword state")
                .AsA("Gamer")
                .IWant("Its animation to be set to the direction it's facing")
                .WithScenario("Object is entering the sword state")
                .Given(AnObjectIsFacingInADirection, direction)
                .When(TheObjectIsInTheSwordState)
                .Then(TheObjectsAnimationIsCorrectForItsDirection, direction)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void SwordSwingingTest(Direction direction)
        {
            new Story("Swinging the sword").Tag("sword")
                .InOrderTo("Make use of the sword")
                .AsA("Gamer")
                .IWant("The sword swinging to end")
                .WithScenario("The sword has been swung")
                .Given(AnObjectIsFacingInADirection, direction)
                    .And(TheObjectIsInTheSwordState)
                    .And(TheAnimationHasFinished)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectShouldEnterAnotherState, _endingState)
                .ExecuteWithReport();
        }

        private void TheObjectIsInTheSwordState()
        {
            _state = new SwordState(_directionable, _directionAnimationSet, _endingState);
        }
    }
}
