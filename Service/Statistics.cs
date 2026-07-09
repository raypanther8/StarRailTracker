using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarRailTracker.Model;

namespace StarRailTracker.Service
{
    static internal class Statistics
    {
        public static int CountByRank(List<GachaLog> gachaLogs, int rank)
        {
            return gachaLogs.Count(log => log.RankType == rank);
        }
        private static List<StatisticData> PullsSinceLastRankList(List<GachaLog> gachaLogs, int rank, int gachaType)
        {
            List<StatisticData> statistics = new(); //用來紀錄要輸出的結果
            // 篩選指定抽卡類型
            List<GachaLog> specificGachaLogs = GachaProcess.FinishingProcess(gachaLogs.Where(log => log.GachaType == gachaType).ToList());

            string name;
            int pullsSinceLastRank = 0;

            // 遍歷篩選後的抽卡紀錄並統計
            foreach (var log in specificGachaLogs)
            {
                pullsSinceLastRank++;
                if (log.RankType == rank)
                {
                    name = log.Name;

                    //將結果加入統計列表
                    statistics.Add(new StatisticData
                    {
                        Name = name,
                        Rank = rank,
                        PullsSinceLastRank = pullsSinceLastRank
                    });

                    pullsSinceLastRank = 0; //重置計數器，因為已經抽到指定的稀有度
                }
            }

            statistics.Add(new StatisticData
            {
                Name = "已墊抽數",
                Rank = rank,
                PullsSinceLastRank = pullsSinceLastRank
            });

            return statistics;
        }
        public static List<StatisticDataList> GetTotalStatisticData(List<GachaLog> gachaLogs)
        {
            List<StatisticDataList> totalStatistics = new();
            int[] gachaTypes = [1, 2, 11, 12]; //所有抽卡類型

            List<StatisticData> statisticsForGachaType = new();
            foreach (int gachaType in gachaTypes)
            {
                for (int rank = 4; rank <= 5; rank++)
                {
                    statisticsForGachaType.AddRange(PullsSinceLastRankList(gachaLogs, rank, gachaType));
                }
                totalStatistics.Add(new StatisticDataList
                {
                    GachaType = gachaType,
                    Data = statisticsForGachaType
                });
                statisticsForGachaType = new();
            }

            return totalStatistics;
        }
    }
}
