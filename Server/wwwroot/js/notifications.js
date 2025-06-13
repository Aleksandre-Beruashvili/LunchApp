// notifications.js
function showNotif(msg) {
    const toast = document.createElement('div');
    toast.className = 'toast align-items-center text-white bg-warning border-0 position-fixed bottom-0 end-0 m-3';
    toast.setAttribute('role', 'alert');
    toast.innerHTML = `
    <div class="d-flex">
      <div class="toast-body">${msg}</div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
    </div>
  `;
    document.body.append(toast);
    new bootstrap.Toast(toast, { delay: 5000 }).show();
}

export function initNotifications() {
    setInterval(async () => {
        const res = await fetch('/api/orders/notifications', {
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }
        });
        const data = await res.json();
        if (data.message) showNotif(data.message);
    }, 30000);
}
