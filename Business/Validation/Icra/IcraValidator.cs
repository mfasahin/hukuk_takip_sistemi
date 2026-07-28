using Entity.Dto;
using FluentValidation;
using System;

namespace Business.Validation
{
    public class IcraValidator : AbstractValidator<IcraDto>
    {
        public IcraValidator()
        {
            // 1. Müşteri Seçimi
            RuleFor(x => x.MusteriId)
                .NotEmpty().WithMessage("Lütfen bir müşteri seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir müşteri seçiniz.");

            // 2. Ürün Seçimi
            RuleFor(x => x.UrunId)
                .NotEmpty().WithMessage("Lütfen bir ürün seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir ürün seçiniz.");

            // 3. İhtar Seçimi
            RuleFor(x => x.IhtarUrunId)
                .NotEmpty().WithMessage("Lütfen bir ihtar seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir ihtar seçiniz.");

            // 4. Mahkeme Seçimi
            RuleFor(x => x.MahkemeId) // Eğer Mahkeme String ise: RuleFor(x => x.Mahkeme).NotEmpty()...
                .NotEmpty().WithMessage("Lütfen bir mahkeme seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir mahkeme seçiniz.");

            // 5. İcra Dosya No
            RuleFor(x => x.IcraDosyaNo)
                .NotEmpty().WithMessage("İcra dosya numarası boş bırakılamaz.");

            // 6. İcra Takip Tarihi
            RuleFor(x => x.IcraTakipTar)
                .NotEmpty().WithMessage("İcra takip tarihi boş bırakılamaz.");
        }
    }
}