using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OLiOYouxi.OCoroutine.Internals
{
    internal class OCoroutineWrapperInternal
    {
        #region -- 单例 --
        static private OCoroutineWrapperInternal instance = null;
        private OCoroutineWrapperInternal()
        {

        }
        static internal OCoroutineWrapperInternal ForNewOrExistOCoroutineWrapperInternal()
            => instance == null ? instance = new OCoroutineWrapperInternal() : instance;
        #endregion

        #region -- Internal APIMethods --
        internal OCoroutine GetCoroutine(ref ScriptableObject @object, ref IEnumerator routine)
            => OCoroutine.ForNewOCoroutine(@object, routine);

        #endregion
    }
}
