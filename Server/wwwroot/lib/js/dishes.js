/* wwwroot/admin/js/dishes.js */
console.log("🟢 dishes.js module loaded");

const DishManager = (() => {
    /* ───────── helpers ───────── */
    const $ = id => document.getElementById(id);
    const debounce = (fn, ms = 300) => { let t; return (...a) => { clearTimeout(t); t = setTimeout(() => fn.apply(this, a), ms); }; };
    const idOf = d => d?.id ?? d?.dishId;                 // unify id field
    const api = (url, opt = {}) => {
        const token = localStorage.getItem("token");
        return fetch(url, { ...opt, headers: { ...opt.headers, Authorization: `Bearer ${token}` } })
            .then(async r => r.ok ? r.json() : Promise.reject(await r.json().catch(() => ({ status: r.status }))));
    };

    /* ───────── state & cache ───────── */
    let els = {};
    const st = { all: [], page: 1, size: 5, busy: false };

    /* ───────── life-cycle ───────── */
    function init() { cacheEls(); bind(); load(); }
    function cacheEls() {
        els = {
            modal: $("dishModal"), modalTitle: $("modalTitle"), form: $("dishForm"),
            open: $("openModal"), close: [$("closeModal"), $("closeModalBtn")],
            list: $("dishList"), search: $("searchInput"),
            prev: $("prevPage"), next: $("nextPage"), info: $("pageInfo"),
            file: $("imageFile"), imgWrap: $("currentImage"), rmImg: $("removeImage"),
            err: { form: $("formError"), name: $("nameError"), portion: $("portionError"), price: $("priceError") },
            stats: { tot: $("totalDishes"), avl: $("availableDishes"), top: $("popularDish") }
        };
    }

    /* ───────── events ───────── */
    function bind() {
        els.open?.addEventListener("click", () => modal(true));
        els.close.forEach(b => b?.addEventListener("click", () => modal(false)));
        window.addEventListener("keydown", e => e.key === "Escape" && modal(false));

        els.prev?.addEventListener("click", () => { st.page--; render(); });
        els.next?.addEventListener("click", () => { st.page++; render(); });
        els.search?.addEventListener("input", debounce(() => { st.page = 1; render(); }));

        els.file?.addEventListener("change", e => {
            if (!e.target.files[0]) return;
            const r = new FileReader();
            r.onload = ev => { els.imgWrap.querySelector("img").src = ev.target.result; els.imgWrap.classList.remove("hidden"); };
            r.readAsDataURL(e.target.files[0]);
        });
        els.rmImg?.addEventListener("click", () => { els.file.value = ""; els.imgWrap.classList.add("hidden"); });

        els.form?.addEventListener("submit", submitForm);
        els.list?.addEventListener("click", listAction);
    }

    /* ───────── CRUD ───────── */
    async function load() {
        showSkeleton(true);
        try { st.all = await api("/api/dishes/all"); }
        catch { alert("Load failed."); }
        finally { showSkeleton(false); render(); }
    }
    const save = (fd, id) => api(id ? `/api/dishes/${id}` : "/api/dishes", { method: id ? "PUT" : "POST", body: fd });
    const remove = id => api(`/api/dishes/${id}`, { method: "DELETE" });
    const toggle = (id, v) => api(`/api/dishes/${id}/availability`, { method: "PATCH", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ isAvailable: v }) });

    /* ───────── render ───────── */
    function render() {
        const q = els.search.value.trim().toLowerCase();
        const list = q ? st.all.filter(d => d.name.toLowerCase().includes(q)) : st.all;
        const pages = Math.max(1, Math.ceil(list.length / st.size));
        st.page = Math.min(st.page, pages);

        const view = list.slice((st.page - 1) * st.size, st.page * st.size);
        els.list.innerHTML = view.map(card).join("");

        els.info.textContent = `Page ${st.page} of ${pages}`;
        els.prev.disabled = st.page === 1; els.next.disabled = st.page === pages;

        els.stats.tot.textContent = st.all.length;
        els.stats.avl.textContent = st.all.filter(d => d.isAvailable).length;
        els.stats.top.textContent = st.all.length ? st.all.reduce((p, c) => (p.orderCount || 0) > (c.orderCount || 0) ? p : c).name : "–";
    }

    const card = d => `
  <article class="dish-card bg-dark2 p-5 rounded-xl shadow-lg flex flex-col sm:flex-row gap-5">
    ${d.imageUrl ? `<img src="${d.imageUrl}" alt="${d.name}" class="w-full sm:w-32 h-32 object-cover rounded-lg border border-zinc-700">`
            : `<div class="w-32 h-32 bg-dark3 rounded-lg flex items-center justify-center flex-shrink-0">
          <i class="fas fa-utensils text-3xl text-zinc-600"></i></div>`}
    <div class="flex-1">
      <header class="flex justify-between items-start mb-1">
        <h3 class="text-xl font-bold">${d.name}</h3>
        <span class="text-orange-500 font-bold">$${d.price.toFixed(2)}</span>
      </header>
      <div class="flex flex-wrap items-center gap-3 text-sm mb-2">
        <span class="status-badge ${d.isAvailable ? 'status-available' : 'status-disabled'}">
          <i class="fas fa-${d.isAvailable ? 'check' : 'times'} mr-1 text-xs"></i>${d.isAvailable ? 'Available' : 'Disabled'}</span>
        <span class="text-zinc-400"><i class="fas fa-weight-hanging mr-1 text-xs"></i>Portion: ${d.portionSize} ${d.portion}</span>
      </div>
      <p class="text-zinc-300 mb-4">${d.description || "No description."}</p>
      <div class="flex flex-wrap gap-3 text-sm">
        <button data-act="edit"   data-id="${idOf(d)}" class="bg-blue-700 hover:bg-blue-600 px-4 py-2 rounded-lg"><i class="fas fa-edit mr-1"></i>Edit</button>
        <button data-act="del"    data-id="${idOf(d)}" class="bg-red-700 hover:bg-red-600 px-4 py-2 rounded-lg"><i class="fas fa-trash mr-1"></i>Delete</button>
        <button data-act="toggle" data-id="${idOf(d)}" class="${d.isAvailable ? 'bg-zinc-700 hover:bg-zinc-600' : 'bg-green-700 hover:bg-green-600'} px-4 py-2 rounded-lg">
          <i class="fas fa-${d.isAvailable ? 'ban' : 'check'} mr-1"></i>${d.isAvailable ? 'Disable' : 'Enable'}</button>
      </div>
    </div>
  </article>`;

    /* ───────── modal ───────── */
    function modal(on, d = null) {
        if (!on) { els.modal.setAttribute("aria-hidden", "true"); els.modal.classList.add("hidden"); document.body.style.overflow = ""; return; }

        els.form.reset(); $("dishId").value = "";
        els.imgWrap.classList.add("hidden"); els.file.value = "";
        els.modalTitle.textContent = d ? "Edit Dish" : "Add Dish";

        if (d) {
            $("dishId").value = idOf(d);
            $("name").value = d.name;
            $("portionSize").value = d.portionSize ?? d.PortionSize ?? "";
            $("portion").value = d.portion;
            $("price").value = d.price;
            $("description").value = d.description || "";
            if (d.imageUrl) {
                els.imgWrap.querySelector("img").src = d.imageUrl;
                els.imgWrap.classList.remove("hidden");
            }
        }

        els.modal.classList.remove("hidden");
        els.modal.setAttribute("aria-hidden", "false");
        document.body.style.overflow = "hidden";
    }

    /* ───────── form ───────── */
    async function submitForm(e) {
        e.preventDefault();
        if (st.busy) return;
        clearErrs();
        if (!validate()) return;

        const btn = els.form.querySelector('button[type="submit"]');
        const old = btn.innerHTML; btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Saving…'; btn.disabled = true; st.busy = true;

        try { await save(new FormData(els.form), $("dishId").value || null); modal(false); await load(); }
        catch { els.err.form.textContent = "Save failed."; els.err.form.classList.remove("hidden"); }
        finally { btn.innerHTML = old; btn.disabled = false; st.busy = false; }
    }
    function validate() {
        let ok = true;
        if (!$("name").value.trim()) { els.err.name.classList.remove("hidden"); ok = false; }
        if (!$("portion").value) { els.err.portion.classList.remove("hidden"); ok = false; }
        if (!(+$("price").value > 0)) { els.err.price.classList.remove("hidden"); ok = false; }
        if (!ok) { els.err.form.textContent = "Fix highlighted fields."; els.err.form.classList.remove("hidden"); }
        return ok;
    }
    const clearErrs = () => Object.values(els.err).forEach(e => e.classList.add("hidden"));

    /* ───────── list actions ───────── */
    async function listAction(e) {
        const btn = e.target.closest("[data-act]"); if (!btn) return;
        const id = btn.dataset.id; const dish = st.all.find(d => String(idOf(d)) === id); if (!dish) return;

        if (btn.dataset.act === "edit") return modal(true, dish);
        if (btn.dataset.act === "del") {
            if (!confirm(`Delete "${dish.name}"?`)) return;
            try { await remove(id); st.all = st.all.filter(d => String(idOf(d)) !== id); render(); }
            catch { alert("Delete failed."); }
        }
        if (btn.dataset.act === "toggle") {
            try { await toggle(id, !dish.isAvailable); dish.isAvailable = !dish.isAvailable; render(); }
            catch { alert("Status update failed."); }
        }
    }

    /* ───────── misc ───────── */
    const showSkeleton = on => { if (on) els.list.innerHTML = '<div class="skeleton h-40"></div>'.repeat(3); };

    return { init };
})();

/* export for dashboard */
export function init() { DishManager.init(); }
