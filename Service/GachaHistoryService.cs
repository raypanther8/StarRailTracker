using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarRailTracker.Service
{
    static internal class GachaHistoryService
    {
        static public async Task<List<Model.GachaLog>> GetGachaHistory(string url)
        {
            HttpClient client = GlobalClient.Client;
            var dejson_options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // 忽略大小寫
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString// 允許將JSON string轉換成int/bool
            };

            const int pageSize = 100;//每頁筆數
            //Fate卡池找不到資料尚未支援
            int[] gachaTypes = [1, 2, 11, 12];//抽卡類型：常駐、新手、限定角色、限定光錐
            int end_id = 0;//結束id

            string json_result = await client.GetStringAsync(url);
            Model.GachaResponse gachaResponse = System.Text.Json.JsonSerializer.Deserialize<Model.GachaResponse>(json_result, dejson_options);//json decode
            List<Model.GachaLog> gachaLogs = gachaResponse.Data.List;//取得抽卡紀錄
            return gachaLogs;
        }
    }
}
