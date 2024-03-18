$(document).ready(function() {
    var userId =  localStorage.getItem("userId");
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/RefundRequestByUser/' + userId,
            type: 'GET',
            success: function(response) {
                console.log(response);
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