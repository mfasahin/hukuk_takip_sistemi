using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class KullaniciModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string KullaniciAd { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }

    public class KullaniciRegisterModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(25, ErrorMessage = "Kullanıcı adı en fazla 25 karakter olabilir")]
        public string KullaniciAd { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor")]
        public string SifreTekrar { get; set; }
    }
}