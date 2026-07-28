using Entity.Concrete;
using Entity.Dto;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIcraService
    {
        ValidationResult Validate(IcraDto icraDto);
        Icra GetById(Guid id);
        IcraDto GetByIdIcra(Guid id);
        List<IcraDto> GetIcraDto();

        void Add(IcraDto icraDto);
        void Update(IcraDto icraDto);
        void Delete(Guid id);

        // Kademeli seçim metotları (isForUpdate = false varsayılan parametreli)
        List<UrunDto> GetUrunlerByMusteri(Guid musteriId, bool isForUpdate = false);
        List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId);

        bool IhtaraBagliIcraVarMi(Guid ihtarId);
    }
}