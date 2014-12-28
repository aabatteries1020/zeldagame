using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeldaGame
{
    public interface ICollisionDetector<T, TResult> where TResult : struct
    {
        TResult? DetectCollision(ICollidable<T> a, ICollidable<T> b);
        TResult? CalculateBoundary(ICollidable<BoundingBox> parent, IReadOnlyDictionary<ICollidable<T>, TResult> collection);
    }
}
