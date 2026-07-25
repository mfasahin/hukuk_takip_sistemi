function resetCreateForm(formId, extraCallback) {
    var $form = $("#" + formId);
    if ($form.length === 0) return;

    // Native input/textarea/checkbox/radio - tarayıcının kendi reset() metodu
    $form[0].reset();

    // Select alanlarını placeholder'a (ilk <option>, genelde "Seçiniz") döndür
    $form.find("select").each(function () {
        this.selectedIndex = 0;
    });

    // Tarih input'larını garantiye almak için ayrıca boşalt
    $form.find("input[type='date']").val("");

    // Validasyon mesaj/stillerini temizle
    $form.find(".text-danger").empty();
    $form.find(".field-validation-error")
        .removeClass("field-validation-error")
        .addClass("field-validation-valid");
    $form.find(".input-validation-error").removeClass("input-validation-error");

    // Modüle özel ek temizlik (chip listeleri, kademeli dropdown'lar vb.)
    if (typeof extraCallback === "function") {
        extraCallback();
    }
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

    // YENİ: Modal her açılmadan HEMEN ÖNCE formu sıfırla.
    // "show.bs.modal" tercih edildi çünkü kapatma şekli (X, backdrop, Escape)
    // ne olursa olsun, bir sonraki açılışta form garanti şekilde boş olur.
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
            $.ajax({ url: createUrl, type: 'POST', data: $(this).serialize() })
                .done(function (result) {
                    if (result.success) {
                        alert(entityName + " eklendi!");

                        // YENİ: Kaydetme başarılıysa, kapatmadan önce formu sıfırla
                        resetCreateForm("createForm", options.onResetCreateForm);

                        $("#createModal").modal("hide");
                        location.reload();
                    } else {
                        alert("Ekleme başarısız: " + (result.error || ""));
                    }
                })
                .fail(function (xhr) { showError("Ekleme sırasında hata", xhr); });
        });

    // UPDATE - butona tıklanınca veri çekilir, form doldurulur, modal açılır
    $(document).off("click" + ns, ".updateBtn")
        .on("click" + ns, ".updateBtn", function () {
            var id = $(this).data("id");

            $.get(getUrl, { id: id })
                .done(function (data) {
                    fields.forEach(function (f) {
                        if (data[f] === undefined) return;
                        var $input = $("#updateForm #" + f);
                        if ($input.length === 0) return; // alan formda yoksa sessizce geç

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
            $.ajax({ url: updateUrl, type: 'POST', data: $(this).serialize() })
                .done(function (result) {
                    if (result.success) {
                        alert(entityName + " güncellendi!");
                        $("#updateModal").modal("hide");
                        location.reload();
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
            if (!confirm("Bu kaydı silmek istediğine emin misin?")) return;

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
                        alert(entityName + " silindi!");
                        location.reload();
                    } else {
                        alert("Silme başarısız: " + (result.error || ""));
                    }
                })
                .fail(function (xhr) { showError("Silme sırasında hata", xhr); });
        });
}