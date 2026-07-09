using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarRailTracker.Model;

namespace StarRailTracker.Service
{
    static internal class GachaProcess
    {
        static public List<GachaLog> Combine(List<GachaLog> gachaLogs1, List<GachaLog> gachaLogs2)
        {
            var Warps = gachaLogs1.Concat(gachaLogs2).DistinctBy(x => x.Id).ToList();
            return Warps;
        }
        static public List<GachaLog> FinishingProcess(List<GachaLog> gachaLogs)
        {
            //去重、排序
            var Warps = gachaLogs.DistinctBy(x => x.Id).ToList();
            Warps = Warps.DistinctBy(x => x.Id)
                .OrderBy(x => x.GachaType)
                .ThenBy(x => x.Id.Length)
                .ThenBy(x => x.Id)
                .ToList();
            return Warps;
        }
    }
}
