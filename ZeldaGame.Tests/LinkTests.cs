using NUnit.Framework;
using Rhino.Mocks;
using StoryQ;
using System;
using ZeldaGame.States;

namespace ZeldaGame.Tests
{
    [TestFixture]
    public class LinkTests
    {
        private Link _link;
        private IResourceManager _resourceManager;
        private IDirectionAnimationSet _directionAnimationSet;
        private IControllable _controllable;
        private Equipment _equipment;
        private IGroupAnimationSet _groupAnimationSet;
        private IState _state;

        private const string LinkResource = "Link";
        private const string StandingResource = "Standing";
        private const string WalkingResource = "Walking";
        private IItem _item;

        [SetUp]
        public void SetUp()
        {
            _directionAnimationSet = MockRepository.GenerateStub<IDirectionAnimationSet>();
            _groupAnimationSet = MockRepository.GenerateStub<IGroupAnimationSet>();
            _state = MockRepository.GenerateStub<IState>();
            _resourceManager = MockRepository.GenerateMock<IResourceManager>();
            _controllable = MockRepository.GenerateStub<IControllable>();
            _equipment = new Equipment();
            _item = null;
        }

        [Test]
        public void CreationTest()
        {
            new Story("Link").Tag("link")
                .InOrderTo("Setup initial state")
                .AsA("Gamer")
                .IWant("To see link standing on the screen")
                .WithScenario("Link is on screen")
                .Given(TheResourceManagerHasBeenSetUpWithGroupResource, LinkResource)
                    .And(TheGroupAnimationSetHasBeenSetUpWithResource, StandingResource)
                .When(LinkHasBeenCreated)
                .Then(ResourcesShouldHaveBeenLoaded, StandingResource)
                .And(LinksStateShouldBeOfType, typeof(StandingState))
                .ExecuteWithReport();
        }

        [Test]
        public void StandingTest()
        {
            new Story("Link").Tag("link")
                .InOrderTo("Do nothing")
                .AsA("Gamer")
                .IWant("Link to not to do anything")
                .WithScenario("No keys pressed")
                .Given(TheResourceManagerHasBeenSetUpWithGroupResource, LinkResource)
                    .And(TheGroupAnimationSetHasBeenSetUpWithResource, StandingResource)
                    .And(LinkHasBeenCreated)
                .When(AdvanceLogicIsCalled)
                .Then(ResourcesShouldHaveBeenLoaded, StandingResource)
                    .And(LinksStateShouldBeOfType, typeof(StandingState))
                .ExecuteWithReport();
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void MovingTest(Direction direction)
        {
            new Story("Link").Tag("link")
                .InOrderTo("Move around the screen")
                .AsA("Gamer")
                .IWant("Link to move")
                .WithScenario("Direction keys pressed")
                .Given(TheResourceManagerHasBeenSetUpWithGroupResource, LinkResource)
                    .And(TheGroupAnimationSetHasBeenSetUpWithResource, StandingResource)
                    .And(TheGroupAnimationSetHasBeenSetUpWithResource, WalkingResource)
                    .And(LinkHasBeenCreated)
                    .And(IIntendToMoveInADirection, direction)
                .When(AdvanceLogicIsCalled)
                .Then(ResourcesShouldHaveBeenLoaded, WalkingResource)
                    .And(LinksStateShouldBeOfType, typeof(MovingState))
                .ExecuteWithReport();
        }

        [TestCase(EquipmentSlots.CLeft)]
        [TestCase(EquipmentSlots.CTop)]
        [TestCase(EquipmentSlots.CRight)]
        public void ItemTest(EquipmentSlots slot)
        {
            new Story("Link").Tag("link")
                .InOrderTo("Use an item")
                .AsA("Gamer")
                .IWant("To use the item from an equipment slot")
                .WithScenario("Item equipped")
                .Given(TheResourceManagerHasBeenSetUpWithGroupResource, LinkResource)
                    .And(TheGroupAnimationSetHasBeenSetUpWithResource, StandingResource)
                    .And(LinkHasBeenCreated)
                    .And(WeHaveAnItem)
                    .And(ItemIsEquippedToSlot, slot)
                    .And(ItemIsBeingUsed, slot)
                .When(AdvanceLogicIsCalled)
                    .And(LinksStateShouldBe, _state)
                .ExecuteWithReport();
        }

        private void ItemIsBeingUsed(EquipmentSlots slot)
        {
            _controllable.Stub(e => e.Slot).Return(slot);
        }

        private void LinksStateShouldBe(IState state)
        {
            Assert.That(_link.State, Is.EqualTo(state));
        }

        private void WeHaveAnItem()
        {
            _item = MockRepository.GenerateStub<IItem>();
            _item.Stub(e => e.CreateState(Arg<IDirectionable>.Is.Equal(_link), Arg<IGroupAnimationSet>.Is.Equal(_groupAnimationSet), Arg<Func<IState>>.Is.Anything)).Return(_state);
        }

        private void ItemIsEquippedToSlot(EquipmentSlots slot)
        {
            _link.Equipment[slot] = _item;
        }

        private void AdvanceLogicIsCalled()
        {
            _link.AdvanceLogic();
        }

        private void LinksStateShouldBeOfType(Type type)
        {
            Assert.That(_link.State, Is.InstanceOf(type));
        }

        private void ResourcesShouldHaveBeenLoaded(string resource)
        {
            _groupAnimationSet.AssertWasCalled(e => e.LoadDirectionSet(resource));
        }

        private void LinkHasBeenCreated()
        {
            _link = new Link(null,_resourceManager, _controllable);
        }

        private void TheGroupAnimationSetHasBeenSetUpWithResource(string resource)
        {
            _groupAnimationSet.Stub(e => e.LoadDirectionSet(resource)).Return(_directionAnimationSet);
        }

        private void TheResourceManagerHasBeenSetUpWithGroupResource(string resource)
        {
            _resourceManager.Stub(e => e.LoadGroupSet(resource)).Return(_groupAnimationSet);
        }

        private void IIntendToMoveInADirection(Direction direction)
        {
            _controllable.Stub(e => e.Direction).Return(direction);
        }
    }
}
