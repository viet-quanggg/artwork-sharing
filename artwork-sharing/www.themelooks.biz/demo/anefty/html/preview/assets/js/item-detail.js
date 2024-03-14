window.onload = async function LoadItemDetail() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const artworkId = urlParams.get('id');
    var url = "https://localhost:7270/api/Artwork/" + artworkId;

    var request = new Request(url);
    var rs = await fetch(request);

    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('price-artwork').innerText = pTxt.price;
        document.getElementById('name-artist').innerText = pTxt.artist.user.name;
        document.getElementById('artwork-tilte').innerText = pTxt.name;
        document.getElementById('artwork-description').innerText = pTxt.description;

        await CheckLike(artworkId);

        pTxt.comments.forEach(element => {
            document.getElementById('u-cmt').innerHTML += `     <li>
            <h6><b>${element.commentedUser.name}</b></h6>
            <h6>${element.content}</h6>
        </li>`
        });
        document.getElementById('btn-send-cmt').innerHTML = `                                <button onclick="Send-Cmt('${artworkId}')" type="button" class="btn btn-primary">Comments</button>`
    }
}

async function CheckLike(id) {
    var uid = localStorage.getItem("uid");
    var url = "https://localhost:7270/api/Like?artworkId=" + id + "&" + "userId=" + uid;
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        console.log(pTxt);
        if (pTxt.result == "true") {
            document.getElementById('like-artwork').innerHTML = `  <div onclick="SendLike('${id}')"  class="love-react style--two is-active"></div>
            <div class="love-count">${pTxt.likeViewModels.length}</div>`
        } else {
            document.getElementById('like-artwork').innerHTML = `  <div onclick="SendLike('${id}')"  class="love-react style--two"></div>
            <div class="love-count">${pTxt.likeViewModels.length}</div>`
        }
    }
}

async function SendLike(id) {
    var obj = {
        "userId": localStorage.getItem("uid"),
        "artworkId": id,
    }
    var url = "https://localhost:7270/api/Like";
    var resquest = new Request(url);
    var resultFet = await fetch(url,
        {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Token': token,
            },
            body: JSON.stringify(obj)
        }
    );
    if (resultFet.ok) {
        await CheckLike(id);
    }
}