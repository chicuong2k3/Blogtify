

export function addDisqusComments() {
    var d = document, s = d.createElement('script');
    s.src = 'https://symphonix.disqus.com/embed.js';
    s.setAttribute('data-timestamp', +new Date());
    (d.head || d.body).appendChild(s);
}


export function addScrollListener(key) {
    function handler() {
        localStorage.setItem(key, window.scrollY);
    }

    window._scrollHandler = handler;
    window.addEventListener("scroll", handler);
}

export function removeScrollListener(key) {
    if (window._scrollHandler) {
        window.removeEventListener("scroll", window._scrollHandler);
        window._scrollHandler = null;
    }
}

export function loadScrollPosition(key) {
    const pos = localStorage.getItem(key);
    if (pos) {
        window.scrollTo(0, parseInt(pos));
    }
}


export function readingProgressInit(dotNetHelper, article) {
    const update = () => {
        const scrollTop = window.scrollY;
        const height = article.scrollHeight - window.innerHeight;
        const progress = height > 0 ? Math.min((scrollTop / height) * 100, 100) : 0;

        dotNetHelper.invokeMethodAsync('UpdateProgress', progress);
    };

    window.addEventListener('scroll', update);
    update();
}