using System;

namespace ZeldaGame
{
    public interface IItem
    {
        IState CreateState(IDirectionable directionable, IGroupAnimationSet groupAnimationSet, Func<IState> endingStateCallback);
    }
}
