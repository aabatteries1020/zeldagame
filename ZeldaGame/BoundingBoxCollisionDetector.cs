using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame
{
    public class BoundingBoxCollisionDetector : ICollisionDetector<BoundingBox, Rectangle>
    {
        private bool DetectCollision(Rectangle a, Rectangle b)
        {
            if (a.Right <= b.Left)
                return false;
            if (b.Right <= a.Left)
                return false;
            if (a.Bottom <= b.Top)
                return false;
            if (b.Bottom <= a.Top)
                return false;

            return true;
        }

        public Rectangle? DetectCollision(ICollidable<BoundingBox> a, ICollidable<BoundingBox> b)
        {
            if (!BoundingBox.DoesCollide(a.Area.Area, b.Area.Area))
            {
                return null;
            }

            return Rectangle.Intersect(a.Area.Area, b.Area.Area);
        }

        public Rectangle? CalculateBoundary(ICollidable<BoundingBox> parent, IReadOnlyDictionary<ICollidable<BoundingBox>, Rectangle> collection)
        {
            Rectangle? rect = null;

            foreach (var i in collection)
            {
                if(parent != null && !parent.Area.IsCenter(i.Value))
                {
                    continue;
                }
                
                if(!i.Key.Area.IsCenter(i.Value))
                {
                    continue;
                }

                if(rect == null)
                {

                }

                rect = rect.HasValue ? Rectangle.Union(rect.Value, i.Key.Area.Center) : i.Key.Area.Center;
            }

            return rect;
        }
    }
}
