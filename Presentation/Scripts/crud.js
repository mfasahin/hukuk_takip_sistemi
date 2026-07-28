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

    // Doğrulama hata işaretlerini temizle
    $form.find(".is-invalid").removeClass("is-invalid");
    $form.find(".invalid-feedback").remove();

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

    // Önceki hata mesajlarını ve kırmızı kenarlıkları temizle
    $form.find(".is-invalid").removeClass("is-invalid");
    $form.find(".invalid-feedback").remove();

    var allValues = {};
    $form.find("input, select, textarea").each(function () {
        var $input = $(this);
        var name = $input.attr("name") || $input.attr("id");
        if (!name) return;
        allValues[name] = $input.val();
    });

    // Form tamamen boş mu kontrolü
    var allEmpty = Object.keys(allValues).every(function (key) {
        return !allValues[key] || allValues[key].toString().trim() === "";
    });

    if (allEmpty) {
        $form.prepend('<div class="invalid-feedback d-block alert alert-danger mb-3">Tüm alanlar boş bırakılamaz.</div>');
        return false;
    }

    // Elemanları tek tek doğrula
    $form.find("input, select, textarea").each(function () {
        var $input = $(this);
        var name = $input.attr("name") || "";
        var id = $input.attr("id") || "";

        if (skipFields.indexOf(name) !== -1 || skipFields.indexOf(id) !== -1) return;
        if ($input.prop("disabled") || $input.attr("type") === "hidden") return;

        var value = $input.val();
        var valTrimmed = value ? value.toString().trim() : "";
        var errorMsg = null;

        // 1. Özel kural tanımlanmışsa önceliği ona ver
        if (customRules[name] && typeof customRules[name] === "function") {
            errorMsg = customRules[name](value, allValues);
        }
        else if (customRules[id] && typeof customRules[id] === "function") {
            errorMsg = customRules[id](value, allValues);
        }
        // 2. Boş değer kontrolü
        else if (!valTrimmed) {
            errorMsg = "Bu alan zorunludur.";
        }
        // 3. İcra Dosya No varsayılan Regex format doğrulaması (YYYY/SIRA E.)
        else if (name.toLowerCase().indexOf("icradosyano") !== -1 || id.toLowerCase().indexOf("icradosyano") !== -1) {
            var dosyaNoRegex = /^(19|20)\d{2}\/\d{1,7}\s?[eE]\.$/;
            if (!dosyaNoRegex.test(valTrimmed)) {
                errorMsg = "Format hatalı! Örn: 2026/1234 E.";
            }
        }

        // Hata varsa ilgili input altına uyarı ekle
        if (errorMsg) {
            isValid = false;
            $input.addClass("is-invalid");
            $input.after('<div class="invalid-feedback d-block">' + errorMsg + '</div>');
        }
    });

    return isValid;
}

// Otomatik Telefon Formatlama Maskeleri
$(document).on('input', 'input[name="MustTelNo"], input[name="MUST_TEL_NO"], input[name="AvktTelNo"], input[name="AVKT_TEL_NO"], input[name="OfisTelNo"], input[name="OFIS_TEL-NO"]', function () {
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

// Modal & Bildirim Yardımcıları
function closeModalThenShowSuccess(modalId, message, onClose) {
    var modalEl = document.getElementById(modalId);
    if (!modalEl) return;
    var modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    $(modalEl).off("hidden.bs.modal.chain").on("hidden.bs.modal.chain", function () {
        showSuccessModal(message, onClose);
    });

    modal.hide();
}

function showSuccessModal(message, onClose) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            icon: 'success',
            title: 'Başarılı!',
            text: message
        }).then(function () {
            if (typeof onClose === "function") onClose();
        });
        return;
    }

    $("#successModalMessage").text(message);
    var modalEl = document.getElementById("successModal");
    if (!modalEl) {
        alert(message);
        if (typeof onClose === "function") onClose();
        return;
    }
    var modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    $(modalEl).off("hidden.bs.modal.success").on("hidden.bs.modal.success", function () {
        if (typeof onClose === "function") onClose();
    });

    modal.show();
}

function showConfirmModal(message, onConfirm) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            title: 'Emin misiniz?',
            text: message,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, Sil!',
            cancelButtonText: 'İptal'
        }).then((result) => {
            if (result.isConfirmed) {
                if (typeof onConfirm === "function") onConfirm();
            }
        });
        return;
    }

    $("#confirmModalMessage").text(message);
    var modalEl = document.getElementById("confirmModal");
    if (!modalEl) {
        if (confirm(message)) { if (typeof onConfirm === "function") onConfirm(); }
        return;
    }
    var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    var $okBtn = $("#confirmModalOkBtn");

    $okBtn.off("click.confirm").on("click.confirm", function () {
        modal.hide();
        if (typeof onConfirm === "function") onConfirm();
    });

    modal.show();
}

// Ana CRUD Başlatıcı Fonksiyon
function initCrud(entityName, fields, options) {
    options = options || {};
    var getUrl = options.getUrl || ('/' + entityName + '/Get' + entityName);
    var createUrl = options.createUrl || ('/' + entityName + '/Create');
    var updateUrl = options.updateUrl || ('/' + entityName + '/Update');
    var deleteUrl = options.deleteUrl || ('/' + entityName + '/Delete');
    var ns = ".crud-" + entityName;

    $("#createModal").off("show.bs.modal" + ns)
        .on("show.bs.modal" + ns, function () {
            resetCreateForm("createForm", options.onResetCreateForm);
        });

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

            var skip = options.validationSkipFields || ["MusteriId", "UrunId"];

            // 1. Önyüz Form Doğrulaması
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
                            showErrorModal("Bu TC kimlik numarasına ait müşteri zaten kayıtlı.");
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
                    showErrorModal(result.message || result.error || "Ekleme işlemi başarısız.");
                }
            })
            .fail(function (xhr) {
                var msg = (xhr && xhr.responseJSON && xhr.responseJSON.message)
                    ? xhr.responseJSON.message
                    : "Sunucu ile iletişimde hata oluştu (" + (xhr ? xhr.status : "?") + ")";
                showErrorModal(msg);
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
                        var $input = $("#updateForm #" + f);
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

            var skip = options.validationSkipFields || ["MusteriId", "UrunId"];

            if (!validateForm("updateForm", {
                skipFields: skip,
                customRules: options.validationRules
            })) {
                return false;
            }

            $.ajax({ url: updateUrl, type: 'POST', data: $(this).serialize() })
                .done(function (result) {
                    if (result.success) {
                        closeModalThenShowSuccess("updateModal", entityName + " başarıyla güncellendi.", function () {
                            location.reload();
                        });
                    } else {
                        showErrorModal(result.message || result.error || "Güncelleme başarısız.");
                    }
                })
                .fail(function (xhr) {
                    showErrorModal("Güncelleme sırasında sunucu hatası oluştu (" + xhr.status + ")");
                });
        });

    // DELETE
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
                        showErrorModal("Silme işlemi sırasında hata oluştu: " + xhr.statusText);
                    });
            });
        });
}

// Select Açılır Liste Taşmasını Engelleme
$(document).on('focus', '.modal-body select.form-select', function () {
    if ($(this).find('option').length > 5) {
        $(this).attr('size', '5');
    }
}).on('change blur', '.modal-body select.form-select', function () {
    $(this).removeAttr('size');
});