$(document).ready(function() {
    var artistId = localStorage.getItem("artistId");
    
    $(document).on('click', '#sendRequest', function (event) {
        event.preventDefault(); // Prevent the default form submission behavior
        createArtworkRequest(artistId); // Call the createArtworkRequest function
    });

    function createArtworkRequest(artistId) {
        var requestDescription = document.getElementById('description').value;
        var requestPrice = document.getElementById("request-price").value;
        var requestDeposit = document.getElementById("percent-deposit").value;
        var requestDeadline = document.getElementById("requestDate").value;
        var artistID = artistId;
        var userID = 'a6f05ea7-d058-4563-abf0-b28cdf7c46d3';

        if(requestDescription.trim() === ""){
            showWarning("Description can not be empty!");
            
        }else if(requestPrice.trim() === "" || requestPrice <= 0 || requestPrice > 10000){
            showWarning("Request Price must be higher than 0 and smaller than 10000");
            
        }else if(requestDeadline.trim() === "" || new Date(requestDeadline) < new Date())
        {
            showWarning("Deadline Time can not be empty and not Today !");
        } else {
            var data = {
                audienceId : userID,
                artistId : artistID,
                description : requestDescription,
                requestedPrice : requestPrice,
                requestedDeposit : requestDeposit,
                requestedDeadlineDate : requestDeadline
            }

            $.ajax({
                url : "https://localhost:7270/api/artworkrequest/createartworkrequest",
                method : "POST",
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (response) {
                    showSuccess("Your request has been sent !");
                    setTimeout(function() {
                        window.location.href = "ArtistProfile.html";
                    }, 3000);
                },
                error: function (err) {
                    showError("Something is wrong, please try again!");
                    console.log('Can not create request', err);
                }
            });
        }
    }
});


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

