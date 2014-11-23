using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public class BowFiringState : IState
    {
        private IArrowFactory _arrowFactory;
        private IDirectionable _directionable;
        private IDirectionAnimationSet _directionAnimationSet;
        private Func<IState> _endingStateCallback;

        public BowFiringState(IArrowFactory arrowFactory, IDirectionable directionable, IDirectionAnimationSet directionAnimationSet, Func<IState> endingStateCallback)
        {
            _arrowFactory = arrowFactory;
            _directionable = directionable;
            _directionAnimationSet = directionAnimationSet;
            _endingStateCallback = endingStateCallback;

            _directionable.Animation = _directionAnimationSet[_directionable.Direction];
            _arrowFactory.CreateArrow(_directionable.Position, _directionable.Direction);
        }

        public void AdvanceLogic()
        {
            if(_directionable.Animation.IsComplete)
            {
                _directionable.State = _endingStateCallback();
            }
        }

        public bool CanUseItems
        {
            get { return false; }
        }
    }
}
