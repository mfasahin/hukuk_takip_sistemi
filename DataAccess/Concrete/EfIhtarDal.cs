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
        // Listeleme (Index) için
        public List<IhtarDto> GetIhtarWithRelations()
        {
            using (var context = new AppDbContext())
            {
                var result = from i in context.IHTAR
                             join m in context.MUSTERI on i.MUSTERI_ID equals m.MUSTERI_ID
                             join a in context.AVUKAT on i.AVUKAT_ID equals a.AVUKAT_ID
                             join s in context.SUBE on i.SUBE_ID equals s.SUBE_ID
                             where i.SIL_TAR_ZMN == null
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

                                 Urunler = (from iu in context.IHTAR_URUN
                                            join u in context.URUN on iu.URUN_ID equals u.URUN_ID
                                            where iu.IHTAR_ID == i.IHTAR_ID
                                            select new UrunDto
                                            {
                                                UrunId = u.URUN_ID,
                                                UrunAd = u.URUN_AD
                                            }).ToList()
                             };

                return result.ToList();
            }
        }

        // Tekil kayıt (GetIhtar modal doldurma) için — Urunler dahil
        public IhtarDto GetByIdWithRelations(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var result = from i in context.IHTAR
                             join m in context.MUSTERI on i.MUSTERI_ID equals m.MUSTERI_ID
                             join a in context.AVUKAT on i.AVUKAT_ID equals a.AVUKAT_ID
                             join s in context.SUBE on i.SUBE_ID equals s.SUBE_ID
                             where i.IHTAR_ID == id
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

                                 Urunler = (from iu in context.IHTAR_URUN
                                            join u in context.URUN on iu.URUN_ID equals u.URUN_ID
                                            where iu.IHTAR_ID == i.IHTAR_ID
                                            select new UrunDto
                                            {
                                                UrunId = u.URUN_ID,
                                                UrunAd = u.URUN_AD
                                            }).ToList()
                             };

                var dto = result.FirstOrDefault();
                if (dto != null)
                    dto.SecilenUrunler = dto.Urunler.Select(u => u.UrunId).ToList();

                return dto;
            }
        }

        // Update için: ham entity + IhtarUrunler koleksiyonu yüklü (senkronizasyon amaçlı)
        public Ihtar GetEntityWithUrunlerIncluded(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var ihtar = context.IHTAR.FirstOrDefault(i => i.IHTAR_ID == id);
                if (ihtar == null) return null;

                var urunler = context.IHTAR_URUN
                    .Where(iu => iu.IHTAR_ID == id)
                    .ToList();

                context.Entry(ihtar)
                       .Collection(i => i.IhtarUrunler)
                       .IsLoaded = true;

                return ihtar;
            }
        }
    }
}