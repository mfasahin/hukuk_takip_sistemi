var urunOptions = {
    validationRules: {
        "UrunAd": function (value) {
            if (!value || value.trim() === "") {
                return "Ürün adı boş bırakılamaz.";
            }
            if (value.length > 25) {
                return "Ürün adı en fazla 25 karakter olabilir.";
            }
            return null;
        },
        "UrunKod": function (value) {
            if (!value || value.trim() === "") {
                return "Ürün kodu boş bırakılamaz.";
            }
            if (value.length > 5) {
                return "Ürün kodu en fazla 5 karakter olabilir.";
            }
            return null;
        },
        "SonGecerlilikTar": function (value) {
            if (!value || value.trim() === "") {
                return "Son geçerlilik tarihi boş bırakılamaz.";
            }

            var secilenTarih = new Date(value);
            var bugun = new Date();

            // Saat/dakika farklarını sıfırlayıp sadece gün bazlı karşılaştırıyoruz
            bugun.setHours(0, 0, 0, 0);
            secilenTarih.setHours(0, 0, 0, 0);

            if (isNaN(secilenTarih.getTime())) {
                return "Geçerli bir tarih giriniz.";
            }

            if (secilenTarih < bugun) {
                return "Son geçerlilik tarihi bugünden önce bir tarih olamaz.";
            }

            return null;
        }
    }
};

window.crudOptions = urunOptions;