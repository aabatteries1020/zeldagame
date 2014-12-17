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
        private CollisionManager _collisionManager;
        private ICollidable _objectA;
        private ICollidable _objectB;

        [TestCase(0, 0, 1, 1, 0, 0, 1, 1)]
        [TestCase(0, 0, 2, 2, 0, 0, 2, 2)]
        [TestCase(0, 0, 2, 2, 1, 0, 2, 2)]
        [TestCase(0, 0, 2, 2, -1, 0, 2, 2)]
        [TestCase(0, 0, 2, 2, 0, 1, 2, 2)]
        [TestCase(0, 0, 2, 2, 0, -1, 2, 2)]
        [TestCase(0, 0, 1, 1, 0, 0, 2, 1)]
        [TestCase(0, 0, 1, 1, 0, 0, 1, 2)]
        [TestCase(0, 0, 1, 1, -1, 0, 2, 1)]
        [TestCase(0, 0, 1, 1, 0, -1, 1, 2)]
        public void CollisionTest(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
        {
            new Story("Bounding Box Collisions").Tag("collision")
                .InOrderTo("Respond to multiple objects colliding")
                .AsA("Developer")
                .IWant("To know if one object is colliding with another")
                .WithScenario("Two Objects are colliding")
                .Given(WeHaveACollisionManager)
                    .And(AnObjectIsAddedToTheCollisionManagerWithBoundingBox, x1, y1, w1, h1)
                    .And(AnotherObjectIsAddedToTheCollisionManagerWithBoundingBox, x2, y2, w2, h2)
                .When(CheckingForCollisions)
                .Then(ACollisionShouldBeDetected)
                .ExecuteWithReport();
        }

        [TestCase(0, 0, 1, 1, 1, 0, 1, 1)]
        [TestCase(0, 0, 1, 1, -1, 0, 1, 1)]
        [TestCase(0, 0, 1, 1, 0, 1, 1, 1)]
        [TestCase(0, 0, 1, 1, 0, -1, 1, 1)]
        [TestCase(0, 0, 2, 2, 2, 0, 2, 2)]
        [TestCase(0, 0, 2, 2, -2, 0, 2, 2)]
        [TestCase(0, 0, 2, 2, 0, 2, 2, 2)]
        [TestCase(0, 0, 2, 2, 0, -2, 2, 2)]
        public void NonCollisionTest(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
        {
            new Story("Bounding Box Collisions").Tag("collision")
                .InOrderTo("Respond to multiple objects colliding")
                .AsA("Developer")
                .IWant("To know if one object is colliding with another")
                .WithScenario("Two Objects are not colliding")
                .Given(WeHaveACollisionManager)
                    .And(AnObjectIsAddedToTheCollisionManagerWithBoundingBox, x1, y1, w1, h1)
                    .And(AnotherObjectIsAddedToTheCollisionManagerWithBoundingBox, x2, y2, w2, h2)
                .When(CheckingForCollisions)
                .Then(ACollisionShouldNotBeDetected)
                .ExecuteWithReport();
        }

        private void WeHaveACollisionManager()
        {
            _collisionManager = new CollisionManager();
        }

        private void AnObjectIsAddedToTheCollisionManagerWithBoundingBox(int x, int y, int w, int h)
        {
            _objectA = MockRepository.GenerateStub<ICollidable>();

            _objectA.Stub(e => e.BoundingBox).Return(new Rectangle(x, y, w, h));

            _collisionManager.Add(_objectA);
        }

        private void AnotherObjectIsAddedToTheCollisionManagerWithBoundingBox(int x, int y, int w, int h)
        {
            _objectB = MockRepository.GenerateStub<ICollidable>();

            _objectB.Stub(e => e.BoundingBox).Return(new Rectangle(x, y, w, h));

            _collisionManager.Add(_objectB);
        }

        private void CheckingForCollisions()
        {
            _collisionManager.CalculateCollisions();
        }

        private void ACollisionShouldBeDetected()
        {
            var collisionCollectionA = _collisionManager.GetCollisionsFor(_objectA);

            Assert.That(collisionCollectionA, Contains.Item(_objectB));

            var collisionCollectionB = _collisionManager.GetCollisionsFor(_objectB);

            Assert.That(collisionCollectionB, Contains.Item(_objectA));
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
