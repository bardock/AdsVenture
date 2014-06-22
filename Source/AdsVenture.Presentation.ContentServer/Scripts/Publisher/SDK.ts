module Publisher {

    export class SDK {

        constructor() {
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = <HTMLElement>containers[i];
                // TODO request
                var response = "<iframe src=\"//adv1.content.avt.com/ContentReference/SampleFluidExternal\"></iframe>";
                c.insertAdjacentHTML('beforeend', response);
            }
        }
    }
}
new Publisher.SDK();
