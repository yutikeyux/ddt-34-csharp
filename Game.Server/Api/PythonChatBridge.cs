using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Game.Server.Api
{
    internal static class PythonChatBridge
    {
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(2)
        };

        private static readonly string BOT_URL = "http://31.11.64.28:9600/game/chat";
        private static readonly string SECRET = "supersecret";

        public static void Send(string username, int level, string message, int picId = 0, string avatarUrl = null)
        {
            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    var payload = new
                    {
                        username,
                        level,
                        message,
                        picId = picId == 0 ? (int?)null : picId,
                        avatarUrl
                    };

                    var req = new HttpRequestMessage(HttpMethod.Post, BOT_URL);
                    req.Headers.Add("X-Game-Secret", SECRET);
                    req.Content = new StringContent(
                        JsonConvert.SerializeObject(payload),
                        Encoding.UTF8,
                        "application/json"
                    );

                    await _http.SendAsync(req).ConfigureAwait(false);
                }
                catch { }
            });

        }
    }
}
