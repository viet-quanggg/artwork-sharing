"use strict";!function(e){function t(){e(window).width()<1200?e(".nav-wrapper .nav-wrap-inner").hide():e(".nav-wrapper .nav-wrap-inner").show()}e('ul.nav li a[href="#"]').on("click",function(e){e.preventDefault()}),e(".nav-wrapper").menumaker({title:"<span></span>",format:"multitoggle"}),e(e(window)).on("scroll",function(){e("ul.nav").hasClass("open")||e("#menu-button").removeClass("menu-opened")}),e(window).on("resize",function(){t()}),t();var a=e(".header").outerHeight();e(".header-main.love-sticky").parent(".header").css({height:a+"px"});var n=e(".love-sticky");function i(){var t=e(".nav-wrapper .nav > li > ul"),a=e(".nav-wrapper .nav > li > ul ul");t.each(function(){e(window).width()>1199&&e(this).offset().left+e(this).width()>e(window).width()&&e(this).css({left:"auto",right:"0"})}),a.each(function(){e(window).width()>1199&&e(this).offset().left+e(this).width()>e(window).width()&&e(this).css({left:"auto",right:"100%"})})}e(window).on("scroll",function(){e(window).scrollTop()<100?n.removeClass("sticky fadeInDown animated"):n.addClass("sticky fadeInDown animated")}),i(),e(window).resize(i),e("[data-bg-img]").css("background-image",function(){return'url("'+e(this).data("bg-img")+'")'}).removeAttr("data-bg-img").addClass("bg-img");var o=function(e,t){return void 0===e?t:e};e(".swiper").each(function(){var t=e(this);new Swiper(t[0],{slidesPerView:o(t.data("swiper-items"),1),spaceBetween:o(t.data("swiper-margin"),0),loop:o(t.data("swiper-loop"),!0),autoHeight:o(t.data("swiper-auto-height"),!1),autoplay:o(t.data("swiper-autoplay"),!1),breakpoints:o(t.data("swiper-breakpoints"),{}),centeredSlides:o(t.data("swiper-center"),!1),direction:o(t.data("swiper-direction"),"horizontal"),effect:o(t.data("swiper-effect"),"slide"),navigation:{nextEl:o(t.data("swiper-navigation-next"),".swiper-button-next"),prevEl:o(t.data("swiper-navigation-prev"),".swiper-button-prev")},pagination:{el:o(t.data("swiper-pagination-el"),".swiper-pagination"),dynamicBullets:o(t.data("swiper-pagination-dynamic-bullets"),!0),clickable:o(t.data("swiper-pagination-clickable"),!0)}})}),e("img.svg").each(function(){var e=jQuery(this),t=e.attr("id"),a=e.attr("class"),n=e.attr("src");jQuery.get(n,function(n){var i=jQuery(n).find("svg");void 0!==t&&(i=i.attr("id",t)),void 0!==a&&(i=i.attr("class",a+" replaced-svg")),!(i=i.removeAttr("xmlns:a")).attr("viewBox")&&i.attr("height")&&i.attr("width")&&i.attr("viewBox","0 0 "+i.attr("height")+" "+i.attr("width")),e.replaceWith(i)},"xml")});var s,r=e('[data-trigger="map"]');r.length&&(s=r.data("map-options"),window.initMap=function(){r.css("min-height","530px"),r.each(function(){var t,a,n,i,o=e(this);s=o.data("map-options"),a=parseFloat(s.latitude,10),n=parseFloat(s.longitude,10),i=parseFloat(s.zoom,10),t=new google.maps.Map(o[0],{center:{lat:a,lng:n},zoom:i,scrollwheel:!1,disableDefaultUI:!0,zoomControl:!0,styles:[{featureType:"water",elementType:"geometry",stylers:[{color:"#e9e9e9"},{lightness:17}]},{featureType:"landscape",elementType:"geometry",stylers:[{color:"#f5f5f5"},{lightness:20}]},{featureType:"road.highway",elementType:"geometry.fill",stylers:[{color:"#ffffff"},{lightness:17}]},{featureType:"road.highway",elementType:"geometry.stroke",stylers:[{color:"#ffffff"},{lightness:29},{weight:.2}]},{featureType:"road.arterial",elementType:"geometry",stylers:[{color:"#ffffff"},{lightness:18}]},{featureType:"road.local",elementType:"geometry",stylers:[{color:"#ffffff"},{lightness:16}]},{featureType:"poi",elementType:"geometry",stylers:[{color:"#f5f5f5"},{lightness:21}]},{featureType:"poi.park",elementType:"geometry",stylers:[{color:"#dedede"},{lightness:21}]},{elementType:"labels.text.stroke",stylers:[{visibility:"on"},{color:"#ffffff"},{lightness:16}]},{elementType:"labels.text.fill",stylers:[{saturation:36},{color:"#333333"},{lightness:40}]},{elementType:"labels.icon",stylers:[{visibility:"off"}]},{featureType:"transit",elementType:"geometry",stylers:[{color:"#f2f2f2"},{lightness:19}]},{featureType:"administrative",elementType:"geometry.fill",stylers:[{color:"#fefefe"},{lightness:20}]},{featureType:"administrative",elementType:"geometry.stroke",stylers:[{color:"#fefefe"},{lightness:17},{weight:1.2}]}]}),t=new google.maps.Marker({position:{lat:a,lng:n},map:t,animation:google.maps.Animation.DROP,draggable:!1,icon:"assets/img/map-marker.png"})})},initMap()),e(window).on("load",function(){e(".preloader-svg").animate({"stroke-dasharray":890},3e3,function(){e(this).find("path").css("fill","#FF0076"),e(".preloader").fadeOut(200)})}),e(".contact-form-wrapper").on("","form",function(t){t.preventDefault();var a=e(this);e.post(a.attr("action"),a.serialize(),function(t){t=e.parseJSON(t),a.parent(".contact-form-wrapper").find(".form-response").html("<span>"+t[1]+"</span>")})});var l=e(".back-to-top");if(l.length){var c=function(){e(window).scrollTop()>400?l.addClass("show"):l.removeClass("show")};c(),e(window).on("scroll",function(){c()}),l.on("click",function(t){t.preventDefault(),e("html,body").animate({scrollTop:0},700)})}function t(){var t=e(".mobile-menu-panel .mobile_menu");t.find("ul li").parents(".mobile_menu ul li").addClass("has-sub-item").prepend('<span class="submenu-button"></span>'),t.find(".submenu-button").on("click",function(){e(this).toggleClass("submenu-opened"),e(this).siblings("ul").hasClass("open")?e(this).siblings("ul").removeClass("open").slideUp("fast"):e(this).siblings("ul").addClass("open").slideDown("fast")})}t();var p=e("#particles");e(window).on("load",function(){p.length&&particlesJS("particles",{particles:{number:{value:200,density:{enable:!0,value_area:800}},color:{value:"#ffffff"},shape:{type:"circle",stroke:{width:0,color:"#000000"},polygon:{nb_sides:5},image:{src:"img/github.svg",width:100,height:100}},opacity:{value:.5,random:!1,anim:{enable:!1,speed:1,opacity_min:.1,sync:!1}},size:{value:3,random:!0,anim:{enable:!1,speed:40,size_min:.1,sync:!1}},line_linked:{enable:!1,distance:150,color:"#ffffff",opacity:.4,width:1},move:{enable:!0,speed:2,direction:"none",random:!1,straight:!1,out_mode:"out",bounce:!1,attract:{enable:!1,rotateX:600,rotateY:1200}}},interactivity:{detect_on:"canvas",events:{onhover:{enable:!1,mode:"repulse"},onclick:{enable:!0,mode:"push"},resize:!0},modes:{grab:{distance:400,line_linked:{opacity:1}},bubble:{distance:400,size:40,duration:2,opacity:8,speed:3},repulse:{distance:200,duration:.4},push:{particles_nb:4},remove:{particles_nb:2}}},retina_detect:!0})}),e(".love-react").on("click",function(t){t.preventDefault(),e(this).toggleClass("is-active")}),e(".countdown").countdown({date:"08/16/2022 23:59:59"});var f=e(".popup-video");f.length&&f.magnificPopup({type:"iframe"}),e(document).ready(function(){e("select").niceSelect()}),e(window).on("load",function(){e(document).ready(function(){var t=e(".grid").isotope({itemSelector:".element-item",layoutMode:"fitRows"});e("#filters").on("click","button",function(){var a=e(this).attr("data-filter");t.isotope({filter:a})}),e("#filters").each(function(t,a){var n=e(a);n.on("click","button",function(){n.find(".is-checked").removeClass("is-checked"),e(this).addClass("is-checked")})})})}),e(".btn-follow").on("click",function(t){var a=this;t.preventDefault(),e(this).html('<span class="spinner-border text-light"></span>'),setTimeout(function(){e(a).addClass("followed").html("Followed")},500)});var d=e(".move-img");e(".layer").mousemove(function(e){var t=1*e.pageX/-100,a=1*e.pageY/-80;d.css({transform:"translate3d("+t+"px, "+a+"px,0)"})}),jQuery(document).ready(function(e){e(".counter").counterUp({delay:10,time:2e3})}),e("#copy-link-btn").on("click",function(t){t.preventDefault();var a=e("<input>");e("body").append(a),a.val(e("#get-link").val()).select(),document.execCommand("copy"),a.remove()}),e(".plus").on("click",function(){var t=e(this).parent().find("input"),a=parseInt(t.val());isNaN(a)||t.val(a+1)}),e(".minus").on("click",function(){var t=e(this).parent().find("input"),a=parseInt(t.val());!isNaN(a)&&a>1&&t.val(a-1)}),e(".contact-form-wrap").on("","form",function(t){t.preventDefault();var a=e(this);e.post(a.attr("action"),a.serialize(),function(t){t=e.parseJSON(t),a.parent(".contact-form-wrap").find(".form-response").html("<span>"+t[1]+"</span>")})});var u=(new Date).getFullYear();e(".currentYear").html(u)}(jQuery);