using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarRailTracker.Service
{
    static internal class GachaProcess
    {
        static public List<Model.GachaLog> Combine(List<Model.GachaLog> gachaLogs1, List<Model.GachaLog> gachaLogs2)
        {
            var Warps = gachaLogs1.Concat(gachaLogs2).DistinctBy(x => x.Id).ToList();
            return Warps;
        }
        static public List<Model.GachaLog> FinishingProcess(List<Model.GachaLog> gachaLogs)
        {
            //去重、排序
            var Warps = gachaLogs.DistinctBy(x => x.Id).ToList();
            Warps = Warps.OrderBy(x => x.GachaType)
                .ThenBy(x => x.Id.Length)
                .ThenBy(x => x.Id)
                .ToList();
            return Warps;
        }
    }
}
