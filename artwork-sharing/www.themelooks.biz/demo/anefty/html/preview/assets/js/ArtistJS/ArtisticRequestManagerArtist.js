var token = localStorage.getItem("token");

$(document).ready(function() {
    // Initialize DataTable
    $('#artisticTable').DataTable();
    // Function to fetch data from API and populate the table
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/GetArtworkRequestsByArtist',
            type: 'GET',
            headers: {
                'Authorization': "Bearer " + token

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
                    
                    if(item.status == 0){
                        var links = '<a class="text-capitalize" id="denyButton" data-id="' + item.id + '">'+'<button class="btn btn-primary">Deny </button>'+'</a>' + ' | ' +
                            '<a class="text-capitalize"  id="acceptButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Accept </button>'+'</a>';
                    }
                    if(item.status == 1){
                        var links = 
                            '<a class="text-capitalize" >'+'<button class="btn btn-primary" >Waiting for deposit</button>'+'</a>';
                    }else if(item.status == 2){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Update your work</button>'+'</a>' 
                    }else if(item.status == 3){
                        var links = '<a class="text-capitalize" >'+'<button class="btn btn-primary" >Deneid</button>'+'</a>';
                    }else if(item.status == 4){
                        var links = '<a class="text-capitalize" >'+'<button class="btn btn-primary" >Completed</button>'+'</a>';
                    }

                    $('#artisticTable').DataTable().row.add([
                        item.description,
                        formattedDate,
                        item.audience.name,
                        item.requestedPrice + "$",
                        item.requestedDeposit + "$",
                        status,
                        formattedDate2,
                        links,
                        '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>'
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

    $(document).on('click', '#denyButton', function (event) {
        event.preventDefault();
        var requestId = $(this).data('id');
        $('#denyconfirmModal').modal('show');

        $('#closeDenyButton').click(function () {
            $('#denyconfirmModal').modal('hide'); // Corrected from dismiss to hide
        });

        $('.close').click(function () {
            $('#denyconfirmModal').modal('hide'); // Corrected from dismiss to hide
        })

        $(document).on('click', '#confirmDenyButton', function () {
            event.preventDefault(); // Prevent the default form submission behavior
            cancelArtworkRequest(requestId); // Call the createRefundRequest function
            // $('#myModal').modal('hide'); // Hide the modal
        });

    });

    function cancelArtworkRequest(requestId) {
        $.ajax({
            url: 'https://localhost:7270/CancelArtworkRequestByArtist/' + requestId,
            type: 'PUT',
            success: function(response) {
                if(response === true){
                    $('#denyconfirmModal').modal('hide'); // Corrected from dismiss to hide
                    showSuccess("The request has been denied!");
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

    $(document).on('click', '#acceptButton', function (event) {
        event.preventDefault();
        var requestId = $(this).data('id');
        $('#acceptconfirmModal').modal('show');

        $('#closeAcceptButton').click(function () {
            $('#acceptconfirmModal').modal('hide'); // Corrected from dismiss to hide
        });

        $('.close').click(function () {
            $('#acceptconfirmModal').modal('hide'); // Corrected from dismiss to hide
        })

        $(document).on('click', '#confirmAcceptButton', function () {
            event.preventDefault(); // Prevent the default form submission behavior
            acceptArtworkRequest(requestId); // Call the createRefundRequest function
            // $('#myModal').modal('hide'); // Hide the modal
        });

    });
    
    function acceptArtworkRequest(requestId) {
        $.ajax({
            url: 'https://localhost:7270/AcceptArtworkRequestByArtist/' + requestId,
            type: 'PUT',
            success: function(response) {
                if(response === true){
                    $('#acceptconfirmModal').modal('hide'); // Corrected from dismiss to hide
                    showSuccess("The request has been accepted!");
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