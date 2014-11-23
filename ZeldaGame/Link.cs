using Microsoft.Xna.Framework;
using ZeldaGame.States;

namespace ZeldaGame
{
    public class Link : IDirectionable
    {
        private IResourceManager _resourceManager;
        private IControllable _controllable;

        public Vector2 Position
        {
            get;
            set;
        }

        public Direction Direction
        {
            get;
            set;
        }

        public IState State
        {
            get;
            set;
        }

        public IAnimation Animation
        {
            get;
            set;
        }

        public Equipment Equipment
        {
            get;
            private set;
        }

        public Link(IResourceManager resourceManager, IControllable controllable)
        {
            _resourceManager = resourceManager;
            _controllable = controllable;

            Equipment = new Equipment();
            State = CreateStandingState();
        }

        private IGroupAnimationSet GroupSet
        {
            get { return _resourceManager.LoadGroupSet("Link"); }
        }

        private IState CreateStandingState()
        {
            return new StandingState(this, GroupSet.LoadDirectionSet("Standing"));
        }

        public void AdvanceLogic()
        {
            var stateType = State.GetType();

            if(stateType == typeof(StandingState) && _controllable.Direction != ZeldaGame.Direction.None)
            {
                State = new MovingState(this, GroupSet.LoadDirectionSet("Walking"), _controllable, CreateStandingState, 2f);
            }
            else if(State.CanUseItems && _controllable.Slot != EquipmentSlots.Unassigned)
            {
                var item = Equipment[_controllable.Slot];

                if(item != null)
                {
                    this.State = item.CreateState(this, GroupSet, CreateStandingState);
                }
            }
        }
    }
}
