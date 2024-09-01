$(".ck").text('(033) 2358-0611');
$(".cb").text('(0342) 2569214 / 2662377');
$(".cm").text('(03222) 275524 / 275673');
$(".cn").text('(0353) 2699011 / 2699010');

$(".view").click(function () {
    try {
        $("#name").text($(this).closest('div').siblings('h4').text());
        $("#imgView").prop("src", $(this).prop('src'));
        $("#popupModalView").modal("show");
    }
    catch (err) {
        console.log(err);
    }
});
$(".close-clear").click(function () {
    $("#imgView").prop("src", null);
});
$(window).on("load", function (e) {
    HideLoaderNew();
});
