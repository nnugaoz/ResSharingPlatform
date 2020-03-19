/*
* Written by eligeske
* downloaded from eligeske.com
* have fun. nerd.
*/

$(document).ready(function () {

    // hover
    $('#rating_btns li').hover(function () {
        $rating = $(this).text();
        $('#rating_on').css('width', rateWidth($rating));
    });

    // mouseout
    $('#rating_btns li').mouseout(function () {
        $('#rating_on').css('width', "0px");
    });

    //click
    $('#rating_btns li').click(function () {
        $rating = $(this).text();
        setStarValue($rating);
    });

    //edit
    $('#rate_edit').click(function () {
        starClear();
    });

    //¸³Öµ
    function setStarValue(rating) {
        $('#rating').text(rating);
        $('#rating_output').val(rating);
        $pos = starSprite(rating);
        $('#large_stars').css('background-position', "0px " + $pos);
        $('#rating_btns').hide();
        $('#rating_on').hide();
        $('#rated').fadeIn();
    }

    //Çå³ý
    function starClear() {
        $('#rated').hide();
        $('#rating_btns').fadeIn();
        $('#rating_on').fadeIn(); 
        $("#rating_output").val("not rated");
    }

    function rateWidth($rating) {
        $rating = parseFloat($rating);
        switch ($rating) {
            case 0.0: $width = "0px"; break;
            case 0.5: $width = "14px"; break;
            case 1.0: $width = "28px"; break;
            case 1.5: $width = "42px"; break;
            case 2.0: $width = "56px"; break;
            case 2.5: $width = "70px"; break;
            case 3.0: $width = "84px"; break;
            case 3.5: $width = "98px"; break;
            case 4.0: $width = "112px"; break;
            case 4.5: $width = "126px"; break;
            case 5.0: $width = "140px"; break;
            default: $width = "0px";
        }
        return $width;
    }

    function starSprite($rating) {
        $rating = parseFloat($rating);
        switch ($rating) {
            case 0.0: $pos = "0px"; break;
            case 0.5: $pos = "-21px"; break;
            case 1.0: $pos = "-42px"; break;
            case 1.5: $pos = "-63px"; break;
            case 2.0: $pos = "-84px"; break;
            case 2.5: $pos = "-105px"; break;
            case 3.0: $pos = "-126px"; break;
            case 3.5: $pos = "-147px"; break;
            case 4.0: $pos = "-168px"; break;
            case 4.5: $pos = "-189px"; break;
            case 5.0: $pos = "-210px"; break;
            default: $pos = "0px";
        }
        return $pos;
    }

});	

