// function initCrud(entityName, fields) {
//     // CREATE
//     $(document).on("click", "#createSaveBtn", function () {
//         $("#createForm").submit();
//     });

//     $("#createForm").on("submit", function (e) {
//         e.preventDefault();
//         $.ajax({
//             url: '/' + entityName + '/Create',
//             type: 'POST',
//             data: $(this).serialize(),
//             success: function (result) {
//                 if (result.success) {
//                     alert(entityName + " eklendi!");
//                     $("#createModal").modal("hide");
//                     location.reload();
//                 } else {
//                     alert("Ekleme başarısız: " + (result.error || ""));
//                 }
//             }
//         });
//     });

//     // UPDATE
//     $(document).on("click", ".updateBtn", function () {
//         var id = $(this).data("id");

//         $.get('/' + entityName + '/Get' + entityName, { id: id }, function (data) {
//             console.log("Gelen data:", data);

//             fields.forEach(function (f) {
//                 if (data[f] !== undefined) {
//                     var $input = $("#updateForm #" + f);

//                     if ($input.attr("type") === "date" && data[f]) {
//                         var dateVal;

//                         // /Date(1783976400000)/ formatını yakala
//                         if (typeof data[f] === "string" && data[f].indexOf("/Date") === 0) {
//                             var timestamp = parseInt(data[f].match(/\d+/)[0], 10);
//                             dateVal = new Date(timestamp);
//                         } else {
//                             dateVal = new Date(data[f]);
//                         }

//                         // yyyy-MM-dd formatını lokal saat üzerinden üret
//                         var year = dateVal.getFullYear();
//                         var month = ("0" + (dateVal.getMonth() + 1)).slice(-2);
//                         var day = ("0" + dateVal.getDate()).slice(-2);

//                         $input.val(year + "-" + month + "-" + day);
//                     } else {
//                         $input.val(data[f]);
//                     }
//                 }
//             });

//             $("#updateModal").modal("show");
//         });
//     });

//     $(document).on("click", "#saveBtn", function () {
//         $("#updateForm").submit();
//     });

//     $("#updateForm").on("submit", function (e) {
//         e.preventDefault();
//         $.ajax({
//             url: '/' + entityName + '/Update',
//             type: 'POST',
//             data: $(this).serialize(),
//             success: function (result) {
//                 if (result.success) {
//                     alert(entityName + " güncellendi!");
//                     $("#updateModal").modal("hide");
//                     location.reload();
//                 } else {
//                     alert("Kaydetme başarısız: " + (result.error || ""));
//                 }
//             }
//         });
//     });

//     // DELETE
//     $(document).on("click", ".deleteBtn", function () {
//         var id = $(this).data("id");
//         if (confirm("Bu kaydı silmek istediğine emin misin?")) {
//             $.ajax({
//                 url: '/' + entityName + '/Delete',
//                 type: 'POST',
//                 data: { id: id, __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() },
//                 success: function (result) {
//                     if (result.success) {
//                         alert(entityName + " silindi!");
//                         location.reload();
//                     } else {
//                         alert("Silme başarısız: " + (result.error || ""));
//                     }
//                 }
//             });
//         }
//     });
// }


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
