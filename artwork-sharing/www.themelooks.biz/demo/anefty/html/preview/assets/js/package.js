// Define the URL of the API endpoint
const apiUrl = 'https://localhost:7270/ManagePackage?pageIndex=1&pageSize=3';

// Function to render packages on the HTML template
function renderPackages(packages) {
  const packagesContainer = document.getElementById('packagesContainer');
  packages.forEach(package => {
    const packageHtml = `
      <div class="swiper-slide">
        <div class="single-product style--one">
          <img src="assets/img/product/product1.png" alt="" />
          <div class="product-content">
            <div class="product-top">
              <h5>${package.name}</h5>
              <div class="d-flex justify-content-between">
                <h6>${package.price} $</h6>
                <div class="countdown-wrap">
                  <ul class="countdown">
                    <li><span class="hours">${package.price*24}</span></li>
                    <li class="ms-1 me-1">:</li>
                    <li><span class="minutes">00</span></li>
                    <li class="ms-1 me-1">:</li>
                    <li><span class="seconds">00</span></li>
                    <li class="text-uppercase ms-1">left</li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="product-bottom">
              <div class="button-group">
                <a href="#" class="btn-circle love-react mr-10"></a>
                <div class="dropdown mr-10">
                  <button class="btn-circle btn-border dropdown-toggle" data-bs-toggle="dropdown">
                    <img src="assets/img/icons/share.svg" alt="" class="svg" />
                  </button>
                  <ul class="dropdown-menu">
                    <li><a class="dropdown-item" target="_blank" href="https://www.facebook.com/"><img src="assets/img/icons/facebook.svg" alt="" /> Share on Facebook</a></li>
                    <li><a class="dropdown-item" target="_blank" href="https://www.twitter.com/"><img src="assets/img/icons/twitter.svg" alt="" /> Share on Twitter</a></li>
                    <li><a class="dropdown-item" target="_blank" href="https://www.instagram.com/"><img src="assets/img/icons/instagram.svg" alt="" /> Share on Instagram</a></li>
                    <li><a class="dropdown-item" target="_blank" href="https://www.linkedin.com/"><img src="assets/img/icons/linkedin.svg" alt="" /> Share on Linkedin</a></li>
                  </ul>
                </div>
                <button onclick="redirectToItemDetails('${package.id}')" class="btn btn-border btn-sm">
                <img src="assets/img/icons/judge-icon.svg" alt="" class="svg" /> Check Out
              </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    `;
    packagesContainer.insertAdjacentHTML('beforeend', packageHtml);
  });
}
function redirectToItemDetails() {
  const queryString = window.location.search;
  const urlParams = new URLSearchParams(queryString);
  const packageId = urlParams.get('id');
  console.log(packageId);
}
// Fetch data from the API
fetch(apiUrl)
  .then(response => {
    // Check if the request was successful
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    // Parse the JSON response
    return response.json();
  })
  .then(data => {
    // Process the retrieved data
    console.log('Data retrieved from the API:', data);
    // Render packages on the HTML template
    renderPackages(data);
  })
  .catch(error => {
    // Handle any errors that occurred during the fetch operation
    console.error('Error fetching data:', error);
  });
