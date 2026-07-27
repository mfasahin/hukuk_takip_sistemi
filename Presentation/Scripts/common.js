function showErrorModal(message) {
    document.getElementById("errorMessage").textContent = message;
    var modal = new bootstrap.Modal(document.getElementById("errorModal"));
    modal.show();
}

// function showSuccessModal(message, callback) {
//     document.getElementById("successModalMessage").textContent = message;
//     var modal = new bootstrap.Modal(document.getElementById("successModal"));
//     modal.show();

//     if (callback) {
//         document.getElementById("successModal")
//             .addEventListener('hidden.bs.modal', callback, { once: true });
//     }
// }

// function showConfirmModal(message, onConfirm) {
//     document.getElementById("confirmModalMessage").textContent = message;
//     var modal = new bootstrap.Modal(document.getElementById("confirmModal"));
//     modal.show();

//     document.getElementById("confirmModalOkBtn").onclick = function () {
//         modal.hide();
//         if (onConfirm) onConfirm();
//     };
// }
