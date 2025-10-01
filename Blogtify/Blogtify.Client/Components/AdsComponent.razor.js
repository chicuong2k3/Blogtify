export function renderAd(container, client, slot) {
    if (!container) return;

    container.innerHTML = "";

    const ins = document.createElement("ins");
    ins.className = "adsbygoogle";
    ins.style.display = "inline-block";
    ins.style.width = "728px";
    ins.style.height = "90px";
    ins.setAttribute("data-ad-client", client);
    ins.setAttribute("data-ad-slot", slot);

    container.appendChild(ins);

    try {
        (adsbygoogle = window.adsbygoogle || []).push({});
    } catch (e) {
        console.warn("Adsense error:", e);
    }
}
