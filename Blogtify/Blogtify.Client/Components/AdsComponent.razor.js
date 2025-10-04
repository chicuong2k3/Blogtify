export function renderAd() {
    setTimeout(() => {
        try {
            (adsbygoogle = window.adsbygoogle || []).push({});
        } catch (e) {
            console.warn("Adsense error:", e);
        }
    }, 100);
}
