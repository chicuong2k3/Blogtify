export function renderAd(container, client, slot) {
    if (!container) return;

    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }

    container.innerHTML = "";

    const ins = document.createElement("ins");
    ins.className = "adsbygoogle";
    ins.style.display = "inline-block";
    ins.style.width = "728px";
    ins.style.height = "90px";
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
