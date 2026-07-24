using System;
using System.Linq;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class BudgetExceededException : ValidationException
    {
        public decimal CurrentTotalAllocated { get; }
        public decimal NewAllocation { get; }
        public decimal TotalBudget { get; }

        public BudgetExceededException(decimal currentTotal, decimal newAllocation, decimal budget)
            : base($"Proje toplam adam/gün bütçesi aşılmaktadır! (Mevcut Tahsis: {currentTotal}, Yeni Atanacak: {newAllocation}, Toplam Bütçe: {budget})")
        {
            CurrentTotalAllocated = currentTotal;
            NewAllocation = newAllocation;
            TotalBudget = budget;
        }
    }

    public static class ValidationRules
    {
        // 1. İzin Tarih Kontrolü
        public static void ValidateLeaveDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date < DateTime.Today)
            {
                throw new ValidationException("Geçmiş tarihli izin girişi yapılamaz! Başlangıç tarihi bugünden küçük olamaz.");
            }

            if (endDate.Date < startDate.Date)
            {
                throw new ValidationException("İzin bitiş tarihi, başlangıç tarihinden önce olamaz.");
            }
        }

        // 2. Proje Bütçe Aşım Kontrolü
        public static void ValidateProjectBudget(AppDbContext context, int projectId, int? excludeAllocationId, decimal allocatedManDay)
        {
            var project = context.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                throw new ValidationException("Proje bulunamadı!");
            }

            // Sum all other allocations for this project
            var currentAllocationsQuery = context.ProjectAllocations
                .Where(a => a.ProjectId == projectId);

            if (excludeAllocationId.HasValue)
            {
                currentAllocationsQuery = currentAllocationsQuery.Where(a => a.Id != excludeAllocationId.Value);
            }

            decimal currentTotal = currentAllocationsQuery.Sum(a => (decimal?)a.AllocatedManDay) ?? 0m;
            decimal totalWithNew = currentTotal + allocatedManDay;

            if (totalWithNew > project.TotalManDayBudget)
            {
                throw new BudgetExceededException(currentTotal, allocatedManDay, project.TotalManDayBudget);
            }
        }
    }
}
