using CoreTweet;
using System;
using System.Threading.Tasks;

namespace TwitterBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            var keys = new
            {
                ConsumerKey = "[you cosumer key]",
                ConsumerSecret = "[you consumer secret",
                AccessToken = "[access token to a bot account]",
                AccessSecret = "[access secret to a bot account]",
            };

            var tokens = Tokens.Create(keys.ConsumerKey, keys.ConsumerSecret, keys.AccessToken, keys.AccessSecret);
            var message = $@".NET Core からつぶやき
{string.Join(" ", args)}
{DateTime.Now}";
            await tokens.Statuses.UpdateAsync(new { status = message });
        }
    }
}
