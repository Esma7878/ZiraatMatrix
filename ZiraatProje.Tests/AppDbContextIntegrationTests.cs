using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Tests
{
    public class AppDbContextIntegrationTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void CanAddAndRetrieveProjectWithAllocations()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var user = new User
            {
                Name = "Zeynep",
                Surname = "Kaya",
                Title = "Analist",
                Team = "Finansman",
                Email = "zeynep@ziraat.com"
            };
            context.Users.Add(user);
            context.SaveChanges();

            var project = new Project
            {
                ProjectName = "Kredi Değerlendirme Modülü",
                ProjectStatus = "Devam Ediyor",
                ProjectType = "Proje",
                Team = "Finansman",
                TotalManDayBudget = 100m,
                Year = 2026,
                Quarter = 3
            };
            context.Projects.Add(project);
            context.SaveChanges();

            var allocation = new ProjectAllocation
            {
                ProjectId = project.Id,
                UserId = user.Id,
                AllocatedManDay = 25m,
                ActualManDay = 10m
            };
            context.ProjectAllocations.Add(allocation);
            context.SaveChanges();

            // Act
            var savedProject = context.Projects
                .Include(p => p.ProjectAllocations)
                .ThenInclude(pa => pa.User)
                .FirstOrDefault(p => p.Id == project.Id);

            // Assert
            Assert.NotNull(savedProject);
            Assert.Equal("Kredi Değerlendirme Modülü", savedProject.ProjectName);
            Assert.Single(savedProject.ProjectAllocations);
            Assert.Equal(25m, savedProject.ProjectAllocations.First().AllocatedManDay);
            Assert.Equal("Zeynep Kaya", savedProject.ProjectAllocations.First().User?.FullName);
        }

        [Fact]
        public void CanAddAndRetrieveUserWithShiftsAndLeaves()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var user = new User
            {
                Name = "Emre",
                Surname = "Şahin",
                Title = "Yazılımcı",
                Team = "Takip",
                Email = "emre@ziraat.com"
            };
            context.Users.Add(user);
            context.SaveChanges();

            var shiftType = new ShiftType { ShiftName = "Firewall Geçiş Nöbeti" };
            context.ShiftTypes.Add(shiftType);
            context.SaveChanges();

            var shift = new Shift
            {
                UserId = user.Id,
                ShiftTypeId = shiftType.Id,
                ShiftDate = DateTime.Today
            };
            context.Shifts.Add(shift);

            var leave = new Leave
            {
                UserId = user.Id,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(15),
                Status = "Approved"
            };
            context.Leaves.Add(leave);

            context.SaveChanges();

            // Act
            var savedUser = context.Users
                .Include(u => u.Shifts)
                .ThenInclude(s => s.ShiftType)
                .Include(u => u.Leaves)
                .FirstOrDefault(u => u.Id == user.Id);

            // Assert
            Assert.NotNull(savedUser);
            Assert.Single(savedUser.Shifts);
            Assert.Equal("Firewall Geçiş Nöbeti", savedUser.Shifts.First().ShiftType?.ShiftName);
            Assert.Single(savedUser.Leaves);
            Assert.Equal("Approved", savedUser.Leaves.First().Status);
        }

        [Fact]
        public void CanAddAndRetrieveChatMessagesWithReadStates()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var sender = new User { Name = "Merve", Surname = "Güneş", Email = "merve@ziraat.com", Team = "Tahsis", Title = "Yönetici" };
            var receiver = new User { Name = "Oğuz", Surname = "Yıldız", Email = "oguz@ziraat.com", Team = "Tahsis", Title = "Dev" };
            context.Users.AddRange(sender, receiver);
            context.SaveChanges();

            var msg = new ChatMessage
            {
                SenderUserId = sender.Id,
                ReceiverUserId = receiver.Id,
                TargetType = "Direct",
                MessageText = "Yarınki canlı sürüm nöbeti hazır mı?",
                SentAt = DateTime.Now,
                IsRead = false
            };
            context.ChatMessages.Add(msg);

            var readState = new ChatMessageReadState
            {
                UserId = receiver.Id,
                ChannelKey = $"Direct_{sender.Id}",
                LastReadMessageId = msg.Id,
                LastReadAt = DateTime.Now
            };
            context.ChatMessageReadStates.Add(readState);

            context.SaveChanges();

            // Act
            var savedMsg = context.ChatMessages
                .Include(m => m.SenderUser)
                .Include(m => m.ReceiverUser)
                .FirstOrDefault();

            var savedReadState = context.ChatMessageReadStates.FirstOrDefault(r => r.UserId == receiver.Id);

            // Assert
            Assert.NotNull(savedMsg);
            Assert.Equal("Merve Güneş", savedMsg.SenderUser?.FullName);
            Assert.Equal("Oğuz Yıldız", savedMsg.ReceiverUser?.FullName);
            Assert.NotNull(savedReadState);
            Assert.Equal($"Direct_{sender.Id}", savedReadState.ChannelKey);
        }

        [Fact]
        public void CanAddAndQueryMonthlyReleaseShiftsAndCustomShifts()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            var release = new MonthlyReleaseShift
            {
                MonthName = "Ağustos 2026",
                ReleaseDate = new DateTime(2026, 8, 20),
                AssignedUsers = "Ahmet Yılmaz, Mehmet Demir",
                JiraTicketNo = "ZIR-1045",
                IsFinished = false
            };
            context.MonthlyReleaseShifts.Add(release);

            var custom = new CustomShift
            {
                Topic = "Sunucu Güvenlik Yaması",
                ShiftDate = new DateTime(2026, 8, 22),
                AssignedUsers = "Ali Öztürk",
                IsFinished = false
            };
            context.CustomShifts.Add(custom);

            context.SaveChanges();

            // Act
            var savedRelease = context.MonthlyReleaseShifts.FirstOrDefault(r => r.JiraTicketNo == "ZIR-1045");
            var savedCustom = context.CustomShifts.FirstOrDefault(c => c.Topic.Contains("Güvenlik"));

            // Assert
            Assert.NotNull(savedRelease);
            Assert.Contains("Ahmet Yılmaz", savedRelease.AssignedUsers);
            Assert.NotNull(savedCustom);
            Assert.Equal("Ali Öztürk", savedCustom.AssignedUsers);
        }
    }
}
