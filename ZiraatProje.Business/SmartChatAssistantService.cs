using System;
using System.Collections.Generic;
using System.Linq;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class ChatChannelSummary
    {
        public string ChannelName { get; set; } = string.Empty;
        public int TotalMessageCount { get; set; }
        public string SummaryText { get; set; } = string.Empty;
        public List<string> KeyTopics { get; set; } = new List<string>();
        public List<string> ActiveParticipants { get; set; } = new List<string>();
    }

    public class SmartChatAssistantService
    {
        public ChatChannelSummary SummarizeChannel(string channelName, IEnumerable<ChatMessage> messages)
        {
            var summary = new ChatChannelSummary
            {
                ChannelName = channelName
            };

            var msgList = messages?.ToList() ?? new List<ChatMessage>();
            summary.TotalMessageCount = msgList.Count;

            if (!msgList.Any())
            {
                summary.SummaryText = $"'{channelName}' kanalında henüz özetlenecek mesaj bulunmuyor.";
                return summary;
            }

            summary.ActiveParticipants = msgList.Select(m => m.SenderUser?.FullName ?? "Kullanıcı").Distinct().Take(5).ToList();

            var recentMessages = msgList.OrderByDescending(m => m.SentAt).Take(15).ToList();

            foreach (var msg in recentMessages)
            {
                if (!string.IsNullOrWhiteSpace(msg.MessageText))
                {
                    if (msg.MessageText.Contains("nöbet", StringComparison.OrdinalIgnoreCase) || msg.MessageText.Contains("shift", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!summary.KeyTopics.Contains("🌙 Nöbet Değişimi & Çizelge Konuları"))
                            summary.KeyTopics.Add("🌙 Nöbet Değişimi & Çizelge Konuları");
                    }
                    if (msg.MessageText.Contains("izin", StringComparison.OrdinalIgnoreCase) || msg.MessageText.Contains("tatil", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!summary.KeyTopics.Contains("🌴 İzin Talepleri & Planlamaları"))
                            summary.KeyTopics.Add("🌴 İzin Talepleri & Planlamaları");
                    }
                    if (msg.MessageText.Contains("proje", StringComparison.OrdinalIgnoreCase) || msg.MessageText.Contains("jira", StringComparison.OrdinalIgnoreCase) || msg.MessageText.Contains("task", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!summary.KeyTopics.Contains("🚀 Proje Teslimi & Jira Görevleri"))
                            summary.KeyTopics.Add("🚀 Proje Teslimi & Jira Görevleri");
                    }
                }
            }

            if (!summary.KeyTopics.Any())
            {
                summary.KeyTopics.Add("💬 Ekip İçi Günlük Çalışma İletişimi");
            }

            var participantsStr = string.Join(", ", summary.ActiveParticipants);
            summary.SummaryText = $"'{channelName}' kanalında son dönemde {summary.TotalMessageCount} mesaj paylaşıldı. Aktif katılımcılar: {participantsStr}. Öne çıkan konular: {string.Join(" • ", summary.KeyTopics)}.";

            return summary;
        }
    }
}
