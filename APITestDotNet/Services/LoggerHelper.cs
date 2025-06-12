using System;
using System.IO;

public static class LoggerHelper
{
    public static void LogError(string source, Exception ex)
    {
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
        Directory.CreateDirectory(logDirectory); 

        string fileName = $"log_{DateTime.Now:yyyyMMdd}.txt";
        string fullPath = Path.Combine(logDirectory, fileName);

        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}\n{ex}\n";

        File.AppendAllText(fullPath, logEntry);
    }
}