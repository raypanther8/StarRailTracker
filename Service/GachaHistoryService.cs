using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using StarRailTracker.Model;

namespace StarRailTracker.Service
{
    static internal class GachaHistoryService
    {
        static public async Task<List<Model.GachaLog>> GetGachaHistory(string url)
        {
            List<Model.GachaLog> AllGachaLogs = [];
            HttpClient client = GlobalClient.Client;
            var dejsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // 忽略大小寫
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString// 允許將JSON string轉換成int/bool
            };

            const int pageSize = 100;//每頁筆數
            //Fate卡池找不到資料尚未支援
            int[] gachaTypes = [1, 2, 11, 12];//抽卡類型：常駐、新手、限定角色、限定光錐
            string endId = "0";//結束id
            UriBuilder builder = new(url);
            var queryParams = HttpUtility.ParseQueryString(builder.Query);


            foreach (int gachaType in gachaTypes)
            {
                bool hasMore = true;
                Console.WriteLine($"正在獲取抽卡紀錄，抽卡類型：{gachaType} {gachaType.ToString()}"); // Log the current gacha type being processed
                queryParams.Set("gacha_type", gachaType.ToString());
                queryParams.Set("size", pageSize.ToString());
                queryParams.Set("end_id", "0");
                builder.Query = queryParams.ToString();

                while (hasMore)
                {
                    await Task.Delay(200); // 延遲1秒，避免過於頻繁的請求
                    string requestUrl = builder.ToString();
                    string jsonResult = await client.GetStringAsync(requestUrl);
                    Model.GachaResponse gachaResponse = System.Text.Json.JsonSerializer.Deserialize<Model.GachaResponse>(jsonResult, dejsonOptions);//json decode

                    if (gachaResponse.Retcode != 0)//是否有更多資料
                    {
                        Console.WriteLine($"獲取失敗，請檢察URL是否有問題"); // Log the error message
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


            /*-----------------
            string json_result = await client.GetStringAsync(url);
            Model.GachaResponse gachaResponse = System.Text.Json.JsonSerializer.Deserialize<Model.GachaResponse>(json_result, dejson_options);//json decode
            List<Model.GachaLog> gachaLogs = gachaResponse.Data.List;//取得抽卡紀錄
            return gachaLogs;*/
            AllGachaLogs =  GachaProcess.FinishingProcess(AllGachaLogs);
            return AllGachaLogs;
        }
    }
}
