
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
