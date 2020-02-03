using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OLiOYouxi.OSystem
{
    using OLiOYouxi.OSystem.Internals;

    /// <summary>
    /// 记录红黄绿讯息
    /// </summary>
    public class DebugFather
    {
        #region -- Private Data --
        private string colorString = string.Empty;
        private Dictionary<ColorName, string> colorName = null;

        #endregion

        #region -- 单例 --
        static private DebugFather instance = new DebugFather();
        private DebugFather()
        {
            IEnumerable<KeyValuePair<ColorName, string>> keyValuePairs = new KeyValuePair<ColorName, string>[3]
            {
                new KeyValuePair<ColorName, string>(ColorName.green, "green"),
                new KeyValuePair<ColorName, string>(ColorName.red, "red"),
                new KeyValuePair<ColorName, string>(ColorName.yellow, "yellow")
            };
            //init
            colorString = "<color=>{0}</color>";
            colorName = keyValuePairs.ToDictionary(f => f.Key, f => f.Value);
        }

        #endregion

        #region -- Public ShotC --
        /// <summary>
        /// 记录绿色讯息
        /// </summary>
        static public string Log
        {
            set => instance.ToLog(value);
        }
        /// <summary>
        /// 记录黄色讯息
        /// </summary>
        static public string LogWarn
        {
            set => instance.ToLogWarn(value);
        }
        /// <summary>
        /// 记录红色讯息
        /// </summary>
        static public string LogErr
        {
            set => instance.ToLogErr(value);
        }
        #endregion

        #region -- Private APIMethods --
        private void ToLog(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Debug.LogFormat(colorString.Insert(7, colorName[ColorName.green]), message);
        }

        private void ToLogWarn(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Debug.LogFormat(colorString.Insert(7, colorName[ColorName.yellow]), message);
        }

        private void ToLogErr(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Debug.LogFormat(colorString.Insert(7, colorName[ColorName.red]), message);
        }

        #endregion
    }
}
