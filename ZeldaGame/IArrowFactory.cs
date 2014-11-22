using Microsoft.Xna.Framework;

namespace ZeldaGame
{
    public interface IArrowFactory
    {
        void CreateArrow(Vector2 vector2, Direction direction);
    }
}
