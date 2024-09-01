$('#txtURL').css("text-transform", "none");
$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
$("#btnAdd").on("click", Add);
$("#btnSave").on("click", Save);
function ClearData() {
    $('#divPopupBody input').val('');
    $('#divPopupBody textarea').val('');
    $('#divPopupBody select').val(DefaultSetting.DefaultValEnc);
    $("#tblSchoolListBody").empty();
    $('#trUrl1').css("display", "none");
    $('#trUrl2').css("display", "none");
    $('#trPdf1').css("display", "none");
    $('#trPdf2').css("display", "none");
    $('#S').val('');
}
function ClearViewData() {
    $("#trViewData").empty();
}
function chkLinkType() {
    if ($("#ddlLinkType").val() != DefaultSetting.DefaultValEnc) {
        if ($("#ddlLinkType option:selected").text().toUpperCase() == SchoolInbox.LinkType.URL) {
            $('#trUrl1').css("display", "");
            $('#trUrl2').css("display", "");
            $('#trPdf1').css("display", "none");
            $('#trPdf2').css("display", "none");
        }
        else if ($("#ddlLinkType option:selected").text().toUpperCase() == SchoolInbox.LinkType.PDF) {
            $('#trUrl1').css("display", "none");
            $('#trUrl2').css("display", "none");
            $('#trPdf1').css("display", "");
            $('#trPdf2').css("display", "");
        }
        else {
            //alert(OperationError());
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
            url: "/AdminPreSchoolInbox/GetLinkTypeList",
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
$('#chkAllSchool').click(function () {
    if ($(this).prop("checked")) {
        $("#txtSchoolName").prop('readonly', true);
    }
    else {
        $("#txtSchoolName").prop('readonly', false);
    }
    $("#tblSchoolListBody").empty();
    $("#S").val(SchoolInbox.All);
});
$("#txtSchoolName").autocomplete({
    minLength: 1,
    disabled: false,
    delay: 100,
    source: function (request, response) {
        $.ajax({
            url: "/AdminPreSchoolInbox/GetSchoolList",
            dataType: "json",
            data: { s: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.SchoolName, value: item.SchoolName, SI: item.PreSchoolId }
                }))
            },
            error: function (err) {
                console.log(err.responseText);
                alert(OperationError());
            }
        });
    },
    select: function (e, i) {
        //$("#txtSchoolName").val(i.item.label);
        setTimeout(function () { $("#txtSchoolName").val(''); }, 50);
        makeSchoolList(i.item.SI, i.item.label);
    }
});
function makeSchoolList(si, sn) {
    try {
        var data = $("#S").val();

        //check whether school is already present or not. if present, do not add school
        var data_ary = data.split(",");
        if (data_ary.indexOf(si) == -1) {
            if (data == null || data == '' || data == SchoolInbox.All) {
                data = si;
            }
            else {
                data += ',' + si;
            }
            $("#S").val(data);

            //view in table
            var size = $('#tblSchoolList >tbody >tr').length + 1;
            var element = null;
            var content = $('#tblSample tr');
            element = content.clone();
            element.prop('id', 'rec-' + size);
            element.find('.SampleName').text(sn);
            element.find('.deleteRecord').prop('data-id', size);
            element.appendTo('#tblSchoolListBody');
            element.find('.sn').html(size);
            console.log($("#S").val());
        }
    }
    catch (err) {
        console.log(err.responseText);
        alert(OperationError());
    }
}
$("#tblSchoolList").on('click', '.deleteRecord', function (e) {
    try {
        e.preventDefault();
        if (confirm(SchoolInbox.Info.RemoveSchool)) {
            var id = $(this).prop('data-id');
            $('#rec-' + id).remove();
            var data_ary = $("#S").val().split(",");
            data_ary.splice((parseInt(id) - 1), 1);
            $("#S").val(data_ary.toString());
            //console.log($("#S").val());

            //regnerate index number on table
            $('#tblSchoolListBody tr').each(function (index) {
                $(this).find('span.sn').html(parseInt(index + 1));
                $(this).prop('id', 'rec-' + parseInt(index + 1));
                $(this).find('.deleteRecord').prop('data-id', parseInt(index + 1));
            });
        }
    }
    catch (err) {
        console.log(err.responseText);
        alert(OperationError());
    }
});
function Add() {
    ClearData();
    $('#AlertInfo').hide();
    $("#e").val(setET(1));
    $("#I").val(DefaultSetting.DefaultValEnc);
    $("#lblModalHeader").text("Notice :: Add New");
    GetLinkTypeList(DefaultSetting.DefaultValEnc);
    $("#popupModal").modal("show");
}
function Save() {
    $('#AlertInfo').hide();
    try {
        if ($("#e").val() != setET(3)) {
            if ($("#S") == null || $("#S").val() == null || $("#S").val() == DefaultSetting.DefaultValEnc) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(OperationError());
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($.trim($("#txtBodyText").val()) == DefaultSetting.EmptyVal) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(SchoolInbox.Validation.ContentRequired);
                $("#txtBodyText").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtBodyText").val()))) {
                $('#AlertInfo').show();
                $('#AlertInfoMsg').text(SchoolInbox.Validation.InvalidContent);
                $("#txtBodyText").focus();
                $('#popupModal').scrollTop(0);
                return false;
            }
            if ($("#ddlLinkType").val() != DefaultSetting.DefaultValEnc) {
                if ($("#ddlLinkType option:selected").text().toUpperCase() == SchoolInbox.LinkType.URL) {
                    if ($.trim($("#txtURL").val()) == DefaultSetting.EmptyVal) {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(SchoolInbox.Validation.URLRequired);
                        $("#txtURL").focus();
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
                    if (!chkDataFormat(RegexType.Alpha, $.trim($("#txtURL").val()))) {
                        $('#AlertInfo').show();
                        $('#AlertInfoMsg').text(SchoolInbox.Validation.InvalidURL);
                        $("#txtURL").focus();
                        $('#popupModal').scrollTop(0);
                        return false;
                    }
                }
                if ($("#ddlLinkType option:selected").text().toUpperCase() == SchoolInbox.LinkType.PDF) {
                    if ($("#e").val() == setET(1)) {
                        var postedFiles = document.getElementsByName("postedFiles");
                        if (postedFiles == null) {
                            $('#AlertInfo').show();
                            $('#AlertInfoMsg').text(SchoolInbox.Validation.PDFRequired);
                            $('#popupModal').scrollTop(0);
                            return false;
                        }
                        if (postedFiles[0].value == null || postedFiles[0].value == "") {
                            $('#AlertInfo').show();
                            $('#AlertInfoMsg').text(SchoolInbox.Validation.PDFRequired);
                            $('#popupModal').scrollTop(0);
                            return false;
                        }
                        var pExt = /[^.]+$/.exec(postedFiles[0].value);
                        for (i = 0; i < postedFiles.length; i++) {
                            if (pExt[i].toLowerCase() != "pdf") {
                                $('#AlertInfo').show();
                                $('#AlertInfoMsg').text(SchoolInbox.Validation.PdfFileExt);
                                $('#popupModal').scrollTop(0);
                                return false;
                            }
                        }
                        if (postedFiles[0].files[0].size > SchoolInbox.PdfAllowedSize) {
                            $('#AlertInfo').show();
                            $('#AlertInfoMsg').text(SchoolInbox.Validation.PdfFileSize);
                            $('#popupModal').scrollTop(0);
                            return false;
                        }
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
        var i = e.data.record.InboxId;

        $.ajax({
            url: "/AdminPreSchoolInbox/GetSchoolInboxView",
            type: "GET",
            dataType: "json",
            data: { 'i': '' + i + '' },
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#trViewData").append(data.View);
                $("#popupModalView").modal("show");
            },
            error: function (error) {
                alert(OperationError());
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Edit(e) {
    ClearData();
    $('#AlertInfo').hide();
    $("#e").val(setET(2));
    try {
        var i = e.data.record.InboxId;
        $.ajax({
            url: "/AdminPreSchoolInbox/GetSchoolInboxEdit",
            type: "GET",
            dataType: "json",
            data: { 'i': '' + i + '' },
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#I").val(i);
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
                $("#txtURL").val(e.data.record.URL);
                $("#P").val(e.data.record.PdfFilePath);
                if (data.SchoolList == SchoolInbox.All) {

                    $("#S").val(SchoolInbox.All);
                    $("#tblSchoolListBody").empty();
                    $('#chkAllSchool').prop("checked", true);
                    $("#txtSchoolName").prop('readonly', true);
                }
                else {
                    $("#S").val(e.data.record.SchoolIdList);
                    $("#tblSchoolListBody").append(data.SchoolList);
                    $('#chkAllSchool').prop("checked", false);
                    $("#txtSchoolName").prop('readonly', false);
                }
                $("#popupModal").modal("show");
            },
            error: function (error) {
                alert(OperationError());
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
    }
}
function Remove(e) {
    $("#e").val(setET(3));
    $("#I").val(e.data.record.InboxId);
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
        console.log(err.message);
    }
};
function Search() {
    $("#GridNotice").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
    ShowLoader();
}
window.onbeforeunload = DisableButtons;