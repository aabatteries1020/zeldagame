using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ZeldaGame
{
    public enum CollisionType
    {
        Static,
        Dynamic
    }

    public interface ICollidable<T>
    {
        T Area { get; }

        CollisionType Type { get; }
    }
}
