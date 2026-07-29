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

var icraOptions = {
    validationSkipFields: [],

    validationRules: {
        "MusteriId": function (value, allValues) {
            // Müşteri seçili değilse uyarısını ver
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir müşteri seçiniz.";
            }
            return null;
        },
        "createMusteriSelect": function (value, allValues) {
            if (!value || value.trim() === "" || value === "0") {
                return "Lütfen bir müşteri seçiniz.";
            }
            return null;
        },
        "UrunId": function (value, allValues) {
            // DOM'dan aktif formun Müşteri değerini canlı oku
            var $activeForm = $("#createForm:visible, #updateForm:visible");
            var musteriVal = $activeForm.find("[name='MusteriId'], #createMusteriSelect, #updateMusteriSelect").val();

            // Müşteri henüz seçilmemişse Ürün uyarısını KESİNLİKLE gösterme!
            if (!musteriVal || musteriVal.trim() === "" || musteriVal === "0" || musteriVal === "00000000-0000-0000-0000-000000000000") {
                return null;
            }

            // Müşteri seçilmiş ama ürün seçilmemişse göster
            if (!value || value.trim() === "" || value === "0" || value === "00000000-0000-0000-0000-000000000000") {
                return "Lütfen bir ürün seçiniz.";
            }
            return null;
        },
        "IhtarUrunId": function (value, allValues) {
            // DOM'dan aktif formun Ürün değerini canlı oku
            var $activeForm = $("#createForm:visible, #updateForm:visible");
            var urunVal = $activeForm.find("[name='UrunId'], #createUrunSelect, #updateUrunSelect").val();

            // Ürün henüz seçilmemişse İhtar uyarısını KESİNLİKLE gösterme!
            if (!urunVal || urunVal.trim() === "" || urunVal === "0" || urunVal === "00000000-0000-0000-0000-000000000000") {
                return null;
            }

            // Ürün seçilmiş ama ihtar seçilmemişse göster
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

window.icraOptions = icraOptions;
window.crudOptions = icraOptions;