// ==========================================================================
// SWEETALERT2 BİLDİRİM VE POP-UP YARDIMCILARI
// ==========================================================================

function showErrorModal(message) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            icon: 'error',
            title: 'Hata!',
            html: message,
            confirmButtonColor: '#01538b',
            confirmButtonText: 'Tamam'
        });
    } else {
        alert(message);
    }
}

function showSuccessModal(message, onClose) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            icon: 'success',
            title: 'Başarılı!',
            text: message,
            confirmButtonColor: '#01538b',
            confirmButtonText: 'Tamam'
        }).then(function (result) {
            if (result.isConfirmed || result.isDismissed) {
                if (typeof onClose === "function") onClose();
            }
        });
    } else {
        alert(message);
        if (typeof onClose === "function") onClose();
    }
}

function closeModalThenShowSuccess(modalId, message, onClose) {
    var modalEl = document.getElementById(modalId);
    if (modalEl) {
        var modal = bootstrap.Modal.getInstance(modalEl) || bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.hide();
    }
    showSuccessModal(message, onClose);
}

function showConfirmModal(message, onConfirm) {
    if (typeof Swal !== "undefined") {
        Swal.fire({
            title: 'Emin misiniz?',
            text: message,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#01538b',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Tamam!',
            cancelButtonText: 'İptal'
        }).then(function (result) {
            if (result.isConfirmed) {
                if (typeof onConfirm === "function") onConfirm();
            }
        });
    } else {
        if (confirm(message)) {
            if (typeof onConfirm === "function") onConfirm();
        }
    }
}

// ==========================================================================
// KLAVYE GİRİŞ KISITLAMALARI VE MASKELER
// ==========================================================================

$(document).ready(function () {
    $(document).on('input', '.only-number', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $(document).on('input', '.only-text', function () {
        this.value = this.value.replace(/[^a-zA-ZğüşöçıİĞÜŞÖÇ\s]/g, '');
    });
});

$(document).on('input', 'input[name="MustTelNo"], input[name="MUST_TEL_NO"], input[name="AvktTelNo"], input[name="AVKT_TEL_NO"], input[name="OfisTelNo"]', function () {
    let rawValue = $(this).val().replace(/\D/g, '');
    if (rawValue.startsWith('0')) rawValue = rawValue.substring(1);
    if (rawValue.length > 10) rawValue = rawValue.substring(0, 10);

    let formatted = '';
    if (rawValue.length > 0) formatted += rawValue.substring(0, 3);
    if (rawValue.length > 3) formatted += ' ' + rawValue.substring(3, 6);
    if (rawValue.length > 6) formatted += ' ' + rawValue.substring(6, 8);
    if (rawValue.length > 8) formatted += ' ' + rawValue.substring(8, 10);

    $(this).val(formatted);
});