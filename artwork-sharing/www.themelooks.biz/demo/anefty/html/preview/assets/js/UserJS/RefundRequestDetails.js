var token = localStorage.getItem("token");

$(document).ready(function() {
    var refundId =  localStorage.getItem("refundId");
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/RefundRequestDetailByUser/' + refundId,
            type: 'GET',
            headers: {
                'Authorization': "Bearer " + token
            },
            success: function(response) {
                console.log(response)
                if(response != null){
                    loadRefundDetails(response);
                    
                }else{
                    document.getElementById("refund-details").clear();
                    document.getElementById("refund-details").append("<h2>There is nothing to display here!</h2>");
                }
                
            },
            error: function(xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }
    // Initial data fetch when the page loads
    fetchData();

    function loadRefundDetails(response) {
        if(response.transaction.arwork != null){
            document.getElementById("artwork-tilte").textContent = "Refund request for Artwork: " + response.transaction.artwork.name;
        }else if(response.transaction.artworkService != null){
            document.getElementById("artwork-tilte").textContent = "Refund request for Artwork Request: " + response.transaction.artworkService.description;
            
        }else if(response.transaction.package != null){
            document.getElementById("artwork-tilte").textContent = "Refund request for Package: " + response.transaction.package.description;
        }
        var datetimeString = response.refundRequestDate;
        var datetime = new Date(datetimeString);
        var formattedDateTime = datetime.toLocaleDateString('en-GB');
        document.getElementById("request-date").innerHTML = "Requested Date: " + formattedDateTime;
        document.getElementById("refund-id").innerHTML = "Refund Id: " + response.id;
        document.getElementById("transaction-id").innerHTML = "Transaction Id: " + response.transaction.id;
        
        if(response.transaction.arwork != null){
            document.getElementById("price-artwork").textContent = response.transaction.artwork.price;
        }else if(response.transaction.artworkService != null){
            var depositAmount = response.transaction.artworkService.requestedDeposit;
            var requestedPrice = response.transaction.artworkService.requestedPrice;

            var content = "Deposit amount: " + depositAmount + '<br>' + "Requested price: " + requestedPrice;
            document.getElementById("price-artwork").innerHTML = content;
            
        }else if(response.transaction.package != null){
            document.getElementById("artwork-tilte").textContent = "Refund request for Package: " + response.transaction.package.price;
        }
        
        document.getElementById("refund-description").textContent = "Description: " + response.description;
        document.getElementById("refund-reason").innerHTML = "Reason: " + '<br>' + response.reason;
        document.getElementById("refund-status").textContent = "Status: " + response.status;
        if(response.status == "Pending"){
            document.getElementById("cancel-button").style.display = "block";
        }else if(response.status == "CanceledByUser"){
            document.getElementById("cancel-button").style.display = "none";
            
        }

        if(response.transaction.arwork != null){
            document.getElementById("artwork-artist").textContent = response.transaction.artwork.artist.user.name;
        }else if(response.transaction.artworkService != null){
            document.getElementById("artwork-artist").textContent = response.transaction.artworkService.artist.user.name;
            
        }else if(response.transaction.package != null){
            document.getElementById("artwork-artist").textContent = 'Artwork Sharing System';
        }
        
        
    }
    
    

});

$(document).ready(function () {
    $(document).on('click', '#cancel-button', function () {
        var id = localStorage.getItem("refundId");
        $('#myModal').modal('show');
        $('#itemId').text(id);

        // Click event for the close button inside the modal
        $('#closeButton').click(function () {
            $('#myModal').modal('hide'); // Corrected from dismiss to hide
        });

        $('.close').click(function () {
            $('#myModal').modal('hide'); // Corrected from dismiss to hide
        })

        $(document).on('click', '#confirmButton', function () {
            cancelRequest(id); // Call the createRefundRequest function
            $('#myModal').modal('hide'); // Hide the modal
        });
    });
    
    function cancelRequest(id) {
        $.ajax({
            url: 'https://localhost:7270/CancelRequestByUser/' + id,
            type: 'PUT',
            headers: {
                'Authorization': "Bearer " + token
            },
            success: function(response) {
                if(response === true){
                    $('#myModal').modal('hide'); // Corrected from dismiss to hide
                    location.reload();
                }else{
                    document.getElementById("refund-details").clear();
                    document.getElementById("refund-details").append("<h2>There is nothing to display here!</h2>");
                }

            },
            error: function(xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
        
    }

});




