using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class BusinessServices
    {
        // Helper to get DbContext
        private AppDbContext CreateContext()
        {
            return new AppDbContext();
        }

        public User? Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            using var context = CreateContext();
            return context.Users
                .FirstOrDefault(u => u.Email.ToLower().Trim() == email.ToLower().Trim() && u.Password == password);
        }

        #region Team Operations
        public List<Team> GetAllTeams()
        {
            using var context = CreateContext();
            return context.Teams.AsNoTracking().OrderBy(t => t.TeamName).ToList();
        }

        public void AddTeam(Team team)
        {
            using var context = CreateContext();
            if (string.IsNullOrWhiteSpace(team.TeamName))
                throw new ValidationException("Ekip adı boş bırakılamaz.");

            if (context.Teams.Any(t => t.TeamName.ToLower().Trim() == team.TeamName.ToLower().Trim()))
                throw new ValidationException("Bu isimde bir ekip zaten mevcut.");

            if (string.IsNullOrWhiteSpace(team.Color))
            {
                team.Color = TeamColorHelper.GetDefaultColorForTeam(team.TeamName);
            }

            context.Teams.Add(team);
            context.SaveChanges();
        }

        public void DeleteTeam(int teamId)
        {
            using var context = CreateContext();
            var team = context.Teams.Find(teamId);
            if (team != null)
            {
                context.Teams.Remove(team);
                context.SaveChanges();
            }
        }
        #endregion

        #region User Operations
        public List<User> GetAllUsers()
        {
            using var context = CreateContext();
            return context.Users
                .Include(u => u.Shifts)
                .Include(u => u.Leaves)
                .Include(u => u.ProjectAllocations)
                .AsNoTracking()
                .ToList();
        }

        public void AddUser(User user)
        {
            using var context = CreateContext();
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Surname))
                throw new ValidationException("Ad ve Soyad alanları boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(user.Title))
                throw new ValidationException("Unvan alanı boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(user.Team))
                throw new ValidationException("Ekip alanı boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
                throw new ValidationException("Geçerli bir kurumsal e-posta adresi girilmelidir.");

            context.Users.Add(user);
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            using var context = CreateContext();
            var existing = context.Users.Find(user.Id);
            if (existing == null) throw new ValidationException("Kullanıcı bulunamadı.");

            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Surname))
                throw new ValidationException("Ad ve Soyad alanları boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(user.Title))
                throw new ValidationException("Unvan alanı boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(user.Team))
                throw new ValidationException("Ekip alanı boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
                throw new ValidationException("Geçerli bir kurumsal e-posta adresi girilmelidir.");

            existing.Name = user.Name;
            existing.Surname = user.Surname;
            existing.Title = user.Title;
            existing.Team = user.Team;
            existing.Email = user.Email;
            existing.Phone = user.Phone;
            existing.IsAdmin = user.IsAdmin;

            context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            using var context = CreateContext();
            var user = context.Users.Find(userId);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        public void ResetPassword(string email, string phone, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ValidationException("Lütfen kurumsal e-posta adresinizi giriniz.");
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ValidationException("Lütfen yeni şifrenizi giriniz.");
            if (newPassword.Length < 4) throw new ValidationException("Yeni şifre en az 4 karakter olmalıdır.");

            using var context = CreateContext();
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
            if (user == null)
            {
                throw new ValidationException("Bu e-posta adresine ait kullanıcı bulunamadı.");
            }

            if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(user.Phone))
            {
                var cleanInputPhone = new string(phone.Where(char.IsDigit).ToArray());
                var cleanUserPhone = new string(user.Phone.Where(char.IsDigit).ToArray());
                if (!string.IsNullOrWhiteSpace(cleanInputPhone) && !string.IsNullOrWhiteSpace(cleanUserPhone))
                {
                    if (!cleanUserPhone.EndsWith(cleanInputPhone) && !cleanInputPhone.EndsWith(cleanUserPhone))
                    {
                        throw new ValidationException("Girdiğiniz telefon numarası sistemdeki bilgilerinizle eşleşmedi.");
                    }
                }
            }

            user.Password = newPassword;
            context.SaveChanges();
        }
        #endregion

        #region Shift Operations
        public List<ShiftType> GetAllShiftTypes()
        {
            using var context = CreateContext();
            return context.ShiftTypes.AsNoTracking().ToList();
        }

        public void AddShiftType(ShiftType shiftType)
        {
            using var context = CreateContext();
            if (string.IsNullOrWhiteSpace(shiftType.ShiftName))
                throw new ValidationException("Nöbet tipi adı boş bırakılamaz.");

            context.ShiftTypes.Add(shiftType);
            context.SaveChanges();
        }

        public void AddShiftType(string shiftName)
        {
            if (string.IsNullOrWhiteSpace(shiftName)) return;
            using var context = CreateContext();
            var trimmed = shiftName.Trim();
            if (!context.ShiftTypes.Any(s => s.ShiftName.ToLower() == trimmed.ToLower()))
            {
                context.ShiftTypes.Add(new ShiftType { ShiftName = trimmed });
                context.SaveChanges();
            }
        }

        public List<Shift> GetAllShifts()
        {
            using var context = CreateContext();
            return context.Shifts
                .Include(s => s.User)
                .Include(s => s.ShiftType)
                .AsNoTracking()
                .OrderBy(s => s.ShiftDate)
                .ToList();
        }

        public void AddShift(Shift shift)
        {
            using var context = CreateContext();
            if (shift.UserId <= 0) throw new ValidationException("Nöbetçi personel seçilmelidir.");
            if (shift.ShiftTypeId <= 0) throw new ValidationException("Nöbet tipi seçilmelidir.");
            if (string.IsNullOrWhiteSpace(shift.JiraTicketNo)) throw new ValidationException("Jira Kayıt Numarası girilmelidir.");

            context.Shifts.Add(shift);
            context.SaveChanges();
        }

        public void UpdateShift(Shift shift)
        {
            using var context = CreateContext();
            var existing = context.Shifts.Find(shift.Id);
            if (existing == null) throw new ValidationException("Nöbet kaydı bulunamadı.");

            if (shift.UserId <= 0) throw new ValidationException("Nöbetçi personel seçilmelidir.");
            if (shift.ShiftTypeId <= 0) throw new ValidationException("Nöbet tipi seçilmelidir.");
            if (string.IsNullOrWhiteSpace(shift.JiraTicketNo)) throw new ValidationException("Jira Kayıt Numarası girilmelidir.");

            existing.UserId = shift.UserId;
            existing.ShiftTypeId = shift.ShiftTypeId;
            existing.ShiftDate = shift.ShiftDate;
            existing.JiraTicketNo = shift.JiraTicketNo;
            existing.ExternalLink = shift.ExternalLink;

            context.SaveChanges();
        }

        public void DeleteShift(int shiftId)
        {
            using var context = CreateContext();
            var shift = context.Shifts.Find(shiftId);
            if (shift != null)
            {
                context.Shifts.Remove(shift);
                context.SaveChanges();
            }
        }

        public void TriggerExternalLink(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                throw new ValidationException($"Dış bağlantı açılamadı: {ex.Message}");
            }
        }

        #region Monthly Release Shifts
        public List<MonthlyReleaseShift> GetAllMonthlyReleaseShifts()
        {
            using var context = CreateContext();
            return context.MonthlyReleaseShifts.AsNoTracking().OrderBy(r => r.ReleaseDate).ToList();
        }

        public void AddMonthlyReleaseShift(MonthlyReleaseShift shift)
        {
            using var context = CreateContext();
            if (string.IsNullOrWhiteSpace(shift.MonthName)) throw new ValidationException("Yaygınlaştırma adı boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(shift.AssignedUsers)) throw new ValidationException("Sorumlu analist/yazılımcı seçilmelidir.");

            shift.JiraTicketNo ??= string.Empty;
            shift.ExternalLink ??= string.Empty;
            shift.Note ??= string.Empty;
            shift.CreatedByUserName ??= string.Empty;
            shift.CreatedAt ??= DateTime.Now;

            try
            {
                context.MonthlyReleaseShifts.Add(shift);
                context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new ValidationException($"Veritabanı Güncelleme Hatası: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
        }

        public void UpdateMonthlyReleaseShift(MonthlyReleaseShift shift)
        {
            using var context = CreateContext();
            var existing = context.MonthlyReleaseShifts.Find(shift.Id);
            if (existing == null) throw new ValidationException("Yaygınlaştırma kaydı bulunamadı.");

            existing.MonthName = shift.MonthName;
            existing.ReleaseDate = shift.ReleaseDate;
            existing.AssignedUsers = shift.AssignedUsers;
            existing.JiraTicketNo = shift.JiraTicketNo ?? string.Empty;
            existing.ExternalLink = shift.ExternalLink ?? string.Empty;
            existing.Note = shift.Note ?? string.Empty;
            existing.IsFinished = shift.IsFinished;
            if (!string.IsNullOrWhiteSpace(shift.CreatedByUserName))
            {
                existing.CreatedByUserName = shift.CreatedByUserName;
            }
            if (!string.IsNullOrWhiteSpace(shift.UpdatedByUserName))
            {
                existing.UpdatedByUserName = shift.UpdatedByUserName;
                existing.UpdatedAt = shift.UpdatedAt ?? DateTime.Now;
            }

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new ValidationException($"Veritabanı Güncelleme Hatası: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
        }

        public void DeleteMonthlyReleaseShift(int id)
        {
            using var context = CreateContext();
            var item = context.MonthlyReleaseShifts.Find(id);
            if (item != null)
            {
                context.MonthlyReleaseShifts.Remove(item);
                context.SaveChanges();
            }
        }
        #endregion

        #region Custom Shifts
        public List<CustomShift> GetAllCustomShifts()
        {
            using var context = CreateContext();
            return context.CustomShifts.AsNoTracking().OrderBy(c => c.ShiftDate).ToList();
        }

        public void AddCustomShift(CustomShift shift)
        {
            using var context = CreateContext();
            if (string.IsNullOrWhiteSpace(shift.Topic)) throw new ValidationException("Nöbet konusu/tipi boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(shift.AssignedUsers)) throw new ValidationException("Nöbetçi personel(ler) seçilmelidir.");

            shift.Description ??= string.Empty;
            shift.ExternalLink ??= string.Empty;
            shift.CreatedByUserName ??= string.Empty;
            shift.CreatedAt ??= DateTime.Now;

            try
            {
                context.CustomShifts.Add(shift);
                context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new ValidationException($"Veritabanı Güncelleme Hatası: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
        }

        public void UpdateCustomShift(CustomShift shift)
        {
            using var context = CreateContext();
            var existing = context.CustomShifts.Find(shift.Id);
            if (existing == null) throw new ValidationException("Nöbet kaydı bulunamadı.");

            existing.Topic = shift.Topic;
            existing.Description = shift.Description ?? string.Empty;
            existing.AssignedUsers = shift.AssignedUsers;
            existing.ShiftDate = shift.ShiftDate;
            existing.ExternalLink = shift.ExternalLink ?? string.Empty;
            existing.IsFinished = shift.IsFinished;
            if (!string.IsNullOrWhiteSpace(shift.CreatedByUserName))
            {
                existing.CreatedByUserName = shift.CreatedByUserName;
            }
            if (!string.IsNullOrWhiteSpace(shift.UpdatedByUserName))
            {
                existing.UpdatedByUserName = shift.UpdatedByUserName;
                existing.UpdatedAt = shift.UpdatedAt ?? DateTime.Now;
            }

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new ValidationException($"Veritabanı Güncelleme Hatası: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
        }

        public void DeleteCustomShift(int id)
        {
            using var context = CreateContext();
            var item = context.CustomShifts.Find(id);
            if (item != null)
            {
                context.CustomShifts.Remove(item);
                context.SaveChanges();
            }
        }
        #endregion
        #endregion

        #region Leave Operations
        public List<Leave> GetAllLeaves()
        {
            using var context = CreateContext();
            return context.Leaves
                .Include(l => l.User)
                .AsNoTracking()
                .OrderBy(l => l.StartDate)
                .ToList();
        }

        public void AddLeave(Leave leave)
        {
            if (leave.UserId <= 0) throw new ValidationException("Personel seçilmelidir.");

            // Apply Business Validation Rules
            ValidationRules.ValidateLeaveDates(leave.StartDate, leave.EndDate);

            // Compute automatic fields
            leave.Year = (short)leave.StartDate.Year;
            leave.Quarter = (byte)((leave.StartDate.Month - 1) / 3 + 1);

            using var context = CreateContext();
            context.Leaves.Add(leave);
            context.SaveChanges();
        }

        public void UpdateLeave(Leave leave)
        {
            if (leave.UserId <= 0) throw new ValidationException("Personel seçilmelidir.");

            // Apply Business Validation Rules
            ValidationRules.ValidateLeaveDates(leave.StartDate, leave.EndDate);

            // Compute automatic fields
            leave.Year = (short)leave.StartDate.Year;
            leave.Quarter = (byte)((leave.StartDate.Month - 1) / 3 + 1);

            using var context = CreateContext();
            var existing = context.Leaves.Find(leave.Id);
            if (existing == null) throw new ValidationException("İzin kaydı bulunamadı.");

            existing.UserId = leave.UserId;
            existing.StartDate = leave.StartDate;
            existing.EndDate = leave.EndDate;
            existing.Year = leave.Year;
            existing.Quarter = leave.Quarter;

            // Also persist status-related fields so Pending/Approved/Notification changes are saved
            existing.Status = leave.Status;
            existing.RequestNote = leave.RequestNote;
            existing.IsSpecialRequest = leave.IsSpecialRequest;
            existing.RequestedAt = leave.RequestedAt;
            existing.ApprovedAt = leave.ApprovedAt;
            existing.ApprovedByUserName = leave.ApprovedByUserName;
            existing.PreviousStartDate = leave.PreviousStartDate;
            existing.PreviousEndDate = leave.PreviousEndDate;
            existing.IsNotificationSeen = leave.IsNotificationSeen;

            context.SaveChanges();
        }

        public Leave? GetExistingLeaveForUser(int userId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null)
        {
            using var context = CreateContext();
            var query = context.Leaves
                .Include(l => l.User)
                .Where(l => l.UserId == userId && l.Status != "Rejected" && l.Status != "Cancelled")
                .Where(l => l.StartDate.Date <= endDate.Date && l.EndDate.Date >= startDate.Date);

            if (excludeLeaveId.HasValue)
            {
                query = query.Where(l => l.Id != excludeLeaveId.Value);
            }

            var candidates = query.ToList();
            foreach (var leave in candidates)
            {
                // Calculate effective ranges
                bool leaveIsHourly = (leave.StartDate.TimeOfDay != TimeSpan.Zero || leave.EndDate.TimeOfDay != TimeSpan.Zero);
                bool reqIsHourly = (startDate.TimeOfDay != TimeSpan.Zero || endDate.TimeOfDay != TimeSpan.Zero);

                DateTime leaveStart = leaveIsHourly ? leave.StartDate : leave.StartDate.Date;
                DateTime leaveEnd = leaveIsHourly ? leave.EndDate : leave.EndDate.Date.AddDays(1);

                DateTime reqStart = reqIsHourly ? startDate : startDate.Date;
                DateTime reqEnd = reqIsHourly ? endDate : endDate.Date.AddDays(1);

                if (leaveStart < reqEnd && leaveEnd > reqStart)
                {
                    return leave;
                }
            }

            return null;
        }

        public void DeleteLeave(int leaveId, string adminUserName = "")
        {
            using var context = CreateContext();
            var leave = context.Leaves.Find(leaveId);
            if (leave != null)
            {
                leave.Status = "Cancelled";
                leave.ApprovedByUserName = string.IsNullOrWhiteSpace(adminUserName) ? "Yönetici" : adminUserName;
                leave.IsNotificationSeen = false; // Send cancellation notification to user
                context.SaveChanges();
            }
        }

        #region Leave Approval & Role Capacity Workflow
        public class RoleCapacityCheckResult
        {
            public bool Has50PercentOverlap { get; set; }
            public int TotalRoleMembers { get; set; }
            public int MaxAbsentMembersOnOverlapDays { get; set; }
            public List<DateTime> OverlappingDates { get; set; } = new List<DateTime>();
            public string DetailedMessage { get; set; } = string.Empty;
        }

        public RoleCapacityCheckResult CheckLeaveRoleCapacityOverlap(int userId, DateTime startDate, DateTime endDate, int? currentLeaveId = null)
        {
            using var context = CreateContext();
            var result = new RoleCapacityCheckResult();
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null || string.IsNullOrWhiteSpace(user.Team) || string.IsNullOrWhiteSpace(user.Title))
                return result;

            var teamRoleMembers = context.Users
                .Where(u => u.Team == user.Team && u.Title == user.Title)
                .ToList();

            int totalRoleMembers = teamRoleMembers.Count;
            result.TotalRoleMembers = totalRoleMembers;
            if (totalRoleMembers == 0) return result;

            var teamRoleUserIds = teamRoleMembers.Select(m => m.Id).ToList();

            var existingLeaves = context.Leaves
                .Where(l => teamRoleUserIds.Contains(l.UserId) && l.Status != "Rejected" && l.Status != "Cancelled")
                .Where(l => l.StartDate <= endDate && l.EndDate >= startDate)
                .ToList();

            if (currentLeaveId.HasValue)
            {
                existingLeaves = existingLeaves.Where(l => l.Id != currentLeaveId.Value).ToList();
            }

            int maxAbsent = 0;
            var overlappingDates = new List<DateTime>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                bool reqIsHourly = (startDate.TimeOfDay != TimeSpan.Zero || endDate.TimeOfDay != TimeSpan.Zero);
                DateTime reqStart = reqIsHourly ? startDate : date;
                DateTime reqEnd = reqIsHourly ? endDate : date.AddDays(1);

                var absentUserIdsOnDate = existingLeaves
                    .Where(l => l.UserId != userId)
                    .Where(l => {
                        bool lIsHourly = (l.StartDate.TimeOfDay != TimeSpan.Zero || l.EndDate.TimeOfDay != TimeSpan.Zero);
                        DateTime lStart = lIsHourly ? l.StartDate : l.StartDate.Date;
                        DateTime lEnd = lIsHourly ? l.EndDate : l.EndDate.Date.AddDays(1);

                        // Only overlaps if their times cross
                        return lStart < reqEnd && lEnd > reqStart;
                    })
                    .Select(l => l.UserId)
                    .Distinct()
                    .Count();

                int totalAbsentWithUser = absentUserIdsOnDate + 1;
                double absentRatio = (double)totalAbsentWithUser / totalRoleMembers;

                // A conflict ONLY occurs if there is AT LEAST ONE OTHER colleague in the same role on leave on this date AND the absent ratio is >= 50%
                if (absentUserIdsOnDate > 0 && absentRatio >= 0.5)
                {
                    overlappingDates.Add(date);
                    if (totalAbsentWithUser > maxAbsent)
                    {
                        maxAbsent = totalAbsentWithUser;
                    }
                }
            }

            if (overlappingDates.Any())
            {
                result.Has50PercentOverlap = true;
                result.MaxAbsentMembersOnOverlapDays = maxAbsent;
                result.OverlappingDates = overlappingDates;

                var dateStr = string.Join(", ", overlappingDates.Take(5).Select(d => d.ToString("dd.MM.yyyy")));
                if (overlappingDates.Count > 5) dateStr += $" ve {overlappingDates.Count - 5} gün daha";

                result.DetailedMessage = $"'{user.Team}' ekibindeki '{user.Title}' rolündeki {totalRoleMembers} personelden {maxAbsent} kadarı ({dateStr}) tarihlerinde izinli görünmektedir (%50 veya üzeri rol çakışması). Özel izin gereklidir.";
            }

            return result;
        }

        public void ApproveLeave(int leaveId, string adminUserName)
        {
            using var context = CreateContext();
            var leave = context.Leaves.Find(leaveId);
            if (leave == null) throw new ValidationException("İzin kaydı bulunamadı.");

            leave.PreviousStartDate = null;
            leave.PreviousEndDate = null;
            leave.Status = "Approved";
            leave.ApprovedAt = DateTime.Now;
            leave.ApprovedByUserName = string.IsNullOrWhiteSpace(adminUserName) ? "Yönetici" : adminUserName;
            leave.IsNotificationSeen = false; // Notify the user

            context.SaveChanges();
        }

        public void RejectLeave(int leaveId, string adminUserName = "")
        {
            using var context = CreateContext();
            var leave = context.Leaves.Find(leaveId);
            if (leave == null) throw new ValidationException("İzin kaydı bulunamadı.");

            if (leave.PreviousStartDate.HasValue && leave.PreviousEndDate.HasValue)
            {
                // Revert back to previously approved leave dates
                leave.StartDate = leave.PreviousStartDate.Value;
                leave.EndDate = leave.PreviousEndDate.Value;
                leave.Year = (short)leave.StartDate.Year;
                leave.Quarter = (byte)((leave.StartDate.Month - 1) / 3 + 1);
                leave.PreviousStartDate = null;
                leave.PreviousEndDate = null;
                leave.Status = "Approved"; // Revert to Approved with original dates
                leave.RequestNote = "❌ Değişiklik talebi reddedildi, eski onaylı izin geçerli tutuldu.";
            }
            else
            {
                leave.Status = "Rejected";
            }

            leave.ApprovedByUserName = string.IsNullOrWhiteSpace(adminUserName) ? "Yönetici" : adminUserName;
            leave.IsNotificationSeen = false; // Notify the user
            context.SaveChanges();
        }

        public List<Leave> GetUnseenNotificationsForUser(string userFullName)
        {
            using var context = CreateContext();
            var user = context.Users
                .FirstOrDefault(u => (u.Name + " " + u.Surname) == userFullName);
            if (user == null) return new List<Leave>();

            var unseen = context.Leaves
                .Include(l => l.User)
                .Where(l => l.UserId == user.Id &&
                            !l.IsNotificationSeen &&
                            (l.Status == "Approved" || l.Status == "Rejected" || l.Status == "Cancelled"))
                .ToList();

            // Filter out self-notifications (where the action was performed by the user themselves)
            unseen = unseen.Where(l => string.IsNullOrWhiteSpace(l.ApprovedByUserName) ||
                                       (!l.ApprovedByUserName.Equals(userFullName, StringComparison.OrdinalIgnoreCase) &&
                                        !l.ApprovedByUserName.Equals(user.FullName, StringComparison.OrdinalIgnoreCase)))
                           .ToList();

            var now = DateTime.Now;
            var expired = new List<Leave>();
            var valid = new List<Leave>();

            foreach (var l in unseen)
            {
                var actionTime = l.ApprovedAt ?? l.RequestedAt ?? now;
                if ((now - actionTime).TotalDays > 30)
                {
                    expired.Add(l);
                }
                else
                {
                    valid.Add(l);
                }
            }

            if (expired.Count > 0)
            {
                foreach (var exp in expired)
                {
                    exp.IsNotificationSeen = true;
                }
                context.SaveChanges();
            }

            return valid;
        }

        public void MarkNotificationsSeenForUser(string userFullName)
        {
            using var context = CreateContext();
            var user = context.Users
                .FirstOrDefault(u => (u.Name + " " + u.Surname) == userFullName);
            if (user == null) return;

            var unseen = context.Leaves
                .Where(l => l.UserId == user.Id && !l.IsNotificationSeen)
                .ToList();

            foreach (var l in unseen)
                l.IsNotificationSeen = true;

            context.SaveChanges();
        }

        #endregion
        #endregion

        #region Project Management
        public List<Project> GetAllProjects()
        {
            using var context = CreateContext();
            return context.Projects
                .Include(p => p.ProjectMonthlyCosts)
                .AsNoTracking()
                .OrderByDescending(p => p.Id)
                .ToList();
        }

        public List<Project> GetProjectsByQuarter(short year, byte quarter, string team = "")
        {
            using var context2 = CreateContext();
            var query = context2.Projects
                .Include(p => p.ProjectMonthlyCosts)
                .AsNoTracking()
                .Where(p => p.Year == year && p.Quarter == quarter);

            if (!string.IsNullOrWhiteSpace(team))
            {
                query = query.Where(p => p.Team == team || p.Team == "Tüm Ekipler" || string.IsNullOrEmpty(p.Team));
            }

            return query.OrderByDescending(p => p.Id).ToList();
        }

        public List<ProjectMonthlyCost> GetProjectMonthlyCosts(int projectId)
        {
            using var context = CreateContext();
            return context.ProjectMonthlyCosts
                .Include(pmc => pmc.User)
                .AsNoTracking()
                .Where(pmc => pmc.ProjectId == projectId)
                .ToList();
        }

        public List<ProjectMonthlyCost> GetMonthlyCostsForProjects(IEnumerable<int> projectIds)
        {
            if (projectIds == null || !projectIds.Any()) return new List<ProjectMonthlyCost>();
            var ids = projectIds.ToHashSet();
            using var context = CreateContext();
            return context.ProjectMonthlyCosts
                .Include(pmc => pmc.User)
                .AsNoTracking()
                .Where(pmc => ids.Contains(pmc.ProjectId))
                .ToList();
        }

        public List<ProjectAllocation> GetAllocationsForProjects(IEnumerable<int> projectIds)
        {
            if (projectIds == null || !projectIds.Any()) return new List<ProjectAllocation>();
            var ids = projectIds.ToHashSet();
            using var context = CreateContext();
            return context.ProjectAllocations
                .Include(pa => pa.User)
                .AsNoTracking()
                .Where(pa => ids.Contains(pa.ProjectId))
                .ToList();
        }

        public void SaveProjectWithMonthlyCosts(Project project, List<ProjectMonthlyCost> monthlyCosts, List<ProjectAllocation>? allocations = null, string currentUserName = "")
        {
            using var context = CreateContext();
            if (string.IsNullOrWhiteSpace(project.ProjectName))
                throw new ValidationException("Proje Adı / Talep Özeti boş bırakılamaz.");

            bool isCompletedNow = string.Equals(project.ProjectStatus, "Tamamlandı", StringComparison.OrdinalIgnoreCase);

            if (project.Id == 0)
            {
                if (!string.IsNullOrWhiteSpace(currentUserName))
                {
                    project.CreatedByUserName = currentUserName;
                    project.UpdatedByUserName = currentUserName;
                    if (isCompletedNow) project.CompletedByUserName = currentUserName;
                }
                project.CreatedAt ??= DateTime.Now;
                project.UpdatedAt = DateTime.Now;
                if (isCompletedNow) project.CompletedAt ??= DateTime.Now;

                context.Projects.Add(project);
                context.SaveChanges();

                if (monthlyCosts != null && monthlyCosts.Any())
                {
                    foreach (var mc in monthlyCosts)
                    {
                        if (mc.ManDays > 0) // Only save > 0
                        {
                            mc.ProjectId = project.Id;
                            mc.User = null; // Prevent tracking conflicts
                            context.ProjectMonthlyCosts.Add(mc);
                        }
                    }
                    context.SaveChanges();
                }

                if (allocations != null && allocations.Any())
                {
                    foreach (var alloc in allocations)
                    {
                        if (alloc.ActualManDay > 0) // Only save > 0
                        {
                            alloc.ProjectId = project.Id;
                            alloc.User = null; // Prevent tracking conflicts
                            context.ProjectAllocations.Add(alloc);
                        }
                    }
                    context.SaveChanges();
                }
            }
            else
            {
                var existing = context.Projects
                    .Include(p => p.ProjectMonthlyCosts)
                    .Include(p => p.ProjectAllocations)
                    .FirstOrDefault(p => p.Id == project.Id);

                if (existing == null) throw new ValidationException("Güncellenecek proje bulunamadı.");

                existing.PergelNo = project.PergelNo;
                existing.ProjectName = project.ProjectName;
                existing.Summary = project.Summary;
                existing.ProjectStatus = project.ProjectStatus;
                existing.ProjectType = project.ProjectType;
                existing.ExternalCompanyName = project.ExternalCompanyName;
                existing.ExternalCost = project.ExternalCost;
                existing.Team = project.Team;
                existing.Year = project.Year;
                existing.Quarter = project.Quarter;
                existing.Stakeholders = project.Stakeholders;
                existing.Gmy = project.Gmy;
                existing.BusinessUnit = project.BusinessUnit;
                existing.Description = project.Description;
                existing.AssignedUserNames = project.AssignedUserNames;
                existing.AssignedAnalystNames = project.AssignedAnalystNames;
                existing.AssignedDeveloperNames = project.AssignedDeveloperNames;
                existing.AssignedUserIds = project.AssignedUserIds;
                existing.TotalManDayBudget = project.TotalManDayBudget;
                existing.AnalystPlannedManDays = project.AnalystPlannedManDays;
                existing.DeveloperPlannedManDays = project.DeveloperPlannedManDays;
                existing.AnalistAy1 = project.AnalistAy1;
                existing.AnalistAy2 = project.AnalistAy2;
                existing.AnalistAy3 = project.AnalistAy3;
                existing.YazilimciAy1 = project.YazilimciAy1;
                existing.YazilimciAy2 = project.YazilimciAy2;
                existing.YazilimciAy3 = project.YazilimciAy3;
                existing.ActualManDays = project.ActualManDays;
                existing.AnalystActualManDays = project.AnalystActualManDays;
                existing.DeveloperActualManDays = project.DeveloperActualManDays;
                existing.PlannedReleaseDate = project.PlannedReleaseDate;
                existing.ActualReleaseDate = project.ActualReleaseDate;

                if (!string.IsNullOrWhiteSpace(currentUserName))
                {
                    if (string.IsNullOrWhiteSpace(existing.CreatedByUserName)) existing.CreatedByUserName = currentUserName;
                    existing.UpdatedByUserName = currentUserName;
                    if (isCompletedNow && string.IsNullOrWhiteSpace(existing.CompletedByUserName)) existing.CompletedByUserName = currentUserName;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(project.UpdatedByUserName)) existing.UpdatedByUserName = project.UpdatedByUserName;
                    if (isCompletedNow && !string.IsNullOrWhiteSpace(project.CompletedByUserName)) existing.CompletedByUserName = project.CompletedByUserName;
                }

                existing.UpdatedAt = DateTime.Now;
                if (isCompletedNow && existing.CompletedAt == null)
                {
                    existing.CompletedAt = DateTime.Now;
                }

                // Refresh monthly costs
                var oldCosts = context.ProjectMonthlyCosts.Where(c => c.ProjectId == project.Id).ToList();
                context.ProjectMonthlyCosts.RemoveRange(oldCosts);

                if (monthlyCosts != null && monthlyCosts.Any())
                {
                    foreach (var mc in monthlyCosts)
                    {
                        mc.Id = 0;
                        mc.ProjectId = project.Id;
                        mc.User = null; // EF Core tracking hatasını önlemek için
                        context.ProjectMonthlyCosts.Add(mc);
                    }
                }

                // Refresh allocations (person actual efforts)
                var oldAllocations = context.ProjectAllocations.Where(a => a.ProjectId == project.Id).ToList();
                context.ProjectAllocations.RemoveRange(oldAllocations);

                if (allocations != null && allocations.Any())
                {
                    foreach (var alloc in allocations)
                    {
                        alloc.Id = 0;
                        alloc.ProjectId = project.Id;
                        alloc.User = null; // EF Core tracking hatasını önlemek için
                        context.ProjectAllocations.Add(alloc);
                    }
                }

                context.SaveChanges();
                context.SaveChanges();
            }
        }

        public void ClearAllSyntheticCosts()
        {
            using var context = CreateContext();
            var allCosts = context.ProjectMonthlyCosts.ToList();
            var allAlloc = context.ProjectAllocations.ToList();
            context.ProjectMonthlyCosts.RemoveRange(allCosts);
            context.ProjectAllocations.RemoveRange(allAlloc);
            
            // Also reset all project totals to 0
            var allProj = context.Projects.ToList();
            foreach(var p in allProj)
            {
                p.ActualManDays = 0;
            }
            context.SaveChanges();
        }

        public void DeleteProject(int projectId)
        {
            using var context = CreateContext();
            var p = context.Projects
                .Include(proj => proj.ProjectMonthlyCosts)
                .FirstOrDefault(proj => proj.Id == projectId);

            if (p != null)
            {
                if (p.ProjectMonthlyCosts.Any())
                {
                    context.ProjectMonthlyCosts.RemoveRange(p.ProjectMonthlyCosts);
                }
                context.Projects.Remove(p);
                context.SaveChanges();
            }
        }
        #endregion

        #region Project Allocation Operations
        public List<ProjectAllocation> GetAllocationsByProject(int projectId)
        {
            using var context = CreateContext();
            return context.ProjectAllocations
                .Include(pa => pa.User)
                .Where(pa => pa.ProjectId == projectId)
                .ToList();
        }

        public void AddProjectAllocation(ProjectAllocation allocation)
        {
            if (allocation.ProjectId <= 0) throw new ValidationException("Geçerli bir proje seçilmelidir.");
            if (allocation.UserId <= 0) throw new ValidationException("Personel seçilmelidir.");
            if (allocation.AllocatedManDay <= 0) throw new ValidationException("Atanan adam/gün sıfırdan büyük olmalıdır.");

            using var context = CreateContext();

            // Check budget allocation validation rules
            ValidationRules.ValidateProjectBudget(context, allocation.ProjectId, null, allocation.AllocatedManDay);

            context.ProjectAllocations.Add(allocation);
            context.SaveChanges();
        }

        public void UpdateProjectAllocation(ProjectAllocation allocation)
        {
            if (allocation.ProjectId <= 0) throw new ValidationException("Geçerli bir proje seçilmelidir.");
            if (allocation.UserId <= 0) throw new ValidationException("Personel seçilmelidir.");
            if (allocation.AllocatedManDay <= 0) throw new ValidationException("Atanan adam/gün sıfırdan büyük olmalıdır.");

            using var context = CreateContext();

            // Check budget allocation validation rules (excluding current allocation's previous value)
            ValidationRules.ValidateProjectBudget(context, allocation.ProjectId, allocation.Id, allocation.AllocatedManDay);

            var existing = context.ProjectAllocations.Find(allocation.Id);
            if (existing == null) throw new ValidationException("Proje tahsisi bulunamadı.");

            existing.ProjectId = allocation.ProjectId;
            existing.UserId = allocation.UserId;
            existing.AllocatedManDay = allocation.AllocatedManDay;

            context.SaveChanges();
        }

        public void DeleteProjectAllocation(int allocationId)
        {
            using var context = CreateContext();
            var alloc = context.ProjectAllocations.Find(allocationId);
            if (alloc != null)
            {
                context.ProjectAllocations.Remove(alloc);
                context.SaveChanges();
            }
        }
        #endregion

        #region Chat Operations (With Daily Reset / Auto-Cleanup)
        public void CleanupOldChatMessages()
        {
            try
            {
                using var context = CreateContext();
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatMessages')
                    BEGIN
                        CREATE TABLE ChatMessages (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            SenderUserId INT NOT NULL,
                            ReceiverUserId INT NULL,
                            TargetType NVARCHAR(50) NOT NULL,
                            TargetTeam NVARCHAR(50) NULL,
                            MessageText NVARCHAR(MAX) NOT NULL,
                            SentAt DATETIME2 NOT NULL,
                            IsRead BIT NOT NULL
                        );
                    END
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatGroups')
                    BEGIN
                        CREATE TABLE ChatGroups (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            GroupName NVARCHAR(100) NOT NULL,
                            MemberUserIds NVARCHAR(500) NOT NULL,
                            CreatedByUserId INT NOT NULL,
                            CreatedAt DATETIME2 NOT NULL
                        );
                    END
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatMessageReadStates')
                    BEGIN
                        CREATE TABLE ChatMessageReadStates (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            UserId INT NOT NULL,
                            ChannelKey NVARCHAR(100) NOT NULL,
                            LastReadMessageId INT NOT NULL,
                            LastReadAt DATETIME2 NOT NULL
                        );
                    END
                ");

                var today = DateTime.Today;
                var oldMessages = context.ChatMessages
                    .Where(m => m.SentAt < today)
                    .ToList();

                if (oldMessages.Any())
                {
                    context.ChatMessages.RemoveRange(oldMessages);
                    context.SaveChanges();
                }

                // If all messages were deleted or no messages remain for today, reset read states
                if (!context.ChatMessages.Any())
                {
                    var allStates = context.ChatMessageReadStates.ToList();
                    if (allStates.Any())
                    {
                        context.ChatMessageReadStates.RemoveRange(allStates);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                // Ignore cleanup errors on system startup
            }
        }

        public ChatGroup CreateChatGroup(string groupName, List<int> memberUserIds, int createdByUserId)
        {
            if (string.IsNullOrWhiteSpace(groupName)) throw new ValidationException("Lütfen bir grup adı giriniz.");
            if (!memberUserIds.Contains(createdByUserId)) memberUserIds.Add(createdByUserId);

            using var context = CreateContext();
            var group = new ChatGroup
            {
                GroupName = groupName.Trim(),
                MemberUserIds = string.Join(",", memberUserIds.Distinct()),
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.Now
            };
            context.ChatGroups.Add(group);
            context.SaveChanges();
            return group;
        }

        public List<ChatGroup> GetChatGroupsForUser(int userId)
        {
            try
            {
                using var context = CreateContext();
                var all = context.ChatGroups.AsNoTracking().ToList();
                string idStr = userId.ToString();
                return all.Where(g => g.MemberUserIds.Split(',').Contains(idStr)).ToList();
            }
            catch
            {
                return new List<ChatGroup>();
            }
        }

        public void SendChatMessage(ChatMessage msg)
        {
            if (string.IsNullOrWhiteSpace(msg.MessageText)) return;

            CleanupOldChatMessages();

            msg.SentAt = DateTime.Now;
            msg.IsRead = false;

            using var context = CreateContext();
            context.ChatMessages.Add(msg);
            context.SaveChanges();
        }

        public List<ChatMessage> GetChatMessagesForChannel(int currentUserId, string targetType, int? targetUserId = null, string targetTeam = "")
        {
            using var context = CreateContext();
            var today = DateTime.Today;
            var query = context.ChatMessages
                .Include(m => m.SenderUser)
                .Include(m => m.ReceiverUser)
                .AsNoTracking()
                .Where(m => m.SentAt >= today);

            if (targetType == "General")
            {
                query = query.Where(m => m.TargetType == "General");
            }
            else if (targetType == "Team")
            {
                query = query.Where(m => m.TargetType == "Team" && m.TargetTeam == targetTeam);
            }
            else if (targetType == "Group" && !string.IsNullOrWhiteSpace(targetTeam))
            {
                query = query.Where(m => m.TargetType == "Group" && m.TargetTeam == targetTeam);
            }
            else if (targetType == "Direct" && targetUserId.HasValue)
            {
                int otherId = targetUserId.Value;
                query = query.Where(m => m.TargetType == "Direct" &&
                                         ((m.SenderUserId == currentUserId && m.ReceiverUserId == otherId) ||
                                          (m.SenderUserId == otherId && m.ReceiverUserId == currentUserId)));
            }
            else
            {
                return new List<ChatMessage>();
            }

            return query.OrderBy(m => m.SentAt).ToList();
        }

        public static string GetChannelKey(int currentUserId, string targetType, int? targetUserId = null, string targetTeam = "")
        {
            if (targetType == "General")
                return "General";
            if (targetType == "Team")
                return $"Team:{(targetTeam ?? "").Trim().ToLower()}";
            if (targetType == "Group")
                return $"Group:{(targetTeam ?? "").Trim()}";
            if (targetType == "Direct" && targetUserId.HasValue)
            {
                int u1 = Math.Min(currentUserId, targetUserId.Value);
                int u2 = Math.Max(currentUserId, targetUserId.Value);
                return $"Direct:{u1}_{u2}";
            }
            return targetType;
        }

        public void MarkMessagesAsRead(int currentUserId, string targetType, int? targetUserId = null, string targetTeam = "")
        {
            using var context = CreateContext();
            string channelKey = GetChannelKey(currentUserId, targetType, targetUserId, targetTeam);

            var channelMsgs = GetChatMessagesForChannel(currentUserId, targetType, targetUserId, targetTeam);
            int maxMsgId = channelMsgs.Any() ? channelMsgs.Max(m => m.Id) : 0;

            if (maxMsgId > 0)
            {
                var state = context.ChatMessageReadStates.FirstOrDefault(s => s.UserId == currentUserId && s.ChannelKey == channelKey);
                if (state == null)
                {
                    state = new ChatMessageReadState
                    {
                        UserId = currentUserId,
                        ChannelKey = channelKey,
                        LastReadMessageId = maxMsgId,
                        LastReadAt = DateTime.Now
                    };
                    context.ChatMessageReadStates.Add(state);
                }
                else if (maxMsgId > state.LastReadMessageId)
                {
                    state.LastReadMessageId = maxMsgId;
                    state.LastReadAt = DateTime.Now;
                }
            }

            if (targetType == "Direct" && targetUserId.HasValue)
            {
                int otherId = targetUserId.Value;
                var unreadDirects = context.ChatMessages.Where(m => m.TargetType == "Direct" && m.SenderUserId == otherId && m.ReceiverUserId == currentUserId && !m.IsRead).ToList();
                foreach (var m in unreadDirects)
                {
                    m.IsRead = true;
                }
            }

            context.SaveChanges();
        }

        public int GetUnreadMessageCountForChannel(int currentUserId, string targetType, int? targetUserId = null, string targetTeam = "")
        {
            using var context = CreateContext();
            string channelKey = GetChannelKey(currentUserId, targetType, targetUserId, targetTeam);
            var today = DateTime.Today;

            var readState = context.ChatMessageReadStates.AsNoTracking().FirstOrDefault(s => s.UserId == currentUserId && s.ChannelKey == channelKey);
            int lastReadId = readState?.LastReadMessageId ?? 0;

            if (targetType == "General")
            {
                return context.ChatMessages.AsNoTracking()
                    .Count(m => m.SentAt >= today && m.TargetType == "General" && m.SenderUserId != currentUserId && m.Id > lastReadId);
            }
            else if (targetType == "Team")
            {
                return context.ChatMessages.AsNoTracking()
                    .Count(m => m.SentAt >= today && m.TargetType == "Team" && m.TargetTeam.ToLower() == targetTeam.ToLower() && m.SenderUserId != currentUserId && m.Id > lastReadId);
            }
            else if (targetType == "Group" && !string.IsNullOrWhiteSpace(targetTeam))
            {
                return context.ChatMessages.AsNoTracking()
                    .Count(m => m.SentAt >= today && m.TargetType == "Group" && m.TargetTeam == targetTeam && m.SenderUserId != currentUserId && m.Id > lastReadId);
            }
            else if (targetType == "Direct" && targetUserId.HasValue)
            {
                int otherId = targetUserId.Value;
                return context.ChatMessages.AsNoTracking()
                    .Count(m => m.SentAt >= today && m.TargetType == "Direct" && m.SenderUserId == otherId && m.ReceiverUserId == currentUserId && m.Id > lastReadId && !m.IsRead);
            }

            return 0;
        }

        public List<User> GetActiveDirectChatUsersForUser(int currentUserId)
        {
            using var context = CreateContext();
            var today = DateTime.Today;
            var directMessages = context.ChatMessages
                .AsNoTracking()
                .Where(m => m.TargetType == "Direct" &&
                            (m.SentAt >= today || !m.IsRead) &&
                            (m.SenderUserId == currentUserId || m.ReceiverUserId == currentUserId))
                .ToList();

            var partnerUserIds = directMessages
                .Select(m => m.SenderUserId == currentUserId ? m.ReceiverUserId : m.SenderUserId)
                .Where(id => id.HasValue && id.Value != currentUserId)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            return context.Users
                .AsNoTracking()
                .Where(u => partnerUserIds.Contains(u.Id))
                .ToList();
        }

        public DateTime GetLastMessageTimeForChannel(int currentUserId, string targetType, int? targetUserId = null, string targetTeam = "")
        {
            using var context = CreateContext();
            var query = context.ChatMessages.AsNoTracking().AsQueryable();

            if (targetType == "General")
                query = query.Where(m => m.TargetType == "General");
            else if (targetType == "Team")
                query = query.Where(m => m.TargetType == "Team" && m.TargetTeam.ToLower() == targetTeam.ToLower());
            else if (targetType == "Group" && !string.IsNullOrWhiteSpace(targetTeam))
                query = query.Where(m => m.TargetType == "Group" && m.TargetTeam == targetTeam);
            else if (targetType == "Direct" && targetUserId.HasValue)
            {
                int otherId = targetUserId.Value;
                query = query.Where(m => m.TargetType == "Direct" &&
                                         ((m.SenderUserId == currentUserId && m.ReceiverUserId == otherId) ||
                                          (m.SenderUserId == otherId && m.ReceiverUserId == currentUserId)));
            }

            var lastMsg = query.OrderByDescending(m => m.SentAt).FirstOrDefault();
            return lastMsg?.SentAt ?? DateTime.MinValue;
        }

        public int GetUnreadMessageCountForUser(int currentUserId, string userTeam = "")
        {
            using var context = CreateContext();
            var user = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == currentUserId);
            string actualTeam = user?.Team ?? userTeam;

            int countGeneral = GetUnreadMessageCountForChannel(currentUserId, "General");
            int countTeam = string.IsNullOrWhiteSpace(actualTeam) ? 0 : GetUnreadMessageCountForChannel(currentUserId, "Team", null, actualTeam);

            int countDirect = 0;
            var directPartners = GetActiveDirectChatUsersForUser(currentUserId);
            foreach (var partner in directPartners)
            {
                countDirect += GetUnreadMessageCountForChannel(currentUserId, "Direct", partner.Id);
            }

            int countGroups = 0;
            try
            {
                var myGroupIds = GetChatGroupsForUser(currentUserId).Select(g => g.Id.ToString()).ToList();
                foreach (var gId in myGroupIds)
                {
                    countGroups += GetUnreadMessageCountForChannel(currentUserId, "Group", null, gId);
                }
            }
            catch { }

            return countDirect + countGeneral + countTeam + countGroups;
        }

        public void ClearAllChatMessages()
        {
            using var context = CreateContext();
            var all = context.ChatMessages.ToList();
            if (all.Any())
            {
                context.ChatMessages.RemoveRange(all);
            }
            var allStates = context.ChatMessageReadStates.ToList();
            if (allStates.Any())
            {
                context.ChatMessageReadStates.RemoveRange(allStates);
            }
            context.SaveChanges();
        }
        #endregion


    }
}
