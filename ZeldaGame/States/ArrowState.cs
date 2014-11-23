namespace ZeldaGame
{
    public class ArrowState : IState
    {
        private MovingState _state;

        private class Controller : IControllable
        {
            public Controller(Direction direction)
            {
                Direction = direction;
            }

            public Direction Direction
            {
                get;
                private set;
            }

            public EquipmentSlots Slot
            {
                get;
                set;
            }
        }

        public ArrowState(IDirectionable directionable, IDirectionAnimationSet directionAnimationSet)
        {
            _state = new MovingState(directionable, directionAnimationSet, new Controller(directionable.Direction), null, 1.0f);
        }

        public void AdvanceLogic()
        {
            _state.AdvanceLogic();
        }

        public bool CanUseItems
        {
            get { return false; }
        }
    }
}
