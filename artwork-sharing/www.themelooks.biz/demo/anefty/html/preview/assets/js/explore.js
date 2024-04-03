window.onload = async function () {
    await LoadArtwork();
}

async function LoadArtwork() {
    var ic = sessionStorage.getItem('ic') + "";
    var icint

    if (ic == undefined || ic == null) {
        icint = 0;
    } else {
        icint = parseInt(ic)
    }
    if (icint + "" == "NaN") {
        icint = 0;
    }
    var url = "https://localhost:7270/api/Artwork?IsPopular=true&IsAscRecent=true&PageIndex=" + icint;
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('artworkitem').innerHTML = ` <div class="col-lg-4 col-md-6">
        <div class="single-product mb-30"><img src="assets/img/product/product4.png" alt="">
            <div class="product-content">
                <div class="product-top">
                    <h5>Colorful Abstract Painting</h5>
                    <div class="d-flex justify-content-between">
                        <h6>0.69 ETH</h6>
                    </div>
                </div>
                <div class="product-bottom">
                    <div class="button-group">
                       <a href="item-details.html" class="btn btn-border btn-sm"><img
                                src="assets/img/icons/judge-icon.svg" alt="" class="svg">Buy now</a>
                    </div>
                </div>
            </div>
        </div>
    </div>`;
        pTxt.forEach(element => {
            document.getElementById('artworkitem').innerHTML += `   <div class="col-lg-4 col-md-6">
            <div class="single-product mb-30"><img src="assets/img/product/product4.png" alt="">
                <div class="product-content">
                    <div class="product-top">
                        <h5>${element.name}</h5>
                        <div class="d-flex justify-content-between">
                            <h6>${element.price}</h6>
                        </div>
                    </div>
                    <div class="product-bottom">
                        <div class="button-group">
                           <a href="item-details.html?id=${element.id}" class="btn btn-border btn-sm"><img
                                    src="assets/img/icons/judge-icon.svg" alt="" class="svg">Details</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>`
        });
    }
}
async function SearchItem() {
    var ic = sessionStorage.getItem('ic') + "";
    var icint
    if (ic == undefined || ic == null) {
        icint = 0;
    } else {
        icint = parseInt(ic)
    }
    if (icint + "" == "NaN") {
        icint = 0;
    }
    var popular = document.getElementById('pop').checked
    var recent = document.getElementById('rec').checked
    console.log('pop' + popular);
    console.log('rec' + recent);

    var ks = document.getElementById('ksid').value;
    var url = "";
    if (ks + "" == "") {
        url = "https://localhost:7270/api/Artwork?IsPopular=true&IsAscRecent=" + recent + "&PageIndex=" + icint;
    } else {
        url = "https://localhost:7270/api/Artwork?IsPopular=true&IsAscRecent=" + recent + "&PageIndex=" + icint + "&description=" + ks + "&name=" + ks;
    }
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('artworkitem').innerHTML = ` <div class="col-lg-4 col-md-6">
        <div class="single-product mb-30"><img src="assets/img/product/product4.png" alt="">
            <div class="product-content">
                <div class="product-top">
                    <h5>Colorful Abstract Painting</h5>
                    <div class="d-flex justify-content-between">
                        <h6>0.69 ETH</h6>
                    </div>
                </div>
                <div class="product-bottom">
                    <div class="button-group">
                       <a href="item-details.html" class="btn btn-border btn-sm"><img
                                src="assets/img/icons/judge-icon.svg" alt="" class="svg">Buy now</a>
                    </div>
                </div>
            </div>
        </div>
    </div>`;
        pTxt.forEach(element => {
            document.getElementById('artworkitem').innerHTML += `   <div class="col-lg-4 col-md-6">
            <div class="single-product mb-30"><img src="assets/img/product/product4.png" alt="">
                <div class="product-content">
                    <div class="product-top">
                        <h5>${element.name}</h5>
                        <div class="d-flex justify-content-between">
                            <h6>${element.price}</h6>
                        </div>
                    </div>
                    <div class="product-bottom">
                        <div class="button-group">
                           <a href="item-details.html" class="btn btn-border btn-sm"><img
                                    src="assets/img/icons/judge-icon.svg" alt="" class="svg">Buy now</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>`
        });
    }
}


async function GetMore() {
    var ic = sessionStorage.getItem('ic');
    var icint
    if (ic == undefined || ic == null) {
        icint = 0;
    } else {
        icint = parseInt(ic)
    }
    if (icint + "" == "NaN") {
        icint = 0;
    }
    icint = icint + 1;
    console.log(icint)
    sessionStorage.setItem('ic', icint);
    await LoadArtwork();
}