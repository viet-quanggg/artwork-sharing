$(document).ready(function() {
    // Initialize DataTable
    $('#userTable').DataTable();

    // Function to fetch data from API and populate the table
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/api/usercontroller',
            type: 'GET',
            success: function(response) {
                console.log(response);
                // Clear existing table data
                $('#userTable').DataTable().clear().destroy();

                // Populate table with API data
                $.each(response, function(index, item) {
                    var statusText = item.status ? 'Active' : 'Deactivate';
                    var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">' + '<button class="btn btn-primary">Details</button>' + '</a>' + ' | ' +
                        '<a class="text-capitalize"  id="statusButton" data-id="' + item.id + '" href="">' + '<button class="btn btn-primary">Change Status</button>'+ '</a>';
                    $('#userTable').DataTable().row.add([
                        item.name,
                        item.bankAccount,
                        item.gender,
                        statusText,
                        item.normalizedUserName.toLowerCase(),
                        item.normalizedEmail.toLowerCase(),
                        item.phoneNumber,
                        // item.roleId,
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
    $(document).on('click', '#statusButton', function () {
        var id = $(this).data('id');

        $.ajax({
            url: 'https://localhost:7270/ChangeUserStatus/' + id,
            method: 'put',
            success: function () {
                location.reload();
            }
        })

    } )

})