using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Editor.Models
{
    public enum BackupType
    {
        [JsonPropertyName("full")]
        Full,
        [JsonPropertyName("differential")]
        Differential,
        [JsonPropertyName("incremental")]
        Incremental
    }
}
