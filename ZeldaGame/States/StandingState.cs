namespace ZeldaGame.States
{
    public class StandingState : IState
    {
        public StandingState(IDirectionable directionable, IDirectionAnimationSet directionAnimationSet)
        {
            directionable.Animation = directionAnimationSet[directionable.Direction];
        }

        public void AdvanceLogic()
        {
            
        }

        public bool CanUseItems
        {
            get { return true; }
        }
    }
}
