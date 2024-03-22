window.onload = async function(){
    await loadArtwork();
   // await loadData(data);
}
 //loadData();
async function loadArtwork(){
    try {
        const queryString = window.location.search;
        const response = await fetch("https://localhost:7270/api/Payment"+queryString);
        console.log(queryString);
        //console.log(response);

// Tách các phần của query string thành một đối tượng JSON
const cleanQueryString = queryString.startsWith("?") ? queryString.slice(1) : queryString;

// Phân tích query string thành một đối tượng JSON
const params = cleanQueryString.split('&').reduce((acc, param) => {
    const [key, value] = param.split('=');
    acc[key] = decodeURIComponent(value.replace(/\+/g, ' '));
    return acc;
}, {});
loadData(params);
// In ra đối tượng JSON
console.log(params);
        console.log("https://localhost:7270/api/Payment"+queryString);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        
        // Xử lý dữ liệu nhận được ở đây
       // console.log(data.json()+'data');
      
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}

function loadData(params){
    console.log(params + 'ne');
    const packagesContainer = document.getElementById('kkk');
    const type = params.vnp_OrderInfo; // Lấy giá trị type từ query string
    const totalBill = params.vnp_Amount / 100; // Chia cho 100 vì giá trị ban đầu là số tiền (vnd)
    const dateString = params.vnp_PayDate; // Chuỗi thời gian
// Phân tích chuỗi thời gian thành các phần tương ứng
const year = parseInt(dateString.substring(0, 4));
const month = parseInt(dateString.substring(4, 6)) - 1; // Trừ đi 1 vì tháng trong Date bắt đầu từ 0
const day = parseInt(dateString.substring(6, 8));
const hours = parseInt(dateString.substring(8, 10));
const minutes = parseInt(dateString.substring(10, 12));
const seconds = parseInt(dateString.substring(12, 14));

// Tạo đối tượng Date từ các phần đã phân tích
const createdDate = new Date(year, month, day, hours, minutes, seconds);

//createdDate.toLocaleDateString()
    const packageHtml = `
        <tbody>
            <tr>
                <td class="content-block">
                    <h2>Thanks for using our app</h2>
                </td>
            </tr>
            <tr>
                <td class="content-block">
                    <table class="invoice">
                        <tbody>
                        
                          
                            <tr>
                            <td><br><br>${type}</td>
                        </tr>
                            <tr>
                                <td>
                                    <table class="invoice-items" cellpadding="0" cellspacing="0">
                                        <tbody>
                                        <tr>
                                                <td>BankAcountNO</td>
                                                <td class="alignright"> ${params.vnp_BankTranNo}

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>CardType</td>
                                                <td class="alignright"> ${params.vnp_CardType
                                                    }

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Time</td>
                                                <td class="alignright"> ${createdDate
                                                    }

                                                </td>
                                            </tr>
                                            <tr>
                                            <td>BankAcountNO</td>
                                            <td class="alignright"> ${params.vnp_BankTranNo}

                                            </td>
                                        </tr>
                                        <tr>
                                        <td>BankCode</td>
                                        <td class="alignright"> ${params.vnp_BankCode}</td>
                                    </tr>
                                            <tr>
                                                <td>Price</td>
                                                <td class="alignright"> ${totalBill.toFixed(2)} VNG</td>
                                            </tr>
                                            
                                            <tr class="total">
                                                <td class="alignright" width="80%">Total</td>
                                                <td class="alignright"> ${totalBill.toFixed(2)} VNG</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="content-block">
                    <a href="/Home.html">Back Home</a>
                </td>
            </tr>
            <tr>
                <td class="content-block">
                    Company Inc. FPT University
                </td>
            </tr>
        </tbody>
    `;
    
    packagesContainer.insertAdjacentHTML('beforeend', packageHtml);
}