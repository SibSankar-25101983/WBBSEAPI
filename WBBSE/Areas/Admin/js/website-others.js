$('#txtURL').css("text-transform", "none");
$("#txtBodyText").css('text-transform', 'none');
$(".alpha").keypress(function (e) { return alpha(e); });
$(".checkAddress").keypress(function (e) { return checkAddress(e); });
//$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
getModuleList();
function ClearData() {
    $('#divPopupBody input').val('');
    $('#divPopupBody textarea').val('');
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $('#trContent1').css("display", "none");
    $('#trContent2').css("display", "none");
    $('#trUrl1').css("display", "none");
    $('#trUrl2').css("display", "none");
    $('#trPdf1').css("display", "none");
    $('#trPdf2').css("display", "none");
    $('#trNew').css("display", "none");
    $('#trArchive').css("display", "none");
}
function ClearViewData() {
    $("#trViewData").empty();
}
function chkLinkType() {
    if ($("#ddlLinkType").val() != DefaultSetting.DefaultValEnc) {
        if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.URL) {
            $('#trContent1').css("display", "none");
            $('#trContent2').css("display", "none");
            $('#trPdf1').css("display", "none");
            $('#trPdf2').css("display", "none");
            $('#trUrl1').css("display", "");
            $('#trUrl2').css("display", "");
            $('#trNew').css("display", "");
            $('#trArchive').css("display", "");
        }
        else if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.PDF) {
            $('#trContent1').css("display", "none");
            $('#trContent2').css("display", "none");
            $('#trUrl1').css("display", "none");
            $('#trUrl2').css("display", "none");
            $('#trPdf1').css("display", "");
            $('#trPdf2').css("display", "");
            $('#trNew').css("display", "");
            $('#trArchive').css("display", "");
        }
        else if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.CONTENT) {
            $('#trUrl1').css("display", "none");
            $('#trUrl2').css("display", "none");
            $('#trPdf1').css("display", "none");
            $('#trPdf2').css("display", "none");
            $('#trContent1').css("display", "");
            $('#trContent2').css("display", "");
            $('#trNew').css("display", "none");
            $('#trArchive').css("display", "none");
        }
        else {
            alert(OperationError());
            $("#ddlLinkType").val(DefaultSetting.DefaultValEnc);
        }
    }
    else {
        $('#trContent1').css("display", "none");
        $('#trContent2').css("display", "none");
        $('#trUrl1').css("display", "none");
        $('#trUrl2').css("display", "none");
        $('#trPdf1').css("display", "none");
        $('#trPdf2').css("display", "none");
        $('#trNew').css("display", "none");
        $('#trArchive').css("display", "none");
    }
}
function GetLinkTypeList(d) {
    $("#ddlLinkType").empty();
    $("#ddlLinkType").append(new Option("Select Link Type", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminContent/GetLinkTypeList",
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
    $("#lblModalHeader").text($("#ddlMenu option:selected").text() + " :: Add New");
    GetLinkTypeList(DefaultSetting.DefaultValEnc);
    $("#popupModal").modal("show");
}
function Save() {
    $('#AlertInfo').hide();
    try {
        if ($("#e").val() != setET(3)) {
            if ($("#ddlLinkType").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(Content.Validation.LinkTypeRequired);
                $("#ddlLinkType").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.CONTENT) {
                if ($.trim($("#txtBodyText").val()) == DefaultSetting.EmptyVal) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Content.Validation.ContentRequired);
                    $("#txtBodyText").focus();
                    $('#popupModal').scrollTop(0);
                    return false;
                }
            }
            if ($("#ddlLinkType option:selected").text().toUpperCase() == Content.LinkType.URL) {
                if ($.trim($("#txtURL").val()) == DefaultSetting.EmptyVal) {
                    $('#AlertInfo').show();
                    $('#AlertInfoMsg').text(Content.Validation.URLRequired);
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
                    if (postedFiles[0].files[0].size > Content.EBookPdfAllowedSize) {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(Content.Validation.MaxFileSizeBookPdf);
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
                }
            }
        }
        var x = confirm(Msg_Confirm());
        if (x) {
            $("#M").val($("#ddlMenu").val());
            $("#N").val($("#ddlMenu option:selected").text());
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
        $("#lblModalHeaderView").text($("#ddlMenu option:selected").text() + " :: View");
        $("#trViewData").append("<tr><td>Link Type</td><td>" + e.data.record.LinkType + "</td></tr>");
        if (e.data.record.LinkType == Content.LinkType.CONTENT) {
            $("#trViewData").append("<tr><td>Content</td><td>" + e.data.record.BodyTextOriginal + "</td></tr>");
        }
        else if (e.data.record.LinkType == Content.LinkType.URL) {
            $("#trViewData").append("<tr><td>URL Link</td><td><a href='" + e.data.record.URL + "' target='_blank' rel='noopener'>" + e.data.record.URL + "</a></td></tr>");
        }
        else {
            $("#trViewData").append("<tr><td>PDF Link</td><td><a href='/Web/PdfViewer?l=" + e.data.record.ContentId + "&" + new Date().getTime() + "' target='_blank' rel='noopener'>Click to view pdf</a></td></tr>");
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
        $("#lblModalHeader").text($("#ddlMenu option:selected").text() + " :: Edit");
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
        $("#txtBodyText").val(e.data.record.BodyTextOriginal);
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
function getModuleList() {
    $("#btnAdd").css("display", "none");
    $("#ddlMenu").empty();
    $("#ddlMenu").append(new Option("Select Module", DefaultSetting.DefaultValEnc));
    try {
        $.ajax({
            url: "/AdminContent/GetModuleList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.MenuName, item.MenuCode);
                    $("#ddlMenu").append(opt);
                });
            },
            error: function (error) {
                console.log(error.responseText);
                window.location.href = "../../../../Error/Unexpected.html";
            }
        });
    }
    catch (err) {
        console.log(err.message);
        window.location.href = "../../../../Error/Unexpected.html";
    }
}
$("#ddlMenu").change(function (e) {
    if ($(this).val() != DefaultSetting.DefaultValEnc) {
        $("#btnAdd").css("display", "");
        $("#GridContents").off('DOMSubtreeModified', chkMod);
        grid.reload({ searchString: $(this).val() });
        ShowLoader();
    }
    else {
        $("#btnAdd").css("display", "none");
        $("#GridContents").off('DOMSubtreeModified', chkMod);
        grid.reload({ searchString: "" });
        ShowLoader();
    }
});
//function Search() {
//    $("#GridContents").off('DOMSubtreeModified', chkMod);
//    grid.reload({ searchString: $("#txtSearch").val() });
//    ShowLoader();
//}
window.onbeforeunload = DisableButtons;
