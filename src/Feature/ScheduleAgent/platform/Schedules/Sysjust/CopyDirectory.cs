using Sitecore.Configuration;
using Sitecore.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class CopyDirectory : SitecronAgentBase
    {
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                string baseTargetFolder = Settings.GetSetting("targetFolder");
                string endDestFolder = Settings.GetSetting("destFolder");
                string sourceFolder = Path.Combine(baseTargetFolder, "999");

                string newFolderName = DateTime.Now.ToString("yyyyMMdd");
                string newFolder = Path.Combine(endDestFolder, newFolderName);
                string newnineFolder = Path.Combine(endDestFolder, "999");

                try
                {
                    if (!Directory.Exists(newFolder))
                    {
                        Directory.CreateDirectory(newFolder);
                    }

                    if (Directory.Exists(newnineFolder))
                    {
                        Directory.Delete(newnineFolder, true);
                    }

                    CopyDirectory(sourceFolder, newnineFolder);
                    CopyDirectory(sourceFolder, newFolder);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error: {ex.Message}", this);
                }
            });
        }

        private void CopyDirectory(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (string file in Directory.GetFiles(source))
            {
                string destFile = Path.Combine(destination, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (string subdirectory in Directory.GetDirectories(source))
            {
                string destSubdirectory = Path.Combine(destination, Path.GetFileName(subdirectory));
                CopyDirectory(subdirectory, destSubdirectory);
            }
        }
    }
}