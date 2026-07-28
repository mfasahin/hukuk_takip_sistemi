using Entity.Concrete;
using FluentValidation;

namespace Business.Validation
{
    public class AvukatValidator : AbstractValidator<Avukat>
    {
        public AvukatValidator()
        {
            // 1. Avukat Adı (NVARCHAR(25))
            RuleFor(a => a.AVKT_AD)
                .NotEmpty().WithMessage("Avukat adı boş bırakılamaz.")
                .MaximumLength(25).WithMessage("Avukat adı en fazla 25 karakter olabilir.");

            // 2. Avukat Soyadı (NVARCHAR(25))
            RuleFor(a => a.AVKT_SOYAD)
                .NotEmpty().WithMessage("Avukat soyadı boş bırakılamaz.")
                .MaximumLength(25).WithMessage("Avukat soyadı en fazla 25 karakter olabilir.");

            // 3. TBB Sicil No (VARCHAR(10) - Sadece Rakam)
            RuleFor(a => a.TBB_SICIL_NO)
                .NotEmpty().WithMessage("TBB Sicil No boş bırakılamaz.")
                .MaximumLength(10).WithMessage("TBB Sicil No en fazla 10 karakter olabilir.")
                .Matches(@"^[0-9]+$").WithMessage("TBB Sicil No sadece rakamlardan oluşmalıdır.");

            // 4. E-posta (VARCHAR(25))
            RuleFor(a => a.AVKT_EPOSTA)
                .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
                .MaximumLength(25).WithMessage("E-posta adresi en fazla 25 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            // 5. Telefon Numarası (VARCHAR(15))
            RuleFor(a => a.AVKT_TEL_NO)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.")
                .MaximumLength(15).WithMessage("Telefon numarası en fazla 15 karakter olabilir.")
                .Matches(@"^[0-9\s]+$").WithMessage("Geçerli bir telefon numarası giriniz.");

            // 6. Hukuk Büro Adı (NVARCHAR(50) - Opsiyonel ise kaldırılabilecek NotEmpty)
            RuleFor(a => a.HKK_BURO_AD)
                .MaximumLength(50).WithMessage("Hukuk büro adı en fazla 50 karakter olabilir.");

            // 7. Hukuk Büro Adresi (NVARCHAR(70))
            RuleFor(a => a.HKK_BURO_ADRES)
                .MaximumLength(70).WithMessage("Hukuk büro adresi en fazla 70 karakter olabilir.");

            // 8. Ofis Telefon Numarası (VARCHAR(15))
            RuleFor(a => a.OFIS_TEL_NO)
                .MaximumLength(15).WithMessage("Ofis telefon numarası en fazla 15 karakter olabilir.")
                .Matches(@"^[0-9\s]*$").WithMessage("Geçerli bir ofis telefon numarası giriniz.");
        }
    }
}