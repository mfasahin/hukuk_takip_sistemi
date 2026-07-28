using Entity.Concrete;
using FluentValidation;
using System;

public class UrunValidator : AbstractValidator<Urun>
{
    public UrunValidator()
    {
        // 1. Ürün Adı Doğrulaması (NVARCHAR(25))
        RuleFor(x => x.URUN_AD)
            .NotEmpty().WithMessage("Ürün adı boş bırakılamaz.")
            .MaximumLength(25).WithMessage("Ürün adı en fazla 25 karakter olabilir.");

        // 2. Ürün Kodu Doğrulaması (VARCHAR(5))
        RuleFor(x => x.URUN_KOD)
            .NotEmpty().WithMessage("Ürün kodu boş bırakılamaz.")
            .MaximumLength(5).WithMessage("Ürün kodu en fazla 5 karakter olabilir.");

        // 3. Son Geçerlilik Tarihi Doğrulaması (DATETIME)
        RuleFor(x => x.SON_GECERLILIK_TAR)
            .NotEmpty().WithMessage("Son geçerlilik tarihi boş bırakılamaz.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Son geçerlilik tarihi bugünden önce bir tarih olamaz.");
    }
}