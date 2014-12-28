using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ZeldaGame.States;

namespace ZeldaGame
{
    public class Link : IDirectionable
    {
        private IResourceManager _resourceManager;
        private IControllable _controllable;
        private CollisionManager<BoundingBox, Rectangle> _collisionManager;

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

        public Link(CollisionManager<BoundingBox, Rectangle> collisionManager, IResourceManager resourceManager, IControllable controllable)
        {
            _collisionManager = collisionManager;
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
                State = new MovingState(_collisionManager, this, GroupSet.LoadDirectionSet("Walking"), _controllable, CreateStandingState, 2f);
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

        private Index2 gridSize = new Index2(3, 3);

        public BoundingBox Area
        {
            get
            {
                return new BoundingBox(new Rectangle((int)Position.X, (int)Position.Y, 16, 8), gridSize);
            }
        }

        public CollisionType Type
        {
            get { return CollisionType.Dynamic; }
        }
    }
}
