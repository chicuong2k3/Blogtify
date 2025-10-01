
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