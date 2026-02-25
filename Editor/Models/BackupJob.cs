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

        [JsonPropertyName("name")]
        public string Name { get; set; } = "Nová záloha";

        [JsonPropertyName("sources")]
        public List<string> Sources { get; set; } = new List<string>();

        [JsonPropertyName("targets")]
        public List<string> Targets { get; set; } = new List<string>();

        [JsonPropertyName("timing")]
        public string Timing { get; set; } = "* * * * *";

        [JsonPropertyName("retention")] 
        public BackupRetention Retention { get; set; } = new BackupRetention() { Count = 1, Size = 1};

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("method")]
        public BackupType Method { get; set; } = BackupType.Full;


        public BackupJob Clone()
        {
            return new BackupJob
            {
                Id = this.Id,
                Name = this.Name,
                Sources = new List<string>(this.Sources),
                Targets = new List<string>(this.Targets),
                Timing = this.Timing,
                Method = this.Method,
                Retention = new BackupRetention
                {
                    Count = this.Retention.Count,
                    Size = this.Retention.Size
                }
            };
        }

    }
}
