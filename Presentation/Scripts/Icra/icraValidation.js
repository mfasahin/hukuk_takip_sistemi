var icraOptions = {
    validationRules: {
        "MusteriId": function (value) {
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir müşteri seçiniz.";
            }
            return null;
        },
        "UrunId": function (value) {
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir ürün seçiniz.";
            }
            return null;
        },
        "IhtarUrunId": function (value) { // Veya DTO/Modal yapınıza göre "IhtarId"
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir ihtar seçiniz.";
            }
            return null;
        },
        "MahkemeId": function (value) { // Eğer Mahkeme serbest metin/string ise bu kuralın ilk if kontrolü yeterlidir
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir mahkeme seçiniz.";
            }
            return null;
        },
        "IcraDosyaNo": function (value) {
            if (!value || value.trim() === "") {
                return "İcra dosya numarası boş bırakılamaz.";
            }
            return null;
        },
        "IcraTakipTarih": function (value) { // Veya DTO/Modal yapınıza göre "IcraTakipTarihi"
            if (!value || value.trim() === "") {
                return "İcra takip tarihi boş bırakılamaz.";
            }
            var date = new Date(value);
            if (isNaN(date.getTime())) {
                return "Geçerli bir icra takip tarihi giriniz.";
            }
            return null;
        }
    }
};

window.crudOptions = icraOptions;