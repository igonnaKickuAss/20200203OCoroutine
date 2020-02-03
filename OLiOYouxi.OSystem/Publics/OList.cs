using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLiOYouxi.OSystem
{
    /// <summary>
    /// 不会产生GC的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OList<T> : IEnumerable<T>, IEnumerable
    {
        #region -- Private Data --
        private T[] ts = null;
        private int m_Count = 0;
        private int m_Capacity = 0;

        #endregion

        #region -- Public ShotC --
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>T</returns>
        public T this[int index]
        {
            get => ts[index];
            set => ts[index] = value;
        }
        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity
        {
            get => m_Capacity;
            set => m_Capacity = value;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count => m_Count;

        #endregion

        #region -- New --
        /// <summary>
        /// 不会产生GC的集合
        /// </summary>
        /// <param name="capacity"></param>
        public OList(int capacity = 10)
        {
            m_Count = 0;
            m_Capacity = capacity;
            this.ts = new T[m_Capacity];
        }
        /// <summary>
        /// 不会产生GC的集合
        /// </summary>
        /// <param name="ts"></param>
        public OList(T[] ts)
        {
            m_Count = ts.Length;
            m_Capacity = ts.Length * 2;
            this.ts = new T[m_Capacity];
            for (int i = 0, length = this.ts.Length; i < length; i++) this.ts[i] = ts[i];
        }

        #endregion

        #region -- Public APIMethods --
        /// <summary>
        /// 末尾添加一个元素
        /// </summary>
        /// <param name="item">元素</param>
        public void Add(T item)
        {
            if(m_Count >= m_Capacity)
            {
                m_Capacity *= 2;
                Array.Resize<T>(ref ts, m_Capacity);
            }
            ts[m_Count] = item;
            m_Count += 1;
        }
        /// <summary>
        /// 末尾添加多个元素
        /// </summary>
        /// <param name="items">多个元素</param>
        public void AddRange(T[] items)
        {
            for (int i = 0, length = items.Length; i < length; i++) Add(items[i]);
        }
        /// <summary>
        /// 移除指定元素
        /// </summary>
        /// <param name="item">元素</param>
        public void Remove(T item) => RemoveAt(Array.IndexOf(ts, item));
        /// <summary>
        /// 移除指定位置的元素
        /// </summary>
        /// <param name="index">索引</param>
        public void RemoveAt(int index)
        {
            if (index == -1 || index >= m_Count) return;
            ArrayList arrayOperation = new ArrayList();
            arrayOperation.AddRange(ts);
            arrayOperation.RemoveAt(index);
            T[] _ts = new T[arrayOperation.Count];
            arrayOperation.CopyTo(_ts);
            arrayOperation.Clear();
            ts = _ts;        //重新赋值，之前的数组在该方法结束后不会再有任何这个方法里的变量去引用他了，出栈清空栈内存
            m_Count -= 1;
            m_Capacity -= 1;
        }
        /// <summary>
        /// 清理集合
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Count; i++) ts[i] = default(T);
            m_Count = 0;
        }
        #endregion

        #region -- Interface APIMethods --
        /// <summary>
        /// 获得枚举数
        /// </summary>
        /// <returns>枚举数</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0, length = ts.Length; i < length; i++)
            {
                if (i < m_Count) yield return ts[i];
                else yield break;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
