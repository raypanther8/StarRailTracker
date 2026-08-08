using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StarRailTracker.Service
{
    internal class WarpUrlExtractor
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 自動尋找並獲取所有有效的星穹鐵道躍遷歷史紀錄網址
        /// </summary>
        /// <returns>包含所有有效網址的 List<string></returns>
        public static async Task<List<string>> GetWarpUrls()
        {
            Console.WriteLine("正在自動尋找有效的躍遷歷史紀錄網址，請稍候...");
            List<string> validUrls = new();

            // 1. 取得 Player.log 路徑
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string locallowPath = Path.Combine(appData, "..", "LocalLow", "Cognosphere", "Star Rail");
            string logPath = Path.Combine(locallowPath, "Player.log");

            if (!File.Exists(logPath))
            {
                // 如果不存在，嘗試找前一次的 log
                logPath = Path.Combine(locallowPath, "Player-prev.log");
                if (!File.Exists(logPath)) return validUrls;
            }

            // 2. 解析遊戲安裝路徑
            string gamePath = "";
            try
            {
                var lines = File.ReadLines(logPath).Take(11);
                foreach (var line in lines)
                {
                    if (line.StartsWith("Loading player data from "))
                    {
                        gamePath = line.Replace("Loading player data from ", "")
                                       .Replace("data.unity3d", "")
                                       .Trim();
                        break;
                    }
                }
            }
            catch
            {
                return validUrls; // 讀取紀錄失敗
            }

            if (string.IsNullOrEmpty(gamePath)) return validUrls;

            // 3. 尋找最新的快取資料夾
            string webCachesPath = Path.Combine(gamePath, "webCaches");
            if (!Directory.Exists(webCachesPath)) return validUrls;

            string newestCacheFolder = Directory.GetDirectories(webCachesPath)
                .Select(d => new DirectoryInfo(d))
                .OrderByDescending(d => d.Name, StringComparer.OrdinalIgnoreCase) // 依版本號排序
                .FirstOrDefault()?.FullName;

            if (string.IsNullOrEmpty(newestCacheFolder)) return validUrls;

            string cacheDataPath = Path.Combine(newestCacheFolder, "Cache", "Cache_Data", "data_2");
            if (!File.Exists(cacheDataPath)) return validUrls;

            // 4. 讀取快取檔案並解析網址
            try
            {
                // 將檔案複製到暫存區，避免檔案被遊戲佔用時無法讀取
                string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                File.Copy(cacheDataPath, tempFilePath, true);

                // 讀取成 UTF-8 字串
                string cacheContent = File.ReadAllText(tempFilePath, Encoding.UTF8);

                // 安全刪除暫存檔
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);

                // 利用原腳本中的 '1/0/' 作為切分點
                string[] rawSegments = cacheContent.Split(new[] { "1/0/" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var segment in rawSegments)
                {
                    // 檢查是否為 http 開頭且包含 gacha 關鍵字
                    if (segment.StartsWith("http") && (segment.Contains("getGachaLog") || segment.Contains("getLdGachaLog")))
                    {
                        // 擷取到第一個 \0 (Null 終止符) 之前的網址字串
                        string url = segment.Split('\0')[0];

                        // 5. 驗證網址是否有效
                        if (await IsValidWarpUrlAsync(url))
                        {
                            // 避免重複加入相同的網址
                            if (!validUrls.Contains(url))
                            {
                                validUrls.Add(url);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 處理檔案讀取或解析期間的異常
            }

            Console.WriteLine($"共找到 {validUrls.Count} 個有效的躍遷歷史紀錄網址。");
            return validUrls;
        }

        /// <summary>
        /// 透過 API 請求驗證躍遷網址是否有效
        /// </summary>
        private static async Task<bool> IsValidWarpUrlAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(jsonString);
                    // 檢查 JSON 帶回來的 retcode 是否為 0 (代表成功)
                    if (doc.RootElement.TryGetProperty("retcode", out JsonElement retcodeElement))
                    {
                        return retcodeElement.GetInt32() == 0;
                    }
                }
            }
            catch
            {
                // 網路請求失敗或 JSON 解析失敗
            }
            return false;
        }
    }
}
