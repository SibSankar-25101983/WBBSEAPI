$(".alpha").keypress(function (e) { return alpha(e); });
$("#btnSearch").on("click", Search);
//$("#btnAdd").on("click", add);
//$("#btnSave").on("click", save);
function ClearViewData() {
    $("#tblViewData").empty();
}
function View(e) {
    ClearViewData();
    try {
        var s = e.data.record.PreSchoolId;
        $("#P").val(s);

        $.ajax({
            url: "/AdminPreSchoolEditPermission/GetSchoolView",
            type: "GET",
            dataType: "json",
            data: { 's': '' + s + '' },
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#tblViewData").append(data.View);
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
$("#btnUnlock").click(function () {
    try {
        var d = DefaultSetting.DefaultValN;
        var s = $("#P").val();
        var flag = 0;

        $.ajax({
            url: "/AdminPreSchoolEditPermission/ChkEdit",
            type: "GET",
            dataType: "json",
            data: { 's': '' + s + '' },
            contentType: "application/json",
            async: false,
            success: function (data) {
                d = data.d;
                if (d == DefaultSetting.DefaultValY) {
                    flag = 0;
                }
                else {
                    alert(School.Validation.UnLockNotPermitted);
                    flag = 1;
                }
            },
            error: function (xhr) {
                alert(OperationError());
                console.log(xhr.responseText);
                return false;
            }
        });
        if (flag == 0) {
            $("#e").val(setET(5));
            try {
                var x = confirm(School.Info.SchoolProfileUnlockAdmin);
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
        else {
            return false;
        }
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
        return false;
    }

});
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
    $("#GridPreSchoolEditPermission").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), searchType: $("#ddlSearchType").val() });
}
window.onbeforeunload = DisableButtons;
