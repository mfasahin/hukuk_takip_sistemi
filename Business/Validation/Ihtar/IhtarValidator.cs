using Entity.Dto;
using FluentValidation;
using System;

namespace Business.Validation
{
    public class IhtarValidator : AbstractValidator<IhtarDto> // veya IhtarModel
    {
        public IhtarValidator()
        {
            // 1. Müşteri Seçimi
            RuleFor(x => x.MusteriId)
                .NotEmpty().WithMessage("Lütfen bir müşteri seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir müşteri seçiniz.");

            // 2. Ürün Seçimi
            RuleFor(x => x.UrunId)
                .NotEmpty().WithMessage("Lütfen bir ürün seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir ürün seçiniz.");

            // 3. Borç Tutarı (0'dan büyük olmalı)
            RuleFor(x => x.BorcTutar)
                .NotNull().WithMessage("Borç tutarı boş bırakılamaz.")
                .GreaterThan(0).WithMessage("Borç tutarı 0'dan büyük olmalıdır.");

            // 4. İhtar Tarihi
            RuleFor(x => x.IhtarTarih)
                .NotEmpty().WithMessage("İhtar tarihi boş bırakılamaz.");

            // 5. Şube Seçimi
            RuleFor(x => x.SubeId)
                .NotEmpty().WithMessage("Lütfen bir şube seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir şube seçiniz.");

            // 6. Avukat Seçimi
            RuleFor(x => x.AvukatId)
                .NotEmpty().WithMessage("Lütfen bir avukat seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir avukat seçiniz.");
        }
    }
}