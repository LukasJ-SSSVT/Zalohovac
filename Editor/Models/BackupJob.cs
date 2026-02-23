using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Editor.Models
{
    public class BackupJob
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sources")]
        public List<String> Sources { get; set; }

        [JsonPropertyName("targets")]
        public List<String> Targets { get; set; }

        [JsonPropertyName("timing")]
        public string Timing { get; set; }

        [JsonPropertyName("retention")]
        public BackupRetention BackupRetention { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("method")]
        public BackupType BackupType { get; set; }
    }
}
