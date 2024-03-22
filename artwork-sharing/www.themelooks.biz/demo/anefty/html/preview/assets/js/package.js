// Define the URL of the API endpoint
const apiUrl = 'https://localhost:7270/ManagePackage?pageIndex=1&pageSize=3';
const token = localStorage.getItem('token');
// Function to render packages on the HTML template
function renderPackages(packages) {
  const packagesContainer = document.getElementById('packagesContainer');
  const cookies = document.cookie;

// Chia tách chuỗi cookie thành một mảng các cookie riêng lẻ
// const cookieArray = cookies.split(';');

// // Duyệt qua mỗi cookie để tìm token
// let token = '';
// cookieArray.forEach(cookie => {
//     const cookieParts = cookie.split('=');
//     const cookieName = cookieParts[0].trim();
//     const cookieValue = cookieParts[1];
//     if (cookieName === 'accessToken') {
//         token = cookieValue;
//     }
// });
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
                
              <button data-id="${package.id}" class="btn btn-border btn-sm" onclick="checkoutButtonClickHandler(event)">
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
fetch(apiUrl,{
  method: "GET", 
  headers: {
    'Content-Type': 'application/json',  
    'Authorization': `Bearer ${token}`                        
}
      
})
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

  function checkoutButtonClickHandler(event) {
    // Define the URL of the API endpoint for checkout
    const packageId = event.target.dataset.id;
    console.log("hi iam here"+ packageId)
    const checkoutApiUrl = `https://localhost:7270/ManagePackage/32c5d536-a8dc-4e87-9018-11348de74b74/checkout?PackageId=${packageId}`;
  
    fetch(checkoutApiUrl, {
        method: "PUT", // Sử dụng phương thức PUT
       
        headers: {
          'Content-Type': 'application/json',  
          'Authorization': `Bearer ${token}`                        
      }
    })
    .then(response => {
        // Check if the request was successful
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        // Parse the JSON response
        return response.json();
    })
    .then(responseData => {
      console.log("Checkout successful:", responseData);
      // Trích xuất requestUri từ đối tượng responseData
      const requestUri = responseData.requestMessage.requestUri;
      console.log("Request URI:", requestUri);
      //window.location.href = "https://example.com";
      convertlink(requestUri);
      // Redirect or perform any other action after successful checkout
    })
}

function convertlink(checkoutApiUrl){
  fetch(checkoutApiUrl, {
      method: "GET", // Sử dụng phương thức GET
      headers: {
        'Content-Type': 'application/json',  
        'Authorization': `Bearer ${token}`                        
    }
  })
  .then(response => {
      // Check if the request was successful
      if (!response.ok) {
          throw new Error('Network response was not ok');
      }
      // Parse the response body as text
      return response.text();
  })
  .then(responseText => {
      // Process the response text
      console.log("Response body:", responseText);
      window.location.href = responseText;
      // Điều chỉnh logic của bạn ở đây dựa trên nội dung của responseText
  })
  .catch(error => {
      // Handle errors that occurred during the fetch operation
      console.error('Error fetching data:', error);
  });
}

  
