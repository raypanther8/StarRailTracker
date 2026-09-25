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
        public string Id { get; set; }
    }
    public struct StatisticDataList
    {
        public int GachaType { get; set; }
        public readonly string GachaTypeName
        {
            get
            {
                return Contents.GachaTypes.TryGetValue(GachaType, out string name) ? name : Contents.unknownGachaType;
            }
        }
        public List<StatisticData> Data { get; set; }
    }
}
