using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete
{
    public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
    {
        // Ortak DTO sorgusu - hem liste hem tekil kayıt için kullanılır.
        private IQueryable<IhtarDto> IhtarDtoQuery(AppDbContext context)
        {
            return from i in context.IHTAR
                   join m in context.MUSTERI on i.MUSTERI_ID equals m.MUSTERI_ID
                   join a in context.AVUKAT on i.AVUKAT_ID equals a.AVUKAT_ID
                   join s in context.SUBE on i.SUBE_ID equals s.SUBE_ID
                   join ih in context.IHTAR_URUN on i.IHTAR_ID equals ih.IHTAR_ID
                   join u in context.URUN on ih.URUN_ID equals u.URUN_ID
                   select new IhtarDto 
                   {
                       IhtarId = i.IHTAR_ID,
                       BorcTutar = i.BORC_TUTAR,
                       IhtarTarih = i.IHTAR_TAR_ZMN,

                       MusteriId = m.MUSTERI_ID,
                       MusteriNo = m.MUST_NO,
                       MusteriAd = m.MUST_AD,
                       MusteriSoyad = m.MUST_SOYAD,

                       SubeId = s.SUBE_ID,
                       SubeAd = s.SUBE_ADI,
                       SubeKod = s.SUBE_KODU,

                       AvukatId = a.AVUKAT_ID,
                       AvukatAd = a.AVKT_AD,
                       AvukatSoyad = a.AVKT_SOYAD,

                       SilTarZmn = i.SIL_TAR_ZMN,

                       UrunAd = u.URUN_AD
                   };
        }


        // silinmemiş ihtar bilgileri DTO listesi olarak döner
        public List<IhtarDto> GetIhtarDto()
        {
            using (var context = new AppDbContext())
            {
                return IhtarDtoQuery(context)
                    .Where(dto => dto.SilTarZmn == null)
                    .ToList();
            }
        }

        public IhtarDto GetByIdIhtarDto(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var dto = IhtarDtoQuery(context)
                    .FirstOrDefault(x => x.IhtarId == id);

                if (dto != null)
                {
                    // İhtara bağlı ürün ilişkisini bul
                    var ihtarUrun = context.IHTAR_URUN
                        .FirstOrDefault(iu => iu.IHTAR_ID == id);

                    if (ihtarUrun != null)
                    {
                        dto.UrunId = ihtarUrun.URUN_ID;
                    }
                }

                return dto;
            }
        }

        // GERÇEK İMPLEMENTASYON - tüm güncelleme işlemi tek context içinde
        public void UpdateIhtarWithUrunler(IhtarDto model)
        {
            using (var context = new AppDbContext())
            {
                var ihtar = context.IHTAR.FirstOrDefault(i => i.IHTAR_ID == model.IhtarId);
                if (ihtar == null)
                    throw new Exception("Kayıt bulunamadı");

                // İhtar alanlarını güncelle
                ihtar.BORC_TUTAR = model.BorcTutar;
                ihtar.IHTAR_TAR_ZMN = model.IhtarTarih;
                ihtar.MUSTERI_ID = model.MusteriId;
                ihtar.AVUKAT_ID = model.AvukatId;
                ihtar.SUBE_ID = model.SubeId;
                ihtar.GNC_TAR_ZMN = DateTime.Now;

                // Mevcut ürün ilişkisini bul
                var mevcutIhtarUrun = context.IHTAR_URUN
                    .FirstOrDefault(iu => iu.IHTAR_ID == model.IhtarId);

                // Eğer ürün seçilmişse güncelle/ekle
                if (model.UrunId != Guid.Empty)
                {
                    if (mevcutIhtarUrun != null)
                    {
                        // Güncelle
                        mevcutIhtarUrun.URUN_ID = model.UrunId;
                        mevcutIhtarUrun.GNC_TAR_ZMN = DateTime.Now;
                    }
                    else
                    {
                        // Yeni ekle
                        context.IHTAR_URUN.Add(new IhtarUrun
                        {
                            IHTAR_URUN_ID = Guid.NewGuid(),
                            IHTAR_ID = ihtar.IHTAR_ID,
                            URUN_ID = model.UrunId,
                            GRS_TAR_ZMN = DateTime.Now
                        });
                    }
                }
                else
                {
                    // Ürün seçilmemişse mevcut ilişkiyi kaldır
                    if (mevcutIhtarUrun != null)
                        context.IHTAR_URUN.Remove(mevcutIhtarUrun);
                }

                context.SaveChanges();
            }
        }

    }
}