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
    foreach (var gachaLog in gachaLogs)
    {
        Console.WriteLine($"{gachaLog.GachaType}：[{gachaLog.Time}] {gachaLog.Name} ({gachaLog.ItemType}) - {gachaLog.RankType}★");
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
