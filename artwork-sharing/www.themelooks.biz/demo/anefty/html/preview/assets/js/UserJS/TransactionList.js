

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
                    $('#transactionTable').DataTable().row.add([
                        item.id,
                        item.artwork.name,
                        item.totalBill,
                        item.createdDate,
                        item.status,
                        item.type,
                        item.artwork.artistId
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