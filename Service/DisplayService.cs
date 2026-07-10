using StarRailTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarRailTracker.Service
{
    static internal class DisplayService
    {
        static private readonly List<string> userModesForDisplay = [
            "0:結束程式",
            "1:透過URL匯入抽卡紀錄，並輸出Json檔案",
            "2:透過Json檔案輸入過往紀錄",
            "3:分析並查詢過往紀錄",
            "4:彙整所有抽卡紀錄為單一檔案"
            ];
        static private readonly List<string> historyFunctionModes = [
            "1:全卡池 五星",
            "2:全卡池 四星",
            "3:單一卡池 四、五星(不建議用，因為資料會很多)",
            "4:單一卡池 單一星級"
            ];
        static private readonly List<int> gachaTypes = [1, 2, 11, 12];
        static private readonly List<string> gachaTypesForDisplay = [
            "1:常駐卡池",
            "2:新手卡池",
            "11:角色卡池",
            "12:光錐卡池"
            ];
        static public string ChooseFunction()//初始功能列表
        {
            string function;

            Console.WriteLine();
            Console.WriteLine("======功能列表======");
            foreach (string userModeForDisplay in userModesForDisplay)
            {
                Console.WriteLine(userModeForDisplay);
            }

            Console.WriteLine();
            Console.WriteLine("請輸入欲執行的功能：");
            function = Console.ReadLine();

            return function;
        }

        //搜尋紀錄功能執行(需input記錄檔)，獨立執行
        static public void ChooseHistoryFuction(List<StatisticDataList> statistics)
        {
            int targetGachaType = 0;
            int targetRank = 0;
            StatisticDataList tempStatistics;

            Console.WriteLine("======可查詢的類型======");
            foreach (string historyFunctionMode in historyFunctionModes){Console.WriteLine(historyFunctionMode);}

            Console.WriteLine("請輸入欲查詢的類型：");
            string choice1 = Console.ReadLine();
            switch (choice1)
            {
                case "1"://全卡池五星
                    foreach (StatisticDataList statisticDataList in statistics)
                    {
                        Console.WriteLine($"======{statisticDataList.GachaTypeName}======");
                        List<StatisticData> statisticDatas = statisticDataList.Data.Where(x => x.Rank == 5).ToList();
                        foreach(StatisticData statisticData in statisticDatas)
                        {
                            if (statisticData.Name == "已墊抽數")
                            {
                                Console.WriteLine($"{statisticData.Rank} 星 {statisticData.Name}：{statisticData.PullsSinceLastRank}");
                            }
                            else
                            {
                                Console.WriteLine($"{statisticData.Rank} 星，花費 {statisticData.PullsSinceLastRank,-2} 抽，{statisticData.Name}");
                            }
                        }
                    }
                    break;
                case "2"://全卡池四星
                    foreach (StatisticDataList statisticDataList in statistics)
                    {
                        Console.WriteLine($"======{statisticDataList.GachaTypeName}======");
                        List<StatisticData> statisticDatas = statisticDataList.Data.Where(x => x.Rank == 4).ToList();
                        foreach (StatisticData statisticData in statisticDatas)
                        {
                            if (statisticData.Name == "已墊抽數")
                            {
                                Console.WriteLine($"{statisticData.Rank} 星 {statisticData.Name}：{statisticData.PullsSinceLastRank}");
                            }
                            else
                            {
                                Console.WriteLine($"{statisticData.Rank} 星，花費 {statisticData.PullsSinceLastRank,-2} 抽，{statisticData.Name}");
                            }
                        }
                    }
                    break;
                case "3"://單一卡池 四、五星
                    Console.WriteLine("======可查詢的卡池類別======");
                    foreach (string gachaTypeForDisplay in gachaTypesForDisplay) { Console.WriteLine(gachaTypeForDisplay); }
                    Console.WriteLine("輸入欲查詢的卡池類別：");
                    targetGachaType = int.Parse(Console.ReadLine());
                    
                    if (gachaTypes.Contains(targetGachaType)){
                        tempStatistics = statistics.First(x => x.GachaType == targetGachaType);
                        foreach (StatisticData statisticData in tempStatistics.Data)
                        {
                            if (statisticData.Name == "已墊抽數")
                            {
                                Console.WriteLine($"{statisticData.Rank} 星 {statisticData.Name}：{statisticData.PullsSinceLastRank}");
                            }
                            else
                            {
                                Console.WriteLine($"{statisticData.Rank} 星，花費 {statisticData.PullsSinceLastRank,-2} 抽，{statisticData.Name}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("無此卡池");
                    }
                        break;
                case "4"://單一卡池 單一星級
                    Console.WriteLine("======可查詢的卡池類別======");
                    foreach (string gachaTypeForDisplay in gachaTypesForDisplay) { Console.WriteLine(gachaTypeForDisplay); }
                    Console.WriteLine("輸入欲查詢的卡池類別：");
                    targetGachaType = int.Parse(Console.ReadLine());
                    Console.WriteLine("輸入欲查詢的星級(4/5)：");
                    targetRank = int.Parse(Console.ReadLine());

                    if (gachaTypes.Contains(targetGachaType) && targetRank == 4 || targetRank == 5)
                    {
                        tempStatistics = statistics.First(x => x.GachaType == targetGachaType);
                        foreach (StatisticData statisticData in tempStatistics.Data.Where(x => x.Rank == targetRank))
                        {
                            if (statisticData.Name == "已墊抽數")
                            {
                                Console.WriteLine($"{statisticData.Rank} 星 {statisticData.Name}：{statisticData.PullsSinceLastRank}");
                            }
                            else
                            {
                                Console.WriteLine($"{statisticData.Rank} 星，花費 {statisticData.PullsSinceLastRank,-2} 抽，{statisticData.Name}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("無此卡池或星級選項");
                    }
                    break;
                default:
                    Console.WriteLine("無效操作！");
                    break;
            }
        }
    }
}
