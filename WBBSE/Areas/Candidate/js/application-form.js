$("#btnApply").on("click", Save);
var price = 0, pps = 0;
function CalculatePrice() {
    //var pps = parseFloat('@(Session[SessionNames.PostPublicationPrice] == null ? "0" : Session[SessionNames.PostPublicationPrice].ToString())');
    var sc = 0;
    $('#txtPrice').html('0');
    $(".apply").each(function () {
        if ($(this).prop("checked")) {
            sc++;
        }
    });
    price = pps * sc;
    $('#txtPrice').html(price + ' <i class="fas fa-rupee-sign"></i>');
}
$(".apply").click(function () {
    if ($(this).prop("checked")) {
        $(this).val("Y");
    }
    else {
        $(this).val("N");
    }
    CalculatePrice();
});
function Save() {
    $('#divE').css('display', 'none');
    $('#liErrMsg').html('');
    try {
        var c = 0;
        $(".apply").each(function () {
            if ($(this).prop("checked")) {
                c++;
            }
        });
        if (c == 0) {
            $('#divE').css('display', '');
            $('#liErrMsg').html(PostPublication.Required.SubjectSelection);
            $(window).scrollTop($("#content").offset().top);
            return false;
        }
        var x = confirm(Msg_Confirm());
        if (x) {
            ShowLoaderNew();
        }
        return x;
    }
    catch (err) {
        alert(OperationError());
        return false;
    }
}

function ChkLink() {
    if (price > 0) {
        $('#lnkPayment').css('display', '');
    }
    else {
        $('#lnkPayment').css('display', 'none');
    }

}
