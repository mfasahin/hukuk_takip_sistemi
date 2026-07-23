using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IIcraService
    {
        
        Icra GetById(Guid id);
        void Add(Icra icra);
        void Update(Icra icra);
        List<IcraDto> GetIcraDto();
        IcraDto GetByIdIcra(Guid id);
        List<UrunDto> GetUrunlerByMusteri(Guid musteriId);
        List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId);

    }
}