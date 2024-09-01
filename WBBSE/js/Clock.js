$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "../../../../Services/WSCommon.asmx/GetTime",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            data = response.d;
            initializeClock();
        },
        error: function (msg) {
            console.log(msg.responseText);
        }
    });
});
function initializeClock() {
    var timeAry = data.split(':');
    hours = timeAry[0];
    minutes = timeAry[1];
    seconds = timeAry[2];
    tick();
    setInterval(tick, 1000);
}
function tick() {
    seconds++;
    if (seconds > 59) {
        minutes++;
        seconds = 0;
    }
    if (minutes > 59) {
        hours++;
        minutes = 0;
    }
    if (hours > 23) {
        hours = 0;
    }
    $("#time").html(" <strong><u>" + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2) + "</u></strong>");
}
