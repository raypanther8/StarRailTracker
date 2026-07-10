// See https://aka.ms/new-console-template for more information

using StarRailTracker.Model;
using StarRailTracker.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

List<GachaLog> gachaLogs = [];
List<GachaLog> tempGachaLogs = [];
List<StatisticDataList> statistics = [];

Console.WriteLine("=== 星鐵抽卡追蹤器 ===");

string userMode = DisplayService.ChooseFunction();

try
{
    while (userMode != "0")
    {
        switch (userMode)
        {
            case "1"://透過URL輸入紀錄
                Console.WriteLine("\n輸入URL: ");
                string url = Console.ReadLine();//讀取URL

                tempGachaLogs = await GachaHistoryService.GetGachaHistory(url);//取得抽卡紀錄
                Console.WriteLine($"\n總共取得 {tempGachaLogs.Count} 筆抽卡紀錄。");
                //存到當下程式中的內容
                gachaLogs.AddRange(tempGachaLogs);
                gachaLogs = GachaProcess.FinishingProcess(gachaLogs);

                //GachaHistoryService.OutputJson(gachaLogs); // 儲存結果到JSON檔案
                break;
            case "2"://讀取Json紀錄
                Console.WriteLine("暫不支援");
                break;
            case "3"://分析抽卡
                statistics = Statistics.GetTotalStatistics(gachaLogs);
                DisplayService.ChooseHistoryFuction(statistics);
                break;
            case "4"://彙整檔案為單一Json
                Console.WriteLine("暫不支援");
                break;
            default:
                Console.WriteLine("無效操作！");
                break;
        }
        Console.WriteLine("按任意鍵繼續...");
        Console.ReadKey();
        userMode = DisplayService.ChooseFunction();
    }
    /*
    Console.WriteLine("\n輸入URL: ");
    string url = Console.ReadLine();

    List<GachaLog> gachaLogs = await GachaHistoryService.GetGachaHistory(url);//取得抽卡紀錄
    Console.WriteLine($"\n總共取得 {gachaLogs.Count} 筆抽卡紀錄。");

    GachaHistoryService.OutputJson(gachaLogs); // 儲存結果到JSON檔案

    List<StatisticDataList> statistics = Statistics.GetTotalStatistics(gachaLogs);*/
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
