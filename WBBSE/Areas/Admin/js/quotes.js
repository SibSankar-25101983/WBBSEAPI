$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
function ClearData() {
    $('#divPopupBody input').val('');
}
function ClearViewData() {
    $("#tblViewBodyText").empty();
}
function Add() {
    ClearData();
    $('#AlertInfo').hide();
    $("#e").val(setET(1));
    $("#C").val(DefaultSetting.DefaultValEnc);
    $("#lblModalHeader").text("Quote :: Add Content");
    $("#popupModal").modal("show");
}
function Save() {
    $('#AlertInfo').hide();

    try {
        if ($("#e").val() != setET(3)) {
            if ($.trim($("#txtBodyText").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.ContentRequired);
                $("#txtBodyText").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtBodyText").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.InvalidContent);
                $("#txtBodyText").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtFooterText").val()) != DefaultSetting.EmptyVal) {
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtFooterText").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Content.Validation.InvalidContent);
                    $("#txtFooterText").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
        }

        var x = confirm(Msg_Confirm());
        if (x) {
            $("#popupModal").modal("hide");
            ShowLoader();
        }
        return x;
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
        return false;
    }
}
function View(e) {
    ClearViewData();

    try {
        $("#tblViewBodyText").text(e.data.record.BodyText);
        $("#tblViewFooterText").text(e.data.record.FooterText);
        $("#popupModalView").modal("show");
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Edit(e) {
    $("#lblModalHeader").text("");
    ClearData();

    try {
        var c = e.data.record.ContentId;
        $('#AlertInfo').hide();
        $("#e").val(setET(2));
        $("#C").val(c);
        $("#lblModalHeader").text("Quote :: Edit Content");
        $("#txtBodyText").val(e.data.record.BodyText);
        $("#txtFooterText").text(e.data.record.FooterText);
        $("#popupModal").modal("show");
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Remove(e) {
    var c = e.data.record.ContentId;
    $("#e").val(setET(3));
    $("#C").val(c);
    $("#btnSave").click();
}

var chkMod = function (e) {
    try {
        if (e.target.parentElement.nodeName.toLowerCase() == "td") {
            console.log(e);
            window.location.href = "../../../../Error/Unexpected.html";
        }
    }
    catch (err) {
    }
};
function Search() {
    $("#GridQuotes").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
    ShowLoader();
}
window.onbeforeunload = DisableButtons;
