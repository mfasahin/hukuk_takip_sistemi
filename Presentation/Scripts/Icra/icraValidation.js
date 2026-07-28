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
        "IhtarUrunId": function (value) {
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir ihtar seçiniz.";
            }
            return null;
        },
        "MahkemeId": function (value) {
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir mahkeme seçiniz.";
            }
            return null;
        },
        // Hem name hem ID eşleşmesi için alternatifli Regex kontrolü
        "IcraDosyaNo": checkIcraDosyaNoFormat,
        "createIcraDosyaNo": checkIcraDosyaNoFormat,

        "IcraTakipTar": function (value) {
            if (!value || value.toString().trim() === "") {
                return "İcra takip tarihi boş bırakılamaz.";
            }
            return null;
        }
    }
};

function checkIcraDosyaNoFormat(value) {
    if (!value || value.trim() === "") {
        return "İcra dosya numarası boş bırakılamaz.";
    }
    var dosyaNoRegex = /^(19|20)\d{2}\/\d{1,7}\s?[eE]\.$/;
    if (!dosyaNoRegex.test(value.trim())) {
        return "Format hatalı! Doğru Format Örn: 2026/1234 E.";
    }
    return null;
}

window.icraOptions = icraOptions;
window.crudOptions = icraOptions;