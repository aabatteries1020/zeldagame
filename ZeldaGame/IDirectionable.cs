using Microsoft.Xna.Framework;

namespace ZeldaGame
{
    public interface IDirectionable : ICollidable<BoundingBox>
    {
        Vector2 Position
        {
            get;
            set;
        }

        Direction Direction
        {
            get;
            set;
        }

        IState State { get; set; }
        IAnimation Animation { get; set; }
    }
}
