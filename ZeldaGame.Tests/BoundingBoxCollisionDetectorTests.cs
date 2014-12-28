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
    public class BoundingBoxCollisionDetectorTests
    {
        private BoundingBoxCollisionDetector _collisionDetector;
        private ICollidable<BoundingBox> _objectA;
        private ICollidable<BoundingBox> _objectB;
        private Rectangle? _result;
        private Dictionary<ICollidable<BoundingBox>, Rectangle> _list;
        private Rectangle? _boundary;

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
                .Given(WeHaveABoundingBoxCollisionDetector)
                    .And(AnObjectHasABoundingBoxOf, x1, y1, w1, h1)
                    .And(AnotherObjectHasABoundingBoxOf, x2, y2, w2, h2)
                .When(DetectingACollision)
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
                .Given(WeHaveABoundingBoxCollisionDetector)
                    .And(AnObjectHasABoundingBoxOf, x1, y1, w1, h1)
                    .And(AnotherObjectHasABoundingBoxOf, x2, y2, w2, h2)
                .When(DetectingACollision)
                .Then(ACollisionShouldNotBeDetected)
                .ExecuteWithReport();
        }

        [TestCase(0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1)]
        [TestCase(0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2)]
        [TestCase(0, 0, 2, 2, 1, 0, 2, 2, 0, 0, 3, 2)]
        [TestCase(0, 0, 2, 2, -1, 0, 2, 2, -1, 0, 3, 2)]
        [TestCase(0, 0, 2, 2, 0, 1, 2, 2, 0, 0, 2, 3)]
        [TestCase(0, 0, 2, 2, 0, -1, 2, 2, 0, -1, 2, 3)]
        [TestCase(0, 0, 1, 1, 0, 0, 2, 1, 0, 0, 2, 1)]
        [TestCase(0, 0, 1, 1, 0, 0, 1, 2, 0, 0, 1, 2)]
        [TestCase(0, 0, 1, 1, -1, 0, 2, 1, -1, 0, 2, 1)]
        [TestCase(0, 0, 1, 1, 0, -1, 1, 2, 0, -1, 1, 2)]
        public void CollisionBoundaryTest(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2, int x3, int y3, int w3, int h3)
        {
            new Story("Bounding Box Collisions").Tag("collision")
                .InOrderTo("Correct an Objects Position if it's colliding")
                .AsA("Developer")
                .IWant("To know what the collision boundary is")
                .WithScenario("Two Objects have collided")
                .Given(WeHaveABoundingBoxCollisionDetector)
                    .And(WeHaveAList)
                    .And(AnObjectIsAddedToTheListWithBoundingBoxOf, x1, y1, w1, h1)
                    .And(AnObjectIsAddedToTheListWithBoundingBoxOf, x2, y2, w2, h2)
                .When(CalculatingTheBoundary)
                .Then(TheBoundaryShouldBeEqualTo, x3, y3, w3, h3)
                .ExecuteWithReport();
        }

        private void TheBoundaryShouldBeEqualTo(int x, int y, int w, int h)
        {
            Assert.That(_boundary, Is.EqualTo(new Rectangle(x, y, w, h)));
        }

        private void CalculatingTheBoundary()
        {
            _boundary = _collisionDetector.CalculateBoundary(null, _list);
        }

        private void AnObjectIsAddedToTheListWithBoundingBoxOf(int x, int y, int w, int h)
        {
            var collidable = MockRepository.GenerateStub<ICollidable<BoundingBox>>();

            var rect = new Rectangle(x, y, w, h);

            collidable.Stub(e => e.Area).Return(new BoundingBox(rect));

            _list.Add(collidable, rect);
        }

        private void WeHaveAList()
        {
            _list = new Dictionary<ICollidable<BoundingBox>,Rectangle>();
        }

        private void WeHaveABoundingBoxCollisionDetector()
        {
            _collisionDetector = new BoundingBoxCollisionDetector();
        }

        private void AnObjectHasABoundingBoxOf(int x, int y, int w, int h)
        {
            _objectA = MockRepository.GenerateStub<ICollidable<BoundingBox>>();

            _objectA.Stub(e => e.Area).Return(new BoundingBox(new Rectangle(x, y, w, h)));
        }

        private void AnotherObjectHasABoundingBoxOf(int x, int y, int w, int h)
        {
            _objectB = MockRepository.GenerateStub<ICollidable<BoundingBox>>();

            _objectB.Stub(e => e.Area).Return(new BoundingBox(new Rectangle(x, y, w, h)));
        }

        private void DetectingACollision()
        {
             _result = _collisionDetector.DetectCollision(_objectA, _objectB);
        }

        private void ACollisionShouldBeDetected()
        {
            Assert.That(_result, Is.Not.Null);
        }

        private void ACollisionShouldNotBeDetected()
        {
            Assert.That(_result, Is.Null);
        }
    }
}
