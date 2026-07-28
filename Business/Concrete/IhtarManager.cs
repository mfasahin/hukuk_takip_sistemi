using Business.Abstract;
using Business.Validation;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class IhtarManager : IIhtarService
    {
        private readonly IIhtarDal _ihtarDal;
        private readonly IIhtarUrunDal _ihtarUrunDal;
        private readonly IhtarValidator _validator = new IhtarValidator();

        public IhtarManager(IIhtarDal ihtarDal, IIhtarUrunDal ihtarUrunDal)
        {
            _ihtarDal = ihtarDal;
            _ihtarUrunDal = ihtarUrunDal;
        }

        public ValidationResult Validate(IhtarDto ihtarDto)
        {
            return _validator.Validate(ihtarDto);
        }

        public List<Ihtar> GetAll()
        {
            return _ihtarDal.GetAll();
        }

        public void Add(IhtarDto ihtarDto)
        {
            // 1. FluentValidation Çalıştırma (IhtarDto üzerinden doğrulanır)
            ValidationResult result = _validator.Validate(ihtarDto);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. Ekstra İş Kuralları (Borç tutarı negatiflik kontrolü)
            if (ihtarDto.BorcTutar < 0)
            {
                throw new Exception("Borç tutarı negatif olamaz.");
            }

            // 3. DTO'dan Entity Dönüşümü
            var ihtar = new Ihtar
            {
                IHTAR_ID = Guid.NewGuid(),
                MUSTERI_ID = ihtarDto.MusteriId,
                BORC_TUTAR = ihtarDto.BorcTutar,
                IHTAR_TAR_ZMN = ihtarDto.IhtarTarih,
                SUBE_ID = ihtarDto.SubeId,
                AVUKAT_ID = ihtarDto.AvukatId
            };

            // 4. İhtar Kaydı Ekleme
            _ihtarDal.Add(ihtar);

            // 5. İhtar ile Ürün Arasındaki İlişkiyi IhtarUrun Tablosuna Ekleme
            var ihtarUrun = new IhtarUrun
            {
                IHTAR_URUN_ID = Guid.NewGuid(),
                IHTAR_ID = ihtar.IHTAR_ID,
                URUN_ID = ihtarDto.UrunId
            };

            _ihtarUrunDal.Add(ihtarUrun);
        }

        public void Update(Ihtar ihtar)
        {
            _ihtarDal.Update(ihtar);
        }

        public void Delete(Ihtar ihtar)
        {
            _ihtarDal.Delete(ihtar);
        }

        public Ihtar GetById(Guid id)
        {
            return _ihtarDal.Get(Ihtar => Ihtar.IHTAR_ID == id);
        }
       
        public List<IhtarDto> GetIhtarDto()
        {
            return _ihtarDal.GetIhtarDto();
        }
        public IhtarDto GetByIdIhtarDto(Guid id)
        {
            return _ihtarDal.GetByIdIhtarDto(id);
        }
        public bool MusteriyeBagliIhtarVarMi(Guid musteriId)
        {
            return _ihtarDal.MusteriyeBagliIhtarVarMi(musteriId);
        }
        public bool AvukataBagliIhtarVarMi(Guid avukatId)
        {
            return _ihtarDal.AvukataBagliIhtarVarMi(avukatId);
        }
    }
}