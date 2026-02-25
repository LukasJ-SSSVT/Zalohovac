using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public string Name {  get; set; } = "New backup";

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("method")]
        public BackupType Method { get; set; } = BackupType.Full;

        [JsonPropertyName("timing")]
        public string Timing { get; set; } = "* * * * *";

        [JsonPropertyName("retention")]
        public BackupRetention Retention { get; set; } = new BackupRetention() { Count = 1, Size = 1 };

        [JsonPropertyName("sources")]
        public List<string> Sources { get; set; } = new List<string>();

        [JsonPropertyName("targets")]
        public List<string> Targets { get; set; } = new List<string>();

        public List<string> GetPropertyNames()
        {
            List<PropertyInfo> properties = this.GetType().GetProperties().ToList();
            List<string> result = new List<string>();

            for (int i = 1; i < properties.Count; i++)
            {
                result.Add(properties[i].Name);
            }

            return result;
        }

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
