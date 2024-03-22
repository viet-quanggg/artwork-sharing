$(document).ready(async function () {
    GetArtwork();
    CountArtwork();
    CountArtist();
    CountTransaction();
})

async function GetArtwork() {
    try {
        const response = await $.ajax({
            url: 'https://localhost:7270/api/Artwork?ArtistId=ee7cc9e0-3938-43f8-973f-3210c61851cc&IsPopular=true',
            type: 'GET',
        });
        const productContainer = document.getElementById('product-container');
        productContainer.replaceChildren(); // Clear the container
        response.forEach(element => {
            const swiperSlide = document.createElement('div');
            swiperSlide.classList.add('swiper-slide', 'm-lg-2');

            // Create the inner elements based on the structure you provided
            swiperSlide.innerHTML = `
                <div class="col-xl-12 col-lg-12 col-md-12 element-item artwork memes">
                    <div class="single-product style--three">
                        <div class="product-img"><img src="${element.mediaContents[0].media}" alt=""></div>
                        <div class="product-content">
                            <div class="owners">
                                <a href="profile.html" title="${element.artist.user.name}">
                                    <img src="assets/img/media/owner1.png" alt="">
                                </a>
                                <!-- Add more owner images dynamically here if needed -->
                                <span class="user-status white"><img src="assets/img/icons/check2.svg" class="svg" alt=""></span>
                            </div>
                            <div class="product-top">
                                <h5>${element.name}</h5>
                                <div class="d-flex justify-content-between">
                                    <h6 class="c1">${element.price} $</h6>
                                    <h6>${element.mediaContents[0].capacity} to 100</h6>
                                </div>
                            </div>
                        
                        </div>
                    </div>
                </div>
            `;

            // Append the swiper-slide to the product-container
            productContainer.appendChild(swiperSlide);
        });


    } catch (error) {
        console.log(error);
    }
}

async function CountArtwork() {
    try {
        const response = await $.ajax({
            url: 'https://localhost:7270/api/Artwork',
            type: 'GET',
        });
        let count = response.length;
        if (count >= 1000) {
            count = Math.floor(count / 1000) + 'k';
            var total = document.getElementById('CountArtwork');
            total.innerHTML = `
            <h4><span class="counter">${count}</span>k<span class="c1">+</span></h4>
                                <h6>Artwork</h6>
                                ` ;
        }
        else{
            var total = document.getElementById('CountArtwork');
            total.innerHTML = `
            <h4><span class="counter">${count}</span><span class="c1">+</span></h4>
                                <h6>Artwork</h6>
                                ` ;
        }
    
    } catch (error) {
        console.log(error);
    }
}

async function CountArtist(){
    try {
        const response = await $.ajax({
            url: 'https://localhost:7270/GetArtist',
            type: 'GET',
        });
        let count = response.length;
        if (count >= 1000) {
            count = Math.floor(count / 1000) + 'k';
            var total = document.getElementById('CountArtist');
            total.innerHTML = `
            <h4><span class="counter">${count}</span>k<span class="c1">+</span></h4>
                                <h6>Artist</h6>
                                ` ;
        }
            var total = document.getElementById('CountArtist');
            total.innerHTML = `
            <h4><span class="counter">${count}</span><span class="c1">+</span></h4>
                                <h6>Artist</h6>
                                ` ;
        
            const ArtistContainer = document.getElementById('artistSlider');
            ArtistContainer.replaceChildren();
            response.forEach(e => {
                const swiperSlide = document.createElement('div');
                swiperSlide.classList.add('swiper-slide');
                swiperSlide.innerHTML = `
                <div class="featured-artists">
                                    <div class="tp-img"><img src=${e.user.photoUrl} alt=""></div>
                                    <div class="artists-content text-center">
                                        <h5 class="text-white">${e.user.name}</h5><a href="#" class="btn btn-follow">+
                                            Follow</a>
                                    </div>
                                </div>
                `;
                ArtistContainer.appendChild(swiperSlide);
            })
    } catch (error) {
        console.log(error);
    }
}

async function CountTransaction(){
    try {
        const response = await $.ajax({
            url: 'https://localhost:7270/Count?timeRange=day',
            type: 'GET',
        });
        let count = response.length;
        if (count >= 1000) {
            count = Math.floor(count / 1000) + 'k';
            var total = document.getElementById('CountTransaction');
            total.innerHTML = `
            <h4><span class="counter">${count}</span>k<span class="c1">+</span></h4>
                                <h6>Auction</h6>
                                ` ;
        }
        else{
            var total = document.getElementById('CountTransaction');
            total.innerHTML = `
            <h4><span class="counter">${count}</span><span class="c1">+</span></h4>
                                <h6>Auction</h6>
                                ` ;
        }
    
    } catch (error) {
        console.log(error);
    }
}
