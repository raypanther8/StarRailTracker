// See https://aka.ms/new-console-template for more information

using StarRailTracker.Model;
using StarRailTracker.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("=== 星鐵抽卡追蹤器 ===");

Console.WriteLine("\n輸入URL: ");
string url = Console.ReadLine();

try
{
    List<GachaLog> gachaLogs = await GachaHistoryService.GetGachaHistory(url);//取得抽卡紀錄
    Console.WriteLine($"\n總共取得 {gachaLogs.Count} 筆抽卡紀錄。");

    GachaHistoryService.OutputJson(gachaLogs); // 儲存結果到JSON檔案

    List<StatisticDataList> statistics = Statistics.GetTotalStatisticData(gachaLogs);
    foreach (var stat in statistics)
    {
        Console.WriteLine($"\n====== {stat.GachaTypeName} ======");
        foreach (var data in stat.Data)
        {
            if (data.Name == "已墊抽數")
            {
                Console.WriteLine($"{data.Rank} 星已墊抽數：{data.PullsSinceLastRank, -2}");
                continue;
            }
            else if (data.Rank == 5)
            {
                Console.WriteLine($"{data.Rank}星，{data.Name}，花費 {data.PullsSinceLastRank, -2} 抽");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"發生錯誤: {ex.Message}");
}
finally
{
    Console.WriteLine("=== 程式結束 ===");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
