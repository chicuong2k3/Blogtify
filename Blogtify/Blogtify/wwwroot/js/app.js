
window.MathJax = {
    options: {
        enableExplorer: false,
        a11y: { speech: false }
    },
    loader: {
        load: ['input/tex', 'output/chtml']
    },
    tex: {
        inlineMath: [['$', '$'], ['\\(', '\\)']]
    },
    chtml: {
        fontURL: 'https://cdn.jsdelivr.net/npm/mathjax@3/es5/output/chtml/fonts/woff-v2'
    }
};

window.mathjaxReady = new Promise((resolve, reject) => {
    function loadMathJax() {
        if (window.mathInited) {
            resolve(MathJax);
            return;
        }

        window.mathInited = true;
        const script = document.createElement('script');
        script.src = '/js/tex-chtml.js';
        script.onload = () => {
            if (window.MathJax && MathJax.startup && MathJax.startup.promise) {
                MathJax.startup.promise.then(() => resolve(MathJax))
                    .catch(err => reject(err));
            } else {
                reject(new Error('MathJax failed to load'));
            }
        };
        document.head.appendChild(script);
    }

    loadMathJax();
});


window.console.log = new Proxy(console.log, {
    apply(target, thisArg, args) {
        if (args[0]?.includes?.('Havit.Blazor.Components.Web.Bootstrap')) return;
        return Reflect.apply(target, thisArg, args);
    }
});