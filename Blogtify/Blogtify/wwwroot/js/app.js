(function () {
    if (window.location.hostname !== "localhost") {
        console.log = function () { };
        console.debug = function () { };
        console.info = function () { };
        console.warn = function () { };
        console.error = function () { };
    }
})();

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