using Discord.Logger;
using Discord.Logging;
using Discord.Managers;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Discord 
{
    public class Program 
    {
        public static async Task Main(string[] args) {
            var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();
            
            try {
                var fileManager = new FileManager.FileManager();
                await fileManager.InitializeFileManager();
                await ConsoleLogger.LogAsync("FileManager", "FileManager initialized successfully!", ConsoleLogger.LogSeverityLevels.LOADING);
            } catch (Exception ex) {
                await ConsoleLogger.LogAsync("Program", $"Unhandled exception: {ex.Message}", ConsoleLogger.LogSeverityLevels.FATAL);
            }
            
            await MemoryUsageTracker.StartMemoryTrackingAsync();
            await MainAsync(client, services);
        }
        private static async Task MainAsync(DiscordSocketClient client, IServiceProvider services) {
            var tokenManager = new TokenManager();
            var getTokenConfiguration = tokenManager.GetTokenValue();

            var discordConfiguration = new DiscordSocketConfig {
                MessageCacheSize = 100,
                LogLevel = LogSeverity.Debug
            };

            client.Log += LogDiscordEvent;
            await client.LoginAsync(TokenType.Bot, getTokenConfiguration);
            await client.StartAsync();

            client.Ready += async () => 
            {
                await ConsoleLogger.LogAsync("Discord", "System is now operational!", ConsoleLogger.LogSeverityLevels.OK);
            };
            
            await Task.Delay(-1);
        }
        private static IServiceProvider ConfigureServices() {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig 
                {
                    GatewayIntents = GatewayIntents.Guilds,
                    LogLevel = LogSeverity.Debug,          
                    AlwaysDownloadUsers = true             
                }))
                .BuildServiceProvider();
        }
        private static Task LogDiscordEvent(LogMessage message) {
            return ConsoleLogger.LogAsync("Discord", message.ToString(), ConsoleLogger.LogSeverityLevels.INFO);
        }
    }
}