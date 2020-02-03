using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OLiOYouxi.OCoroutine
{
    using Wrapper = OLiOYouxi.OCoroutine.Internals.OCoroutineWrapperInternal;
    using Runner = OLiOYouxi.OCoroutine.Internals.OCoroutineRunnerInternal;
    using Debug = OLiOYouxi.OSystem.DebugFather;

    /// <summary>
    /// 脚本对象拓展
    /// </summary>
    static public class ScriptableObjectExtensions
    {
        /// <summary>
        /// 开启协程
        /// </summary>
        /// <param name="object">脚本对象</param>
        /// <param name="routine">程序</param>
        /// <returns>OCoroutine</returns>
        static public OCoroutine StartCoroutine(this ScriptableObject @object, IEnumerator routine)
        {
            if (routine == null) throw new ArgumentNullException(nameof(routine));
            Runner runner = Runner.ForNewOrExistOCoroutineRunnerInternal();
            Wrapper wrapper = Wrapper.ForNewOrExistOCoroutineWrapperInternal();
            if (runner == null || wrapper == null) throw new Exception("致命错误！");
            return runner.Register(wrapper.GetCoroutine(ref @object, ref routine));
        }
        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="object">脚本对象</param>
        /// <param name="coroutine">协程</param>
        static public void StopCoroutine(this ScriptableObject @object, OCoroutine coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException(nameof(coroutine));
            Runner runner = Runner.ForNewOrExistOCoroutineRunnerInternal();
            if (runner == null) throw new Exception("致命错误！");
            if (runner.Cancel(coroutine)) Debug.Log = "已经结束协程！";
            else Debug.LogErr = "抱歉，无法结束。。";
        }
        /// <summary>
        /// 停止所有协程
        /// </summary>
        /// <param name="object">脚本对象</param>
        static public void StopAllCoroutine(this ScriptableObject @object)
        {
            Runner runner = Runner.ForNewOrExistOCoroutineRunnerInternal();
            if (runner == null) throw new Exception("致命错误！");
            if (runner.Terminate()) Debug.Log = "已经结束所有协程！";
            else Debug.LogErr = "抱歉，无法结束。。";
        }
    }
}
