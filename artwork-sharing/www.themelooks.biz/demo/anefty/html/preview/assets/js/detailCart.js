window.onload = async function(){
    await loadArtwork();
}

async function loadArtwork(){
    try {
        const queryString = window.location.search;
        const response = await fetch("https://localhost:7270/api/Payment?"+queryString);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        // Xử lý dữ liệu nhận được ở đây
        console.log(data);
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
    }
}
