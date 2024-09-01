$("#btnSearch").on("click", Search);
if (screen.availWidth <= 1130.0) {
    $("#GridSyllabusCurriculum").addClass("table-responsive");
}
else {
    $("#GridSyllabusCurriculum").removeClass("table-responsive");
}
$("#ddlFormType").change(function () {
    $("#txtSearch").val("");
    $("#GridSyllabusCurriculum").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", archiveYN: $("#ddlFormType").val() });
    ShowLoader();
});

function Search() {
    $("#GridSyllabusCurriculum").off('DOMSubtreeModified', chkMod);
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
