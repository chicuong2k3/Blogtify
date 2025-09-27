

let disqusLoaded = false;

export function addDisqusComments() {
    if (!disqusLoaded) {
        const d = document, s = d.createElement('script');
        s.src = 'https://code-magic.disqus.com/embed.js';
        s.setAttribute('data-timestamp', +new Date());
        (d.head || d.body).appendChild(s);
        disqusLoaded = true;
    }
}

export function resetDisqus(identifier, url) {
    if (window.DISQUS) {
        window.DISQUS.reset({
            reload: true,
            config: function () {
                this.page.identifier = identifier;
                this.page.url = url;
            }
        });
    }
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