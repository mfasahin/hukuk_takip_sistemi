$(function () {
    var deleteSelectedButtonInstance = null;

    // Çoklu seçim sayısına göre "Seçilenleri Sil" butonunu ve
    // satır içi Güncelle linklerini aç/kapat.
    function onSelectionChanged(e) {
        var selectedCount = e.component.getSelectedRowKeys().length;

        // 1) Toplu sil butonu: 2+ seçiliyse aktif
        if (deleteSelectedButtonInstance) {
            deleteSelectedButtonInstance.option("disabled", selectedCount < 2);
        }

        // 2) 2+ seçiliyse tüm satırlardaki "Güncelle" bağlantılarını devre dışı bırak
        var multipleSelected = selectedCount >= 2;
        $("#gridContainer .updateBtn").each(function () {
            $(this).toggleClass("disabled-action", multipleSelected);
        });
    }

    // "disabled-action" class'ı taşıyan Güncelle linklerine tıklamayı,
    // crud.js'in kendi delegated handler'ı çalışmadan ÖNCE (capture fazında) engeller.
    document.addEventListener("click", function (e) {
        var target = e.target.closest(".updateBtn");
        if (target && target.classList.contains("disabled-action")) {
            e.stopImmediatePropagation();
            e.preventDefault();
        }
    }, true);

    $("#gridContainer").dxDataGrid({
        dataSource: window.musteriData,
        keyExpr: "MusteriId",

        showBorders: false,
        showRowLines: true,
        showColumnLines: false,
        rowAlternationEnabled: true,
        columnAutoWidth: true,
        wordWrapEnabled: true,
        hoverStateEnabled: true,

        // Çoklu seçim + kalıcı checkbox sütunu
        selection: {
            mode: "multiple",
            showCheckBoxesMode: "always",
            selectAllMode: "page"
        },
        onSelectionChanged: onSelectionChanged,

        paging: { pageSize: 10 },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 50],
            showInfo: true,
            showNavigationButtons: true
        },

        filterRow: { visible: true, applyFilter: "auto" },
        headerFilter: { visible: true },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Müşteri ara..."
        },

        toolbar: {
            items: [
                {
   
                },
                "searchPanel",
                {
                    location: "after",
                    widget: "dxButton",
                    options: {
                        icon: "trash",
                        text: "Seçilenleri Sil",
                        type: "danger",
                        stylingMode: "outlined",
                        disabled: true,
                        onInitialized: function (e) {
                            deleteSelectedButtonInstance = e.component;
                        },
                        onClick: function () {
                            var grid = $("#gridContainer").dxDataGrid("instance");
                            var selectedIds = grid.getSelectedRowKeys();

                            if (selectedIds.length < 2) return;
                            if (!confirm(selectedIds.length + " kaydı silmek istediğinize emin misiniz?")) return;

                            deleteSelectedMusteriler(selectedIds, grid);
                        }
                    }
                },
                {
                    location: "after",
                    widget: "dxButton",
                    options: {
                        icon: "plus",
                        text: "Müşteri Ekle",
                        type: "success",
                        stylingMode: "contained",
                        onClick: function () {
                            $("#createModal").modal("show");
                        }
                    }
                }
            ]
        },

        columns: [
            { dataField: "MustNo", caption: "Müşteri Numarası", width: 110 },
            { dataField: "MustAd", caption: "Ad" },
            { dataField: "MustSoyad", caption: "Soyad" },
            { dataField: "MustKimlikNo", caption: "TC Kimlik", width: 130 },
            { dataField: "MustVknNo", caption: "Vergi No", width: 130 },
            {
                dataField: "MustEposta",
                caption: "E-Posta",
                cellTemplate: function (container, options) {
                    if (!options.value) {
                        $("<span>")
                            .addClass("text-muted")
                            .text("—")
                            .appendTo(container);
                        return;
                    }

                    $("<a>")
                        .attr("href", "mailto:" + options.value)
                        .attr("title", "E-posta gönder")
                        .addClass("grid-link text-primary")
                        .text(options.value)
                        .appendTo(container);
                }
            },
            { dataField: "MustTelNo", caption: "Telefon Numarası", width: 140 },
            {
                caption: "İşlem",
                width: 120,
                alignment: "center",
                allowFiltering: false,
                allowSorting: false,
                cellTemplate: function (container, options) {
                    var id = options.data.MusteriId;

                    var wrapper = $("<div>")
                        .addClass("dropdown")
                        .css("position", "relative"); // menü için referans noktası

                    $("<button>")
                        .attr("type", "button")
                        .addClass("btn btn-sm btn-outline-secondary")
                        .attr("data-bs-toggle", "dropdown")
                        .attr("aria-expanded", "false")
                        .html("&#8942;") // üç nokta ikonu
                        .appendTo(wrapper);

                    var menu = $("<ul>")
                        .addClass("dropdown-menu dropdown-menu-end")
                        .css("z-index", "1050"); // menünün üstte görünmesi için

                    // Güncelle butonu
                    $("<li>").append(
                        $("<button>")
                            .addClass("btn btn-sm btn-warning updateBtn dropdown-item")
                            .attr("data-id", id)
                            .text("Güncelle")
                    ).appendTo(menu);

                    // Sil butonu
                    $("<li>").append(
                        $("<button>")
                            .addClass("btn btn-sm btn-danger deleteBtn dropdown-item")
                            .attr("data-id", id)
                            .text("Sil")
                    ).appendTo(menu);

                    wrapper.append(menu).appendTo(container);
                }
            }
        ]
    });

// Seçili kayıtları sırayla siler (mevcut tekil Delete endpoint'i üzerinden).
function deleteSelectedMusteriler(ids, grid) {
    var basarili = 0;
    var toplam = ids.length;

    var istekler = ids.map(function (id) {
        return $.ajax({
            url: "/Musteri/Delete",
            type: "POST",
            data: {
                id: id,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            }
        }).done(function (result) {
            if (result.success) basarili++;
        });
    });

    $.when.apply($, istekler).always(function () {
        alert(basarili + " / " + toplam + " kayıt silindi.");
        location.reload();
    });
}
});