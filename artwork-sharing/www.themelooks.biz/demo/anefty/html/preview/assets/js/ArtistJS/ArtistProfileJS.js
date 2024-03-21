// Get the artistId from the URL
const urlParams = new URLSearchParams(window.location.search);
const artistId = '67875A5D-9B30-46C0-B5BF-4BBF57FA67FA';
//urlParams.get('id');

// Function to fetch artworks for the artist
async function fetchArtworks(artistId, pageIndex) {
    const pageSize = 10;
    try {
        const response = await fetch(`https://localhost:7270/artist/${artistId}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
        if (!response.ok) {
            throw new Error('Failed to fetch artworks');
        }
        const artworks = await response.json();
        displayArtworks(artworks);
    } catch (error) {
        console.error(error.message);
    }
}
// Function to display artworks
function displayArtworks(artworks) {
    const artworksContainer = document.getElementById('artworksContainer');
    artworksContainer.innerHTML = ''; // Clear previous artworks

    artworks.data.forEach(artwork => {
        const artworkElement = document.createElement('div');
artworkElement.classList.add('col-md-6');
artworkElement.innerHTML = `
    <div class="single-product mb-30">
        <img src="${artwork.mediaContents[0].media}" alt="" class="img-fluid"> <!-- Add 'img-fluid' class for Bootstrap to make the image responsive -->
        <div class="product-content">
            <div class="product-top">
                <h5>${artwork.name}</h5>
                <div class="d-flex justify-content-between">
                    <h6>${artwork.price} $</h6>                       
                </div>
            </div>
            <div class="product-bottom">
                <div class="button-group">
                <span> ${artwork.likes.length}</span>
                   
                    <a href="#" class="btn-circle love-react mr-10"></a>
                    <a href="item-details.html" class="btn btn-border btn-sm">
                        <img src="assets/img/icons/judge-icon.svg" alt="" class="svg">
                        Details
                    </a>
                </div>
            </div>
        </div>
    </div>`;

    artworksContainer.appendChild(artworkElement);
    });
    
}

// Initial fetch for artworks
fetchArtworks(artistId, 1);



// $(document).ready(function() {
//     function fetchData() {
//         $.ajax({
//             url: 'https://localhost:7270/GetArtistProfile/41dfeaca-fcee-4d24-aab9-289a53219fa0',
//             type: 'GET',
//             success: function(response) {
//                 document.getElementById("profile-div")
//                     .innerHTML += '</div><a id="requestButton" data-id="'+response.id+'" class="btn btn-primary style--two mr-10 mb-4">' +'<button style="color: white" >Request Service</button>' + '</a></div>';
                
//                 fetchArtistData(response);
//                 console.log(response);
 
//             },
//             error: function(xhr, status, error) {
//                 // Handle error
//                 console.error(xhr.responseText);
//             }
//         });
//     }
//     // Initial data fetch when the page loads
//     fetchData();
//     // setInterval(fetchData, 5000);


//     function fetchArtistData(response) {
//         document.getElementById("artist_name").textContent = response.user.name;
//         document.getElementById("artist_description").textContent = response.bankAccount;
//         document.getElementById("get-link").value = "@" + response.user.normalizedUserName.toLowerCase();
        
//     }
    
    
// });

$(document).ready(function () {
    $(document).on('click', '#requestButton', function () {
        var artistId = $(this).data('id');
        localStorage.setItem("artistId", artistId);
        window.location.href = 'RequestArtisticUser.html';


        // $.ajax({
        //     url: 'https://localhost:7270/api/admin/disableArtwork/' + id,
        //     method: 'put',
        //     success: function () {
        //         location.reload();
        //     }
        // })

    } )

})
