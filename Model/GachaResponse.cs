using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarRailTracker.Model
{
    public class GachaLog
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("gacha_id")]
        public int GachaId { get; set; }

        [JsonPropertyName("gacha_type")]
        public int GachaType { get; set; }

        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; }

        [JsonPropertyName("item_type")]
        public string ItemType { get; set; }

        [JsonPropertyName("rank_type")]
        public int RankType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } // 改用 string 完美避開 BigInteger 限制
    }

    public class Data
    {
        [JsonPropertyName("list")]
        public List<GachaLog> List { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }

    public class GachaResponse
    {
        [JsonPropertyName("retcode")]
        public int Retcode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
}
