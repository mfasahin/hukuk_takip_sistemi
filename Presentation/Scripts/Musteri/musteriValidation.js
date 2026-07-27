var musteriOptions = {
    validationRules: {
        "MustNo": function (value) {
            if (!value || !/^[0-9]+$/.test(value)) return "Müşteri numarası sadece rakamlardan oluşmalıdır.";
            if (parseInt(value) <= 0) return "Müşteri numarası 0'dan büyük olmalıdır.";
            return null;
        },
        "MustAd": function (value) {
            if (!value || !/^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$/.test(value)) return "Müşteri adı sadece harflerden oluşmalıdır.";
            return null;
        },
        "MustSoyad": function (value) {
            if (value && !/^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$/.test(value)) {
                return "Müşteri soyadı sadece harflerden oluşmalıdır.";
            }
            return null;
        },
        "MustKimlikNo": function (value, allValues) {
            var soyadDolu = allValues.MustSoyad && allValues.MustSoyad.trim() !== "";
            var tcDolu = value && value.trim() !== "";

            if (soyadDolu && !tcDolu) return "Soyad girildiğinde TC Kimlik No zorunludur.";
            if (!soyadDolu && tcDolu) return "Soyad girilmediğinde TC Kimlik No boş bırakılmalıdır.";

            if (tcDolu) {
                if (!/^[0-9]+$/.test(value)) return "TC Kimlik No sadece rakamlardan oluşmalıdır.";
                if (value.length !== 11) return "TC Kimlik No 11 haneli olmalıdır.";

                var toplam = 0;
                for (var i = 0; i < 10; i++) {
                    toplam += parseInt(value[i]);
                }
                var sonRakam = parseInt(value[10]);

                if ((toplam % 10) !== sonRakam) {
                    return "Girdiğiniz TC Kimlik Numarası geçersizdir.";
                }
            }
            return null;
        },
        "MustVknNo": function (value, allValues) {
            var soyadDolu = allValues.MustSoyad && allValues.MustSoyad.trim() !== "";
            var vknDolu = value && value.trim() !== "";

            if (!soyadDolu && !vknDolu) return "Soyad girilmediğinde Vergi Kimlik No zorunludur.";
            if (soyadDolu && vknDolu) return "Soyad girildiğinde Vergi Kimlik No boş bırakılmalıdır.";

            if (vknDolu) {
                if (!/^[0-9]+$/.test(value)) return "Vergi Kimlik No sadece rakamlardan oluşmalıdır.";
                if (value.length !== 10) return "Vergi Kimlik No 10 haneli olmalıdır.";
            }
            return null;
        },
        "MustEposta": function (value) {
            if (!value || value.trim() === "") return "E-posta adresi boş geçilemez.";
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) return "Geçerli bir e-posta adresi giriniz.";
            return null;
        },
        "MustTelNo": function (value) {
            if (!value || value.trim() === "") return "Telefon numarası boş bırakılamaz.";
            if (value.length !== 13) return "Telefon numarası 5XX XXX XX XX formatında 10 haneli olmalıdır.";
            return null;
        }
    }
};

window.crudOptions = musteriOptions;