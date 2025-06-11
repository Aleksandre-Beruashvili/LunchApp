const apiBase = '';

export async function login(email, password) {
    const res = await fetch(`${apiBase}/api/account/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    if (!res.ok) return null;
    return await res.json();
}

export async function register(fullName, email, password) {
    const res = await fetch(`${apiBase}/api/account/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ fullName, email, password })
    });
    return res.ok;
}

export async function getMenu() {
    const res = await fetch(`${apiBase}/api/menu/today`);
    return res.ok ? await res.json() : null;
}

export async function getSlots(date) {
    const res = await fetch(`${apiBase}/api/timeslot?date=${date}`);
    return res.ok ? await res.json() : null;
}

export async function placeOrder(token, dto) {
    const res = await fetch(`${apiBase}/api/orders`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(dto)
    });
    return res.ok ? await res.json() : null;
}

