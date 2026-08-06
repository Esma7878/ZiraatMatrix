using System;
using System.Collections.Generic;
using Xunit;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Tests
{
    public class SmartChatAssistantTests
    {
        private readonly SmartChatAssistantService _service;

        public SmartChatAssistantTests()
        {
            _service = new SmartChatAssistantService();
        }

        [Fact]
        public void SummarizeChannel_ShouldExtractKeyTopicsFromMessageText()
        {
            // Arrange
            var messages = new List<ChatMessage>
            {
                new ChatMessage { MessageText = "Gelecek hafta nöbet çizelgesinde değişiklik var mı?", SentAt = DateTime.Now.AddMinutes(-10) },
                new ChatMessage { MessageText = "Jira görevi tamamlandı, proje canlıya alınmaya hazır.", SentAt = DateTime.Now.AddMinutes(-5) },
                new ChatMessage { MessageText = "Ağustos izin talepleri sisteme girildi.", SentAt = DateTime.Now }
            };

            // Act
            var summary = _service.SummarizeChannel("Genel Kanal", messages);

            // Assert
            Assert.Equal("Genel Kanal", summary.ChannelName);
            Assert.Equal(3, summary.TotalMessageCount);
            Assert.Contains("🌙 Nöbet Değişimi & Çizelge Konuları", summary.KeyTopics);
            Assert.Contains("🌴 İzin Talepleri & Planlamaları", summary.KeyTopics);
            Assert.Contains("🚀 Proje Teslimi & Jira Görevleri", summary.KeyTopics);
        }

        [Fact]
        public void SummarizeChannel_ShouldHandleEmptyMessagesList()
        {
            // Act
            var summary = _service.SummarizeChannel("Boş Kanal", new List<ChatMessage>());

            // Assert
            Assert.Equal(0, summary.TotalMessageCount);
            Assert.Contains("henüz özetlenecek mesaj bulunmuyor", summary.SummaryText);
        }
    }
}
