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

        // 2. Proje Bütçe Aşım Kontrolü — Devre dışı bırakıldı (serbest giriş için)
        public static void ValidateProjectBudget(AppDbContext context, int projectId, int? excludeAllocationId, decimal allocatedManDay)
        {
            // Kısıtlama kaldırıldı — kullanıcılar maliyet ve efor verilerini serbestçe girebilir.
            return;
        }
    }
}
