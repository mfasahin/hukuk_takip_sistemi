var musteriOptions = {
    // KİTİT NOKTA: Otomatik müşteri no endpoint'i buraya eklenmeli!
    getNextNoUrl: '/Musteri/GetNextMusteriNo',

    validationRules: {
        "MustNo": function (value) {
            if (!value || value.trim() === "") return "Müşteri numarası boş olamaz.";
            return null;
        },
        "MustAd": function (value) {
            if (!value || value.trim() === "") return "Müşteri adı boş bırakılamaz.";
            return null;
        },
        "MustSoyad": function (value, allValues) {
            if (value && value.trim() !== "" && !/^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$/.test(value)) {
                return "Müşteri soyadı sadece harflerden oluşmalıdır.";
            }
            return null;
        },
        "MustKimlikNo": function (value, allValues) {
            var soyadDolu = allValues.MustSoyad && allValues.MustSoyad.trim() !== "";
            var tcDolu = value && value.trim() !== "";

            if (soyadDolu && !tcDolu) return "Soyad girildiğinde TC Kimlik No zorunludur.";
            if (!soyadDolu && tcDolu) return "TC Kimlik No boş bırakılmalıdır.";

            if (tcDolu) {
                if (!/^[0-9]+$/.test(value)) return "TC Kimlik No sadece rakamlardan oluşmalıdır.";
                if (value.length !== 11) return "TC Kimlik No 11 haneli olmalıdır.";
                if (value[0] === '0') return "TC Kimlik No 0 ile başlayamaz.";

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

            if (!soyadDolu && !vknDolu) return "Vergi Kimlik No zorunludur.";
            if (soyadDolu && vknDolu) return "Vergi Kimlik No boş bırakılmalıdır.";

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
            return null;
        }
    }
};

window.crudOptions = musteriOptions;