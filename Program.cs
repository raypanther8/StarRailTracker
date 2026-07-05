// See https://aka.ms/new-console-template for more information

using StarRailTracker.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("=== 星鐵抽卡追蹤器 ===");

HttpClient client = new();
var dejson_options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true, // 忽略大小寫
    NumberHandling = JsonNumberHandling.AllowReadingFromString// 允許將JSON string轉換成int/bool
};
string url = Console.ReadLine();

try
{
    string json_result = await client.GetStringAsync(url);
    GachaResponse gachaResponse = JsonSerializer.Deserialize<GachaResponse>(json_result, dejson_options);
    List<GachaLog> gachaLogs = gachaResponse.Data.List;

    var Warps = gachaLogs.DistinctBy(x => x.Id).ToList();
    Warps = Warps.OrderByDescending(x => x.Time).ToList();

    Console.WriteLine($"總抽卡數量: {Warps.Count}");
    Console.WriteLine("=== 抽卡紀錄 ===");
    foreach ( GachaLog log in Warps)
    {
        Console.WriteLine($"名稱: {log.Name}, {log.RankType}★ {log.ItemType}");
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
