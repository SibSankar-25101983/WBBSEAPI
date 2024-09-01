$("#btnSearch").on("click", Search);
$("#ddlFormType").change(function () {
    $("#txtSearch").val("");
    $("#GridDownloadForms").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", archiveYN: $("#ddlFormType").val() });
    ShowLoader();
});
function Search() {
    $("#GridDownloadForms").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), archiveYN: $("#ddlFormType").val() });
    ShowLoader();
}
function View(e) {
    if (e.data.record.LinkType == Content.LinkType.PDF) {
        window.open("/Web/PdfViewer?l=" + e.data.record.ContentId, "_blank", "noopener");
    }
    else {
        window.open(e.data.record.URL, "_blank", "noopener");
    }
}
if (screen.availWidth <= 1130.0) {
    $("#GridDownloadForms").addClass("table-responsive");
}
else {
    $("#GridDownloadForms").removeClass("table-responsive");
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
