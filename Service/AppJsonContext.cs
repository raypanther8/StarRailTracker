using System.Text.Json.Serialization;

using StarRailTracker.Model;

namespace StarRailTracker.Service
{
    //為了解決修剪程式碼產生的問題，註冊所有Jsonalize或Deserialize的類別，避免被修剪掉
    // 在這裡註冊所有你需要轉成 JSON 的類別 (包含 List 結構)
    [JsonSerializable(typeof(GachaLog))]
    [JsonSerializable(typeof(System.Collections.Generic.List<GachaLog>))]
    [JsonSerializable(typeof(GachaResponse))]
    internal partial class AppJsonContext : JsonSerializerContext
    {
    }
}