export function renderAd(container, client, slot, layoutKey) {
    if (!container) return;

    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }

    container.innerHTML = "";

    const ins = document.createElement("ins");
    ins.className = "adsbygoogle";
    ins.style.display = "block";
    ins.setAttribute("data-ad-format", "fluid");
    ins.setAttribute("data-ad-layout-key", layoutKey);
    ins.setAttribute("data-ad-client", client);
    ins.setAttribute("data-ad-slot", slot);
    ins.id = "ad-" + Math.random().toString(36).substring(2);
    container.appendChild(ins);

    setTimeout(() => {
        try {
            (adsbygoogle = window.adsbygoogle || []).push({});
        } catch (e) {
            console.warn("Adsense error:", e);
        }
    }, 100);
}
