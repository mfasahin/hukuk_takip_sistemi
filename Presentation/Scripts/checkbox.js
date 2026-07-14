// Tümünü seç/deselect
$(document).on("change", "#selectAll", function () {
    $(".rowCheckbox").prop("checked", $(this).prop("checked"));
});

// Tek tek checkbox değiştiğinde kontrol et
$(document).on("change", ".rowCheckbox", function () {
    // Eğer tüm satır checkbox'ları seçiliyse, selectAll da seçili olsun
    if ($(".rowCheckbox:checked").length === $(".rowCheckbox").length) {
        $("#selectAll").prop("checked", true);
    } else {
        $("#selectAll").prop("checked", false);
    }
});
