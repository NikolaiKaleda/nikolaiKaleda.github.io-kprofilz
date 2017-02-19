var initPhotoSwipeFromDOM1 = function (gallerySelector) {
    // parse slide data (url, title, size ...) from DOM elements 
    // (children of gallerySelector)
    var parseThumbnailElements = function (el) {
        var thumbElements = el.childNodes,
            numNodes = thumbElements.length,
            items = [],
            figureEl,
            linkEl,
            size,
            item;
        for (var i = 0; i < numNodes; i++) {
            figureEl = thumbElements[i]; // <figure> element
            // include only element nodes 
            if (figureEl.nodeType !== 1) {
                continue;
            }
            linkEl = figureEl.children[0]; // <a> element
            size = linkEl.getAttribute('data-size').split('x');
            // create slide object
            item = {
                src: linkEl.getAttribute('href'),
                w: parseInt(size[0], 10),
                h: parseInt(size[1], 10)
            };
            if (figureEl.children.length > 1) {
                // <figcaption> content
                item.title = figureEl.children[1].innerHTML;
            }
            if (linkEl.children.length > 0) {
                // <img> thumbnail element, retrieving thumbnail url
                item.msrc = linkEl.children[0].getAttribute('src');
            }
            item.el = figureEl; // save link to element for getThumbBoundsFn
            items.push(item);
        }
        return items;
    };
    // find nearest parent element
    var closest = function closest(el, fn) {
        return el && (fn(el) ? el : closest(el.parentNode, fn));
    };
    // triggers when user clicks on thumbnail
    var onThumbnailsClick = function (e) {
        e = e || window.event;
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
        var eTarget = e.target || e.srcElement;
        // find root element of slide
        var clickedListItem = closest(eTarget, function (el) {
            return (el.tagName && el.tagName.toUpperCase() === 'FIGURE');
        });
        if (!clickedListItem) {
            return;
        }
        // find index of clicked item by looping through all child nodes
        // alternatively, you may define index via data- attribute
        var clickedGallery = clickedListItem.parentNode,
            childNodes = clickedListItem.parentNode.childNodes,
            numChildNodes = childNodes.length,
            nodeIndex = 0,
            index;
        for (var i = 0; i < numChildNodes; i++) {
            if (childNodes[i].nodeType !== 1) {
                continue;
            }
            if (childNodes[i] === clickedListItem) {
                index = nodeIndex;
                break;
            }
            nodeIndex++;
        }
        if (index >= 0) {
            // open PhotoSwipe if valid index found
            openPhotoSwipe(index, clickedGallery);
        }
        return false;
    };
    // parse picture index and gallery index from URL (#&pid=1&gid=2)
    var photoswipeParseHash = function () {
        var hash = window.location.hash.substring(1),
        params = {};
        if (hash.length < 5) {
            return params;
        }
        var vars = hash.split('&');
        for (var i = 0; i < vars.length; i++) {
            if (!vars[i]) {
                continue;
            }
            var pair = vars[i].split('=');
            if (pair.length < 2) {
                continue;
            }
            params[pair[0]] = pair[1];
        }
        if (params.gid) {
            params.gid = parseInt(params.gid, 10);
        }
        return params;
    };
    var openPhotoSwipe = function (index, galleryElement, disableAnimation, fromURL) {
        var pswpElement = document.querySelectorAll('.pswp')[0],
            gallery,
            options,
            items;
        items = parseThumbnailElements(galleryElement);
        // define options (if needed)
        options = {
            // define gallery index (for URL)
            galleryUID: galleryElement.getAttribute('data-pswp-uid'),
            getThumbBoundsFn: function (index) {
                // See Options -> getThumbBoundsFn section of documentation for more info
                var thumbnail = items[index].el.getElementsByTagName('img')[0], // find thumbnail
                    pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
                    rect = thumbnail.getBoundingClientRect();
                return { x: rect.left, y: rect.top + pageYScroll, w: rect.width };
            }
        };
        // PhotoSwipe opened from URL
        if (fromURL) {
            if (options.galleryPIDs) {
                // parse real index when custom PIDs are used 
                // http://photoswipe.com/documentation/faq.html#custom-pid-in-url
                for (var j = 0; j < items.length; j++) {
                    if (items[j].pid == index) {
                        options.index = j;
                        break;
                    }
                }
            } else {
                // in URL indexes start from 1
                options.index = parseInt(index, 10) - 1;
            }
        } else {
            options.index = parseInt(index, 10);
        }
        // exit if index not found
        if (isNaN(options.index)) {
            return;
        }
        if (disableAnimation) {
            options.showAnimationDuration = 0;
        }
        // Pass data to PhotoSwipe and initialize it
        gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
        gallery.init();
    };
    // loop through all gallery elements and bind events
    var galleryElements = document.querySelectorAll(gallerySelector);
    for (var i = 0, l = galleryElements.length; i < l; i++) {
        galleryElements[i].setAttribute('data-pswp-uid', i + 1);
        galleryElements[i].onclick = onThumbnailsClick;
    }
    // Parse URL and open gallery if it contains #&pid=3&gid=1
    var hashData = photoswipeParseHash();
    if (hashData.pid && hashData.gid) {
        openPhotoSwipe(hashData.pid, galleryElements[hashData.gid - 1], true, true);
    }
};


$(document).ready(function () {

    $('.input-validation-error').parents('.form-group').addClass('has-error');
    $('.field-validation-error').addClass('text-danger');

    //---- TOP SLIDER SETTINGS -----
    $('#my-slide').DrSlider({
        width: undefined,
        height: undefined,
        userCSS: false,
        transitionSpeed: 1000,
        duration: 4000,
        showNavigation: false,
        classNavigation: undefined,
        navigationColor: '#9F1F22',
        navigationHoverColor: '#D52B2F',
        navigationHighlightColor: '#DFDFDF',
        navigationNumberColor: '#000000',
        positionNavigation: 'out-center-bottom',
        navigationType: 'circle',
        showControl: true,
        classButtonNext: undefined,
        classButtonPrevious: undefined,
        controlColor: '#FFFFFF',
        controlBackgroundColor: '#000000',
        positionControl: 'left-right',
        transition: 'fade',
        showProgress: false,
        progressColor: '#797979',
        pauseOnHover: false,
        onReady: undefined
    });

    $('#my-slide').hover(function () {
        $('.button-slider').fadeIn(400, function () { });
    }, function () {
        $('.button-slider').fadeOut(400, function () { });
    });
    //---- End top slider settings -----


    //----- DESCRIPTIONS LI HOVER -----
    $('.descriptions').find('li').hover(function () {
        $(this).find('.fa').removeClass('fa-circle-thin').addClass('fa-circle');
    }, function () {
        $(this).find('.fa').removeClass('fa-circle').addClass('fa-circle-thin');
    });
    //----- End descriptions li hover -----


    //---- EVERYONE TRUST HOVER ICON -----
    $('.everyoneTrustItem').hover(function () {
        $(this).find(".everyoneTrustIcon").css({
            'color': '#ffde00',
            'background-color': '#1c1c1c',
            'border': '#1c1c1c'
        });
    }, function () {
        $(this).find(".everyoneTrustIcon").css({
            'color': '#1c1c1c',
            'background-color': '#fff',
            'border': '#1c1c1c'
        });
    });
    //----- End everyone trust hover icon -----


    //----- IMAGE GALLERY -----
    var initPhotoSwipeFromDOM = function (gallerySelector) {
        // parse slide data (url, title, size ...) from DOM elements 
        // (children of gallerySelector)
        var parseThumbnailElements = function (el) {
            var thumbElements = el.childNodes,
                numNodes = thumbElements.length,
                items = [],
                figureEl,
                linkEl,
                size,
                item;
            for (var i = 0; i < numNodes; i++) {
                figureEl = thumbElements[i]; // <figure> element
                // include only element nodes 
                if (figureEl.nodeType !== 1) {
                    continue;
                }
                linkEl = figureEl.children[0]; // <a> element
                size = linkEl.getAttribute('data-size').split('x');
                // create slide object
                item = {
                    src: linkEl.getAttribute('href'),
                    w: parseInt(size[0], 10),
                    h: parseInt(size[1], 10)
                };
                if (figureEl.children.length > 1) {
                    // <figcaption> content
                    item.title = figureEl.children[1].innerHTML;
                }
                if (linkEl.children.length > 0) {
                    // <img> thumbnail element, retrieving thumbnail url
                    item.msrc = linkEl.children[0].getAttribute('src');
                }
                item.el = figureEl; // save link to element for getThumbBoundsFn
                items.push(item);
            }
            return items;
        };
        // find nearest parent element
        var closest = function closest(el, fn) {
            return el && (fn(el) ? el : closest(el.parentNode, fn));
        };
        // triggers when user clicks on thumbnail
        var onThumbnailsClick = function (e) {
            e = e || window.event;
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
            var eTarget = e.target || e.srcElement;
            // find root element of slide
            var clickedListItem = closest(eTarget, function (el) {
                return (el.tagName && el.tagName.toUpperCase() === 'FIGURE');
            });
            if (!clickedListItem) {
                return;
            }
            // find index of clicked item by looping through all child nodes
            // alternatively, you may define index via data- attribute
            var clickedGallery = clickedListItem.parentNode,
                childNodes = clickedListItem.parentNode.childNodes,
                numChildNodes = childNodes.length,
                nodeIndex = 0,
                index;
            for (var i = 0; i < numChildNodes; i++) {
                if (childNodes[i].nodeType !== 1) {
                    continue;
                }
                if (childNodes[i] === clickedListItem) {
                    index = nodeIndex;
                    break;
                }
                nodeIndex++;
            }
            if (index >= 0) {
                // open PhotoSwipe if valid index found
                openPhotoSwipe(index, clickedGallery);
            }
            return false;
        };
        // parse picture index and gallery index from URL (#&pid=1&gid=2)
        var photoswipeParseHash = function () {
            var hash = window.location.hash.substring(1),
            params = {};
            if (hash.length < 5) {
                return params;
            }
            var vars = hash.split('&');
            for (var i = 0; i < vars.length; i++) {
                if (!vars[i]) {
                    continue;
                }
                var pair = vars[i].split('=');
                if (pair.length < 2) {
                    continue;
                }
                params[pair[0]] = pair[1];
            }
            if (params.gid) {
                params.gid = parseInt(params.gid, 10);
            }
            return params;
        };
        var openPhotoSwipe = function (index, galleryElement, disableAnimation, fromURL) {
            var pswpElement = document.querySelectorAll('.pswp')[0],
                gallery,
                options,
                items;
            items = parseThumbnailElements(galleryElement);
            // define options (if needed)
            options = {
                // define gallery index (for URL)
                galleryUID: galleryElement.getAttribute('data-pswp-uid'),
                getThumbBoundsFn: function (index) {
                    // See Options -> getThumbBoundsFn section of documentation for more info
                    var thumbnail = items[index].el.getElementsByTagName('img')[0], // find thumbnail
                        pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
                        rect = thumbnail.getBoundingClientRect();
                    return { x: rect.left, y: rect.top + pageYScroll, w: rect.width };
                }
            };
            // PhotoSwipe opened from URL
            if (fromURL) {
                if (options.galleryPIDs) {
                    // parse real index when custom PIDs are used 
                    // http://photoswipe.com/documentation/faq.html#custom-pid-in-url
                    for (var j = 0; j < items.length; j++) {
                        if (items[j].pid == index) {
                            options.index = j;
                            break;
                        }
                    }
                } else {
                    // in URL indexes start from 1
                    options.index = parseInt(index, 10) - 1;
                }
            } else {
                options.index = parseInt(index, 10);
            }
            // exit if index not found
            if (isNaN(options.index)) {
                return;
            }
            if (disableAnimation) {
                options.showAnimationDuration = 0;
            }
            // Pass data to PhotoSwipe and initialize it
            gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
            gallery.init();
        };
        // loop through all gallery elements and bind events
        var galleryElements = document.querySelectorAll(gallerySelector);
        for (var i = 0, l = galleryElements.length; i < l; i++) {
            galleryElements[i].setAttribute('data-pswp-uid', i + 1);
            galleryElements[i].onclick = onThumbnailsClick;
        }
        // Parse URL and open gallery if it contains #&pid=3&gid=1
        var hashData = photoswipeParseHash();
        if (hashData.pid && hashData.gid) {
            openPhotoSwipe(hashData.pid, galleryElements[hashData.gid - 1], true, true);
        }
    };
    //----- End image gallery -----


    //----- ROTATOR REVIEWS -----
    //create the slider
    $('.cd-testimonials-wrapper').flexslider({
        selector: ".cd-testimonials > li",
        animation: "slide",
        controlNav: false,
        slideshow: true,
        smoothHeight: true,
        start: function () {
            $('.cd-testimonials').children('li').css({
                'opacity': 1,
                'position': 'relative'
            });
        }
    });
    //open the testimonials modal page
    $('.cd-see-all').on('click', function () {
        $('.cd-testimonials-all').addClass('is-visible');
    });
    //close the testimonials modal page
    $('.cd-testimonials-all .close-btn').on('click', function () {
        $('.cd-testimonials-all').removeClass('is-visible');
    });
    $(document).keyup(function (event) {
        //check if user has pressed 'Esc'
        if (event.which == '27') {
            $('.cd-testimonials-all').removeClass('is-visible');
        }
    });
    //build the grid for the testimonials modal page
    $('.cd-testimonials-all-wrapper').children('ul').masonry({
        itemSelector: '.cd-testimonials-item'
    });
    //----- End rotatoe reviews -----


    //----- OUR PARTNER HOVER -----
    $('.ourPartnersItem').hover(function () {
        $(this).find(".partnerIconBackground").css({
            'background-color': '#ffde00'
        });
    }, function () {
        $(this).find(".partnerIconBackground").css({
            'background-color': '#ededed'
        });
    });
    //----- End everyone trust hover icon -----


    //----- SCROLL TO TOP ----
    $('#scroller').css('display', 'none');
    $(window).scroll(function () {
        if ($(this).scrollTop() > 400) {
            $('#scroller').fadeIn();
        } else {
            $('#scroller').fadeOut();
        }
    });
    $('#scroller').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 400);
        return false;
    });
    $('#scroller').hover(function () {
        $('#scroller').css('opacity', '0.7');
    }, function () {
        $('#scroller').css('opacity', '0.2');
    });
    //End scroll to top


    //----- ABOUT US ----
    $('.aboutContentPosition').hover(function () {
        $(this).find(".descriptionseparator").css({
            'border-color': '#ffde00'
        });
    }, function () {
        $(this).find(".descriptionseparator").css({
            'border-color': '#fff'
        });
    });
    $('.aboutVideosStatisticContainer').hover(function () {
        $(this).find("h4").css({
            'color': '#ffde00'
        });
        $(this).find("p").css({
            'color': '#fff'
        });
        $(this).css({
            'background-color': '#848484'
        });
    }, function () {
        $(this).find("h4").css({
            'color': '#848484'
        });
        $(this).find("p").css({
            'color': '#a9a9a9'
        });
        $(this).css({
            'background-color': '#fff'
        });
    });
    //----- End about us ----


    //----- Delete zoom map to scroll page -----
    $('.overlay').click(function () {
        $(this).remove();
    });
    //----- End delete zoom map to scroll page -----


    //----- CONTACT PAGE -----
    $('.contactItem').hover(function () {
        $(this).css('background-color', '#1c1c1c');
        $(this).find('.contactIcon').css('color', '#ffde00');
        $(this).find('.contactDataTitle').css('color', '#ffde00');
        $(this).find('.contactDataDescription').css('color', '#ffde00');
    }, function () {
        $(this).css('background', 'none');
        $(this).find('.contactIcon').css('color', '#1c1c1c');
        $(this).find('.contactDataTitle').css('color', '#1c1c1c');
        $(this).find('.contactDataDescription').css('color', '#1c1c1c');
    });
    $('.socialItem').hover(function () {
        $(this).css('background-color', '#1c1c1c');
        $(this).find('.contactIcon').css('color', '#ffde00');
        $(this).find('.socialTitle').css('color', '#ffde00');
    }, function () {
        $(this).css('background', 'none');
        $(this).find('.contactIcon').css('color', '#1c1c1c');
        $(this).find('.socialTitle').css('color', '#1c1c1c');
    });
    //----- End contact page -----


    //----- PRODUCTS BY CATEGORY PAGE -----
    //nav hover
    $('.linkTextContainer').hover(function () {
        $(this).css('background-color', 'rgb(35, 35, 35)');
        $(this).find('.linkText').css('color', '#ffde00');
    }, function () {
        $(this).css('background-color', '#fff');
        $(this).find('.linkText').css('color', '#1c1c1c');
    });
    //end nav hover
    //----- End products by category -----


    $('.menu-link').bigSlide();





    //----- PRELOADER -----
    $(window).on('load', function () {
        var $preloader = $('#page-preloader'),
			$spinner = $preloader.find('.spinner');
        $spinner.fadeOut();
        $preloader.delay(350).fadeOut('slow');
    });
    //----- End preloader -----


    //----- ADD TO COMPARE -----
    $('.btn-compare').click(function () {
        $(this).closest('.productItem').find('.addToCompareReact').css('display', 'block');
        var addtocomparelink = $(this).attr('data-urladd');
        console.log(addtocomparelink);
        $.ajax({
            cache: false,
            url: addtocomparelink,
            type: 'post',
            success: function (data) {
                console.log(data);
                $('.productsPage .compareBtnConntainer .btn span').html(data.count);
                $('.compareBtnConntainer').css('display', 'block');
            },
            error: function(jqXHR, exception) {
                console.log(jqXHR.status);
                if (jqXHR.status === 0) {
                    alert('НЕ подключен к интернету!');
                } else if (jqXHR.status == 404) {
                    alert('НЕ найдена страница запроса [404])');
                } else if (jqXHR.status == 500) {
                    alert('НЕ найден домен в запросе [500].');
                } else if (exception === 'parsererror') {
                    alert("Ошибка в коде: \n"+jqXHR.responseText);
                } else if (exception === 'timeout') {
                    alert('Не ответил на запрос.');
                } else if (exception === 'abort') {
                    alert('Прерван запрос Ajax.');
                } else {
                    alert('Неизвестная ошибка:\n' + jqXHR.responseText);
                }
            }
        });
    });

    function ajaxFailure() {
        alert('Xi');
    }


    $('.productsPage').ready(function () {
        if ($.cookie('nop.CompareProducts') == null) return;
        var compareProductsCookie = $.cookie('nop.CompareProducts').split('&');
        if (compareProductsCookie.length > 0) {
            $('.productsPage .compareBtnConntainer .btn span').html(compareProductsCookie.length);
            $('.compareBtnConntainer').css('display', 'block');
        }
        for (var i = 0; i < compareProductsCookie.length; i++) {
            var compareProductsCookieId = compareProductsCookie[i].split('=')[compareProductsCookie[i].split('=').length - 1],
                productsCount = $('.productItem .productLink .cart-button .buttons button').length;
            for (var j = 0; j < productsCount; j ++) {
                var productItemDataAttr = $($('.productItem .productLink .cart-button .buttons button')[j]).attr('data-urladd'),
                    productItemDataId = productItemDataAttr.split('/')[productItemDataAttr.split('/').length - 1];
                if (compareProductsCookieId == productItemDataId) {
                    $($('.productItem .productLink .cart-button .buttons button')[j]).closest('.productItem').find('.addToCompareReact').css('display', 'block');
                }
            };
        };
    });
    //----- End add to compare -----


  


    //----- ADD PRODUCT DETAIL (product page) -----

    $('.productDetailBtn').click(function () {
        $('.detailsItem').fadeOut(400);
        var nameDetailBlock = $(this).data('item');
        var detailClass = "." + nameDetailBlock;
        $(this).closest('.detailInfo').find(detailClass).fadeIn(400);
    });

    $('.linkToDetail').click(function (e) {
        //e.preventDefault();
        $('.detailsItem').fadeOut(0);
        var nameDetailBlock = $(this).data('item'),
            detailClass = "." + nameDetailBlock;
        $(detailClass).fadeIn(0);

        var elementClick = $(this).attr("href"),
            destination = $(elementClick).offset().top;
        $('html,body').animate({ scrollTop: destination }, 1100);
        return false;
    });

    //----- End add product detail -----


    //----- ADD ADVANCED SEARCH (search page) -----

    $('.advancedSearch').click(function () {
        if ($('.hiddenBlock').css('display') == 'none') {
            $('.hiddenBlock').show(400);
            return false;
        } else {
            $('.hiddenBlock').hide(400);
            return false;
        }
    });

    //----- End advanced search -----


    //---— SHOW PRODUCTS AFTER FILTER (products page) —-— 

    $('.set_filter').click(function () {
        var checkedFilters = $('.blankCheckbox:checked'),
            checkedPriceFilters = $('.priceCheckbox:checked'),
            currentUrl = $('.floorCount').attr('data-url'),
            flag = false,
            newUrl = currentUrl + '?specs=';
        for (var i = 0; i < checkedFilters.length; i++) {
            flag = true;
            //newUrl = newUrl + 'specs=';
            var specsNumd = $($('.blankCheckbox:checked')[i]).attr('data-id');
            if (i == checkedFilters.length - 1) {
                newUrl = newUrl + specsNumd;
            } else {
                newUrl = newUrl + specsNumd + ',';
            };
        }
        if (checkedPriceFilters.length > 0) {
            if (checkedFilters.length > 0) {
                newUrl = newUrl + '&price='
            }
            else {
                newUrl = newUrl + '?price='
            }

        }
        for (var j = 0; j < checkedPriceFilters.length; j++) {
            flag = true;
            var priceNumd = $($('.priceCheckbox:checked')[j]).attr('data-price');
            if (j == checkedPriceFilters.length - 1) {
                newUrl = newUrl + priceNumd;
            } else {
                newUrl = newUrl + priceNumd + ',';
            };
        }
        if (flag) {
            console.log(newUrl);
            window.location.href = newUrl;
        } else {
            return false;
        }
    });

    //---— End show products after filter —---


    //----- SHOW PRODUCTS AFTER FILTER (products page) -----

    $('.set_filter').click(function () {
        for (var i = 0; i < $('.productsPage').find('.checkbox').find('input:checked').length; i++) {
            var urlLink = $('.productsPage').find('.checkbox').find('a').attr('href'),
                urlParts = urlLink.split('?');
            alert(urlParts[i]);
        }
    });

    //----- End show products after filter -----







    

    




});


//----- CREATE GALLERY (home page) -----
$('.home-page').ready(function () {
    var urlGallery = $('.home-page').attr('data-createGallery');
    $.ajax({
        cache: false,
        url: urlGallery,
        type: 'post',
        success: function (data) {
            var a = data.data.length;
            for (var i = 0; i < a; i++) {
                var titleImage = data.data[i].ImageGalleryPictureModel.Title,
                    subtitleImage = data.data[i].ImageGalleryPictureModel.AlternateText,
                    imageUrl = data.data[i].ImageGalleryPictureModel.FullSizeImageUrl,
                    smallImgUrl = data.data[i].Url;
                $('.imageGallery .row').append("<div class='col-xs-12 col-sm-12 col-md-4 col-lg-4 slide' onclick='addSmallImgGallery(" + "\"" + smallImgUrl + "\"" + " , this)' data-flag='true' data-class=my-gallery" + data.data[i].Id + "><div class='grid my-gallery" + data.data[i].Id + "' itemscope><figure class='effect-layla' itemprop='associatedMedia' itemscope itemtype='http://schema.org/ImageObject'><a href='" + imageUrl + "' itemprop='contentUrl' data-size='1024x1024'><img src='" + imageUrl + "' itemprop='thumbnail' alt='Image description' /></a><figcaption itemprop='caption description' class='galleryBigImg'><h2 class='nameProject'>" + titleImage + "</h2><p class='descriptionProject'>" + subtitleImage + "</p></figcaption></figure><!--<figure itemprop='associatedMedia' itemscope itemtype='http://schema.org/ImageObject' class='displayNone'><a href='../img/homePage/main/imageGallery/imageHouse/1/2.jpg' itemprop='contentUrl' data-size='964x1024'><img src='../img/homePage/main/imageGallery/imageHouse/1/2.jpg' itemprop='thumbnail' alt='Image description' /></a><figcaption itemprop='caption description'>Площадь дома 180 м2.</figcaption></figure>--></div></div>");

                // execute above function
                //initPhotoSwipeFromDOM('.my-gallery');
            }
        },
        error: function () {
            console.log('sdssssss');
        }
    });
});

//----- End create gallery -----



//----- SMALL IMAGE FOR GALLERY (home page) -----
function addSmallImgGallery(url, elem) {
    var test = $(elem).attr('data-flag');
    var dataClass = "." + $(elem).attr('data-class');
    if (test === "true") {
        $.ajax({
            cache: false,
            url: url,
            type: 'post',
            success: function (data) {
                var a = data.data.length;
                var clickableClass;
                for (var i = 0; i < a; i++) {
                    var titleImage = data.data[i].ImageGalleryPictureModel.Title,
                        subtitleImage = data.data[i].ImageGalleryPictureModel.AlternateText,
                        imageUrl = data.data[i].ImageGalleryPictureModel.FullSizeImageUrl,
                        imageId = data.Id;
                    if (i == 0) {
                        clickableClass = ".photoswipe" + imageId;
                    }
                    $('.imageGallery .row .slide '+ dataClass).append("<figure itemprop='associatedMedia' itemscope itemtype='http://schema.org/ImageObject' class='displayNone'><a class='photoswipe" + imageId + "' href='" + imageUrl + "' itemprop='contentUrl' data-size='964x1024'><img src='" + imageUrl + "' itemprop='thumbnail' alt='Image description'/></a><figcaption itemprop='caption description'>" + titleImage + subtitleImage + "</figcaption></figure>");
                    $(elem).attr('data-flag', 'false'); //== 'false';
                    console.log($(elem).attr('data-flag'));
                }
             
                initPhotoSwipeFromDOM1(dataClass);
                $(clickableClass).click();
            },
            error: function () {
                console.log('Eror');
            }
        })
    } else {
        console.log('Eror1');
    }
}
//function addSmallImgGallery(url, elem) {
//    //var container = $('.imageGallery .row .slide');
//    console.log($(elem).attr('data-flag'));
//    if ($(elem).attr('data-flag') == true) {
//        $.ajax({
//            cache: false,
//            url: url,
//            type: 'post',
//            success: function (data) {
//                var a = data.data.length;
//                console.log(data);
//                //for (var i = 0; i < a; i++) {
//                //    var titleImage = data.data[i].ImageGalleryPictureModel.Title,
//                //        subtitleImage = data.data[i].ImageGalleryPictureModel.AlternateText,
//                //        imageUrl = data.data[i].ImageGalleryPictureModel.FullSizeImageUrl;
//                //    $('.imageGallery .row .slide .my-gallery').append("<figure itemprop='associatedMedia' itemscope itemtype='http://schema.org/ImageObject' class='displayNone'><a href='" + imageUrl + "' itemprop='contentUrl' data-size='964x1024'><img src='" + imageUrl + "' itemprop='thumbnail' alt='Image description'/></a><figcaption itemprop='caption description'>" + titleImage + subtitleImage + "</figcaption></figure>");

//                //    //$('.productsPage .compareBtnConntainer .btn span').html(data.count);
//                //    //$('.compareBtnConntainer').css('display', 'block');

//                //    // execute above function
//                //    elem.data('flag', false);
//                //    initPhotoSwipeFromDOM('.my-gallery');
//               // }
//            },
//            error: function () {
//                alert('Eror');
//            }
//        });
//    } else {
//        return false;
//    }
//}
//----- End small image for gallery -----











//----- SLIDER RANGE ----- (product by category)
$(function () {
    $("#slider-range").slider({
        range: true,
        min: 5000,
        max: 75000,
        values: [15000, 65000],
        slide: function (event, ui) {
            $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });

    $("#slider-range-area").slider({
        range: true,
        min: 50,
        max: 300,
        values: [70, 280],
        slide: function (event, ui) {
            $("#amount-area").val(+ui.values[0] + " - " + ui.values[1]);
        }
    });
    $("#amount-area").val($("#slider-range-area").slider("values", 0) + " - " + $("#slider-range-area").slider("values", 1));

    $("#amount").val("$" + $("#slider-range").slider("values", 0) + " - $" + $("#slider-range").slider("values", 1));
});



//----- End slider range -----








//----- SELECT CHANGE (product by category) -----   
function setLocation(n) {
    window.location.href = n;
}
//----- End select change -----