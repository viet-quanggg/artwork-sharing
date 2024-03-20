$(document).ready(function() {
    // Initialize DataTable
    $('#refundTable').DataTable();

    // Function to fetch data from API and populate the table
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/RefundRequestByUser/32fddca3-6ebf-43c8-87ac-a6948626e2dc',
            type: 'GET',
            success: function(response) {
               
                // console.log(response);
                // Clear existing table data
                $('#refundTable').DataTable().clear().destroy();

                // Populate table with API data
                $.each(response, function(index, item) {
                    var dateTimeString = item.refundRequestDate;
                    var datetime = new Date(dateTimeString);
                    var formattedDate = datetime.toLocaleDateString('en-Gb');
                    var links = '<a class="text-capitalize" id="detailsButton" data-id="' + item.id + '" href="RefundRequestDetailUser.html">'+'<button class="btn btn-primary">Detail</button>'+'</a>';
                    $('#refundTable').DataTable().row.add([
                        // item.id,
                        // item.transactionId,
                        item.transaction.artwork.name,
                        item.transaction.artwork.price + '$',
                       formattedDate,
                        item.status,
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
});

//Add refundId to localStorage
$(document).ready(function () {
    $(document).on('click', '#detailsButton', function () {
        var refundId = $(this).data('id');
       
        localStorage.setItem("refundId", refundId);
    });

});