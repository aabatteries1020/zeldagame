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
    public class CollisionCollectionTests
    {
        private CollisionCollection _collisionCollection;
        private LinkedList<ICollidable> _list;

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
                .Given(WeHaveACollisionCollection)
                .When(AnObjectIsAddedToTheCollisionCollectionWithBoundingBox, x1, y1, w1, h1)
                    .And(AnObjectIsAddedToTheCollisionCollectionWithBoundingBox, x2, y2, w2, h2)
                .Then(TheBoundaryShouldBeEqualTo, x3, y3, w3, h3)
                .ExecuteWithReport();
        }

        private void TheBoundaryShouldBeEqualTo(int x, int y, int w, int h)
        {
            Assert.That(_collisionCollection.BoundingBox, Is.EqualTo(new Rectangle(x, y, w, h)));
        }

        private void WeHaveACollisionCollection()
        {
            _list = new LinkedList<ICollidable>();
            _collisionCollection = new CollisionCollection(_list);
        }

        private void AnObjectIsAddedToTheCollisionCollectionWithBoundingBox(int x, int y, int w, int h)
        {
            var collidable = MockRepository.GenerateStub<ICollidable>();

            collidable.Stub(e => e.BoundingBox).Return(new Rectangle(x, y, w, h));

            _list.AddLast(collidable);
        }        
    }
}
