// crud.js
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

        // Burada entityName parametresini kullanıyoruz
        $.get('/' + entityName + '/Get' + entityName, { id: id }, function (data) {
            console.log("Gelen data:", data);
            fields.forEach(function (f) {
                if (data[f] !== undefined) {
                    $("#updateForm #" + f).val(data[f]); // sadece updateForm içindeki inputları doldur
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
