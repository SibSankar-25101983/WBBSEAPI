function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
$('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
    if (!$(this).next().hasClass('show')) {
        $(this).parents('.dropdown-menu').first().find('.show').removeClass('show');
    }
    var $subMenu = $(this).next('.dropdown-menu');
    $subMenu.toggleClass('show');


    $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
        $('.dropdown-submenu .show').removeClass('show');
    });
    return false;
});
//$('body').bind('cut copy paste', function (e) {
//    e.preventDefault();
//});
//$("body").on("contextmenu", function (e) {
//    return false;
//});
//try {
//    var ah = screen.availHeight;
//    var fp = $("#fWebsite")[0].offsetTop;
//    var fh = $("#fWebsite").height();

//    if ((fp - fh + 200) < ah) {
//        $("#fWebsite").addClass("custom-footer");
//    }
//    else {
//        $("#fWebsite").removeClass("custom-footer");
//    }
//}
//catch (err) {
//    console.log(err.message);
//}
var btnNavigate = document.getElementById("btnNavTopBottom");
window.onscroll = function () { viewTop() };
function viewTop() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        btnNavigate.style.display = "block";
    } else {
        btnNavigate.style.display = "none";
    }
}
$(document).ready(function () {
    //check whether sticky footer is required or not
    if ($('#desk').is(':visible')) {
        chkFooter();
    }
});
