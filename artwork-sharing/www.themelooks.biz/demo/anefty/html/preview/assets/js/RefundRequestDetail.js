// Define the URL parameters to extract the id
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const id = urlParams.get('id');

// If id is not available, you can handle it according to your requirements, for example, redirecting the user to a 404 page
if (!id) {
    window.location.href = '404.html'; // Redirect to 404 page
}

// Define the URL of the API endpoint
const apiUrl = `https://localhost:7270/RefundRequest/${id}`;

// Function to render data on the HTML template
function renderDataOnHTML(data) {
  document.getElementById('product-title').textContent = data[0].transaction.audience.name;
  //document.getElementById('available').textContent = `Available ${data.id}`;
  //document.getElementById('love-count').textContent = data.id;
  //document.getElementById('paragraph').textContent = data.paragraph;
  document.getElementById('price').innerHTML = `<h6>Desciption</h6><h3>${data[0].description}</h3>`;
  document.getElementById('reason').innerHTML = `<h6>Reason</h6><h3>${data[0].reason}</h3>`;
  const formattedDate = formatDate(data[0].refundRequestDate);
  console.log(formattedDate+"??");
  document.getElementById('Time-Rf').innerHTML = `<h6 class="mb-0">${formattedDate}</h6>`;
  document.getElementById('Price-Rf').innerHTML = `<h6 class="mb-0">${data[0].transaction.totalBill}</h6>`;
  
//   document.getElementById('creator-avatar').src = data.creator.avatar;
//   document.getElementById('media-body-creator').getElementsByTagName('h5')[0].textContent = data.creator.name;
//   document.getElementById('owner-avatar').src = data.owner.avatar;
//   document.getElementById('media-body-owner').getElementsByTagName('h5')[0].textContent = data.owner.name;
  
}
function formatDate(dateString) {
  // Chuyển đổi ngày thành đối tượng Date
  var date = new Date(dateString);
  
  // Định dạng ngày tháng năm
  var options = { year: 'numeric', month: 'short', day: 'numeric' };
  var formattedDate = date.toLocaleDateString('en-US', options);
  
  // Định dạng giờ phút giây
  var timeOptions = { hour: '2-digit', minute: '2-digit', second: '2-digit' };
  var formattedTime = date.toLocaleTimeString('en-US', timeOptions);
  
  // Kết hợp ngày và giờ đã định dạng
  var formattedDateTime = formattedDate + ' ' + formattedTime;
  
  return formattedDateTime;
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
    // Render data on the HTML template
    renderDataOnHTML(data);
  })
  .catch(error => {
    // Handle any errors that occurred during the fetch operation
    console.error('Error fetching data:', error);
  });


  // Khi nhấp vào nút "Deny"
$('#btn-border').click(function() {
    updateRefundRequestStatus('Deny');
   
});

// Khi nhấp vào nút "Accept"
$('#btn-sm').click(function() {
    updateRefundRequestStatus('Accept');
   
});

function updateRefundRequestStatus(status) {
    // Lấy id của refund request từ URL
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const id = urlParams.get('id');

    // Kiểm tra xem id có tồn tại không
    if (!id) {
        console.error('Refund request id not found');
        return;
    }
     status = status+"ByAdmin";
    // Gọi API để cập nhật trạng thái của refund request
    const apiUrl = `https://localhost:7270/RefundRequest/${id}/status?status=${status}`;
    $.ajax({
        url: apiUrl,
        method: 'PUT',
        success: function(response) {
            console.log(`Refund request ${id} status updated to ${status}`);
            // Thực hiện các hành động phản hồi sau khi cập nhật thành công
            window.location.href = 'RefundRequestHome.html';
        },
        error: function(xhr, status, error) {
            console.error('Error updating refund request status:', error);
            // Xử lý lỗi nếu cần thiết
        }
    });
}
