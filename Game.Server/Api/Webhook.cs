using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Reflection;
using System.Threading;
using log4net;
using Newtonsoft.Json.Linq;
using Game.Server.GameObjects;

namespace Game.Server.Api
{
    internal class Webhook
    {
        private readonly ILog log;
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);

        private readonly string webhookUrl = "https://discord.com/api/webhooks/1449177833036451961/syGhIP68jlHCR2WXXTXfwsrgkI45eqFXYNwANpo93Jkdv-x1PqUCuzsBmRCVU2N_FNEr";
        private readonly string stateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "discord_chatlog_state.json");

        private const int MaxLines = 25;

        public Webhook()
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void SendGameChatMessage(GamePlayer player, string message)
        {
            if (player?.PlayerCharacter == null) return;
            if (string.IsNullOrEmpty(message)) return;
            _ = SendOrUpdateAsync(player, message);
        }

        public void SendGameChatMessage(GamePlayer player, string username, string message, int picId, string channelName)
        {
            if (player?.PlayerCharacter == null) return;
            if (string.IsNullOrEmpty(message)) return;
            _ = SendOrUpdateAsync(player, message);
        }

        private async global::System.Threading.Tasks.Task SendOrUpdateAsync(GamePlayer player, string message)
        {
            await gate.WaitAsync().ConfigureAwait(false);
            try
            {
                var pc = player.PlayerCharacter;

                var state = LoadState();
                var lines = state["lines"] as JArray ?? new JArray();

                lines.Add($"🎮 **{pc.NickName} (Lv.{pc.Grade}):** {message}");
                while (lines.Count > MaxLines) lines.RemoveAt(0);

                state["lines"] = lines;

                var description = string.Join("\n", lines.ToObject<string[]>());

                var embed = new JObject
                {
                    ["title"] = "Oyun Sohbeti",
                    ["description"] = description,
                    ["timestamp"] = DateTime.UtcNow.ToString("o")
                };

                var payload = new JObject
                {
                    ["username"] = "TRBombom",
                    ["avatar_url"] = "http://31.58.91.182/res/TRBombom.png",
                    ["embeds"] = new JArray(embed)
                };

                var msgId = (state["message_id"] ?? "").ToString();

                if (string.IsNullOrEmpty(msgId))
                {
                    var created = await CreateMessageAsync(payload).ConfigureAwait(false);
                    var newId = created?["id"]?.ToString();
                    if (!string.IsNullOrEmpty(newId))
                    {
                        state["message_id"] = newId;
                        SaveState(state);
                    }
                    return;
                }

                var ok = await PatchMessageAsync(msgId, payload).ConfigureAwait(false);
                if (!ok)
                {
                    var created = await CreateMessageAsync(payload).ConfigureAwait(false);
                    var newId = created?["id"]?.ToString();
                    if (!string.IsNullOrEmpty(newId))
                    {
                        state["message_id"] = newId;
                        SaveState(state);
                    }
                }
                else
                {
                    SaveState(state);
                }
            }
            catch (Exception ex)
            {
                log.Error("Discord chatlog error:", ex);
            }
            finally
            {
                gate.Release();
            }
        }

        private async global::System.Threading.Tasks.Task<JObject> CreateMessageAsync(JObject payload)
        {
            var url = webhookUrl.Contains("?") ? webhookUrl + "&wait=true" : webhookUrl + "?wait=true";
            var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

            var resp = await httpClient.PostAsync(url, content).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
            {
                log.Error($"Discord webhook create {resp.StatusCode}: {body}");
                return null;
            }

            try { return JObject.Parse(body); } catch { return null; }
        }

        private async global::System.Threading.Tasks.Task<bool> PatchMessageAsync(string messageId, JObject payload)
        {
            var url = $"{webhookUrl}/messages/{messageId}";
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            req.Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

            var resp = await httpClient.SendAsync(req).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
            {
                log.Error($"Discord webhook patch {resp.StatusCode}: {body}");
                return false;
            }

            return true;
        }

        private JObject LoadState()
        {
            try
            {
                if (File.Exists(stateFile))
                {
                    var txt = File.ReadAllText(stateFile, Encoding.UTF8);
                    if (!string.IsNullOrWhiteSpace(txt))
                        return JObject.Parse(txt);
                }
            }
            catch { }

            return new JObject
            {
                ["message_id"] = "",
                ["lines"] = new JArray()
            };
        }

        private void SaveState(JObject state)
        {
            try { File.WriteAllText(stateFile, state.ToString(), Encoding.UTF8); } catch { }
        }
    }
}
