using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OLiOYouxi.OCoroutine
{
    using YieldStruct = OLiOYouxi.OCoroutine.Internals.YieldStructInternal;
    /// <summary>
    /// 奥利奥协程
    /// </summary>
    sealed public class OCoroutine
    {
        #region -- Private Data --
        private ScriptableObject @object = null;
        private IEnumerator routine = null;
        private YieldStruct currentYield;

        #endregion

        #region -- New --
        private OCoroutine(ScriptableObject @object, IEnumerator routine)
        {
            this.@object = @object;
            this.routine = routine;
            this.currentYield = new YieldStruct { timeLeft = .0f };
        }

        static internal OCoroutine ForNewOCoroutine(ScriptableObject @object, IEnumerator routine)
            => new OCoroutine(@object, routine);

        #endregion

        #region -- Internal ShotC --
        internal bool IsDone => this.@object == null;

        #endregion

        #region -- Internal APIMethods --
        internal bool Run() => routine.MoveNext();
        internal object Result() => routine.Current;
        internal bool Wait(float timeConsume) => !currentYield.IsDone(timeConsume);
        internal void Update(YieldStruct currentYield) => this.currentYield = currentYield;
        internal void Clear()
        {
            this.@object = null;
            this.routine = null;
        }
        #endregion
    }
}
