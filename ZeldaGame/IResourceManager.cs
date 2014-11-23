using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaGame
{
    public interface IResourceManager
    {
        IGroupAnimationSet LoadGroupSet(string groupSetName);
    }
}
