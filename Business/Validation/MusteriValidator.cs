using Entity.Concrete;
using FluentValidation;

namespace Business.Validation
{
    public class MusteriValidator : AbstractValidator<Musteri>
    {
        public MusteriValidator()
        {
            // Müşteri Adı
            RuleFor(m => m.MUST_AD)
                .NotEmpty().WithMessage("Müşteri adı boş bırakılamaz.")
                .Length(2, 50).WithMessage("Müşteri adı 2 ile 50 karakter arasında olmalıdır.");

            // E-posta
            RuleFor(m => m.MUST_EPOSTA)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            // Telefon Numarası (Zorunlu, 13 karakter [5XX XXX XX XX] ve sadece rakam/boşluk)
            RuleFor(m => m.MUST_TEL_NO)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.")
                .Length(13).WithMessage("Telefon numarası 5XX XXX XX XX formatında olmalıdır.")
                .Matches(@"^[0-9\s]+$").WithMessage("Geçerli bir telefon numarası giriniz.");

            // --- DURUM 1: Soyad Doluysa (Şahıs Müşterisi) ---
            When(m => !string.IsNullOrWhiteSpace(m.MUST_SOYAD), () =>
            {
                // TC Kimlik No zorunlu, 11 hane ve sadece rakam
                RuleFor(m => m.MUST_KIMLIK_NO)
                    .NotEmpty().WithMessage("Soyad girildiğinde TC Kimlik No girilmesi zorunludur.")
                    .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
                    .Matches(@"^[0-9]+$").WithMessage("TC Kimlik No sadece rakamlardan oluşmalıdır.");

                // VKN boş kalmalı
                RuleFor(m => m.MUST_VKN_NO)
                    .Empty().WithMessage("Soyad girildiğinde Vergi Kimlik Numarası boş bırakılmalıdır.");
            });

            // --- DURUM 2: Soyad Boşsa (Kurumsal Müşteri) ---
            When(m => string.IsNullOrWhiteSpace(m.MUST_SOYAD), () =>
            {
                // VKN zorunlu, 10 hane ve sadece rakam
                RuleFor(m => m.MUST_VKN_NO)
                    .NotEmpty().WithMessage("Soyad girilmediğinde Vergi Kimlik Numarası zorunludur.")
                    .Length(10).WithMessage("Vergi Kimlik Numarası 10 haneli olmalıdır.")
                    .Matches(@"^[0-9]+$").WithMessage("Vergi Kimlik Numarası sadece rakamlardan oluşmalıdır.");

                // TC Kimlik No boş kalmalı
                RuleFor(m => m.MUST_KIMLIK_NO)
                    .Empty().WithMessage("Soyad girilmediğinde TC Kimlik No boş bırakılmalıdır.");
            });
        }
    }
}