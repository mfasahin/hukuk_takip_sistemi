// Business/Concrete/KullaniciManager.cs
using Business.Abstract;
using Core.Security;
using DataAccess.Abstract;
using Entity.Concrete;
using System;

namespace Business.Concrete
{
    public class KullaniciManager : IKullaniciService
    {
        private readonly IKullaniciDal _kullaniciDal;

        public KullaniciManager(IKullaniciDal kullaniciDal)
        {
            _kullaniciDal = kullaniciDal;
        }

        public Kullanici GetByUsername(string kullaniciAd)
        {
            return _kullaniciDal.GetByUsername(kullaniciAd);
        }

        public Kullanici Login(string kullaniciAd, string sifre)
        {
            var kullanici = _kullaniciDal.GetByUsername(kullaniciAd);
            if (kullanici == null) return null;

            bool sifreDogru = PasswordHasher.Verify(sifre, kullanici.SIFRE);
            return sifreDogru ? kullanici : null;
        }

        public (bool success, string error) Register(string kullaniciAd, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAd) || string.IsNullOrWhiteSpace(sifre))
                return (false, "Kullanıcı adı ve şifre zorunludur.");

            var mevcut = _kullaniciDal.GetByUsername(kullaniciAd);
            if (mevcut != null)
                return (false, "Bu kullanıcı adı zaten kullanılıyor.");

            var yeniKullanici = new Kullanici
            {
                KULLANICI_ID = Guid.NewGuid(),
                KULLANICI_AD = kullaniciAd,
                SIFRE = PasswordHasher.Hash(sifre),
                GRS_TAR_ZMN = DateTime.Now
            };

            _kullaniciDal.Add(yeniKullanici);

            return (true, null);
        }
    }
}