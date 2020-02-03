using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OLiOYouxi.OCoroutine.Internals
{
    internal class OCoroutineRunnerInternal
    {
        #region -- Private Data --
        private List<OCoroutine> coroutines = null;
        private DateTime prevTime = DateTime.MinValue;

        #endregion

        #region -- 单例 --
        static private OCoroutineRunnerInternal instance = null;
        private OCoroutineRunnerInternal()
        {
            //init
            coroutines = new List<OCoroutine>();
            prevTime = DateTime.Now;
            //event
            EditorApplication.update += OnUpdate;
        }
        static internal OCoroutineRunnerInternal ForNewOrExistOCoroutineRunnerInternal()
            => instance == null ? instance = new OCoroutineRunnerInternal() : instance;
        #endregion

        #region -- Private Events --
        private void OnUpdate()
        {
            float deltaTime = GetDeltaTime(prevTime);
            prevTime = GetCurrentTime();
            if (coroutines.Count == 0) return;
            coroutines.RemoveAll(f => f.IsDone);
            for (int i = coroutines.Count - 1; i >= 0; i--)
            {
                OCoroutine coroutine = coroutines[i];
                if (coroutine.Wait(deltaTime)) continue;
                if (coroutine.Run())
                {
                    object current = coroutine.Result();
                    if (current is null) coroutine.Update(new YieldStructInternal { timeLeft = 1f });
                    else if (current is WaitForSeconds) coroutine.Update(new YieldStructInternal { timeLeft = GetTimeLeft(current) });
                    else throw new Exception("暂时不支持介种类型。。");
                    continue;
                }
                coroutines.Remove(coroutine);
                coroutine.Clear();
            }
        }

        #endregion

        #region -- Internal APIMethods --
        internal OCoroutine Register(OCoroutine coroutine)
        {
            if (coroutines == null) throw new NullReferenceException(nameof(coroutines));
            coroutines.Add(coroutine);
            return coroutine;
        }
        internal bool Cancel(OCoroutine coroutine)
        {
            if (coroutines == null) throw new NullReferenceException(nameof(coroutines));
            else if (coroutines.Count == 0) return true;
            else
            {
                bool outData = coroutines.Remove(coroutine);
                coroutine.Clear();
                return outData;
            }
        }
        internal bool Terminate()
        {
            if (coroutines == null) throw new NullReferenceException(nameof(coroutines));
            else if (coroutines.Count == 0) return true;
            else
            {
                bool outData = false;
                for (int i = coroutines.Count -1; i >= 0; i--)
                {
                    OCoroutine coroutine = coroutines[i];
                    if (coroutines.Remove(coroutine))
                    {
                        coroutine.Clear();
                        outData = true;
                        continue;
                    }
                    break;
                }
                return outData;
            }
        }

        #endregion

        #region -- Private APIMethods --
        private float GetDeltaTime(DateTime prevTime) => (float)DateTime.Now.Subtract(prevTime).TotalMilliseconds / 1000.0f;
        private DateTime GetCurrentTime() => DateTime.Now;
        private float GetTimeLeft(object current) => float.Parse(GetInstanceField(typeof(WaitForSeconds), current, "m_Seconds").ToString());
        private object GetInstanceField(Type type, object instance, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return field.GetValue(instance);
        }

        #endregion
    }
}
