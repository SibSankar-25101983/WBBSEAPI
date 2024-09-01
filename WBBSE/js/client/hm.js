$(document).ready(function () {
    try {
        $('.vtickerUpdates').easyTicker({
            direction: 'up',
            speed: 'slow',
            interval: 5000
        });
        $('.vtickerNotice').easyTicker({
            direction: 'up',
            speed: 'slow',
            interval: 2500
        });
        $('.vticker').easyTicker({
            direction: 'up',
            speed: 'slow',
            interval: 3000
        });
        $('#ulInfo li span').each(function () {
            if ($(this).html() == "" || typeof ($(this).html()) == "undefined") {
                $(this).parent().css("display", "none");
            }
        });
        try {
            document.getElementById("overlay").style.display = "block";
        }
        catch (err) { }

        //adjust slider height
        if ($('#desk').is(':visible')) {
            if ($('.carousel-inner').height() < $('#divDesk').height()) {
                $('.custom-adjust-size').css('height', $('#divDesk').height() + 1);
            }
        }
    }
    catch (err) {
        console.log(err);
    }
});
