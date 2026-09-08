using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarRailTracker.Model
{
    internal class Contents
    {
        public static readonly Dictionary<int, string> GachaTypes = new()//抽卡類型對應表
        {
            { 1, "常駐卡池" },
            { 2, "新手卡池" },
            { 11, "限定角色" },
            { 12, "限定光錐" },
            { 21, "Fate聯動角色" },
            { 22, "Fate聯動光錐" }
        };
        public static readonly string unknownGachaType = "未知類型";
    }
}
