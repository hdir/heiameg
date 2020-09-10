using System;
using Newtonsoft.Json;

namespace HeiaMeg.Models
{
    public class ServerMeta : IServerMeta
    {
        [JsonProperty] public int VersionCode { get; private set; }
        [JsonProperty] public string VersionName { get; private set; }
        [JsonProperty] public string CronSchedule { get; private set; }
        [JsonProperty] public DateTime LastUpdate { get; private set; }
        [JsonProperty] public DateTime ThemesModified { get; private set; }
        [JsonProperty] public DateTime MessagesModified { get; private set; }
        [JsonProperty] public DateTime ContentModified { get; private set; }
        [JsonProperty] public int MinimumModifiedMinutesThreshold { get; private set; }
        [JsonProperty] public int UpdateFrequency { get; private set; }
    }
}