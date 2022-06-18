using SmartFlow.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Services
{
    public class MigrationService : IMigrationService
    {
        public string BackupDatabase()
        {
            try
            {
                string batchFilePath = Resource.MigrationPath + Resource.BackupScript;
                ProcessStartInfo pInfo = new ProcessStartInfo(batchFilePath);
                pInfo.CreateNoWindow = true;
                pInfo.UseShellExecute = false;
                Process.Start(pInfo);
                string backup = File.ReadAllText(Resource.BackupFile);
                return backup;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool RestoreDatabase(string restoreText)
        {
            try
            {
                File.WriteAllText(Resource.RestoreFile, restoreText);
                string batchFilePath = Resource.MigrationPath + Resource.RestoreScript;
                ProcessStartInfo pInfo = new ProcessStartInfo(batchFilePath);
                pInfo.CreateNoWindow = true;
                pInfo.UseShellExecute = false;
                Process.Start(pInfo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
