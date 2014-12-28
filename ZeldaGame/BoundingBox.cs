using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame
{
    public struct BoundingBox
    {
        public BoundingBox(Rectangle rect) : this(rect, new Index2(1, 1))
        {

        }

        public BoundingBox(Rectangle rect, Index2 gridSize) : this()
        {
            Area = rect;
            GridSize = gridSize;
        }

        public Rectangle Area
        {
            get;
            set;
        }

        public Index2 GridSize
        {
            get;
            set;
        }

        public Rectangle Center
        {
            get
            {
                var centerX = GridSize.x / 2;
                var centerY = GridSize.y / 2;

                var x = Area.X + centerX;
                var y = Area.Y + centerY;

                return new Rectangle(x, y, Area.Width / GridSize.x, Area.Height / GridSize.y);
            }
        }

        public static bool DoesCollide(Rectangle a, Rectangle b)
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

        public bool IsCenter(Rectangle rect)
        {
            return DoesCollide(Center, rect);
        }

        public bool IsCenter(Index2 index)
        {
            int centerX = GridSize.x / 2;
            int centerY = GridSize.y / 2;

            return index.x == centerX && index.y == centerY;
        }
    }
}
