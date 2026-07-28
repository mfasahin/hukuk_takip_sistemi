var avukatOptions = {
    validationRules: {
        "AvktAd": function (value) {
            if (!value || value.trim() === "") {
                return "Avukat adı boş bırakılamaz.";
            }
            if (value.length > 25) {
                return "Avukat adı en fazla 25 karakter olabilir.";
            }
            if (!/^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$/.test(value)) {
                return "Avukat adı sadece harflerden oluşmalıdır.";
            }
            return null;
        },
        "AvktSoyad": function (value) {
            if (!value || value.trim() === "") {
                return "Avukat soyadı boş bırakılamaz.";
            }
            if (value.length > 25) {
                return "Avukat soyadı en fazla 25 karakter olabilir.";
            }
            if (!/^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$/.test(value)) {
                return "Avukat soyadı sadece harflerden oluşmalıdır.";
            }
            return null;
        },
        "TbbSicilNo": function (value) {
            if (!value || value.trim() === "") {
                return "TBB Sicil No boş bırakılamaz.";
            }
            if (!/^[0-9]+$/.test(value)) {
                return "TBB Sicil No sadece rakamlardan oluşmalıdır.";
            }
            if (value.length > 10) {
                return "TBB Sicil No en fazla 10 karakter olabilir.";
            }
            return null;
        },
        "AvktEposta": function (value) {
            if (!value || value.trim() === "") {
                return "E-posta adresi boş bırakılamaz.";
            }
            if (value.length > 25) {
                return "E-posta adresi en fazla 25 karakter olabilir.";
            }
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                return "Geçerli bir e-posta adresi giriniz.";
            }
            return null;
        },
        "AvktTelNo": function (value) {
            if (!value || value.trim() === "") return "Telefon numarası boş bırakılamaz.";
            if (value.length !== 13) return "Telefon numarası 5XX XXX XX XX formatında 10 haneli olmalıdır.";
            return null;
        },
        "HkkBuroAd": function (value) {
            if (value && value.length > 50) {
                return "Hukuk büro adı en fazla 50 karakter olabilir.";
            }
            return null;
        },
        "HkkBuroAdres": function (value) {
            if (value && value.length > 70) {
                return "Hukuk büro adresi en fazla 70 karakter olabilir.";
            }
            return null;
        },
        "OfisTelNo": function (value) {
            if (!value || value.trim() === "") return "Telefon numarası boş bırakılamaz.";
            if (value.length !== 13) return "Telefon numarası 5XX XXX XX XX formatında 10 haneli olmalıdır.";
            return null;
        }
    }
};

window.crudOptions = avukatOptions;