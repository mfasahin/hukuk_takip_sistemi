using Entity.Concrete;

namespace Business.Abstract
{
    public interface IKullaniciService
    {
        Kullanici GetByUsername(string kullaniciAd);
        Kullanici Login(string kullaniciAd, string sifre);
        (bool success, string error) Register(string kullaniciAd, string sifre);
    }
}