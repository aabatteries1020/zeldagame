using Axiom.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame.Tests
{
    public class Link
    {
        public const double Speed = 2;

        private IControllable controllable;

        public Link(IControllable controllable)
        {
            // TODO: Complete member initialization
            this.controllable = controllable;
        }
        public void AdvanceLogic()
        {
            throw new NotImplementedException();
        }

        public Vector2 Position
        {
            get { throw new NotImplementedException(); }
        }
    }
}
