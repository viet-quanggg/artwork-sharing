const pageSize = 3; // Định nghĩa pageSize ở đầu tập tin hoặc ở phạm vi có thể truy cập trước khi sử dụng

// Define the URL of the API endpoint
const apiUrl = 'https://localhost:7270/RefundRequest?pageIndex=1&pageSize=3';

// Function to render refund requests on the HTML template
function renderRefundRequests(refundRequests) {
  const dataBody = document.getElementById('dataBody');
  dataBody.innerHTML = '';
  refundRequests.forEach(request => {
    const requestHtml = `
      <tr>
        <td>${request.transactionId}</td>
        <td>${request.refundRequestDate}</td>
        <td>${request.description}</td>
        <td>${request.reason}</td>
        <td>${request.status}</td>
        <td>${request.transaction.totalBill}</td>
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

const pagination = document.getElementById('pagination');
const totalPages =3;
function renderPagination(totalPages) {
  pagination.innerHTML = '';
  for (let i = 1; i <= totalPages; i++) {
    
    const pageButton = document.createElement('button');
    pageButton.textContent = i;
    pageButton.addEventListener('click', () => {
      fetchRefundRequests(i);
    });
    pagination.appendChild(pageButton);
  }
}

function fetchRefundRequests(pageIndex) {
  const apiUrl = `https://localhost:7270/RefundRequest?pageIndex=${pageIndex}&pageSize=${pageSize}`; // Sử dụng biến pageSize đã được định nghĩa
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

let countApiUrl;

fetch('https://localhost:7270/RefundRequest/count')
  .then(response => {
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    return response.json();
  })
  .then(data => {
    countApiUrl = data;
  })
  .catch(error => {
    console.error('Error fetching data:', error);
  });

fetch(apiUrl)
  .then(response => {
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    return response.json();
  })
  .then(data => {
    
    const totalPages = Math.ceil(countApiUrl / pageSize);
    
    renderPagination(totalPages);
    renderRefundRequests(data);
  })
  .catch(error => {
    console.error('Error fetching data:', error);
  });
 