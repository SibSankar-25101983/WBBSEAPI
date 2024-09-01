$(".view").click(function () {
    try {
        $("#name").text($(this).closest('tr').children(':nth-child(2)').text());
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
