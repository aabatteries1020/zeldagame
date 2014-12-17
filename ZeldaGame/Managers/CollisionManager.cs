using System.Collections.Generic;

namespace ZeldaGame
{
    public class CollisionManager
    {
        private LinkedList<ICollidable> _objects = new LinkedList<ICollidable>();
        private Dictionary<ICollidable, LinkedList<ICollidable>> _collisions = new Dictionary<ICollidable, LinkedList<ICollidable>>();

        public void CalculateCollisions()
        {
            _collisions = new Dictionary<ICollidable, LinkedList<ICollidable>>();

            var nodeA = _objects.First;
            LinkedListNode<ICollidable> nodeB = null;

            while(nodeA != null)
            {
                nodeB = nodeA.Next;

                while (nodeB != null)
                {
                    if(Collision(nodeA.Value, nodeB.Value))
                    {
                        CollisionDetected(nodeA.Value, nodeB.Value);
                        CollisionDetected(nodeB.Value, nodeA.Value);
                    }

                    nodeB = nodeB.Next;
                }

                nodeA = nodeA.Next;
            }
        }

        private void CollisionDetected(ICollidable a, ICollidable b)
        {
            LinkedList<ICollidable> collection;

            if(!_collisions.TryGetValue(a, out collection))
            {
                collection = new LinkedList<ICollidable>();

                _collisions.Add(a, collection);
            }

            collection.AddLast(b);
        }

        private bool Collision(ICollidable a, ICollidable b)
        {
            if (a.BoundingBox.Right <= b.BoundingBox.Left)
                return false;
            if (b.BoundingBox.Right <= a.BoundingBox.Left)
                return false;
            if (a.BoundingBox.Bottom <= b.BoundingBox.Top)
                return false;
            if (b.BoundingBox.Bottom <= a.BoundingBox.Top)
                return false;

            return true;
        }

        public CollisionCollection GetCollisionsFor(ICollidable value)
        {
            LinkedList<ICollidable> collection;

            if (!_collisions.TryGetValue(value, out collection))
                return null;

            return new CollisionCollection(collection);
        }

        public void Add(ICollidable value)
        {
            _objects.AddLast(value);
        }
    }
}
