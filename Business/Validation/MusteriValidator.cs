//using Entity.Concrete;
//using FluentValidation;

//namespace Business.Validation
//{
//    public class MusteriValidator : AbstractValidator<Musteri>
//    {
//        public MusteriValidator()
//        {
//            RuleFor(x => x.MUST_AD)
//                .NotEmpty().WithMessage("Ad zorunludur.")
//                .MaximumLength(25).WithMessage("Ad en fazla 25 karakter olabilir.");

//            RuleFor(x => x.MUST_SOYAD)
//                .NotEmpty().WithMessage("Soyad zorunludur.");

//            RuleFor(x => x.MUST_EPOSTA)
//                .NotEmpty().WithMessage("E-posta zorunludur.")
//                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

//            RuleFor(x => x.MUST_KIMLIK_NO)
//                .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
//                .When(x => !string.IsNullOrEmpty(x.MUST_KIMLIK_NO));   // opsiyonel alan, doluysa kontrol et
//        }
//    }
//}