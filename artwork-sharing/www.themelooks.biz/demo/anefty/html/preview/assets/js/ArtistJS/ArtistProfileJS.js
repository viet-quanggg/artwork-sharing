// Get the artistId from the URL

const urlParams = new URLSearchParams(window.location.search);

const artistId1 = 'f630d130-9dfb-4986-b3a3-6a9a1b714304';
const artistId = '60DE5964-13FC-4F7A-91FD-C8C75268D2D0';
//urlParams.get('id');
const token = localStorage.getItem('token');

//let selected = selectElement.value;
let paginated = new Object;

const selectElement = document.querySelector('.select-rounded');

selectElement.onchange = function(event) {    
    const selected = event.target.value;        
    fetchArtworks(artistId, paginated.pageIndex, selected);
};

// Function to fetch artworks for the artist
async function fetchArtworks(artistId, pageIndex, selectedValue="") {   
    try {
        const response = await fetch(`https://localhost:7270/artist/${artistId}?pageIndex=${pageIndex}&orderBy=${selectedValue}`);
        if (!response.ok) {
            throw new Error('Failed to fetch artworks');
        }
        const artworks = await response.json();
        paginated = artworks;
        displayArtworks(artworks);
        updatePaginationButtons(artworks);
    } catch (error) {
        console.error(error.message);
    }
}
function updatePaginationButtons(paginatedResult) {
    const paginationButtonsContainer = document.getElementById('paginationButtons');
    paginationButtonsContainer.innerHTML = ''; // Clear existing buttons
    for (let i = 1; i <= paginatedResult.lastPage; i++) {
        const button = document.createElement('button');
        button.textContent = i;
        button.classList.add('btn', 'btn-primary', 'mx-1');
        if (i === paginatedResult.pageIndex) {
            button.classList.add('current-page');
        }
        button.addEventListener('click', async () => {
            await fetchArtworks(artistId, i);
        });
        paginationButtonsContainer.appendChild(button);
    }
    var nextbtn = document.getElementById('nextPageBtn');
    var prevbtn = document.getElementById('prevPageBtn');
    
    
    if (paginatedResult.isLastPage) {
        nextbtn.style.display = 'none';
    } else {
        nextbtn.style.display = 'block';
        nextbtn.removeEventListener('click');
        nextbtn.addEventListener('click', async () => {
            await fetchArtworks(artistId, paginatedResult.pageIndex + 1);
        });
    }
    if(paginatedResult.pageIndex===1){
        prevbtn.style.display = 'none';
    }else{
        prevbtn.style.display = 'block';
        prevbtn.removeEventListener('click');
        prevbtn.addEventListener('click', async () => {
            await fetchArtworks(artistId, paginatedResult.pageIndex - 1);
        });
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
        <img src="${artwork.mediaContents[0].media??assets/img/product/product4.png}" alt="" class="img-fluid"> <!-- Add 'img-fluid' class for Bootstrap to make the image responsive -->
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


//follow
var followButton = document.getElementById("follow");
followButton.addEventListener("click", async()=>{
    if(token==null){
        window.location.href="login.html";
    }else{
        await fetchFollowArtist(artistId);
    }
    
})

var isFollowed = false;
async function fetchFollowInfor(artistId) {
    try {
        const response = await fetch(`https://localhost:7270/api/Follow/isFollowed/${artistId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',  
                'Authorization': `Bearer ${token}`                        
            }
        });
        if (!response.ok) {
            throw new Error('Failed to fetch follow information');
        }

        const isFollowed = await response.json();
        updateFollowButton(isFollowed);
    } catch (error) {
        console.error('Error:', error);
    }
}


async function fetchFollowArtist(artistId){
   const apiPostFollow = isFollowed?`https://localhost:7270/api/Follow/unfollow/${artistId}`:
   `https://localhost:7270/api/Follow/follow/${artistId}`;
    try {
        const response = await fetch(apiPostFollow,{
        method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(artistId)

            });
        if (!response.ok) {
            console.log(response.text());
            throw new Error('Failed to follow or unfollow');
        }
        isFollowed = !isFollowed;
        updateFollowButton();
    } catch (error) {
        console.error(error.message);
    }
}


//get Follow Infor
fetchFollowInfor(artistId);
function updateFollowButton(){
    if(isFollowed){
        followButton.innerHTML = "";
        followButton.innerHTML = "UnFollow";
    }else{
        followButton.innerHTML = "";
        followButton.innerHTML = "Follow";
    }
}

$(document).ready(function() {
    function fetchData() {
        $.ajax({
            url: 'https://localhost:7270/GetArtistProfile/f630d130-9dfb-4986-b3a3-6a9a1b714304',
            type: 'GET',
            success: function(response) {
                document.getElementById("profile-div")
                    .innerHTML += '</div><a id="requestButton" data-id="'+response.id+'" class="btn btn-primary style--two mr-10 mb-4">' +'<button style="color: white" >Request Service</button>' + '</a></div>';
                
                fetchArtistData(response);
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


    function fetchArtistData(response) {
        document.getElementById("artist_name").textContent = response.user.name;
        document.getElementById("artist_description").textContent = response.bankAccount;
        document.getElementById("get-link").value = "@" + response.user.normalizedUserName.toLowerCase();
        
    }
    
    
});

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
