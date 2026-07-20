using System;

namespace Entity.Abstract
{
    public abstract class BaseEntity
    {
        public DateTime? GRS_TAR_ZMN { get; set; }
        public Guid? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public Guid? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public Guid? SIL_KULLANICI_ID { get; set; }
    }
}
