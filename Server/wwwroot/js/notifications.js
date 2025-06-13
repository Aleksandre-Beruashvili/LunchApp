// notifications.js
function showNotification(message) {
  const alert = document.createElement('div');
  alert.className = 'alert alert-warning position-fixed bottom-0 end-0 m-3';
  alert.innerText = message;
  document.body.appendChild(alert);
  setTimeout(() => alert.remove(), 5000);
}

export function initNotifications() {
  setInterval(async () => {
    const token = localStorage.getItem('token');
    const res = await fetch('/api/orders/notifications', {
      headers: { 'Authorization': 'Bearer ' + token }
    });
    const data = await res.json();
    if (data.message) showNotification(data.message);
  }, 30000);
}
