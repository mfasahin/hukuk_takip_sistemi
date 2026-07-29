// ==========================================================================
// 1. FORM TEMİZLEME VE DOĞRULAMA (VALIDATION) FONKSİYONLARI
// ==========================================================================

function resetCreateForm(formId, extraCallback) {
    var $form = $("#" + formId);
    if ($form.length === 0) return;

    $form[0].reset();

    $form.find("select").each(function () {
        this.selectedIndex = 0;
    });

    $form.find("input[type='date']").val("");

    $form.find(".text-danger").empty();
    $form.find(".field-validation-error")
        .removeClass("field-validation-error")
        .addClass("field-validation-valid");
    $form.find(".input-validation-error").removeClass("input-validation-error");

    // Doğrulama hata işaretlerini ve genel uyarı alanını temizle
    $form.find(".is-invalid").removeClass("is-invalid");
    $form.find(".invalid-feedback").remove();

    clearFormError($form);

    if (typeof extraCallback === "function") {
        extraCallback();
    }
}

function validateForm(formId, options) {
    options = options || {};
    var skipFields = options.skipFields || [];
    var customRules = options.customRules || {};
    var $form = $("#" + formId);
    var isValid = true;

    // Önceki hata işaretlerini ve genel uyarı alanını temizle
    $form.find(".is-invalid").removeClass("is-invalid");
    $form.find(".invalid-feedback").remove();
    clearFormError($form);

    var allValues = {};
    $form.find("input, select, textarea").each(function () {
        var $input = $(this);
        var name = $input.attr("name") || $input.attr("id");
        if (!name) return;
        allValues[name] = $input.val();
    });

    // NOT: "Tüm alanlar boş bırakılamaz" genel kutu uyarısı tamamen kaldırıldı.
    // Artık form tamamen boş olsa dahi her input tek tek kontrol edilecek.

    // Elemanları tek tek doğrula ve her birinin altına kırmızılık bas
    $form.find("input, select, textarea").each(function () {
        var $input = $(this);
        var name = $input.attr("name") || "";
        var id = $input.attr("id") || "";

        if (skipFields.indexOf(name) !== -1 || skipFields.indexOf(id) !== -1) return;
        if ($input.prop("disabled") || $input.attr("type") === "hidden") return;

        var value = $input.val();
        var valTrimmed = value ? value.toString().trim() : "";
        var errorMsg = null;

        // 1. Özel kural tanımlanmışsa çalıştır
        if (customRules[name] && typeof customRules[name] === "function") {
            errorMsg = customRules[name](value, allValues);
        }
        else if (customRules[id] && typeof customRules[id] === "function") {
            errorMsg = customRules[id](value, allValues);
        }
        // 2. Varsayılan zorunlu alan kontrolü
        else if (!valTrimmed || valTrimmed === "0" || valTrimmed === "00000000-0000-0000-0000-000000000000") {
            errorMsg = "Bu alan zorunludur.";
        }

        // Hata varsa SADECE ilgili input/select altına kırmızı uyarı ve ikonu ekle
        if (errorMsg) {
            isValid = false;
            $input.addClass("is-invalid");

            if ($input.next(".invalid-feedback").length === 0) {
                $input.after('<div class="invalid-feedback d-block">' + errorMsg + '</div>');
            }
        }
    });

    return isValid;
}

// Dropdown veya input değiştiğinde kırmızılığı ve hata mesajını anında kaldır
$(document).on("input change", ".is-invalid", function () {
    var $input = $(this);
    var val = $input.val();

    if (val && val.toString().trim() !== "" && val !== "0" && val !== "00000000-0000-0000-0000-000000000000") {
        $input.removeClass("is-invalid");
        $input.next(".invalid-feedback").remove();
    }
});

// ==========================================================================
// 2. KLAVYE GİRİŞ KISITLAMALARI VE MASKELER
// ==========================================================================

$(document).ready(function () {
    $(document).on('input', '.only-number', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $(document).on('input', '.only-text', function () {
        this.value = this.value.replace(/[^a-zA-ZğüşöçıİĞÜŞÖÇ\s]/g, '');
    });
});

$(document).on('input', 'input[name="MustTelNo"], input[name="MUST_TEL_NO"], input[name="AvktTelNo"], input[name="AVKT_TEL_NO"], input[name="OfisTelNo"]', function () {
    let rawValue = $(this).val().replace(/\D/g, '');
    if (rawValue.startsWith('0')) rawValue = rawValue.substring(1);
    if (rawValue.length > 10) rawValue = rawValue.substring(0, 10);

    let formatted = '';
    if (rawValue.length > 0) formatted += rawValue.substring(0, 3);
    if (rawValue.length > 3) formatted += ' ' + rawValue.substring(3, 6);
    if (rawValue.length > 6) formatted += ' ' + rawValue.substring(6, 8);
    if (rawValue.length > 8) formatted += ' ' + rawValue.substring(8, 10);

    $(this).val(formatted);
});

// ==========================================================================
// 3. SWEETALERT2 BİLDİRİM VE POP-UP YARDIMCILARI
// ==========================================================================

function showErrorModal(message) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            icon: 'error',
            title: 'Hata!',
            html: message,
            confirmButtonColor: '#01538b',
            confirmButtonText: 'Tamam'
        });
    } else {
        alert(message);
    }
}

function showSuccessModal(message, onClose) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            icon: 'success',
            title: 'Başarılı!',
            text: message,
            confirmButtonColor: '#01538b',
            confirmButtonText: 'Tamam'
        }).then(function (result) {
            if (result.isConfirmed || result.isDismissed) {
                if (typeof onClose === "function") onClose();
            }
        });
    } else {
        alert(message);
        if (typeof onClose === "function") onClose();
    }
}

function closeModalThenShowSuccess(modalId, message, onClose) {
    var modalEl = document.getElementById(modalId);
    if (modalEl) {
        var modal = bootstrap.Modal.getInstance(modalEl) || bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.hide();
    }
    showSuccessModal(message, onClose);
}

function showConfirmModal(message, onConfirm) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            title: 'Emin misiniz?',
            text: message,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#01538b',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Tamam!',
            cancelButtonText: 'İptal'
        }).then(function (result) {
            if (result.isConfirmed) {
                if (typeof onConfirm === "function") onConfirm();
            }
        });
    } else {
        if (confirm(message)) {
            if (typeof onConfirm === "function") onConfirm();
        }
    }
}

// ==========================================================================
// 4. ANA JENERİK CRUD İŞLEMLERİ (INITCRUD)
// ==========================================================================

function initCrud(entityName, fields, options) {
    options = options || {};
    var getUrl = options.getUrl || ('/' + entityName + '/Get' + entityName);
    var createUrl = options.createUrl || ('/' + entityName + '/Create');
    var updateUrl = options.updateUrl || ('/' + entityName + '/Update');
    var deleteUrl = options.deleteUrl || ('/' + entityName + '/Delete');
    var ns = ".crud-" + entityName;

    // CREATE MODAL AÇILIŞI & OTOMATİK NUMARA YÜKLEME
    $("#createModal").off("show.bs.modal" + ns)
        .on("show.bs.modal" + ns, function () {
            var $form = $("#createForm");
            resetCreateForm("createForm", options.onResetCreateForm);

            var getNextUrl = options.getNextNoUrl || (options.validationRules && options.validationRules.getNextNoUrl);
            if (getNextUrl) {
                $.ajax({
                    url: getNextUrl,
                    type: 'GET',
                    cache: false,
                    success: function (data) {
                        if (!data) return;
                        var generatedNo = data.mustNo || data.autoNo || data.number || data;
                        if (generatedNo) {
                            $form.find("#MustNo, [name='MustNo'], .auto-number-input").val(generatedNo);
                        }
                    },
                    error: function (xhr) {
                        console.error("Otomatik numara alınamadı (" + entityName + "):", xhr);
                    }
                });
            }
        });

    // Kaydet Butonu Tetikleyicisi
    $(document).off("click" + ns, "#createSaveBtn")
        .on("click" + ns, "#createSaveBtn", function (e) {
            e.preventDefault();
            $("#createForm").submit();
        });

    // CREATE SUBMIT
    $("#createForm").off("submit" + ns)
        .on("submit" + ns, function (e) {
            e.preventDefault();
            var $form = $(this);
            var skip = options.validationSkipFields || [];

            if (!validateForm("createForm", {
                skipFields: skip,
                customRules: options.validationRules
            })) {
                return false;
            }

            var tcNo = $form.find("[name='MustKimlikNo']").val();

            if (tcNo && tcNo.trim() !== "") {
                $.ajax({
                    url: '/Musteri/CheckTcExists',
                    type: 'GET',
                    data: { tcNo: tcNo.trim() },
                    success: function (checkResult) {
                        if (checkResult.exists) {
                            showFormError($form, "Bu TC kimlik numarasına ait müşteri zaten kayıtlı.");
                            return;
                        }
                        executeCreateRequest($form);
                    },
                    error: function () {
                        executeCreateRequest($form);
                    }
                });
            } else {
                executeCreateRequest($form);
            }
        });

    function executeCreateRequest($form) {
        clearFormError($form);

        $.ajax({
            url: createUrl,
            type: 'POST',
            data: $form.serialize()
        })
            .done(function (result) {
                if (result.success) {
                    resetCreateForm("createForm", options.onResetCreateForm);
                    closeModalThenShowSuccess("createModal", entityName + " başarıyla eklendi.", function () {
                        location.reload();
                    });
                } else {
                    handleBackendValidationErrors($form, result.message || result.error);
                }
            })
            .fail(function (xhr) {
                var msg = (xhr && xhr.responseJSON && xhr.responseJSON.message)
                    ? xhr.responseJSON.message
                    : "Sunucu ile iletişimde hata oluştu (" + (xhr ? xhr.status : "?") + ")";
                showFormError($form, msg);
            });
    }

    // UPDATE - Veri Getirme
    $(document).off("click" + ns, ".updateBtn")
        .on("click" + ns, ".updateBtn", function () {
            var id = $(this).data("id");

            $.get(getUrl, { id: id })
                .done(function (data) {
                    fields.forEach(function (f) {
                        if (data[f] === undefined) return;
                        var $input = $("#updateForm #" + f) || $("#updateForm #Update" + f);
                        if ($input.length === 0) {
                            $input = $("#updateForm [name='" + f + "']");
                        }
                        if ($input.length === 0) return;

                        if ($input.attr("type") === "date" && data[f]) {
                            var dateVal;
                            if (typeof data[f] === "string" && data[f].indexOf("/Date") === 0) {
                                var ts = parseInt(data[f].match(/\d+/)[0], 10);
                                dateVal = new Date(ts);
                            } else {
                                dateVal = new Date(data[f]);
                            }
                            var y = dateVal.getFullYear();
                            var m = ("0" + (dateVal.getMonth() + 1)).slice(-2);
                            var d = ("0" + dateVal.getDate()).slice(-2);
                            $input.val(y + "-" + m + "-" + d);
                        } else {
                            $input.val(data[f]);
                        }
                    });

                    if (typeof options.onGetSuccess === "function") {
                        options.onGetSuccess(data);
                    }

                    clearFormError($("#updateForm"));
                    $("#updateModal").modal("show");
                })
                .fail(function (xhr) {
                    showErrorModal(entityName + " bilgisi alınamadı (" + xhr.status + ")");
                });
        });

    // UPDATE - Güncelle Kaydet
    $(document).off("click" + ns, "#updateSaveBtn")
        .on("click" + ns, "#updateSaveBtn", function (e) {
            e.preventDefault();
            $("#updateForm").submit();
        });

    $("#updateForm").off("submit" + ns)
        .on("submit" + ns, function (e) {
            e.preventDefault();
            var $form = $(this);
            var skip = options.validationSkipFields || [];

            if (!validateForm("updateForm", {
                skipFields: skip,
                customRules: options.validationRules
            })) {
                return false;
            }

            clearFormError($form);

            $.ajax({ url: updateUrl, type: 'POST', data: $form.serialize() })
                .done(function (result) {
                    if (result.success) {
                        closeModalThenShowSuccess("updateModal", entityName + " başarıyla güncellendi.", function () {
                            location.reload();
                        });
                    } else {
                        handleBackendValidationErrors($form, result.message || result.error);
                    }
                })
                .fail(function (xhr) {
                    var msg = "Güncelleme sırasında sunucu hatası oluştu (" + xhr.status + ")";
                    showFormError($form, msg);
                });
        });

    // DELETE - Silme İşlemi
    $(document).off("click" + ns, ".deleteBtn")
        .on("click" + ns, ".deleteBtn", function () {
            var id = $(this).data("id");

            showConfirmModal("Bu " + entityName + " kaydını silmek istediğinize emin misiniz?", function () {
                $.ajax({
                    url: deleteUrl,
                    type: 'POST',
                    data: {
                        id: id,
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                    }
                })
                    .done(function (result) {
                        if (result.success) {
                            showSuccessModal(entityName + " başarıyla silindi.", function () {
                                location.reload();
                            });
                        } else {
                            showErrorModal(result.message || result.error || "Silme başarısız.");
                        }
                    })
                    .fail(function (xhr) {
                        showErrorModal("Silme işlemi sırasında hata oluştu: " + (xhr.statusText || xhr.status));
                    });
            });
        });
}

// ==========================================================================
// 5. YARDIMCI HATA VE FORM FONKSİYONLARI (UI HATA MESAJLARI)
// ==========================================================================

// Backend'den Dönen Hataları İlgili Elemanların Altına Dağıtan Akıllı Fonksiyon
function handleBackendValidationErrors($form, errors) {
    if (!errors) return;

    var errList = [];
    if (Array.isArray(errors)) {
        errList = errors;
    } else if (typeof errors === "string") {
        var cleanMsg = errors.replace(/^Doğrulama Hataları:\s*/i, "");
        errList = cleanMsg.split(/\.\s+/).map(function (e) { return e.trim(); }).filter(function (e) { return e.length > 0; });
    }

    var mappedAny = false;

    errList.forEach(function (errorMsg) {
        var lowerMsg = errorMsg.toLowerCase();
        var $targetInput = null;

        // Form alanları ile hata metni eşleştirmeleri (Tüm modüller)
        if (lowerMsg.includes("müşteri")) {
            $targetInput = $form.find("#MusteriId, #UpdateMusteriId, #createMusteriSelect, #updateMusteriSelect, [name='MusteriId']");
        } else if (lowerMsg.includes("ürün")) {
            $targetInput = $form.find("#UrunId, #UpdateUrunId, #createUrunSelect, #updateUrunSelect, [name='UrunId']");
        } else if (lowerMsg.includes("ihtar")) {
            $targetInput = $form.find("#IhtarUrunId, #createIhtarUrunSelect, #updateIhtarUrunSelect, [name='IhtarUrunId']");
        } else if (lowerMsg.includes("mahkeme")) {
            $targetInput = $form.find("#MahkemeId, #createMahkemeId, [name='MahkemeId']");
        } else if (lowerMsg.includes("dosya")) {
            $targetInput = $form.find("#IcraDosyaNo, #createIcraDosyaNo, [name='IcraDosyaNo']");
        } else if (lowerMsg.includes("takip") || lowerMsg.includes("tarih")) {
            $targetInput = $form.find("#IcraTakipTar, #createIcraTakipTar, #IhtarTarih, #UpdateIhtarTarih, [name='IcraTakipTar'], [name='IhtarTarih']");
        } else if (lowerMsg.includes("borç")) {
            $targetInput = $form.find("#BorcTutar, #UpdateBorcTutar, [name='BorcTutar']");
        } else if (lowerMsg.includes("şube")) {
            $targetInput = $form.find("#SubeId, #UpdateSubeId, [name='SubeId']");
        } else if (lowerMsg.includes("avukat")) {
            $targetInput = $form.find("#AvukatId, #UpdateAvukatId, [name='AvukatId']");
        }

        if ($targetInput && $targetInput.length > 0) {
            mappedAny = true;
            $targetInput.addClass("is-invalid");

            if ($targetInput.next(".invalid-feedback").length === 0) {
                var formattedMsg = errorMsg.endsWith('.') ? errorMsg : errorMsg + '.';
                $targetInput.after('<div class="invalid-feedback d-block">' + formattedMsg + '</div>');
            }
        }
    });

    if (!mappedAny) {
        showFormError($form, errors);
    }
}

// Form Altındaki Genel UI Hata Alanına Mesaj Yazma (Sadece Tüm Form Boşsa veya Sistemsel Hatada Çalışır)
function showFormError($form, message) {
    var $errContainer = $form.find(".error-message-container");

    if (typeof message === "object") {
        if (Array.isArray(message)) {
            message = message.join("<br/>");
        } else if (message.message) {
            message = message.message;
        } else {
            message = JSON.stringify(message);
        }
    }

    if ($errContainer.length > 0) {
        $errContainer.html(message);
        $errContainer.closest(".row").removeClass("d-none");
    } else {
        showErrorModal(message);
    }
}

// Form Altındaki UI Hata Alanını Temizleme
function clearFormError($form) {
    var $errContainer = $form.find(".error-message-container");
    if ($errContainer.length > 0) {
        $errContainer.empty();
        $errContainer.closest(".row").addClass("d-none");
    }
}