$("#txtFromDate").datepicker({
    format: 'dd-mm-yyyy'
                , changeMonth: true
                , changeYear: true
                , yearRange: "2018:2022"
});
$("#txtToDate").datepicker({
    format: 'dd-mm-yyyy'
    , changeMonth: true
    , changeYear: true
    , yearRange: "2018:2022"
});
$("#btnSearch").click(function () {
    ShowLoader();
});
$('#GridMACRequestList').click(function (e) {
    if (e.target.type == "checkbox") {
        var ref = e.target;
        if ($(ref).prop("checked")) {
            $(ref).val("Y");
        }
        else {
            $(ref).val("N");
        }
    }
});
$("#btnApprove").click(function () {
    if ($("#GridMACRequestList tr:gt(1)").length == 0) {
        alert(MAC.Validation.NoData);
        return false;
    }
    var x = confirm(MAC.Info.CheckApproval);
    if (x) {
        ShowLoader();
    }
    return x;
});
window.onbeforeunload = DisableButtons;
