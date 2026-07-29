using DataAccess.Abstract;
using Entity.Concrete;
using FluentValidation;
using System.Linq;

namespace Business.Validation
{
    public class MusteriValidator : AbstractValidator<Musteri>
    {
        public MusteriValidator()
        {
            // müsteri no
            RuleFor(m => m.MUST_NO)
                .NotEmpty().WithMessage("Müşteri numarası sistem tarafından otomatik atanmalıdır.")
                .Length(8).WithMessage("Müşteri numarası 8 haneli olmalıdır.")
                .Matches(@"^[0-9]{8}$").WithMessage("Müşteri numarası 8 haneli rakamlardan oluşmalıdır.");
            /// Müşteri Adı
            RuleFor(m => m.MUST_AD)
                .NotEmpty().WithMessage("Müşteri adı boş bırakılamaz.")
                .Length(2, 50).WithMessage("Müşteri adı 2 ile 50 karakter arasında olmalıdır.")
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$").WithMessage("Müşteri adı sadece harflerden oluşmalıdır.");

            // Müşteri Soyadı (Opsiyonel / Doldurulursa harf olmalı)
            When(m => !string.IsNullOrWhiteSpace(m.MUST_SOYAD), () =>
            {
                RuleFor(m => m.MUST_SOYAD)
                    .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$").WithMessage("Müşteri soyadı sadece harflerden oluşmalıdır.");
            });

            // E-posta
            RuleFor(m => m.MUST_EPOSTA)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            // Telefon Numarası
            RuleFor(m => m.MUST_TEL_NO)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.")
                .Length(13).WithMessage("Telefon numarası 5XX XXX XX XX formatında olmalıdır.")
                .Matches(@"^[0-9\s]+$").WithMessage("Geçerli bir telefon numarası giriniz.");

            // --- DURUM 1: Soyad Doluysa (Şahıs Müşterisi) ---
            When(m => !string.IsNullOrWhiteSpace(m.MUST_SOYAD), () =>
            {
                RuleFor(m => m.MUST_KIMLIK_NO)
                    .NotEmpty().WithMessage("Soyad girildiğinde TC Kimlik No girilmesi zorunludur.")
                    .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
                    .Matches(@"^[0-9]+$").WithMessage("TC Kimlik No sadece rakamlardan oluşmalıdır.")
                    .Must(GecerliTcKimlikAlgoritmasi).WithMessage("Girdiğiniz TC Kimlik Numarası geçersizdir.");

                RuleFor(m => m.MUST_VKN_NO)
                    .Empty().WithMessage("Soyad girildiğinde Vergi Kimlik Numarası boş bırakılmalıdır.");
            });

            // --- DURUM 2: Soyad Boşsa (Kurumsal Müşteri) ---
            When(m => string.IsNullOrWhiteSpace(m.MUST_SOYAD), () =>
            {
                RuleFor(m => m.MUST_VKN_NO)
                    .NotEmpty().WithMessage("Vergi Kimlik Numarası zorunludur.")
                    .Length(10).WithMessage("Vergi Kimlik Numarası 10 haneli olmalıdır.")
                    .Matches(@"^[0-9]+$").WithMessage("Vergi Kimlik Numarası sadece rakamlardan oluşmalıdır.");

                RuleFor(m => m.MUST_KIMLIK_NO)
                    .Empty().WithMessage("TC Kimlik No boş bırakılmalıdır.");
            });
        }

        // TC Kimlik Algoritması Kontrolü
        private bool GecerliTcKimlikAlgoritmasi(string tcNo)
        {
            if (string.IsNullOrWhiteSpace(tcNo) || tcNo.Length != 11 || !tcNo.All(char.IsDigit))
                return false;

            // İlk 10 rakamın toplamını hesapla
            int toplam = 0;
            for (int i = 0; i < 10; i++)
            {
                toplam += int.Parse(tcNo[i].ToString());
            }

            // 11. rakam
            int sonRakam = int.Parse(tcNo[10].ToString());

            // Toplamın birler basamağı (toplam % 10) 11. rakama eşit mi?
            return (toplam % 10) == sonRakam;
        }
    }
}