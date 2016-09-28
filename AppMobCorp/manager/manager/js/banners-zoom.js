// JavaScript source code for Image Zoom
$(document).ready(function () {

    $(".thezoom").elevateZoom({ tint: true, tintColour: '#F90', tintOpacity: 0.5, zoomWindowPosition: 16 });

    $("#fupload").on('change', function () {
        var oFReader = new FileReader();
        oFReader.readAsDataURL(document.getElementById("fupload").files[0]);

        oFReader.onload = function (oFREvent) {

            document.getElementById("uploadPreview").style.backgroundImage = 'url(' + oFREvent.target.result + ')';

            var image = new Image();
            image.src = oFREvent.target.result;

            image.onload = function () {
                // access image size here for setting up in validation
                $("#fupload").data("img-width", this.width);
                $("#fupload").data("img-height", this.height);
            };

            //document.getElementById("uploadPreview").setAttribute('data-zoom-image', 'url(' + oFREvent.target.result + ')');
            //$(".thezoom").elevateZoom({ tint: true, tintColour: '#F90', tintOpacity: 0.5, zoomWindowPosition: 16 });

        };

    });

});