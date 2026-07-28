function initAvukatTablePagination(tableId, pageSizeSelectId, paginationNavId, tableInfoId) {
    var currentPage = 1;
    var pageSize = parseInt($("#" + pageSizeSelectId).val()) || 10;

    function renderTable() {
        var $rows = $("#" + tableId + " tbody tr");
        var filters = [];

        // Avukat tablosuna özel sınıf: .avukat-column-search
        $(".avukat-column-search").each(function () {
            var colIndex = $(this).data("col");
            var val = $(this).val().toLowerCase().trim();
            filters.push({ col: colIndex, val: val });
        });

        var $filteredRows = $rows.filter(function () {
            var $row = $(this);
            var isMatch = true;

            $.each(filters, function (i, f) {
                if (f.val !== "") {
                    var cellText = $row.find("td").eq(f.col).text().toLowerCase().trim();
                    if (cellText.indexOf(f.val) === -1) {
                        isMatch = false;
                        return false;
                    }
                }
            });

            return isMatch;
        });

        var totalRecords = $filteredRows.length;
        var totalPages = Math.ceil(totalRecords / pageSize) || 1;

        if (currentPage > totalPages) currentPage = totalPages;
        if (currentPage < 1) currentPage = 1;

        $rows.hide();
        var start = (currentPage - 1) * pageSize;
        var end = start + pageSize;
        $filteredRows.slice(start, end).show();

        // Avukat tablosuna özel sayaç ID'leri
        $("#avukatTotalRecordCount").text(totalRecords);
        $("#avukatCurrentPageNum").text(currentPage);
        $("#avukatTotalPagesNum").text(totalPages);

        var startDisplay = totalRecords === 0 ? 0 : start + 1;
        var endDisplay = end > totalRecords ? totalRecords : end;
        $("#" + tableInfoId).text(totalRecords + " kayıttan " + startDisplay + " - " + endDisplay + " arası gösteriliyor.");

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

    // Başlığa tıklandığında arama satırını göster/gizle
    // Not: Avukat tablosunda 8 veri sütunu bulunduğundan "İşlemler" sütununun indeksi 8'dir.
    $("#" + tableId + " thead tr:first-child th").on("click", function () {
        if ($(this).index() === 8) return;
        $("#" + tableId + " thead tr.search-row").toggle();
    });

    $(document).off("keyup change", ".avukat-column-search")
        .on("keyup change", ".avukat-column-search", function () {
            currentPage = 1;
            renderTable();
        });

    $("#" + pageSizeSelectId).off("change").on("change", function () {
        pageSize = parseInt($(this).val());
        currentPage = 1;
        renderTable();
    });

    $(document).off("click", "#" + paginationNavId + " a")
        .on("click", "#" + paginationNavId + " a", function (e) {
            e.preventDefault();
            var page = $(this).data("page");
            if (page && page !== currentPage) {
                currentPage = page;
                renderTable();
            }
        });

    renderTable();
}