using System;
using Newtonsoft.Json;
using SQLite;

namespace HeiaMeg.Models
{
    public class DebugInfo
    {
        [JsonProperty, PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [JsonProperty]
        public DateTime Date { get; set; }
        [JsonProperty]
        public string Status { get; set; }
        [JsonProperty]
        public string Result { get; set; }

        public DebugInfo()
        {
        }

        public DebugInfo(string status, string result) : this()
        {
            Date = DateTime.Now;
            Status = status;
            Result = result;
        }
    }
}
