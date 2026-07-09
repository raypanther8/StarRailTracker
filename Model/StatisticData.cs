using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarRailTracker.Model
{
    public struct StatisticData
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public int PullsSinceLastRank { get; set; }
    }
    public struct StatisticDataList
    {
        public int GachaType { private get; set; }
        public readonly string GachaTypeName
        {
            get
            {
                return GachaType switch
                {
                    1 => "常駐",
                    2 => "新手",
                    11 => "限定角色",
                    12 => "限定光錐",
                    _ => "未知類型"
                };
            }
        }
        public List<StatisticData> Data { get; set; }
    }
}
