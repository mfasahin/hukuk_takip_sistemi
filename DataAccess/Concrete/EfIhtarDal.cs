using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


public class EfIhtarDal : EfEntityRepositoryBase<Ihtar, AppDbContext>, IIhtarDal
{
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
                             MusteriAd = m.MUST_AD + " " + m.MUST_SOYAD,
                             AvukatId = a.AVUKAT_ID,
                             AvukatAd = a.AVKT_AD,
                             SubeId = s.SUBE_ID,
                             SubeAd = s.SUBE_ADI,

                             // ürün: ilişkili ilk IhtarUrun kaydından çekiliyor
                             UrunId = context.IHTAR_URUN
                                 .Where(iu => iu.IHTAR_ID == i.IHTAR_ID)
                                 .Select(iu => (int?)iu.URUN_ID)
                                 .FirstOrDefault() ?? 0,

                             UrunAd = (from iu in context.IHTAR_URUN
                                       join u in context.URUN on iu.URUN_ID equals u.URUN_ID
                                       where iu.IHTAR_ID == i.IHTAR_ID
                                       select u.URUN_AD).FirstOrDefault() ?? string.Empty
                         };

            return result.ToList();
        }
    }

    public Ihtar GetByIdWithRelations(int id)
    {
        using (var context = new AppDbContext())
        {
            // 1) Ana kayıt + tekil ilişkiler (Musteri, Avukat, Sube)
            var result = (from i in context.IHTAR
                          join m in context.MUSTERI on i.MUSTERI_ID equals m.MUSTERI_ID
                          join a in context.AVUKAT on i.AVUKAT_ID equals a.AVUKAT_ID
                          join s in context.SUBE on i.SUBE_ID equals s.SUBE_ID
                          where i.IHTAR_ID == id
                          select new { Ihtar = i, Musteri = m, Avukat = a, Sube = s })
                          .FirstOrDefault();

            if (result == null)
                return null;

            // 2) IhtarUrunler koleksiyonu + her birinin Urun'u
            var urunler = (from iu in context.IHTAR_URUN
                           join u in context.URUN on iu.URUN_ID equals u.URUN_ID
                           where iu.IHTAR_ID == id
                           select new { IhtarUrun = iu, Urun = u })
                           .ToList();

            // 3) KRİTİK: fixup veriyi bağladı ama "yüklendi" olarak işaretlemedi.
            //    Bunu context kapanmadan ÖNCE manuel yapmazsanız, dışarıda
            //    IhtarUrunler'e erişildiğinde EF tekrar DB'ye gitmeye çalışır ve patlar.
            context.Entry(result.Ihtar)
                   .Collection(i => i.IhtarUrunler)
                   .IsLoaded = true;

            // Her IhtarUrun'un içindeki Urun referansı da (tekil ilişki) genelde
            // otomatik IsLoaded=true olur, ama garantiye almak için elle de işaretleyebilirsiniz:
            foreach (var x in urunler)
            {
                context.Entry(x.IhtarUrun)
                       .Reference(iu => iu.Urun)
                       .IsLoaded = true;
            }

            return result.Ihtar;
        }
    }
}