using StarRailTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StarRailTracker.Service
{
    static internal class DisplayService
    {
        static private readonly List<string> userModesForDisplay = [
            "0:結束程式",
            "1:透過URL自動匯入抽卡紀錄 (請先確認有在24小時內於遊戲中開啟過歷史紀錄)",
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
        static private readonly List<int> gachaTypes = Contents.GachaTypes.Keys.ToList();
        
        static public string ChooseFunction()//初始功能列表
        {
            string function;

            Console.WriteLine();
            Console.WriteLine("====== 功能列表 ======");
            foreach (string userModeForDisplay in userModesForDisplay)
            {
                Console.WriteLine(userModeForDisplay);
            }

            Console.WriteLine();
            Console.WriteLine("請輸入欲執行的功能：");
            function = Console.ReadLine();

            return function;
        }

        static public List<GachaLog> InputFromJson()//取得來自Json的紀錄
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            Console.WriteLine("====== 輸入Json檔案 ======");
            Console.WriteLine("請先將檔案放置於Warp History資料夾中");
            Console.WriteLine("請輸入檔案相對路徑(須為Json檔案)");
            Console.WriteLine("範例：若在Warp History/A資料夾/B資料夾/example.json，請輸入：A資料夾/B資料夾/example");
            Console.WriteLine("請輸入檔案位置：");
            //string filePath = @$"Warp History/{Console.ReadLine()}.json";
            string filePath = Path.Combine(baseDir, "Warp History", $"{Console.ReadLine()}.json");

            List<GachaLog> gachaLogs =  GachaHistoryService.GetGachaHistoryFromJson(filePath);//已將檔案路徑不存在的情況處理在其中

            return gachaLogs;
        }

        //搜尋紀錄功能執行(需input記錄檔)，獨立執行
        static public void ChooseHistoryFuction(List<StatisticDataList> statistics)
        {
            int targetGachaType = 0;
            int targetRank = 0;
            StatisticDataList tempStatistics;

            Console.WriteLine("====== 可查詢的類型 ======");
            foreach (string historyFunctionMode in historyFunctionModes){Console.WriteLine(historyFunctionMode);}

            Console.WriteLine();
            Console.WriteLine("請輸入欲查詢的類型：");
            string choice1 = Console.ReadLine();

            Console.WriteLine();

            switch (choice1)
            {
                case "1"://全卡池五星
                    foreach (StatisticDataList statisticDataList in statistics)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"====== {statisticDataList.GachaTypeName} ======");
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
                        Console.WriteLine();
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
                    foreach (var gachaTypeForDisplay in Contents.GachaTypes) {
                        Console.WriteLine($"{gachaTypeForDisplay.Key}：{gachaTypeForDisplay.Value}");
                    }
                    Console.WriteLine("輸入欲查詢的卡池類別：");
                    if (!int.TryParse(Console.ReadLine(), out targetGachaType))
                    {
                        targetGachaType = -1;
                    }

                    Console.WriteLine();

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
                    foreach (var gachaTypeForDisplay in Contents.GachaTypes)
                    {
                        Console.WriteLine($"{gachaTypeForDisplay.Key}：{gachaTypeForDisplay.Value}");
                    }
                    Console.WriteLine("輸入欲查詢的卡池類別：");
                    targetGachaType = int.Parse(Console.ReadLine());
                    Console.WriteLine("輸入欲查詢的星級(4/5)：");
                    if (!int.TryParse(Console.ReadLine(), out targetRank))
                    {
                        targetRank = -1;
                    }

                    Console.WriteLine();

                    if (gachaTypes.Contains(targetGachaType) && (targetRank == 4 || targetRank == 5))
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
