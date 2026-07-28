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
    public class IcraManager : IIcraService
    {
        private readonly IIcraDal _icraDal;
        private readonly IcraValidator _validator = new IcraValidator();

        public IcraManager(IIcraDal icraDal)
        {
            _icraDal = icraDal;
        }
        public ValidationResult Validate(IcraDto icraDto)
        {
            return _validator.Validate(icraDto);
        }
        public Icra GetById(Guid id)
        {
            return _icraDal.Get(i => i.ICRA_ID == id);
        }

        public void Add(IcraDto icraDto)
        {
            // 1. FluentValidation Çalıştırma (IcraDto üzerinden doğrulanır)
            ValidationResult result = _validator.Validate(icraDto);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. DTO'dan Entity Dönüşümü
            // İcra kaydı doğrudan ara tablo olan IhtarUrunId ilişkisini barındırır.
            var icra = new Icra
            {
                ICRA_ID = Guid.NewGuid(),
                IHTAR_URUN_ID = icraDto.IhtarUrunId,
                MAHKEME_ID = icraDto.MahkemeId,
                ICRA_DOSYA_NO = icraDto.IcraDosyaNo,
                ICRA_TAKIP_TAR = icraDto.IcraTakipTar
            };

            // 3. İcra Kaydı Ekleme
            _icraDal.Add(icra);
        }

        public void Update(Icra icra)
        {
            _icraDal.Update(icra);
        }

        public void Delete(Icra icra)
        {
            _icraDal.Delete(icra);
        }
        public List<IcraDto> GetIcraDto()
        {
            return _icraDal.GetIcraDto();
        }

        public IcraDto GetByIdIcra(Guid id)
        {
            return _icraDal.GetByIdIcra(id);
        }
        public List<UrunDto> GetUrunlerByMusteri(Guid musteriId)
        {
            return _icraDal.GetUrunlerByMusteri(musteriId);
        }
        public List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId)
        {
            return _icraDal.GetIhtarlarByMusteriVeUrun(musteriId, urunId);
        }

        public string AddIcra(Icra icra, Guid musteriId, Guid urunId)
        {

            _icraDal.Add(icra);
            return "İcra başarıyla eklendi.";
        }
        public bool IhtaraBagliIcraVarMi(Guid ihtarId)
        {
            return _icraDal.IhtaraBagliIcraVarMi(ihtarId);
        }
    }
}