$(document).ready(async function() {
    try {
      await fetchData();
      $('#searchArtist').on('submit', function(e) {
        e.preventDefault();
        console.log("clicked");
        var searchTerm = $(this).find('input[type="text"]').val();
        searchArtists(searchTerm); 
    });
    } catch (error) {
      console.error('Error:', error);
    }
  });
  
  async function fetchData() {
    try {
      const response = await $.ajax({
        url: 'https://localhost:7270/Artwork?page=1',
        type: 'GET',
      });
  
      // Clear existing table data
      $('#Artworkcollected').empty();
  
      response.forEach(item => {
        GetArtist(item);
      });
    } catch (error) {
      console.error('Error fetching artwork data:', error);
    }
  }
  
  async function GetArtist(All) {
    
    try {
      const response = await $.ajax({
        url: `https://localhost:7270/GetNameArtist/${All.artistId}?page=1`,
        type: 'GET',
      });
  
        $('#Artworkcollected').append(`
            <div class="single-product mb-30">
              <img src="assets/img/product/product1.png" alt="">
              <div class="product-content">
                <div class="product-top">
                  <h5>${All.name}</h5>
                  <div class="d-flex justify-content-between">
                    <h6>${response}</h6>
                    <h6>${All.price} VND</h6>
                  </div>
                </div>
                <div class="product-bottom">
                  <div class="button-group">
                    <a href="#" class="btn-circle love-react mr-10"></a>
                    <div class="dropdown mr-10">
                      <button class="btn-circle btn-border dropdown-toggle" data-bs-toggle="dropdown">
                        <img src="assets/img/icons/share.svg" alt="" class="svg">
                      </button>
                      <ul class="dropdown-menu">
                        <li><a class="dropdown-item" target="_blank" href="https://www.facebook.com/">
                            <img src="assets/img/icons/facebook.svg" alt=""> Share on Facebook
                          </a></li>
                        <li><a class="dropdown-item" target="_blank" href="https://www.twitter.com/">
                            <img src="assets/img/icons/twitter.svg" alt=""> Share on Twitter
                          </a></li>
                        <li><a class="dropdown-item" target="_blank" href="https://www.instagram.com/">
                            <img src="assets/img/icons/instagram.svg" alt=""> Share on Instagram
                          </a></li>
                        <li><a class="dropdown-item" target="_blank" href="https://www.linkedin.com/">
                            <img src="assets/img/icons/linkedin.svg" alt=""> Share on Linkedin
                          </a></li>
                      </ul>
                    </div>
                    <a href="item-details.html" class="btn btn-border btn-sm">
                      <img src="assets/img/icons/judge-icon.svg" alt="" class="svg"> Place Bid
                    </a>
                  </div>
                </div>
              </div>
            </div>
        `);
    } catch (error) {
      console.error('Error fetching artist data:', error);
    }
  }

  async function searchArtists(search){
    console.log(search);
    try {
        const response = await $.ajax({
          url: 'https://localhost:7270/Artwork'+search+'?page=1',
          type: 'GET',
        });
    
        // Clear existing table data
        $('#Artworkcollected').empty();
    
        response.forEach(item => {
          GetArtist(item);
        });
      } catch (error) {
        console.error('Error fetching artwork data:', error);
      }
  }