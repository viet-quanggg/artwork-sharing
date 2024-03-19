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

        await GetUser();
        await CheckLike(artworkId);
        await GetComments(artworkId);

        document.getElementById('btn-send-cmt').innerHTML = `                                <button onclick="SendCmt('${artworkId}')" type="button" class="btn btn-primary">Comments</button>`
    }
}

async function CheckLike(id) {
    var uid = sessionStorage.getItem("uid");
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
        "UserId": sessionStorage.getItem("uid"),
        "ArtworkId": id,
    }
    var url = "https://localhost:7270/api/Like";
    var resquest = new Request(url);
    var resultFet = await fetch(url,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(obj)
        }
    );
    if (resultFet.ok) {
        await CheckLike(id);
    }
}

async function GetComments(id) {
    var url = "https://localhost:7270/api/Comment/" + id;
    var request = new Request(url);
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

async function SendCmt(id) {
    var content = document.getElementById('content-cmt').value;
    if ((content + "").trim() == "") {
        return;
    }
    var obj = {
        "CommentedUserId": sessionStorage.getItem("uid") + "",
        "ArtworkId": id,
        "Content": content
    }
    var url = "https://localhost:7270/api/Comment";
    var resquest = new Request(url);
    var resultFet = await fetch(url,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(obj)
        }
    );
    if (resultFet.ok) {
        document.getElementById('content-cmt').value="";
        await GetComments(id);
    }
}
async function GetUser() {
    var url = "https://localhost:7270/api/usercontroller/getuser?userId=" + sessionStorage.getItem("uid");
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('name-u').innerText = pTxt.name
    }
}