var Publisher;
(function (Publisher) {
    var SDK = (function () {
        function SDK() {
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = containers[i];

                // TODO request
                var response = "<iframe src=\"//adv1.content.avt.com/ContentReference/SampleFluidExternal\"></iframe>";
                c.insertAdjacentHTML('beforeend', response);
            }
        }
        return SDK;
    })();
    Publisher.SDK = SDK;
})(Publisher || (Publisher = {}));
new Publisher.SDK();
//# sourceMappingURL=SDK.js.map
