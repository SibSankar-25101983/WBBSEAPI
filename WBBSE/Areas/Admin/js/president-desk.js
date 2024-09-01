$("#txtBodyText").css('text-transform', 'none');
$(".ContentEdit").keypress(function (e) { return ContentEdit(e); });
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
function ClearData() {
    $('#divPopupBody input').val('');
}
function ClearViewData() {
    $("#tblViewData").empty();
}
function readURL1(input) {
    var mode = $("#e").val();
    var images = document.getElementsByName("postedFiles");
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgSlider').prop('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
function Add() {
    ClearData();
    $('#AlertInfo').hide();
    $('#viewImage').hide();
    $("#e").val(setET(1));
    $("#C").val(DefaultSetting.DefaultValEnc);
    $("#lblModalHeader").text("President's Desk :: Add Content");
    $("#popupModal").modal("show");
}
function Save() {
    $('#AlertInfo').hide();
    try {
        if ($("#e").val() != setET(3)) {
            if ($("#e").val() == setET(1)) {
                var postedFiles = document.getElementsByName("postedFiles");
                if (postedFiles == null) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Image.Required.ImageSelection);
                    $('#popupModal').scrollTop(0);
                    return false;
                }
                if (postedFiles[0].value == null || postedFiles[0].value == "") {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Image.Required.ImageSelection);
                    $('#popupModal').scrollTop(0);
                    return false;
                }
                var pExt = /[^.]+$/.exec(postedFiles[0].value);
                for (i = 0; i < postedFiles.length; i++) {
                    if (pExt[i].toLowerCase() != "jpg" && pExt[i].toLowerCase() != "jpeg" && pExt[i].toLowerCase() != "png") {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(Image.Validation.ImageExt);
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
                }
            }
            if ($.trim($("#txtBodyText").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.ContentRequired);
                $("#txtBodyText").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if (!chkDataFormat(RegexType.ContentEdit, $.trim($("#txtBodyText").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.InvalidContent);
                $("#txtBodyText").focus();
                $('#popupModal').scrollTop(0);
                return false;
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
        $("#imgSliderView").prop("src", e.data.record.ImagePath);
        $("#tblViewData").html(e.data.record.BodyText);
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
        $("#P").val(e.data.record.ImagePath);
        $("#lblModalHeader").text("President's Desk :: Edit Content");
        $("#txtBodyText").val(e.data.record.BodyText);
        $("#imgSlider").prop("src", e.data.record.ImagePath);
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
    $("#P").val(e.data.record.ImagePath);
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
window.onbeforeunload = DisableButtons;
