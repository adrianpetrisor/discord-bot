using System.Diagnostics;
using Discord.Logger;

namespace Discord;

public class MemoryUsageTracker {
    public static double GetMemoryUsageInMb() {
        using var process = Process.GetCurrentProcess();
        return process.WorkingSet64 / 1024.0 / 1024.0;
    }

    public static async Task StartMemoryTrackingAsync(CancellationToken cancellationToken = default) {
        const string systemName = "MemoryUsageTracker";
        
        await ConsoleLogger.LogAsync(systemName, "Memory Usage Tracking started... (Updates every 2 minutes)", ConsoleLogger.LogSeverityLevels.WARNING);

        while (!cancellationToken.IsCancellationRequested)
        {
            double memoryUsed = GetMemoryUsageInMb();
            await ConsoleLogger.LogAsync(systemName, $"Memory Used: {memoryUsed:F2} MB", ConsoleLogger.LogSeverityLevels.INFO);

            try {
                await Task.Delay(12000, cancellationToken);
            } catch (TaskCanceledException) {
                break;
            }
        }
        await ConsoleLogger.LogAsync(systemName, "Memory Usage Tracking stopped.", ConsoleLogger.LogSeverityLevels.WARNING);
    }
}