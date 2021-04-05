
$().ready(function () {
    
    $("#customerDetails").validate({
       rules: {
            FirstName: {
                required: true

            },
            LastName: {
                required: true
            },
            PostCode: {
                required: true

            },
            CustomerEmail: {
                required: true

            },
            PhoneNumber: {
                required: true

           },
            CardNumber: {
                required: true

            }
        },
        highlight: function(element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function(element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        messages: {
            FirstName: {
                required: "Please Type First Name"
            },
            LastName: {
                required: "Last Name Required"
            },
            PostCode: {
                required: "PostCode Required"
            },
            CustomerEmail: {
                required: "Email Required"
            },
            PhoneNumber: {
                required: "Phone Number Required"
            },
            CardNumber: {
                required: "TOFS Card Number Required"
            }
        },
        submitHandler: function (form) {
            $("#submit").attr("disabled", true);

            var dataRow = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            PostCode: $('#postCode').val(),
            CustomerEmail: $('#customerEmail').val(),
            PhoneNumber: $('#phoneNumber').val(),
            CardNumber: $('#cardNumber').val()
            }

        $.ajax({
            // Post Form Details to our API
            url: '/api/v1/Customer/PostCustomer',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(dataRow),
            // When successful show and then close after 1.5 seconds
            success: function () {
                ClearPopupFormValues();
                $("#submit").attr("disabled", false);
                showAlert("Success - Details Saved", "success", 1250);
                setTimeout(window.close, 1500);
            }
            });
            return false; // required to block normal submit since you used ajax
        }
    });
});

function ClearPopupFormValues() {
    $('#firstName').val("");
    $('#lastName').val("");
    $('#postCode').val("");
    $('#customerEmail').val("");
    $('#phoneNumber').val("");
    $('#cardNumber').val("");
}

//Show Alerts
function showAlert(message, type, closeDelay) {
    if ($("#alerts-container").length === 0) {
        // alerts-container does not exist, create it
        $("body").append($('<div id="alerts-container" style="position: fixed;width: 50%; left: 25%; top: 10%;">'));
    }
    $('#alerts-container').html('');
        // default to alert-info; other options include success, warning, danger
        type = type || "info";

        // create the alert div
        var alert = $('<div class="alert alert-' + type + ' fade in">')
            .append(
                $('<button type="button" class="close" data-dismiss="alert">')
                .append("&times;")
            )
            .append(message);

        // add the alert div to top of alerts-container, use append() to add to bottom
        $("#alerts-container").prepend(alert);

        // if closeDelay was passed - set a timeout to close the alert
        if (closeDelay)
            window.setTimeout(function () { alert.alert("close") }, closeDelay);
    }
