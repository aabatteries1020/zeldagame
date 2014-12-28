using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZeldaGame
{
    public class CollisionManager<T, TResult> where TResult : struct
    {
        private LinkedList<ICollidable<T>> _objects = new LinkedList<ICollidable<T>>();
        private Dictionary<ICollidable<T>, Dictionary<ICollidable<T>, TResult>> _collisions;
        private ICollisionDetector<T, TResult> _collisionDetector;

        public ICollisionDetector<T, TResult> CollisionDetector
        {
            get { return _collisionDetector; }
        }

        public CollisionManager(ICollisionDetector<T, TResult> collisionDetectionType)
        {
            _collisionDetector = collisionDetectionType;

            Reset();
        }

        private void Reset()
        {
            _collisions = new Dictionary<ICollidable<T>, Dictionary<ICollidable<T>, TResult>>();
        }

        public void CalculateCollisions()
        {
            Reset();

            var nodeA = _objects.First;
            LinkedListNode<ICollidable<T>> nodeB = null;

            while(nodeA != null)
            {
                nodeB = nodeA.Next;

                while (nodeB != null)
                {
                    var result = _collisionDetector.DetectCollision(nodeA.Value, nodeB.Value);
                    
                    if(result != null)
                    {
                        CollisionDetected(nodeA.Value, nodeB.Value, result.Value);
                        CollisionDetected(nodeB.Value, nodeA.Value, result.Value);
                    }

                    nodeB = nodeB.Next;
                }

                nodeA = nodeA.Next;
            }
        }

        private void CollisionDetected(ICollidable<T> a, ICollidable<T> b, TResult result)
        {
            Dictionary<ICollidable<T>, TResult> collection;

            if(!_collisions.TryGetValue(a, out collection))
            {
                collection = new Dictionary<ICollidable<T>, TResult>();

                _collisions.Add(a, collection);
            }

            collection.Add(b, result);
        }

        public ReadOnlyDictionary<ICollidable<T>, TResult> GetCollisionsFor(ICollidable<T> value)
        {
            Dictionary<ICollidable<T>, TResult> collection;

            if (!_collisions.TryGetValue(value, out collection))
                return null;

            return new ReadOnlyDictionary<ICollidable<T>, TResult>(collection);
        }

        public void Add(ICollidable<T> value)
        {
            _objects.AddLast(value);
        }
    }
}
