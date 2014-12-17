using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ZeldaGame
{
    public class CollisionCollection : IEnumerable<ICollidable>
    {
        private IEnumerable<ICollidable> _collidables;

        public CollisionCollection(IEnumerable<ICollidable> collidables)
        {
            _collidables = collidables;
        }

        public IEnumerator<ICollidable> GetEnumerator()
        {
            return _collidables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collidables.GetEnumerator();
        }

        public Rectangle BoundingBox
        {
            get
            {
                var rect = _collidables.FirstOrDefault().BoundingBox;

                foreach(ICollidable i in _collidables)
                {
                    rect = Rectangle.Union(rect, i.BoundingBox);
                }

                return rect;
            }
        }
    }
}
