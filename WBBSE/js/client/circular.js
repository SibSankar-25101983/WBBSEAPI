$(".alpha").keypress(function (e) { return alpha(e); });
$("#lnkCurrent").prop("href", window.location.href);
$("#btnSearch").on("click", Search);
if (screen.availWidth <= 1130.0) {
    $("#GridCircular").addClass("table-responsive");
}
else {
    $("#GridCircular").removeClass("table-responsive");
}
function Search() {
    $("#GridCircular").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()) });
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
        console.log(err.responseText);
    }
};
