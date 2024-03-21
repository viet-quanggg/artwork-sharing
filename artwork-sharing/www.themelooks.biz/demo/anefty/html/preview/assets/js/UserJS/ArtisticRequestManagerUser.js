$(document).ready(function() {
    // Initialize DataTable
    $('#artisticTable').DataTable();
    // Function to fetch data from API and populate the table
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/GetArtworkRequestsByUser/56a3e149-2c89-4d85-5ac9-08dc4956f46d',
            type: 'GET',
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
                    }else if(item.status == 0 || item.status == 2 || item.status == 3 || item.status == 4){
                        var links = '<a class="text-capitalize" id="detailsButton" href="' + item.id + '">'+'<button class="btn btn-primary">Detail</button>'+'</a>' + ' | ' +
                            '<a class="text-capitalize"  id="cancelButton" data-id="' + item.id + '">'+'<button class="btn btn-primary" >Cancel Request</button>'+'</a>';
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
    // Initial data fetch when the page loads
    fetchData();
    // setInterval(fetchData, 5000);


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


