$(document).ready(function() {
    var refundId =  localStorage.getItem("refundId");
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/RefundRequestDetailByUser/' + refundId,
            type: 'GET',
            success: function(response) {
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
        document.getElementById("artwork-tilte").textContent = "Refund request for " + response.transaction.artwork.name;
        var datetimeString = response.refundRequestDate;
        var datetime = new Date(datetimeString);
        var formattedDateTime = datetime.toLocaleDateString('en-GB');
        document.getElementById("request-date").innerHTML = "Requested Date: " + formattedDateTime;
        document.getElementById("refund-id").innerHTML = "Refund Id: " + response.id;
        document.getElementById("transaction-id").innerHTML = "Transaction Id: " + response.transaction.id;
        document.getElementById("price-artwork").textContent = response.transaction.artwork.price + "$";
        document.getElementById("refund-description").textContent = "Description: " + response.description;
        document.getElementById("refund-reason").innerHTML = "Reason: " + '<br>' + response.reason;
        document.getElementById("refund-status").textContent = "Status: " + response.status;
        document.getElementById("artwork-artist").textContent = response.transaction.artwork.artist.user.name;
        
    }

});


