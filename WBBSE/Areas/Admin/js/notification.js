$('#txtURL').css("text-transform", "none");
$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
function ClearData() {
    $('#divPopupBody input').val('');
    $('#divPopupBody textarea').val('');
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $('#trUrl1').css("display", "none");
    $('#trUrl2').css("display", "none");
    $('#trPdf1').css("display", "none");
    $('#trPdf2').css("display", "none");
}
function ClearViewData() {
    $("#trViewData").empty();
}
function chkLinkType() {
    if ($("#ddlLinkType").val() != DefaultSetting.DefaultValEnc) {
        if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.URL) {
            $('#trUrl1').css("display", "");
            $('#trUrl2').css("display", "");
            $('#trPdf1').css("display", "none");
            $('#trPdf2').css("display", "none");
        }
        else if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.PDF) {
            $('#trUrl1').css("display", "none");
            $('#trUrl2').css("display", "none");
            $('#trPdf1').css("display", "");
            $('#trPdf2').css("display", "");
        }
        else {
            alert(OperationError());
            $("#ddlLinkType").val(DefaultSetting.DefaultValEnc);
        }
    }
    else {
        $('#trUrl1').css("display", "none");
        $('#trUrl2').css("display", "none");
        $('#trPdf1').css("display", "none");
        $('#trPdf2').css("display", "none");
    }
}
function GetLinkTypeList(d) {
    $("#ddlLinkType").empty();
    $("#ddlLinkType").append(new Option("Select Link Type", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminCircular/GetLinkTypeList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.LinkType, item.LinkTypeId);
                    $("#ddlLinkType").append(opt);
                });
                $("#ddlLinkType").val(d);
                if ($("#e").val() == setET(2)) {
                    chkLinkType();
                }
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.message);
    }
}
$("#ddlLinkType").change(function () {
    chkLinkType();
});
function GetNotificationTypeList(d) {
    $("#ddlNotificationType").empty();
    $("#ddlNotificationType").append(new Option("Select Notification Type", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminNotification/GetNotificationTypeList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.NotificationType, item.MenuCode);
                    $("#ddlNotificationType").append(opt);
                });
                $("#ddlNotificationType").val(d);
                //if ($("#e").val() == setET(2)) {

                //}
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.message);
    }
}
$('#chkNewYN').click(function () {
    if ($(this).prop("checked")) {
        $(this).val(DefaultSetting.DefaultValY);
        $('#chkArchiveYN').prop("disabled", true);
    }
    else {
        $(this).val(DefaultSetting.DefaultValN);
        $('#chkArchiveYN').prop("disabled", false);
    }
});
$('#chkArchiveYN').click(function () {
    if ($(this).prop("checked")) {
        $(this).val(DefaultSetting.DefaultValY);
        $('#chkNewYN').prop("disabled", true);
    }
    else {
        $(this).val(DefaultSetting.DefaultValN);
        $('#chkNewYN').prop("disabled", false);
    }
});
function Add() {
    ClearData();
    $('#AlertInfo').hide();
    $("#e").val(setET(1));
    $("#C").val(DefaultSetting.DefaultValEnc);
    $("#lblModalHeader").text("Notification :: Add New");
    GetLinkTypeList(DefaultSetting.DefaultValEnc);
    GetNotificationTypeList(DefaultSetting.DefaultValEnc);
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
            if ($("#ddlNotificationType").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.NotificationTypeRequired);
                $("#ddlNotificationType").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlLinkType").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.LinkTypeRequired);
                $("#ddlLinkType").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.URL) {
                if ($.trim($("#txtURL").val()) == DefaultSetting.EmptyVal) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Content.Validation.URLRequired);
                    $("#txtURL").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
                if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtURL").val()))) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Content.Validation.InvalidURL);
                    $("#txtURL").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.PDF) {
                if ($("#e").val() == setET(1)) {
                    var postedFiles = document.getElementsByName("postedFiles");
                    if (postedFiles == null) {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(Content.Validation.PDFRequired);
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
                    if (postedFiles[0].value == null || postedFiles[0].value == "") {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(Content.Validation.PDFRequired);
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
                    var pExt = /[^.]+$/.exec(postedFiles[0].value);
                    for (i = 0; i < postedFiles.length; i++) {
                        if (pExt[i].toLowerCase() != "pdf") {
                            $('#AlertInfo').show();
                            $('#AlertInfoMsg').text(Content.Validation.PdfFileExt);
                            $('#popupModal').scrollTop(0);
                            return false;
                        }
                    }
                    if (postedFiles[0].files[0].size > Content.PdfAllowedSize) {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(Content.Validation.PdfFileSize);
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
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
        $("#trViewData").append("<tr><td>Content</td><td>" + e.data.record.BodyText + "</td></tr>");
        $("#trViewData").append("<tr><td>Link Type</td><td>" + e.data.record.LinkType + "</td></tr>");
        if (e.data.record.LinkType == Content.LinkType.URL) {
            $("#trViewData").append("<tr><td>URL Link</td><td><a href='" + e.data.record.URL + "' target='_blank' rel='noopener'>" + e.data.record.URL + "</a></td></tr>");
        }
        else {
            $("#trViewData").append("<tr><td>PDF Link</td><td><a href='/Web/PdfViewer?l=" + e.data.record.ContentId + "' target='_blank' rel='noopener'>Click to view pdf</a></td></tr>");
        }
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
        $('#AlertInfo').hide();
        $("#e").val(setET(2));
        $("#C").val(e.data.record.ContentId);
        $("#lblModalHeader").text("Notification :: Edit");
        $("#txtBodyText").val(e.data.record.BodyTextOriginal);
        if (e.data.record.NewYN == DefaultSetting.DefaultValY) {
            $("#chkNewYN").prop("checked", true);
            $("#chkNewYN").val(DefaultSetting.DefaultValY);
            $('#chkArchiveYN').prop("disabled", true);
        }
        else {
            $("#chkNewYN").prop("checked", false);
            $("#chkNewYN").val(DefaultSetting.DefaultValN);
            $('#chkArchiveYN').prop("disabled", false);
        }
        if (e.data.record.ArchiveYN == DefaultSetting.DefaultValY) {
            $("#chkArchiveYN").prop("checked", true);
            $("#chkArchiveYN").val(DefaultSetting.DefaultValY);
            $('#chkNewYN').prop("disabled", true);
        }
        else {
            $("#chkArchiveYN").prop("checked", false);
            $("#chkArchiveYN").val(DefaultSetting.DefaultValN);
            $('#chkNewYN').prop("disabled", false);
        }
        GetLinkTypeList(e.data.record.LinkTypeId);
        GetNotificationTypeList(e.data.record.MenuCode);
        $("#txtURL").val(e.data.record.URL);
        $("#P").val(e.data.record.PdfFilePath);
        $("#popupModal").modal("show");
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Remove(e) {
    $("#e").val(setET(3));
    $("#C").val(e.data.record.ContentId);
    $("#P").val(e.data.record.PdfFilePath);
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
    $("#GridNotification").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
    ShowLoader();
}
window.onbeforeunload = DisableButtons;
