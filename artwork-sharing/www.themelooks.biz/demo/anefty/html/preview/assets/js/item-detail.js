window.onload = async function LoadItemDetail() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    var token = localStorage.getItem('token');
    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);

    const artworkId = urlParams.get('id');
    var url = "https://localhost:7270/api/Artwork/" + artworkId;

    var request = new Request(url, {
        method: 'GET',
        headers: headers
    });
    var rs = await fetch(request);

    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        console.log(pTxt);
        document.getElementById('price-artwork').innerText = pTxt.price;
        // document.getElementById('name-artist').innerText = pTxt.artist.user.name;
        document.getElementById('artwork-tilte').innerText = pTxt.name;
        document.getElementById('artwork-description').innerText = pTxt.description;
        var h5HTML = '<h5 id="name-artist"><a href="ArtistProfile.html?id=' + pTxt.artistId + '">' + pTxt.artist.user.name + '</a></h5>';
        document.getElementById('artistTab').insertAdjacentHTML('beforeend', h5HTML);
        document.getElementById('btnPayment').innerHTML = ` <button  onclick="Payment('${pTxt.id}')" class="btn btn-border btn-sm"><img
        src="assets/img/icons/btn-buy-now-icon.svg" alt="" class="svg"> Buy Now</button>`

        await GetUser();
        await CheckLike(artworkId);
        await GetComments(artworkId);

        document.getElementById('btn-send-cmt').innerHTML = `                                <button onclick="SendCmt('${artworkId}')" type="button" class="btn btn-primary">Comments</button>`
    }
}
async function CheckLike(id) {
    var token = localStorage.getItem('token');

    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);

    var url = "https://localhost:7270/api/Like?artworkId=" + id;
    var request = new Request(url, {
        method: 'GET',
        headers: headers
    });

    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        if (pTxt.result + "" == "true") {
            document.getElementById('like-artwork').innerHTML = `  <div onclick="SendLike('${id}')"  class="love-react style--two is-active"></div>
            <div class="love-count">${pTxt.likeViewModels.length}</div>`
        } else {
            document.getElementById('like-artwork').innerHTML = `  <div onclick="SendLike('${id}')"  class="love-react style--two"></div>
            <div class="love-count">${pTxt.likeViewModels.length}</div>`
        }
    }
}

async function SendLike(id) {
    var token = localStorage.getItem('token');

    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);
    headers.append('Content-Type', 'application/json');

    var obj = {
        "ArtworkId": id,
    }
    var url = "https://localhost:7270/api/Like";
    var resquest = new Request(url);
    var resultFet = await fetch(url,
        {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(obj)
        }
    );
    if (resultFet.ok) {
        await CheckLike(id);
    } else {
        // redirect login
    }
}

async function GetComments(id) {

    var token = localStorage.getItem('token');

    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);

    var url = "https://localhost:7270/api/Comment/" + id;
    var request = new Request(url, {
        method: 'GET',
        headers: headers
    });
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('u-cmt').innerHTML = "";
        var aa = 0
        pTxt.forEach(element => {

            document.getElementById('u-cmt').innerHTML += `<li > 
            <h6><b>${element.commentedUser.name}</b></h6>   
            <h6>${element.content}</h6>
        </li>`
        });
    }
}

async function Payment(id) {
    var token = localStorage.getItem('token');

    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);
    headers.append('Content-Type', 'application/json');
    var obj = {
        "ArtworkId": id,
        "PaymentMethodId": "1AC6A120-F067-4BAC-A413-1DD61A4B997B"
    }
    var url = "https://localhost:7270/api/Transaction";
    var request = new Request(url, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(obj)

    });

    var rs = await fetch(request);

    if (rs.ok) {
        var txt = await rs.text();
        window.location.href = txt + "";
    } else {
        alert('Can payment');
    }
}

async function SendCmt(id) {

    var token = localStorage.getItem('token');

    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);
    headers.append('Content-Type', 'application/json');

    var content = document.getElementById('content-cmt').value;
    if ((content + "").trim() == "") {
        return;
    }
    var obj = {
        "ArtworkId": id,
        "Content": content
    }
    var url = "https://localhost:7270/api/Comment";
    var resquest = new Request(url);
    var resultFet = await fetch(url,
        {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(obj)
        }
    );
    if (resultFet.ok) {
        document.getElementById('content-cmt').value = "";
        await GetComments(id);
    }
}

async function GetUser() {
    var token = localStorage.getItem('token');

    var headers = new Headers();
    headers.append('Authorization', 'Bearer ' + token);
    var url = "https://localhost:7270/api/usercontroller/getuser";
    var request = new Request(url, {
        method: 'GET',
        headers: headers
    });
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('name-u').innerText = pTxt.name
    }
}

