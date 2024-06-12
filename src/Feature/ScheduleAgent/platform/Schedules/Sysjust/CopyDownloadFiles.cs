using Sitecore.Configuration;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class CopyDownloadFiles : SitecronAgentBase
    {
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                string downloadUrl = Settings.GetSetting("downloadUrl");
                string baseTargetFolder = Settings.GetSetting("targetFolder");

                string nineFolder = Path.Combine(baseTargetFolder, "999");

                if (Directory.Exists(nineFolder))
                {
                    Directory.Delete(nineFolder, true);
                }

                Directory.CreateDirectory(nineFolder);

                using (var client = new HttpClient())
                {
                    try
                    {
                        using (var stream = client.GetStreamAsync(downloadUrl).Result)
                        {
                            string fileName = Path.GetFileName(downloadUrl);
                            string time = DateTime.Now.ToString("yyyyMMdd");
                            string targetFilePath = Path.Combine(baseTargetFolder, $"{Path.GetFileNameWithoutExtension(fileName)}_{time}{Path.GetExtension(fileName)}");

                            using (var fileStream = File.Create(targetFilePath))
                            {
                                stream.CopyTo(fileStream);
                            }

                            ZipFile.ExtractToDirectory(targetFilePath, nineFolder);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            });
        }
    }
}