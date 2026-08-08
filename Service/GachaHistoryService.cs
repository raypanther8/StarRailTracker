using StarRailTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace StarRailTracker.Service
{
    [JsonSerializable(typeof(GachaLog))]
    static internal class GachaHistoryService
    {
        static public async Task<List<GachaLog>> GetGachaHistory(string url)
        {
            try
            {
                List<Model.GachaLog> AllGachaLogs = [];
                HttpClient client = GlobalClient.Client;
                var dejsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // 忽略大小寫
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,// 允許將JSON string轉換成int/bool
                    TypeInfoResolver = AppJsonContext.Default
                };

                const int pageSize = 100;//每頁筆數
                                         //Fate卡池找不到資料尚未支援
                //抽卡類型：常駐、新手、限定角色、限定光錐、Fate角色、Fate光錐
                int[] gachaTypes = [1, 2, 11, 12, 21, 22];
                string endId;//結束id
                UriBuilder builder = new(url);
                var queryParams = HttpUtility.ParseQueryString(builder.Query);


                foreach (int gachaType in gachaTypes)
                {
                    bool hasMore = true;
                    Console.WriteLine($"正在獲取抽卡紀錄，抽卡類型：{gachaType}"); // Log the current gacha type being processed

                    queryParams.Set("gacha_type", gachaType.ToString());
                    queryParams.Set("size", pageSize.ToString());
                    queryParams.Set("end_id", "0");
                    builder.Query = queryParams.ToString();

                    while (hasMore)
                    {
                        await Task.Delay(200); // 延遲1秒，避免過於頻繁的請求
                        string requestUrl = builder.ToString();
                        string jsonResult = await client.GetStringAsync(requestUrl);
                        GachaResponse gachaResponse = System.Text.Json.JsonSerializer.Deserialize<GachaResponse>(jsonResult, dejsonOptions);//json decode

                        if (gachaResponse.Retcode != 0)//是否有更多資料
                        {
                            hasMore = false; // 沒有更多資料
                            continue;
                        }
                        else if (gachaResponse.Data == null || gachaResponse.Data.List.Count == 0)
                        {
                            hasMore = false; // 沒有更多資料
                            continue;
                        }
                        else
                        {
                            List<GachaLog> gachaLogs = gachaResponse.Data.List;
                            AllGachaLogs.AddRange(gachaLogs); // 將當前頁的抽卡紀錄加入總列表

                            endId = (BigInteger.Parse(gachaLogs.Last().Id) - 1).ToString(); // 更新結束id為最後一筆的id
                            queryParams.Set("end_id", endId);
                            builder.Query = queryParams.ToString();
                        }
                    }
                }

                AllGachaLogs =  GachaProcess.FinishingProcess(AllGachaLogs);
                return AllGachaLogs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"獲取抽卡紀錄時發生錯誤: {ex.Message}");
                return new List<GachaLog>();
            }
        }
        static public List<GachaLog> GetGachaHistoryFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                var dejsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // 忽略大小寫
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,// 允許將JSON string轉換成int/bool
                    TypeInfoResolver = AppJsonContext.Default
                };

                List<GachaLog> gachaLogs = System.Text.Json.JsonSerializer.Deserialize<List<GachaLog>>(jsonString, dejsonOptions);

                Console.WriteLine($"成功讀取！");
                return gachaLogs;
            }
            else
            {
                Console.WriteLine($"檔案不存在: {filePath}");
                Console.WriteLine("請確認檔案路徑是否正確(放在Warp History資料夾中)，或先使用程式抓取抽卡紀錄並輸出為JSON檔案。");
                return new List<GachaLog>();
            }
        }
        static public bool OutputJson(List<GachaLog> gachaLogs)
        {
            try
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true, // 美化輸出
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 避免中文被轉義
                    TypeInfoResolver = AppJsonContext.Default
                };

                List<GachaLog> gachas;//後面會用到的，每次UID重置
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string jsonString;
                List<int> uids = gachaLogs.Select(log => log.Uid).Distinct().ToList();

                foreach (var uid in uids)
                {
                    //filePath = @$"./Warp History/{uid}/{date}.json";
                    filePath = Path.Combine(baseDir, "Warp History", $"{uid}", $"{date}.json");
                    gachas = gachaLogs.Where(log => log.Uid == uid).ToList();
                    jsonString = System.Text.Json.JsonSerializer.Serialize(gachas, jsonOptions);
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    File.WriteAllText(filePath, jsonString); // 寫入檔案，發布的時候再加回來
                    Console.WriteLine($"已將UID {uid,-9} 的抽卡紀錄輸出為{date}.json");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"輸出抽卡紀錄為JSON檔案時發生錯誤: {ex.Message}");
                return false;
            }
        }
    }
}
