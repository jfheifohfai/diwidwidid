using System;
using System.Net.Http;
using System.Text;
using System.Management;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        try {
            string webhook = args[0];
            var client = new HttpClient();

            string ip = client.GetStringAsync("https://ifconfig.me/ip").GetAwaiter().GetResult().Trim();
            string pcName = Environment.MachineName;
            
            string cpu = "";
            using (var s = new ManagementObjectSearcher("select Name from Win32_Processor"))
                foreach (var obj in s.Get()) cpu = obj["Name"].ToString();

            var gpus = new List<string>();
            using (var s = new ManagementObjectSearcher("select Name from Win32_VideoController"))
                foreach (var obj in s.Get()) gpus.Add(obj["Name"].ToString());
            string gpuList = string.Join(", ", gpus);

            long mem = 0;
            using (var s = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory"))
                foreach (var obj in s.Get()) mem += Convert.ToInt64(obj["Capacity"]);
            int ram = (int)(mem / 1024 / 1024 / 1024);

            var drive = new System.IO.DriveInfo("C");
            long disk = drive.AvailableFreeSpace / 1024 / 1024 / 1024;

            string report = "**REPORT: " + pcName + "**\\n---\\n**IP:** " + ip + "\\n**CPU:** " + cpu + "\\n**GPU:** " + gpuList + "\\n**RAM:** " + ram + "GB\\n**Disk C:** " + disk + "GB volných";
            string json = "{\"content\":\"" + report + "\"}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.PostAsync(webhook, content).GetAwaiter().GetResult();
        }
        catch { }
    }
}
