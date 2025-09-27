(function () {
    if (window.location.hostname !== "localhost") {
        console.log = function () { };
        console.debug = function () { };
        console.info = function () { };
        console.warn = function () { };
        console.error = function () { };
    }
})();