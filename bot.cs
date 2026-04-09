using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        string url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
        
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
}
