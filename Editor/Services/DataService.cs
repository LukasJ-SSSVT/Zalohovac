using Editor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //public void WirteJobs(List<BackupJob> backupJobs)
        //{
        //    using (StreamWriter writer = new StreamWriter("Config.json"))
        //    {
        //        writer.Write("");

        //        foreach (BackupJob backupJob in backupJobs)
        //        {
        //            writer.Write((JsonSerializer.SerializeToDocument(backupJob));
        //        }
        //    }
        //}
    }
}
