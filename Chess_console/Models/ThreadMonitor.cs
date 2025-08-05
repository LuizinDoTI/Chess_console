using System.Diagnostics;

namespace ChessConsole.Models;

/// <summary>
/// Coleta informações de performance sobre threads, memória e CPU.
/// </summary>
public static class ThreadMonitor
{
    public class ThreadInfo
    {
        public int TotalThreads { get; set; }
        public long MemoryUsageMB { get; set; }
        public TimeSpan CpuTime { get; set; }
    }

    public static ThreadInfo GetThreadInfo()
    {
        using (var processo = Process.GetCurrentProcess())
        {
            return new ThreadInfo
            {
                TotalThreads = processo.Threads.Count,
                MemoryUsageMB = processo.WorkingSet64 / 1024 / 1024,
                CpuTime = processo.TotalProcessorTime
            };
        }
    }
}