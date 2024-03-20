
$(document).ready(function() {
    // Initialize DataTable
    $('#transactionTable').DataTable();

    // Function to fetch data from API and populate the table
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/api/Transaction/userTransactions/32fddca3-6ebf-43c8-87ac-a6948626e2dc',
            type: 'GET',
            success: function(response) {
                console.log(response);
                // Clear existing table data
                $('#transactionTable').DataTable().clear().destroy();
               
                // Populate table with API data
                $.each(response, function(index, item) {
                    var statusText = item.status ? 'Completed' : 'Cancel';
                    var dateTimeString = item.createdDate;
                    var datetime = new Date(dateTimeString);
                    var formattedDate = datetime.toLocaleDateString('en-Gb');
                    var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>' + ' | ' +
                        '<a class="text-capitalize"  id="refundButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Refund</button>'+'</a>';
                    $('#transactionTable').DataTable().row.add([
                        // item.id,
                        item.artwork.name,
                        item.totalBill + '$',
                        formattedDate,
                        statusText,
                        item.type,
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
    // Initial data fetch when the page loads
    fetchData();
    // setInterval(fetchData, 5000);


});


//Handle Create Refund Button
$(document).ready(function () {
    $(document).on('click', '#refundButton', function () {
        var id = $(this).data('id');
        $('#myModal').modal('show');
        $('#itemId').text(id);

        // Click event for the close button inside the modal
        $('#closeButton').click(function () {
            $('#myModal').modal('hide'); // Corrected from dismiss to hide
        });
        
        $('.close').click(function () {
            $('#myModal').modal('hide'); // Corrected from dismiss to hide
        })
        
            $(document).on('click', '#requestbutton', function () {
                createRefundRequest(id); // Call the createRefundRequest function
                $('#myModal').modal('hide'); // Hide the modal
            });
    });
    
    
    function createRefundRequest(id) {
        var refundDescription = document.getElementById('refundDescription').value;
        var refundReason = document.getElementById("refundReason").value;
        var transactionId = id;
        
        var data = {
            transactionId : transactionId,
            description : refundDescription,
            reason : refundReason
        }
        
        $.ajax({
            url : "https://localhost:7270/RefundRequest/createRefundRequestUser/",
            method : "POST",
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                console.log('Created Refund', response.id)
                $('#myModal').modal('hide');
                showSuccess("Your refund request has been created!");

            },
            error: function (err) {
                console.log('Can not create refund request', err);
                showError("Something is wrong. Please try again!");
            }
        })
        
        
        
    }
});


function showSuccess(){
    var successToast = document.querySelector('.toast.success');
    var loadingBar1 = successToast.querySelector('.loadingSucces');
    successToast.style.opacity = '1';
    loadingBar1.classList.add('active');

    setTimeout(function () {
        loadingBar1.classList.remove('active');
        successToast.style.opacity = "0";
    }, 2500);
}
function showWarning(){
    var warningToast = document.querySelector('.toast.warning');
    var loadingBar1 = warningToast.querySelector('.loadingWarning');
    warningToast.style.opacity = '1';
    loadingBar1.classList.add('active');

    setTimeout(function () {
        loadingBar1.classList.remove('active');
        warningToast.style.opacity = "0";
    }, 2500);
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


