var ihtarOptions = {
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
        "BorcTutar": function (value) { // BorcTutari -> BorcTutar yapıldı
            if (value === undefined || value === null || value.toString().trim() === "") {
                return "Borç tutarı boş bırakılamaz.";
            }
            var numericValue = parseFloat(value.toString().replace(",", "."));
            if (isNaN(numericValue)) return "Lütfen geçerli bir borç tutarı giriniz.";
            if (numericValue < 0) return "Borç tutarı negatif olamaz.";
            return null;
        },
        "IhtarTarih": function (value) { // IhtarTarihi -> IhtarTarih yapıldı
            if (!value || value.trim() === "") {
                return "İhtar tarihi boş bırakılamaz.";
            }
            return null;
        },
        "SubeId": function (value) {
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir şube seçiniz.";
            }
            return null;
        },
        "AvukatId": function (value) {
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir avukat seçiniz.";
            }
            return null;
        }
    }
};

window.crudOptions = ihtarOptions;