using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OSystem
{
    using OLiOYouxi.OSystem.Internals;
    using Random = UnityEngine.Random;
    using Mono = UnityEngine.MonoBehaviour;
    using Object = UnityEngine.Object;

    /// <summary>
    /// 执行延迟且重复数次的任务
    /// </summary>
    public class CoroutineFather
    {
        #region -- Private Data --
        private Dictionary<int, Coroutine> coroutineMapper = null;
        private Dictionary<int, CoroutineStatusName> statusMapper = null;
        private Mono mono = null;

        #endregion

        #region -- MONO APIMethods --
        /// <summary>
        /// 在Awake中调用
        /// </summary>
        /// <typeparam name="T">继承MonoBehaviour的对象的类型</typeparam>
        /// <param name="origin">对象</param>
        public void Awake<T>(T origin) where T : Object
        {
            coroutineMapper = new Dictionary<int, Coroutine>();
            statusMapper = new Dictionary<int, CoroutineStatusName>();
            mono = origin as MonoBehaviour;
        }
        #endregion

        #region -- Public APIMethods --
        /// <summary>
        /// 执行一个可延迟可重复的任务
        /// </summary>
        /// <param name="action">任务</param>
        /// <param name="delay">延迟</param>
        /// <param name="repeat">重复次数</param>
        /// <returns>int</returns>
        public int StartCoroutine(Action action, float delay, int repeat)
        {
            int randomKey;
            do randomKey = Random.Range(int.MinValue, int.MaxValue);
            while (coroutineMapper.ContainsKey(randomKey) || statusMapper.ContainsKey(randomKey));
            statusMapper.Add(randomKey, CoroutineStatusName.busy);
            Coroutine coroutine = mono.StartCoroutine(DelayFunc(action, delay, repeat, randomKey));
            coroutineMapper.Add(randomKey, coroutine);
            //返回令牌(靠这个结束该协程)
            return randomKey;
        }
        /// <summary>
        /// 根据令牌停止指定的任务
        /// </summary>
        /// <param name="token">令牌</param>
        public void StopCoroutine(int token)
        {
            if (!coroutineMapper.ContainsKey(token) || !statusMapper.ContainsKey(token)) return;
            mono.StopCoroutine(coroutineMapper[token]);
            statusMapper[token] = CoroutineStatusName.pause;
        }
        /// <summary>
        /// 根据令牌继续指定的任务
        /// </summary>
        /// <param name="token">令牌</param>
        public void ContinueCoroutine(int token)
        {
            if (!coroutineMapper.ContainsKey(token) || !statusMapper.ContainsKey(token)) return;
            statusMapper[token] = CoroutineStatusName.busy;
        }
        /// <summary>
        /// 根据令牌取消指定的任务
        /// </summary>
        /// <param name="token">令牌</param>
        public void CancelCoroutine(int token)
        {
            if (!coroutineMapper.ContainsKey(token) || !statusMapper.ContainsKey(token)) return;
            statusMapper[token] = CoroutineStatusName.cancel;
        }

        #endregion

        #region -- Private APIMethods --
        private IEnumerator DelayFunc(Action action, float delay, int repeat, int key)
        {
            WaitForSeconds seconds = new WaitForSeconds(delay);
            while (true)
            {
                if (statusMapper[key] == CoroutineStatusName.cancel) break;
                else if (statusMapper[key] == CoroutineStatusName.pause) yield return null;
                else if (statusMapper[key] == CoroutineStatusName.busy)
                {
                    if (repeat-- <= 0) break;
                    else
                    {
                        yield return seconds;
                        action.Invoke();
                    }
                }
                else break;
            }
            coroutineMapper.Remove(key);
            statusMapper.Remove(key);
        }

        #endregion
    }
}
