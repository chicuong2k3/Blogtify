window.themeSwitcher = {
    setTheme: function (themeName) {
        themeName = themeName.replace(/^"|"$/g, '');

        const link = document.getElementById("theme-stylesheet");
        if (link) {
            link.href = `css/theme-${themeName.toLowerCase()}.css`;
        }

        const lightThemes = ['yeti', 'flatly', 'lumen', 'materia', 'simplex', 'sketchy', 'sandstone'];
        const darkThemes = ['darkly', 'slate', 'superhero', 'vapor', 'solar'];
        const codeBlockLink = document.getElementById("code-block-stylesheet");
        if (codeBlockLink) {
            if (lightThemes.includes(themeName.toLowerCase())) {
                codeBlockLink.href = `css/prism-coy-without-shadows.min.css`;
            }
            else if (darkThemes.includes(themeName.toLowerCase())) {
                codeBlockLink.href = `css/prism-vsc-dark-plus.min.css`;
            }
        }
    },
    setFont: function (font) {
        document.documentElement.style.setProperty("--app-font-family", font);
    },
    setFontSize: function (size) {
        document.documentElement.style.setProperty("--app-font-size", size);
    },
    loadGoogleFont: function (fontName) {
        const id = "dynamic-font-link-" + fontName;
        if (!document.getElementById(id)) {
            const link = document.createElement("link");
            link.id = id;
            link.rel = "stylesheet";
            link.href = `https://fonts.googleapis.com/css2?family=${fontName.replace(/ /g, '+')}&display=swap`;
            document.head.appendChild(link);
        }
        document.body.style.fontFamily = `'${fontName}', sans-serif`;
    }
};

(function () {
    const theme = localStorage.getItem("Theme") || "Yeti";
    themeSwitcher.setTheme(theme);
})();

