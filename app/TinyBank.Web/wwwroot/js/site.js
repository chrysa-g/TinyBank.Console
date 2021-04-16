// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.js-update-customer').on('click',
    (event) => {
        debugger;
        let firstName = $('.js-first-name').val();
        let lastName = $('.js-last-name').val();
        let customerId = $('.js-customer-id').val();

        console.log(`${firstName} ${lastName}`);

        let data = JSON.stringify({
            firstName: firstName,
            lastName: lastName
        });

        // ajax call
        let result = $.ajax({
            url: `/customer/${customerId}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
            // success
        }).fail(failure => {
            // fail
            console.log('Update failed');
        });
    });

$('.js-customers-list tbody tr').on('click',
    (event) => {
        console.log($(event.currentTarget).attr('id'));
    });

$('.js-pay').on('click',
    (event) => {
        $(".spinner-grow").show();
        $(".alert-danger").hide();
        $(".payment-form :input").prop("disabled", true);
        debugger;
        let cardNumber = $('.js-card-number').val();
        let expirationMonth = $('.js-exp-month').val();
        let expirationYear = $('.js-exp-year').val();
        let amount = $('.js-amount').val();

        //console.log(`${firstName} ${lastName}`);

        let data = JSON.stringify({
            cardNumber: cardNumber,
            expirationMonth: parseInt(expirationMonth),
            expirationYear: parseInt(expirationYear),
            amount: amount
        });

        // ajax call
        let result = $.ajax({
            url: `/card/checkout`,
            method: 'POST',
            contentType: 'application/json',
            data: data
        }).done(response => {
            $(".checkout").hide();
            $("#success-message").html("Payment with card " + cardNumber + " for Amount " + amount + " completed successfully");
            $(".alert-success").show();
        }).fail(failure => {
            $(".spinner-grow").hide();
            $("#error-message").html(failure.responseText);
            $(".alert-danger").show();
            $(".payment-form :input").prop("disabled", false);
        });
    });