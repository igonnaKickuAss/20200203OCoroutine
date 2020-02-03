using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLiOYouxi.OCoroutine.Internals
{
    using OLiOYouxi.OCoroutine.Interfaces;

    internal struct YieldStructInternal : IYieldStruct
    {
        public float timeLeft;
        public bool IsDone(float timeConsume) => (timeLeft -= timeConsume) <= 0;
    }
}
