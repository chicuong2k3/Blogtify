

export function renderAd() {
    try {
        (adsbygoogle = window.adsbygoogle || []).push({});
    } catch (e) {
        console.log("Ad render error:", e);
    }
}