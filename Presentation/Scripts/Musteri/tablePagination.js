function initTablePagination(tableId, pageSizeSelectId, paginationNavId, tableInfoId, searchInputId) {
    searchInputId = searchInputId || "globalSearchInput";
    var currentPage = 1;
    var pageSize = parseInt($("#" + pageSizeSelectId).val()) || 10;

    function renderTable() {
        var $rows = $("#" + tableId + " tbody tr:not(#noDataRow)");
        var searchText = $("#" + searchInputId).val() ? $("#" + searchInputId).val().toLowerCase().trim() : "";

        // Hücre Bazlı Hassas Arama Filtresi
        var $filteredRows = $rows.filter(function () {
            if (searchText === "") return true;

            // İşlemler (son sütun) hariç tüm hücreleri tek tek kontrol et
            var $cells = $(this).find("td:not(:last-child)");

            // Hücrelerden en az bir tanesi aranan metni içeriyorsa true döner
            return $cells.toArray().some(function (cell) {
                var cellText = $(cell).text().toLowerCase().trim();
                return cellText.indexOf(searchText) > -1;
            });
        });

        var totalRecords = $filteredRows.length;
        var totalPages = Math.ceil(totalRecords / pageSize) || 1;

        if (currentPage > totalPages) currentPage = totalPages;
        if (currentPage < 1) currentPage = 1;

        $rows.hide();
        $("#noDataRow").remove();

        if (totalRecords === 0) {
            var colCount = $("#" + tableId + " thead th").length;
            $("#" + tableId + " tbody").append('<tr id="noDataRow"><td colspan="' + colCount + '" class="text-center text-muted py-3">Aranan kriterlere uygun kayıt bulunamadı.</td></tr>');
        } else {
            var start = (currentPage - 1) * pageSize;
            var end = start + pageSize;
            $filteredRows.slice(start, end).show();

            var startDisplay = start + 1;
            var endDisplay = end > totalRecords ? totalRecords : end;
            $("#" + tableInfoId).text(totalRecords + " kayıttan " + startDisplay + " - " + endDisplay + " arası gösteriliyor.");
        }

        $("#totalRecordCount").text(totalRecords);
        $("#currentPageNum").text(currentPage);
        $("#totalPagesNum").text(totalPages);

        renderPagination(totalPages);
    }

    function renderPagination(totalPages) {
        var $nav = $("#" + paginationNavId);
        $nav.empty();

        if (totalPages <= 1) return;

        var prevClass = currentPage === 1 ? "disabled" : "";
        $nav.append('<li class="page-item ' + prevClass + '"><a class="page-link" href="javascript:void(0)" data-page="' + (currentPage - 1) + '">&laquo;</a></li>');

        for (var i = 1; i <= totalPages; i++) {
            var activeClass = i === currentPage ? "active" : "";
            $nav.append('<li class="page-item ' + activeClass + '"><a class="page-link" href="javascript:void(0)" data-page="' + i + '">' + i + '</a></li>');
        }

        var nextClass = currentPage === totalPages ? "disabled" : "";
        $nav.append('<li class="page-item ' + nextClass + '"><a class="page-link" href="javascript:void(0)" data-page="' + (currentPage + 1) + '">&raquo;</a></li>');
    }

    // Arama kutusunu (document katmanında delegasyon ile) kaçırmadan dinle
    $(document).off("input keyup search", "#" + searchInputId).on("input keyup search", "#" + searchInputId, function () {
        currentPage = 1;
        renderTable();
    });

    // Sayfa boyutu değiştiğinde
    $(document).off("change", "#" + pageSizeSelectId).on("change", "#" + pageSizeSelectId, function () {
        pageSize = parseInt($(this).val());
        currentPage = 1;
        renderTable();
    });

    // Sayfalama butonlarına tıklandığında
    $(document).off("click", "#" + paginationNavId + " a").on("click", "#" + paginationNavId + " a", function (e) {
        e.preventDefault();
        var page = $(this).data("page");
        if (page && page !== currentPage && page >= 1) {
            currentPage = page;
            renderTable();
        }
    });

    // İlk yüklemede çalıştır
    renderTable();
}