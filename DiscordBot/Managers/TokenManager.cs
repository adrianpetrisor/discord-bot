namespace Discord.Managers
{
    public class TokenManager
    {
        public string GetTokenValue()
        {
            DotNetEnv.Env.Load();
            var token = Environment.GetEnvironmentVariable("TOKEN");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: TOKEN environment variable is not set!");
                return string.Empty;
            }

            return token;
        }
    }
}