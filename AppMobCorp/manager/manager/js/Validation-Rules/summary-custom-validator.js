

$(document).ready(function () {
    
    var msgError = $("#msgValidationError");

    if ($(".field-validation-error").length > 0)
        msgError.show();
    

    var validator = $('form').data('validator');

    if (validator != null) {

        /**
         * Custom Validation for size files in MB.
         **/
        $.validator.addMethod('filesize', function (value, element, param) {
            // param = size (in Megabytes) 
            // element = element to validate (<input>)
            // value = value of the element (file name)
            return this.optional(element) || (element.files[0].size <= param * Math.pow(1024, 2));
        });


        /**
         * Custom Validation for image dimension. For using this validation, data attributes (width and height) must be set.
         **/
        $.validator.addMethod('imgdimension', function (value, element, param) {
            
            var validDimension = true;

            if ($(element).data("img-width") !== undefined && $(element).data("img-height") !== undefined) {

                var width = $(element).data("img-width");
                var height = $(element).data("img-height");

                validDimension = (width == param[0] && height == param[1]);
            }

            return this.optional(element) || validDimension;

        });

        validator.settings.showErrors = function (map, errors) {

            this.defaultShowErrors();

            if ($('div[data-valmsg-summary=true] li:visible').length) {
                this.checkForm();

                if (this.errorList.length) {
                    $(this.currentForm).triggerHandler("invalid-form", [this]);
                    msgError.show();
                }                    
                else
                    $(this.currentForm).resetSummary();
            }
        };

        $('form').each(function () {
            var validator = $(this).data('validator');
            if (validator != null) {

                var errorShown = false;

                validator.settings.showErrors = function (map, errors) {
                    this.defaultShowErrors();

                    var num_errors = validator.numberOfInvalids();

                    if (validator.pendingRequest == 0 && num_errors > 0) {
                        if (!errorShown){
                            msgError.show();
                            errorShown = true;
                        }                            
                    } else {
                        msgError.hide();
                    }


                    if ($('div[data-valmsg-summary=true] li:visible').length) {
                        this.checkForm();
                        if (this.errorList.length){
                            $(this.currentForm).triggerHandler("invalid-form", [this]);
                            msgError.show();
                        }
                            
                        else
                            $(this.currentForm).resetSummary();
                    }
                };
            }
        });

        jQuery.fn.resetSummary = function () {
            var form = this.is('form') ? this : this.closest('form');
            form.find("[data-valmsg-summary=true]")
                .removeClass("validation-summary-errors")
                .addClass("validation-summary-valid")
                .find("ul")
                .empty();

            msgError.hide();

            return this;
        };
    }


});