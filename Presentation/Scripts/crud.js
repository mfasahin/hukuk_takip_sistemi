function initCrud(entityName, fields) {
    // CREATE
    $(document).on("click", "#createSaveBtn", function () {
        $("#createForm").submit();
    });

    $("#createForm").on("submit", function (e) {
        e.preventDefault();
        $.ajax({
            url: '/' + entityName + '/Create',
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    alert(entityName + " eklendi!");
                    $("#createModal").modal("hide");
                    location.reload();
                } else {
                    alert("Ekleme başarısız: " + (result.error || ""));
                }
            }
        });
    });

    // UPDATE
    $(document).on("click", ".updateBtn", function () {
        var id = $(this).data("id");

        $.get('/' + entityName + '/Get' + entityName, { id: id }, function (data) {
            console.log("Gelen data:", data);

            fields.forEach(function (f) {
                if (data[f] !== undefined) {
                    var $input = $("#updateForm #" + f);

                    if ($input.attr("type") === "date" && data[f]) {
                        var dateVal;

                        // /Date(1783976400000)/ formatını yakala
                        if (typeof data[f] === "string" && data[f].indexOf("/Date") === 0) {
                            var timestamp = parseInt(data[f].match(/\d+/)[0], 10);
                            dateVal = new Date(timestamp);
                        } else {
                            dateVal = new Date(data[f]);
                        }

                        // yyyy-MM-dd formatını lokal saat üzerinden üret
                        var year = dateVal.getFullYear();
                        var month = ("0" + (dateVal.getMonth() + 1)).slice(-2);
                        var day = ("0" + dateVal.getDate()).slice(-2);

                        $input.val(year + "-" + month + "-" + day);
                    } else {
                        $input.val(data[f]);
                    }
                }
            });

            $("#updateModal").modal("show");
        });
    });

    $(document).on("click", "#saveBtn", function () {
        $("#updateForm").submit();
    });

    $("#updateForm").on("submit", function (e) {
        e.preventDefault();
        $.ajax({
            url: '/' + entityName + '/Update',
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    alert(entityName + " güncellendi!");
                    $("#updateModal").modal("hide");
                    location.reload();
                } else {
                    alert("Kaydetme başarısız: " + (result.error || ""));
                }
            }
        });
    });

    // DELETE
    $(document).on("click", ".deleteBtn", function () {
        var id = $(this).data("id");
        if (confirm("Bu kaydı silmek istediğine emin misin?")) {
            $.ajax({
                url: '/' + entityName + '/Delete',
                type: 'POST',
                data: { id: id, __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() },
                success: function (result) {
                    if (result.success) {
                        alert(entityName + " silindi!");
                        location.reload();
                    } else {
                        alert("Silme başarısız: " + (result.error || ""));
                    }
                }
            });
        }
    });
}
