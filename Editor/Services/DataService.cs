using Editor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Editor.Services
{
    public class DataService
    {
        public List<BackupJob> GetAllBackupJobs()
        {
            string json = File.ReadAllText("Config.json");

            List<BackupJob> jobs = JsonSerializer.Deserialize<List<BackupJob>>(json);

            return jobs;
        }

        public void WirteJobs(List<BackupJob> backupJobs)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true , Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string output = JsonSerializer.Serialize(backupJobs, options);

            File.WriteAllText("Config.json", output);
        }
    }
}
