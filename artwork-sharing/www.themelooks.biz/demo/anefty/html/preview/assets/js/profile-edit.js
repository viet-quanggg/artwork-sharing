window.onload = async function LoadProfile() {
    var uId = sessionStorage.getItem('uid');
    if ((uId + "").trim() == "") {
        location.replace("../login.html");
    }
    await GetUser();


}
async function GetUser() {
    var url = "https://localhost:7270/api/usercontroller/getuser?userId=" + sessionStorage.getItem("uid");
    var request = new Request(url);
    var rs = await fetch(request);
    if (rs.ok) {
        var txt = await rs.text();
        var pTxt = JSON.parse(txt);
        document.getElementById('name').value = pTxt.name;
        document.getElementById('big-name').innerText = pTxt.name;
        document.getElementById('custom-phone').value = pTxt.phone;
    }
}

async function updtprofile() {
    var name = document.getElementById('name').value;
    var phone = document.getElementById('custom-phone').value;
    
    if ((name + "").trim() == ""|| (phone + "").trim() == "") {
        return;
    }
    var obj = {
        "name": name,
        "phone": phone
    }
    var url = "https://localhost:7270/api/usercontroller/update/";+ sessionStorage.getItem("uid");
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
        await GetUser() ;
    }
}