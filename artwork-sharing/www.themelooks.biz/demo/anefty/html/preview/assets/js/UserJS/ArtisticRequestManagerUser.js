const token = localStorage.getItem('token');
$(document).ready(function() {
    
    // Initialize DataTable
    $('#artisticTable').DataTable();
    // Function to fetch data from API and populate the table
    function fetchPaymentMethod() {
        $.ajax({
            url: 'https://localhost:7270/api/Payment/paymentMethod',
            type: 'GET',
            success: function(response) {
                console.log(response);
                // Clear existing content
                console.log(response); // Verify response structure
                $("#methodSelect").empty(); // Clear existing content
                // Append select tag before looping through options
                var selectTag = $('<select class="form-control" id="methodSelection">');
                $.each(response, function(index, item) {
                    // Append each option to the select tag
                    selectTag.append('<option value="' + item.id + '">' + item.name.toString() + '</option>');
                });
                // Append select tag with options to the methodSelect element
                $("#methodSelect").append(selectTag);
            },
            error: function(xhr, status, error) {
                console.error(xhr.responseText);
                showError("Something is wrong. Please try again!");
            }
        });
    }
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/GetArtworkRequestsByUser',
            type: 'GET',
            headers: {
                "Authorization": "Bearer " + token // Sử dụng Bearer token nếu cần
            },
            success: function(response) {
                
                console.log(response);
                
                
                // Clear existing table data
                $('#artisticTable').DataTable().clear().destroy();

                // Populate table with API data
                $.each(response, function(index, item) {
                    var dateTimeString2 = item.requestedDeadlineDate;
                    var datetime2 = new Date(dateTimeString2);
                    var formattedDate2 = datetime2.toLocaleDateString('en-Gb');
                    
                    var dateTimeString = item.requestedDate;
                    var datetime = new Date(dateTimeString);
                    var formattedDate = datetime.toLocaleDateString('en-Gb');
                    
                    var status = getStatusText(item.status);
                    
                    if(item.status == 1){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>' + ' | ' +
                            '<a class="text-capitalize"  id="payDepositButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Pay Deposit</button>'+'</a>';
                    }else if(item.status == 0){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>' + ' | ' +
                            '<a class="text-capitalize"  id="cancelButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Cancel Request</button>'+'</a>';
                    }else if(item.status == 2){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">View Process</button>'+'</a>'
                    }
                    else if(item.status == 3){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>' + ' | ' +
                            '<a class="text-capitalize"  id="requestAgainButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Request again</button>'+'</a>';
                    }else if(item.status == 4){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">View Detail</button>'+'</a>'
                    }

                    $('#artisticTable').DataTable().row.add([
                        formattedDate,
                        item.description,
                        item.artist.user.name,
                        item.requestedPrice + "$",
                        item.requestedDeposit + "$",
                        status,
                        formattedDate2,
                        links
                        // Add more data if needed
                    ]).draw();
                });
            },
            error: function(xhr, status, error) {
                // Handle error
                console.error(xhr.responseText);
            }
        });
    }

    

    fetchPaymentMethod();
    
    // Initial data fetch when the page loads
    fetchData();
    
    // setInterval(fetchData, 5000);

    $(document).on('click', '#cancelButton', function (event) {
        event.preventDefault();
        var requestId = $(this).data('id');
        $('#confirmModal').modal('show');

        $('#closeButton').click(function () {
            $('#confirmModal').modal('hide'); // Corrected from dismiss to hide
        });

        $('.close').click(function () {
            $('#confirmModal').modal('hide'); // Corrected from dismiss to hide
        })

        $(document).on('click', '#confirmCancelButton', function () {
            event.preventDefault(); // Prevent the default form submission behavior
            cancelArtworkRequest(requestId); // Call the createRefundRequest function
            // $('#myModal').modal('hide'); // Hide the modal
        });
        
        

    });

    function cancelArtworkRequest(requestId) {
        $.ajax({
            url: 'https://localhost:7270/CancelArtworkRequestByUser/' + requestId,
            type: 'PUT',
            success: function(response) {
                if(response === true){
                    $('#confirmModal').modal('hide'); // Corrected from dismiss to hide
                    showSuccess("Your artwork request has been canceled!");
                    setTimeout(function() {
                        location.reload();
                    }, 3000);
                }

            },
            error: function(xhr, status, error) {
                console.error(xhr.responseText);
                showError("Something is wrong. Please try again!");
            }
        });

    }

    $(document).on('click', '#payDepositButton', function (event) {
        event.preventDefault();
        var paymentMethod;
        document.getElementById("methodSelection").addEventListener("change", function() {
            paymentMethod = this.options[this.selectedIndex].innerText;
        });
        var requestId = $(this).data('id');
        var paymentId = document.getElementById("methodSelection").value;
        $('#paymentMethod').modal('show');
        $('#closePaymentButton').click(function () {
            $('#paymentMethod').modal('hide'); // Corrected from dismiss to hide
        });

        $('.close').click(function () {
            $('#paymentMethod').modal('hide'); // Corrected from dismiss to hide
        })

        $(document).on('click', '#confirmPaymentButton', function () {
            event.preventDefault(); // Prevent the default form submission behavior
            proceedToPaymentPage(requestId, paymentId, paymentMethod); // Call the createRefundRequest function
            // $('#myModal').modal('hide'); // Hide the modal
        });



    });

    function proceedToPaymentPage(requestId, paymentId, paymentMethod) {
        $.ajax({
            url: 'https://localhost:7270/CreateTransactionForArtworkServiceDeposit?artworkServiceId=' + requestId 
                 + '&paymentMethodId=' + paymentId,
            type: 'POST',
            headers: {
                "Authorization": "Bearer " + token // Sử dụng Bearer token nếu cần
            },
            success: function(response) {
                if(response != null){
                    $('#confirmModal').modal('hide'); // Corrected from dismiss to hide
                    if(paymentMethod === "Credit Card"){
                        $.ajax({
                            url: 'https://localhost:7270/api/Payment/vnpay/' + response.id,
                            type: 'GET',
                            success: function(response) {
                                if(response != null){
                                    window.location.href = response;
                                }

                            },
                            error: function(xhr, status, error) {
                                console.error(xhr.responseText);
                                showError("Something is wrong. Please try again!");
                            }
                        });
                    }else if(paymentMethod==="PayPal"){
                        $.ajax({
                            url: 'https://localhost:7270/api/Payment/paypal/' + response.id,
                            type: 'GET',
                            success: function(response) {
                                if(response != null){
                                    window.location.href = response;
                                }
                            },
                            error: function(xhr, status, error) {
                                console.error(xhr.responseText);
                                showError("Something is wrong. Please try again!");
                            }
                        });
                    }
                   
                }else{
                    showError("Something is wrong in proceed to payment page! Please try again !");
                }      

            },
            error: function(xhr, status, error) {
                console.error(xhr.responseText);
                showError("Something is wrong. Please try again!");
            }
        });

    }
    
    
    
    

});




function getStatusText(statusInt) {
    switch (statusInt) {
        case 0: return "Pending";
        case 1: return "Accepted";
        case 2: return "InProgress";
        case 3: return "Rejected";
        case 4: return "Completed";
        default: return "Unknown";
    }
    
    
    
}



function showError(message) {
    var errorToast = document.querySelector('.toast.error');
    var loadingBar1 = errorToast.querySelector('.loading1');
    var errorText = errorToast.querySelector('.container-2Text p:last-child');

    errorText.textContent = message;

    errorToast.style.opacity = '1';
    loadingBar1.classList.add('active');

    setTimeout(function() {
        loadingBar1.classList.remove('active');
        errorToast.style.opacity = '0';
    }, 2500);
}

function showSuccess(message){
    var successToast = document.querySelector('.toast.success');
    var loadingBar1 = successToast.querySelector('.loadingSucces');
    var successText = successToast.querySelector('.container-2Text p:last-child');

    successText.textContent = message;
    successToast.style.opacity = '1';
    loadingBar1.classList.add('active');

    setTimeout(function () {
        loadingBar1.classList.remove('active');
        successToast.style.opacity = "0";
    }, 2500);
}
function showWarning(message){
    var warningToast = document.querySelector('.toast.warning');
    var loadingBar1 = warningToast.querySelector('.loadingWarning');
    var warningText = warningToast.querySelector('.container-2Text p:last-child');

    warningText.textContent = message;
    warningToast.style.opacity = '1';
    loadingBar1.classList.add('active');

    setTimeout(function () {
        loadingBar1.classList.remove('active');
        warningToast.style.opacity = "0";
    }, 2500);
}


