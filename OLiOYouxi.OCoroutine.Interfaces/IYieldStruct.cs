using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLiOYouxi.OCoroutine.Interfaces
{
    /// <summary>
    /// 返回结构
    /// </summary>
    public interface IYieldStruct
    {
        /// <summary>
        /// 是否已经把时间消耗完了
        /// </summary>
        /// <param name="timeConsume">耗时</param>
        /// <returns>bool</returns>
        bool IsDone(float timeConsume);
    }
}
