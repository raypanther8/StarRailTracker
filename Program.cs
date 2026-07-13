// See https://aka.ms/new-console-template for more information

using StarRailTracker.Model;
using StarRailTracker.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

List<GachaLog> gachaLogs = [];
List<GachaLog> tempGachaLogs = [];
List<StatisticDataList> statistics = [];

int formerGachaLogsCount;
int tempUid;

Console.WriteLine("=== 星鐵抽卡追蹤器 ===");

string userMode = DisplayService.ChooseFunction();

try
{
    while (userMode != "0")
    {
        switch (userMode)
        {
            case "1"://透過URL輸入紀錄
                formerGachaLogsCount = gachaLogs.Count;
                Console.WriteLine("\n輸入URL: ");
                string url = Console.ReadLine();//讀取URL

                tempGachaLogs = await GachaHistoryService.GetGachaHistory(url);//取得抽卡紀錄
                Console.WriteLine($"\n總共取得 {tempGachaLogs.Count} 筆抽卡紀錄。");

                //存到當下程式中的內容(暫時不加，但不要刪)
                gachaLogs.AddRange(tempGachaLogs);
                gachaLogs = GachaProcess.FinishingProcess(gachaLogs);
                Console.WriteLine($"總共新增 {gachaLogs.Count - formerGachaLogsCount} 筆抽卡紀錄。");

                GachaHistoryService.OutputJson(tempGachaLogs); // 儲存此次取得的結果到JSON檔案
                break;

            case "2"://讀取Json紀錄
                formerGachaLogsCount = gachaLogs.Count;

                //輸入紀錄
                tempGachaLogs = DisplayService.InputFromJson();
                gachaLogs.AddRange(tempGachaLogs);
                gachaLogs = GachaProcess.FinishingProcess(gachaLogs);

                Console.WriteLine($"新增了 {gachaLogs.Count - formerGachaLogsCount} 筆記錄");
                break;

            case "3"://分析抽卡

                Console.WriteLine($"請輸入欲查詢的帳號UID");//確定單一UID
                if (!int.TryParse(Console.ReadLine(), out tempUid))
                {
                    tempUid = -1;
                }
                tempGachaLogs = gachaLogs.Where(log => log.Uid == tempUid).ToList();

                statistics = Statistics.GetTotalStatistics(gachaLogs);//轉換為統計結果
                DisplayService.ChooseHistoryFuction(statistics);
                break;

            case "4"://彙整檔案為單一Json
                Console.WriteLine($"將輸出 {gachaLogs.Count} 筆結果");
                GachaHistoryService.OutputJson(gachaLogs); // 儲存結果到JSON檔案
                Console.WriteLine("已輸出至Warp History資料夾中");
                break;

            default:
                Console.WriteLine("無效操作！");
                break;
        }
        Console.WriteLine("\n按任意鍵繼續...");
        Console.ReadKey();
        Console.Clear();
        userMode = DisplayService.ChooseFunction();
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
