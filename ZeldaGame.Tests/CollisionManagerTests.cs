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
    public class CollisionManagerTests
    {
        private CollisionManager<object, bool> _collisionManager;
        private ICollidable<object> _objectA;
        private ICollidable<object> _objectB;
        private ICollisionDetector<object, bool> _collisionDetector;

        [Test]
        public void CollisionTest()
        {
            new Story("Collision Management").Tag("collision")
                .InOrderTo("Respond to multiple objects colliding")
                .AsA("Developer")
                .IWant("To know if one object is colliding with another")
                .WithScenario("Two Objects are colliding")
                .Given(WeHaveACollisionDetectionType)
                    .And(WeHaveACollisionManager)
                    .And(AnObjectIsAddedToTheCollisionManager)
                    .And(AnotherObjectIsAddedToTheCollisionManager)
                    .And(ACollisionIsExpected)
                .When(CheckingForCollisions)
                .Then(ACollisionShouldBeDetected)
                .ExecuteWithReport();
        }

        public void NonCollisionTest()
        {
            new Story("Collision Management").Tag("collision")
                .InOrderTo("Respond to multiple objects colliding")
                .AsA("Developer")
                .IWant("To know if one object is colliding with another")
                .WithScenario("Two Objects are not colliding")
                .Given(WeHaveACollisionManager)
                    .And(AnObjectIsAddedToTheCollisionManager)
                    .And(AnotherObjectIsAddedToTheCollisionManager)
                .When(CheckingForCollisions)
                .Then(ACollisionShouldNotBeDetected)
                .ExecuteWithReport();
        }

        private void WeHaveACollisionDetectionType()
        {
            _collisionDetector = MockRepository.GenerateStub<ICollisionDetector<object, bool>>();
        }

        private void WeHaveACollisionManager()
        {
            _collisionManager = new CollisionManager<object, bool>(_collisionDetector);
        }

        private void AnObjectIsAddedToTheCollisionManager()
        {
            _objectA = MockRepository.GenerateStub<ICollidable<object>>();

            _collisionManager.Add(_objectA);
        }

        private void AnotherObjectIsAddedToTheCollisionManager()
        {
            _objectB = MockRepository.GenerateStub<ICollidable<object>>();

            _collisionManager.Add(_objectB);
        }

        private void CheckingForCollisions()
        {
            _collisionManager.CalculateCollisions();
        }

        private void ACollisionIsExpected()
        {
            _collisionDetector.Stub(e => e.DetectCollision(Arg<ICollidable<object>>.Is.Anything, Arg<ICollidable<object>>.Is.Anything)).Return(true);
        }

        private void ACollisionIsNotExpected()
        {
            _collisionDetector.Stub(e => e.DetectCollision(Arg<ICollidable<object>>.Is.Anything, Arg<ICollidable<object>>.Is.Anything)).Return(null);
        }

        private void ACollisionShouldBeDetected()
        {
            var collisionCollectionA = _collisionManager.GetCollisionsFor(_objectA);

            Assert.That(collisionCollectionA.Keys, Contains.Item(_objectB));

            var collisionCollectionB = _collisionManager.GetCollisionsFor(_objectB);

            Assert.That(collisionCollectionB.Keys, Contains.Item(_objectA));
        }

        private void ACollisionShouldNotBeDetected()
        {
            var collisionCollectionA = _collisionManager.GetCollisionsFor(_objectA);

            Assert.That(collisionCollectionA, Is.Null);

            var collisionCollectionB = _collisionManager.GetCollisionsFor(_objectB);

            Assert.That(collisionCollectionB, Is.Null);
        }
    }
}
