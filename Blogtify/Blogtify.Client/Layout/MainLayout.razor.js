

let adsScriptLoaded = false;

export function loadAdsense(clientId) {
    if (!adsScriptLoaded && clientId) {
        const script = document.createElement("script");
        script.async = true;
        script.src = `https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=${clientId}`;
        script.crossOrigin = "anonymous";
        document.head.appendChild(script);
        adsScriptLoaded = true;
    }
}