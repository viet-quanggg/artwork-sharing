$(function () {

  try {
    compareYearAndMonthlyValues();
    var val = "year";
    getData(val);
    getTransactionbyDay();
    $('#transactionlist').on('change', '#Softchart', function (e) {
      e.preventDefault();
      getTransactionbyDay();
    });
    $('#transactionbyday').on('change', '#Softchart1', function (e) {
      e.preventDefault();
      var val = $(this).val();

    });
  } catch (error) {
    console.error('Error:', error);
  }
})

async function getData(val) {
  try {
    const response = await $.ajax({
      url: 'https://localhost:7270/Transaction/Chart?timeRange=' + val,
      type: 'GET',
    });

    $('#transactionChart').empty();

    // Initialize an array to store combined date and totalBill objects
    const dateBillPairs1 = [];

    // Extract data from the response
    response.forEach(item => {
      const createdDate = new Date(item.createdDate);
      let key;

      // Format the key based on the selected time range
      if (val === 'day') {
        key = createdDate.toLocaleDateString('VN', { day: 'numeric', month: 'numeric', year: 'numeric' });
      } else if (val === 'month') {
        key = createdDate.toLocaleDateString('VN', { month: 'numeric', year: 'numeric' });
      } else if (val === 'year') {
        key = createdDate.toLocaleDateString('VN', { year: 'numeric' });
      }

      // Find or create the dateBillPair for the key
      let dateBillPair1 = dateBillPairs1.find(pair => pair.date === key);
      if (!dateBillPair1) {
        dateBillPair1 = { date: key, totalBill: 0 };
        dateBillPairs1.push(dateBillPair1);
      }
      dateBillPair1.totalBill += item.totalBill;
    });

    dateBillPairs1.sort((a, b) => {
      const dateA = new Date(a.date.split('/').reverse().join('-'));
      const dateB = new Date(b.date.split('/').reverse().join('-'));
      return dateA - dateB;
    });
    const dates = dateBillPairs1.map(pair => pair.date);
    const totalBills = dateBillPairs1.map(pair => pair.totalBill);

    // Create the chart options
    const chartOptions = {
      chart: {
        type: 'bar',
      },
      series: [{
        name: 'Total Bill',
        data: totalBills,
      }],
      xaxis: {
        categories: dates,
      },
    };

    // Render the chart using ApexCharts
    const chart = new ApexCharts(document.querySelector("#transactionChart"), chartOptions);
    chart.render();

  } catch (error) {
    console.error('Error fetching transaction data:', error);
    $('#transactionChart').text('Error loading chart data.');
  }
}


async function compareYearAndMonthlyValues() {
  try {
    const response = await $.ajax({
      url: 'https://localhost:7270/Transaction/Chart?timeRange=year',
      type: 'GET',
    });

    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();
    const currentMonth = currentDate.getMonth() + 1;

    const dateBillPairs = [];
    response.forEach(item => {
      const createdDate = new Date(item.createdDate);
      const key = createdDate.toLocaleDateString('en-US', { year: 'numeric', month: 'numeric', day: 'numeric' });
      let dateBillPair = dateBillPairs.find(pair => pair.date === key);
      if (!dateBillPair) {
        dateBillPair = { date: key, totalBill: 0 };
        dateBillPairs.push(dateBillPair);
      }
      dateBillPair.totalBill += item.totalBill;
    });

    dateBillPairs.sort((a, b) => new Date(a.date) - new Date(b.date));

    const currentYearData = dateBillPairs.filter(pair => new Date(pair.date).getFullYear() === currentYear);
    const previousYearData = dateBillPairs.filter(pair => new Date(pair.date).getFullYear() === currentYear - 1);

    const currentYearTotal = currentYearData.reduce((acc, pair) => acc + pair.totalBill, 0);
    const previousYearTotal = previousYearData.reduce((acc, pair) => acc + pair.totalBill, 0);
    let yearPercentageChange = ((currentYearTotal - previousYearTotal) / previousYearTotal) * 100;
    if (previousYearTotal <= 0) {
      yearPercentageChange = 100;
    }

    const formattedYearPercentageChange = new Intl.NumberFormat('en-US', {
      style: 'percent',
      minimumFractionDigits: 1,
    }).format(yearPercentageChange / 100);

    const averageCurrentYear = currentYearData.reduce((acc, pair) => acc + pair.totalBill, 0) / 12;
    const averagePreviousYear = previousYearData.reduce((acc, pair) => acc + pair.totalBill, 0) / 12;

    let averagePercentageChange = ((averageCurrentYear - averagePreviousYear) / averagePreviousYear) * 100;
    console.log(averagePreviousYear);

    if (averagePreviousYear <= 0) {
      averagePercentageChange = 100;
    }
    const formattedAveragePercentageChange = new Intl.NumberFormat('en-US', {
      style: 'percent',
      minimumFractionDigits: 1,
    }).format(averagePercentageChange / 100);

    var total = currentYearTotal - previousYearTotal;
    var total1 = averageCurrentYear - averagePreviousYear;
    if (total > 0) {
      $("#Iconyear").addClass("me-1 rounded-circle round-20 d-flex align-items-center justify-content-center bg-light-success");
      $("#Iconyear1").addClass("ti-arrow-up-left text-success");
    } else if (total < 0) {
      $("#Iconyear").addClass("me-2 rounded-circle round-20 d-flex align-items-center justify-content-center bg-light-danger");
      $("#Iconyear1").addClass("ti-arrow-down-right text-danger");
    }
    if (total1 < 0) {
      $("#Iconmonth").addClass("me-2 rounded-circle round-20 d-flex align-items-center justify-content-center bg-light-danger");
      $("#Iconmonth1").addClass("ti-arrow-down-right text-danger");
    } else if (total1 > 0) {
      $("#Iconmonth").addClass("me-1 rounded-circle round-20 d-flex align-items-center justify-content-center bg-light-success");
      $("#Iconmonth1").addClass("ti-arrow-up-left text-success");
    }

    $("#totalyear").text(total + " $");
    $("#percentyear").text(formattedYearPercentageChange);
    $("#lastyear").text(currentYear - 1);
    $("#yearnow").text(currentYear);
    $("#monthlyearn").text(total1.toFixed(2) + " $");
    $("#percentmonthly").text(formattedAveragePercentageChange);
    $("#lastyear1").text(currentYear - 1);

  } catch (error) {
    console.error('Error comparing values:', error);
  }
}

async function displaytransactioninday() {
  try {
    const response = await $.ajax({
      url: 'https://localhost:7270/Transaction/Chart?timeRange=day',
      type: 'GET',
    });
    response.forEach(
      getnameArtist(id)
    );
  } catch (error) {
    console.log(error);
    throw error;
  }
}
async function getnameArtist(id) {
  const response = await $.ajax({
    url: `https://localhost:7270/GetNameArtist/${All.artistId}?page=1`,
    type: 'GET',
  });
}

async function getTransactionbyDay() {
  try {
    const response = await $.ajax({
      url: 'https://localhost:7270/Transaction/Chart?timeRange=day',
      type: 'GET',
    });


    response.forEach(item => {
      console.log (item.artworkId);
      GetArtwork(item);
    });

  } catch (error) {
    console.error('Error fetching transaction data:', error);
    $('#transactionChart').text('Error loading chart data.');
  }
}

async function GetArtwork(Item) {
  try {
    const response = await $.ajax({
      url: 'https://localhost:7270/ArtworkbyId?id='+ Item.artworkId,
      type: 'GET',
    });
    console.log(response);
  } catch (error) {
    console.error('Error fetching transaction data:', error);
  }

}


