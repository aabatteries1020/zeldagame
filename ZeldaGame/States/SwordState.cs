using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame.Tests
{
    public class SwordState : IState
    {
        private IDirectionable _directionable;
        private IDirectionAnimationSet _directionAnimationSet;
        private object _endingState;

        public SwordState(IDirectionable directionable, IDirectionAnimationSet directionAnimationSet, object endingState)
        {
            _directionable = directionable;
            _directionAnimationSet = directionAnimationSet;
            _endingState = endingState;

            _directionable.Animation = _directionAnimationSet[_directionable.Direction];
        }

        public void AdvanceLogic()
        {
            if(_directionable.Animation.IsComplete)
            {
                _directionable.State = _endingState;
            }
        }
    }
}
