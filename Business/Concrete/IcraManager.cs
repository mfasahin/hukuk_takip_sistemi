using Business.Abstract;
using Business.Validation;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dto;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _icraDal.Get(i => i.ICRA_ID == id && i.SIL_TAR_ZMN == null);
        }

        public IcraDto GetByIdIcra(Guid id)
        {
            return _icraDal.GetByIdIcra(id);
        }

        public List<IcraDto> GetIcraDto()
        {
            return _icraDal.GetIcraDto();
        }

        public void Add(IcraDto icraDto)
        {
            // 1. FluentValidation Doğrulaması
            ValidationResult result = _validator.Validate(icraDto);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors);
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. DTO -> Entity Dönüşümü
            var icra = new Icra
            {
                ICRA_ID = Guid.NewGuid(),
                IHTAR_URUN_ID = icraDto.IhtarUrunId,
                MAHKEME_ID = icraDto.MahkemeId,
                ICRA_DOSYA_NO = icraDto.IcraDosyaNo,
                ICRA_TAKIP_TAR = icraDto.IcraTakipTar,
                GRS_TAR_ZMN = DateTime.Now
            };

            // 3. Veritabanına Ekleme
            _icraDal.Add(icra);
        }

        public void Update(IcraDto icraDto)
        {
            // 1. FluentValidation Doğrulaması
            // Not: IcraValidator'da MusteriId/UrunId kuralları varsa sadece IhtarUrunId, Mahkeme, DosyaNo ve Tarih için validation çalıştırmak gerekebilir.
            ValidationResult result = _validator.Validate(icraDto);

            if (!result.IsValid)
            {
                string errorMessages = string.Join("\n", result.Errors.Select(e => e.ErrorMessage));
                throw new Exception($"Doğrulama Hataları:\n{errorMessages}");
            }

            // 2. Mevcut Entity'yi veritabanından çek
            var existingIcra = _icraDal.Get(i => i.ICRA_ID == icraDto.IcraId && i.SIL_TAR_ZMN == null);
            if (existingIcra == null)
            {
                throw new Exception("Güncellenecek icra kaydı bulunamadı.");
            }

            // 3. Hidden alandan gelen veya var olan IhtarUrunId aktarımı
            if (icraDto.IhtarUrunId != Guid.Empty)
            {
                existingIcra.IHTAR_URUN_ID = icraDto.IhtarUrunId;
            }

            // 4. Güncellenebilir alanların atanması
            existingIcra.MAHKEME_ID = icraDto.MahkemeId;
            existingIcra.ICRA_DOSYA_NO = icraDto.IcraDosyaNo;
            existingIcra.ICRA_TAKIP_TAR = icraDto.IcraTakipTar;
            existingIcra.GNC_TAR_ZMN = DateTime.Now;

            // 5. Veritabanında güncelle
            _icraDal.Update(existingIcra);
        }

        public void Delete(Guid id)
        {
            var icra = _icraDal.Get(i => i.ICRA_ID == id);
            if (icra != null)
            {
                // Soft Delete yapısı
                icra.SIL_TAR_ZMN = DateTime.Now;
                _icraDal.Update(icra);
            }
        }

        // 1. Kademe: Müşteriye ait ürünler (isForUpdate varsayılan false kabul eder)
        public List<UrunDto> GetUrunlerByMusteri(Guid musteriId, bool isForUpdate = false)
        {
            return _icraDal.GetUrunlerByMusteri(musteriId, isForUpdate);
        }

        // 2. Kademe: Müşteri ve Ürüne ait İhtarlar
        public List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId)
        {
            return _icraDal.GetIhtarlarByMusteriVeUrun(musteriId, urunId);
        }

        public bool IhtaraBagliIcraVarMi(Guid ihtarId)
        {
            return _icraDal.IhtaraBagliIcraVarMi(ihtarId);
        }
    }
}