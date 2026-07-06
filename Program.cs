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
