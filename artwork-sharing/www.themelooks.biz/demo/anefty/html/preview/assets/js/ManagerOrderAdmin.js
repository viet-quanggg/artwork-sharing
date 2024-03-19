const pageSize = 3; // Định nghĩa pageSize ở đầu tập tin hoặc ở phạm vi có thể truy cập trước khi sử dụng
let keyword = '';
// Define the URL of the API endpoint
const apiUrl = 'https://localhost:7270/ManageOrder/GeTransactionWithPaging?pageIndex=1&pageSize=3';
// Function to handle search form submission
document.getElementById('searchForm').addEventListener('submit', function(event) {
  event.preventDefault(); // Prevent the default form submission behavior

  const keyword = document.getElementById('searchInput').value;

 // dataBody.innerHTML = '';
  pa(keyword);

  fetchRefundRequests(1, keyword); // Fetch refund requests for the first page with the search keyword
});

// Function to render refund requests on the HTML template
function renderRefundRequests(refundRequests) {
  const dataBody = document.getElementById('dataBody');
  dataBody.innerHTML = '';
  refundRequests.forEach(request => {
    const formattedDate = formatDate(request.createdDate);
    const requestHtml = `
      <tr>
        <td>${request.audience.name}</td>
        <td>${formattedDate}</td>
        <td>${request.audience.bankAccount}</td>
        <td>${request.audience.gender}</td>
        <td>${request.status}</td>
        <td>${request.totalBill}</td>
        <td><button class="btn btn-primary btn-sm details-button" data-id="${request.id}">Details</button></td> <!-- Thêm nút Details -->
      </tr>
    `;
    dataBody.insertAdjacentHTML('beforeend', requestHtml);
  });

  // Lấy tất cả các nút "Details"
  const detailsButtons = document.querySelectorAll('.details-button');

  detailsButtons.forEach(button => {
    button.addEventListener('click', () => {
        // Lấy thông tin về hàng tương ứng
        const row = button.closest('tr');
        const id = button.dataset.id;

        // Chuyển hướng sang trang chi tiết với id tương ứng
        window.location.href = `RefundRequestDetail.html?id=${id}`;
    });
});
}
// Hàm chuyển đổi định dạng thời gian
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
const pagination = document.getElementById('pagination');
const totalPages =3;
function renderPagination(totalPages,keyword) {
  pagination.innerHTML = ''; 
  for (let i = 1; i <= totalPages; i++) {
    
    const pageButton = document.createElement('button');
    pageButton.textContent = i;
    pageButton.addEventListener('click', () => {
      fetchRefundRequests(i,keyword);
    });
    pagination.appendChild(pageButton);
  }
}

function fetchRefundRequests(pageIndex,keyword) {
  //console.log(keyword+"me") pa(keyword);

  const apiUrl = `https://localhost:7270/ManageOrder/GeTransactionWithPaging?pageIndex=${pageIndex}&pageSize=${pageSize}&searchKeyword=${keyword}`; // Sử dụng biến pageSize đã được định nghĩa
  fetch(apiUrl)
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(data => {
      renderRefundRequests(data);
    })
    .catch(error => {
      console.error('Error fetching data:', error);
    });
}
function pa(keyword) {
  let countApiUrl;
  const apiUrlCount = `https://localhost:7270/ManageOrder/count?searchKeyword=${keyword}`;
  fetch(apiUrlCount)
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(data => {
      countApiUrl = data;
      // Sau khi lấy được dữ liệu, bạn có thể tiếp tục thực hiện các thao tác khác ở đây
      fetch(apiUrl)
        .then(response => {
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
          return response.json();
        })
        .then(data => {
          const totalPages = Math.ceil(countApiUrl / pageSize);
          renderPagination(totalPages,keyword);
          fetchRefundRequests(1, keyword);
        })
        .catch(error => {
          console.error('Error fetching data:', error);
        });
    })
    .catch(error => {
      console.error('Error fetching data:', error);
    });
}

// Gọi hàm pa với giá trị của keyword
pa(keyword);
