using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class MovingStateTests : DirectionableStateTestBase
    {
        private const float OneStep = 2;
        private const float TwoSteps = OneStep * 2;
        private const float DiagonalStep = OneStep * 0.7071067811865475f;

        private IControllable _controllable;

        [SetUp]
        public void SetUp()
        {
            _controllable = MockRepository.GenerateMock<IControllable>();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void CreationTest(Direction direction)
        {
            new Story("Entering a moving state").Tag("moving")
                .InOrderTo("See the object facing in the right direction in the moving state")
                .AsA("Gamer")
                .IWant("Its animation to be set to the direction in which I intend to move")
                .WithScenario("Object is entering the moving state")
                .Given(IIntendToMoveInADirection, direction)
                .When(TheObjectIsInTheMovingState)
                .Then(TheObjectIsFacingInTheDirectionIIntended, direction)
                    .And(TheObjectsAnimationIsCorrectForItsDirection, direction)
                .ExecuteWithReport();
        }

        [Test]
        public void EndingStateTest()
        {
            new Story("Enter the ending state").Tag("moving")
                .InOrderTo("Move around")
                .AsA("Gamer")
                .IWant("Be able to stop moving")
                .WithScenario("No keys pressed")
                .Given(TheObjectIsInTheMovingState)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectShouldEnterAnotherState, _endingState)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Left | Direction.Down, -DiagonalStep, DiagonalStep)]
        [TestCase(Direction.Left | Direction.Up, -DiagonalStep, -DiagonalStep)]
        [TestCase(Direction.Right | Direction.Down, DiagonalStep, DiagonalStep)]
        [TestCase(Direction.Right | Direction.Up, DiagonalStep, -DiagonalStep)]
        public void DiagonalMovementTest(Direction direction, float expectedX, float expectedY)
        {
            new Story("Move slower diagonally").Tag("moving")
                .InOrderTo("Move in all directions at the same speed")
                .AsA("Gamer")
                .IWant("To move slower when moving diagonally")
                .WithScenario("Multiple direction keys pressed")
                .Given(IIntendToMoveInADirection, direction)
                    .And(TheObjectHasARelativeExpectedXAndYPositionOf, expectedX, expectedY)
                    .And(TheObjectIsInTheMovingState)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectHasMovedToTheExpectedPositionWithin, 0.2f, 0.2f)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Left, Direction.Right, -TwoSteps, 0, 0.1f, 0)]
        [TestCase(Direction.Up, Direction.Down, 0, -TwoSteps, 0, 0.1f)]
        [TestCase(Direction.Right, Direction.Left, TwoSteps, 0, 0.1f, 0)]
        [TestCase(Direction.Down, Direction.Up, 0, TwoSteps, 0, 0.1f)]
        public void PrioritisedMovementTest(Direction originalDirection, Direction nextDirection, float expectedX, float expectedY, float withinX, float withinY)
        {
            new Story("Prioritise existing direction over the opposite direction").Tag("moving")
                .InOrderTo("Continue moving in the same direction when opposite key is pressed")
                .AsA("Gamer")
                .IWant("To continue moving in the same direction")
                .WithScenario("Moving in a direction")
                .Given(IIntendToMoveInADirection, originalDirection)
                    .And(TheObjectHasARelativeExpectedXAndYPositionOf, expectedX, expectedY)
                    .And(TheObjectIsInTheMovingState)
                    .And(AdvanceLogicHasBeenCalled)
                    .And(TheIntendedDirectionChanges, originalDirection | nextDirection)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectHasMovedToTheExpectedPositionWithin, withinX, withinY)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Left, -OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Up, 0, -OneStep, 0, 0.1f)]
        [TestCase(Direction.Right, OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Down, 0, OneStep, 0, 0.1f)]
        public void MovingTest(Direction direction, float expectedX, float expectedY, float withinX, float withinY)
        {
            new Story("Moving in a direction").Tag("moving")
                .InOrderTo("Get to different places")
                .AsA("Gamer")
                .IWant("To move the object on screen")
                .WithScenario("Direction key pressed")
                .Given(IIntendToMoveInADirection, direction)
                    .And(TheObjectHasARelativeExpectedXAndYPositionOf, expectedX, expectedY)
                    .And(TheObjectIsInTheMovingState)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectHasMovedToTheExpectedPositionWithin, withinX, withinY)
                .ExecuteWithReport();
        }

        [TestCase(Direction.Left, Direction.Up | Direction.Down, -OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Right, Direction.Up | Direction.Down, OneStep, 0, 0.1f, 0)]
        [TestCase(Direction.Up, Direction.Left | Direction.Right, 0, -OneStep, 0, 0.1f)]
        [TestCase(Direction.Down, Direction.Left | Direction.Right, 0, OneStep, 0, 0.1f)]
        public void StrafingWithBothKeysTest(Direction originalDirection, Direction nextDirection, float expectedX, float expectedY, float withinX, float withinY)
        {
            new Story("Prevent strafing when both keys are pressed").Tag("moving")
                .InOrderTo("Not have bias when strafing when both keys are pressed")
                .AsA("Gamer")
                .IWant("To not strafe")
                .WithScenario("Moving in a direction")
                .Given(IIntendToMoveInADirection, originalDirection)
                    .And(TheObjectHasARelativeExpectedXAndYPositionOf, expectedX, expectedY)
                    .And(TheObjectIsInTheMovingState)
                    .And(TheIntendedDirectionChanges, originalDirection | nextDirection)
                .When(AdvanceLogicHasBeenCalled)
                .Then(TheObjectHasMovedToTheExpectedPositionWithin, withinX, withinY)
                .ExecuteWithReport();
        }

        private void TheIntendedDirectionChanges(Direction direction)
        {
            _controllable.BackToRecord();
            _controllable.Replay();

            IIntendToMoveInADirection(direction);
        }

        private void TheObjectIsFacingInTheDirectionIIntended(Direction direction)
        {
            Assert.That(_directionable.Direction, Is.EqualTo(direction));
        }

        private void TheObjectIsInTheMovingState()
        {
            _state = new MovingState(_directionable, _directionAnimationSet, _controllable, _endingStateCallback, OneStep);
        }

        private void IIntendToMoveInADirection(Direction direction)
        {
            _controllable.Stub(e => e.Direction).Return(direction);
        }
    }
}
