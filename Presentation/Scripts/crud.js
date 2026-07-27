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

    // Doğrulama hata işaretlerini de temizle
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

    $form.find(".is-invalid").removeClass("is-invalid");
    $form.find(".invalid-feedback").remove();

    // tüm değerleri topla
    var allValues = {};
    $form.find("input, select, textarea").each(function () {
        var $input = $(this);
        var name = $input.attr("name");
        if (!name) return;
        allValues[name] = $input.val();
    });

    // bütün inputlar boş mu?
    var allEmpty = Object.keys(allValues).every(function (key) {
        return !allValues[key] || allValues[key].toString().trim() === "";
    });
    if (allEmpty) {
        isValid = false;
        $form.prepend('<div class="invalid-feedback d-block">Tüm alanlar boş bırakılamaz.</div>');
        return false;
    }

    $form.find("input, select, textarea").each(function () {
        var $input = $(this);
        var name = $input.attr("name");

        if (!name || skipFields.indexOf(name) !== -1) return;
        if ($input.prop("disabled")) return;
        if ($input.attr("type") === "hidden") return;

        var value = $input.val();
        var errorMsg = null;

        // alan bazlı zorunluluk
        if (!value || value.toString().trim() === "") {
            // özel iş kuralları
            if (name === "MustSoyad" && (!allValues.MustVknNo || allValues.MustVknNo.trim() === "")) {
                errorMsg = "Soyad boş olduğunda VKN No zorunludur.";
            }
            if (name === "MustVknNo" && (!allValues.MustSoyad || allValues.MustSoyad.trim() === "")) {
                errorMsg = "Soyad boş olduğunda VKN No zorunludur.";
            }
            if (name === "MustKimlikNo" && allValues.MustSoyad && (!value || value.trim() === "")) {
                errorMsg = "Soyad dolu olduğunda TC Kimlik No zorunludur.";
            }
            if (!errorMsg) {
                errorMsg = "Bu alan zorunludur.";
            }
        } else if (customRules[name]) {
            errorMsg = customRules[name](value, allValues);
        }

        if (errorMsg) {
            isValid = false;
            $input.addClass("is-invalid");
            $input.after('<div class="invalid-feedback d-block">' + errorMsg + '</div>');
        }
    });

    return isValid;
}

function closeModalThenShowSuccess(modalId, message, onClose) {
    var modalEl = document.getElementById(modalId);
    var modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    $(modalEl).off("hidden.bs.modal.chain").on("hidden.bs.modal.chain", function () {
        showSuccessModal(message, onClose);
    });

    modal.hide();
}

function showSuccessModal(message, onClose) {
    $("#successModalMessage").text(message);
    var modalEl = document.getElementById("successModal");
    var modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    $(modalEl).off("hidden.bs.modal.success").on("hidden.bs.modal.success", function () {
        if (typeof onClose === "function") onClose();
    });

    modal.show();
}

// Kullanıcıdan onay ister. Onaylarsa onConfirm callback'i çalışır.
// confirm() yerine Bootstrap modalı kullanır, böylece tüm modallar tutarlı görünür.
function showConfirmModal(message, onConfirm) {
    $("#confirmModalMessage").text(message);
    var modalEl = document.getElementById("confirmModal");
    var modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    var $okBtn = $("#confirmModalOkBtn");

    // Önceki tıklama handler'ını temizle (aksi halde birden fazla eklenip
    // her tıklamada callback birden fazla kez tetiklenir)
    $okBtn.off("click.confirm").on("click.confirm", function () {
        modal.hide();
        if (typeof onConfirm === "function") onConfirm();
    });

    modal.show();
}

function initCrud(entityName, fields, options) {
    options = options || {};
    var getUrl = options.getUrl || ('/' + entityName + '/Get' + entityName);
    var createUrl = options.createUrl || ('/' + entityName + '/Create');
    var updateUrl = options.updateUrl || ('/' + entityName + '/Update');
    var deleteUrl = options.deleteUrl || ('/' + entityName + '/Delete');
    var ns = ".crud-" + entityName;

    function showError(prefix, xhr) {
        console.error(prefix, xhr);
        var msg = (xhr && xhr.responseJSON && xhr.responseJSON.error)
            ? xhr.responseJSON.error
            : "Sunucu ile iletişimde hata oluştu (" + (xhr ? xhr.status : "?") + ")";
        alert(prefix + ": " + msg);
    }

    $("#createModal").off("show.bs.modal" + ns)
        .on("show.bs.modal" + ns, function () {
            resetCreateForm("createForm", options.onResetCreateForm);
        });

    // CREATE
    $(document).off("click" + ns, "#createSaveBtn")
        .on("click" + ns, "#createSaveBtn", function () {
            $("#createForm").submit();
        });

    $("#createForm").off("submit" + ns)
        .on("submit" + ns, function (e) {
            e.preventDefault();

            if (!validateForm("createForm", {
                skipFields: options.validationSkipFields,
                customRules: options.validationRules
            })) {
                return;
            }

            $.ajax({ url: createUrl, type: 'POST', data: $(this).serialize() })
                .done(function (result) {
                    if (result.success) {
                        resetCreateForm("createForm", options.onResetCreateForm);

                        closeModalThenShowSuccess("createModal", entityName + " başarıyla eklendi.", function () {
                            location.reload();
                        });
                    } else {
                        alert("Ekleme başarısız: " + (result.error || ""));
                    }
                })
                .fail(function (xhr) { showError("Ekleme sırasında hata", xhr); });
        });

    // UPDATE
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
                    showError(entityName + " bilgisi alınamadı", xhr);
                });
        });

    $(document).off("click" + ns, "#updateSaveBtn")
        .on("click" + ns, "#updateSaveBtn", function () {
            $("#updateForm").submit();
        });

    $("#updateForm").off("submit" + ns)
        .on("submit" + ns, function (e) {
            e.preventDefault();

            if (!validateForm("updateForm", {
                skipFields: options.validationSkipFields,
                customRules: options.validationRules
            })) {
                return;
            }

            $.ajax({ url: updateUrl, type: 'POST', data: $(this).serialize() })
                .done(function (result) {
                    if (result.success) {
                        closeModalThenShowSuccess("updateModal", entityName + " başarıyla güncellendi.", function () {
                            location.reload();
                        });
                    } else {
                        alert("Kaydetme başarısız: " + (result.error || ""));
                    }
                })
                .fail(function (xhr) { showError("Güncelleme sırasında hata", xhr); });
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
                            // alert yerine modal
                            showErrorModal("Silme başarısız: " + (result.error || ""));
                        }
                    })
                    .fail(function (xhr) {
                        showErrorModal("Silme sırasında hata: " + xhr.statusText);
                    });
            });
        });
}