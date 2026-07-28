using Core.DataAccess;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IIcraDal : IEntityRepository<Icra>
    {
        List<IcraDto> GetIcraDto();
        IcraDto GetByIdIcra(Guid id);
        List<MusteriDto> GetIhtariOlanMusteriler();
        // isForUpdate = false varsayılan parametresi eklendi
        List<UrunDto> GetUrunlerByMusteri(Guid musteriId, bool isForUpdate = false);
        List<IhtarUrunDto> GetIhtarlarByMusteriVeUrun(Guid musteriId, Guid urunId);

        bool IhtaraBagliIcraVarMi(Guid ihtarUrunId);
    }
}