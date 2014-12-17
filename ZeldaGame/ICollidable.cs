using Microsoft.Xna.Framework;

namespace ZeldaGame
{
    public interface ICollidable
    {
        Rectangle BoundingBox { get; }
    }
}
