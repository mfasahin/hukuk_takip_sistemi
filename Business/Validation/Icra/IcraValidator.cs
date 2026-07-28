using Entity.Dto;
using FluentValidation;
using System;

namespace Business.Validation
{
    public class IcraValidator : AbstractValidator<IcraDto>
    {
        public IcraValidator()
        {
            // SADECE YENİ KAYIT (CREATE) ESNASINDA MÜŞTERİ VE ÜRÜN ZORUNLU OLSUN:
            When(x => x.IcraId == Guid.Empty, () =>
            {
                RuleFor(x => x.MusteriId)
                    .NotEmpty().WithMessage("Lütfen bir müşteri seçiniz.")
                    .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir müşteri seçiniz.");

                RuleFor(x => x.UrunId)
                    .NotEmpty().WithMessage("Lütfen bir ürün seçiniz.")
                    .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir ürün seçiniz.");
            });

            // HEM CREATE HEM UPDATE İÇİN HER ZAMAN ZORUNLU OLAN ALANLAR:
            RuleFor(x => x.IhtarUrunId)
                .NotEmpty().WithMessage("Lütfen bir ihtar seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir ihtar seçiniz.");

            RuleFor(x => x.MahkemeId)
                .NotEmpty().WithMessage("Lütfen bir mahkeme seçiniz.")
                .NotEqual(Guid.Empty).WithMessage("Lütfen geçerli bir mahkeme seçiniz.");

            RuleFor(x => x.IcraDosyaNo)
                .NotEmpty().WithMessage("İcra dosya numarası boş geçilemez.");

            RuleFor(x => x.IcraTakipTar)
                .NotEmpty().WithMessage("İcra takip tarihi boş geçilemez.");
        }
    }
}