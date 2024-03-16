

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
                    var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>' + ' | ' +
                        '<a class="text-capitalize"  id="refundButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Refund</button>'+'</a>';
                    $('#transactionTable').DataTable().row.add([
                        item.id,
                        item.artwork.name,
                        item.totalBill,
                        item.createdDate,
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
                setTimeout(() => {
                    window.location.href = "https://localhost:7270/api/Transaction/userTransactions/32fddca3-6ebf-43c8-87ac-a6948626e2dc";
                }, 600);
            },
            error: function (err) {
                console.log('Can not create refund request', err);
            }
        })
        
        
        
    }
});

