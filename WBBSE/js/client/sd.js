$("#btnSearch").on("click", Search);
if (screen.availWidth <= 1130.0) {
    $("#GridSchoolDirectory").addClass("table-responsive");
}
else {
    $("#GridSchoolDirectory").removeClass("table-responsive");
}

function ClearViewData() {
    $("#tblViewData").empty();
}

function showGrid() {
    $("#divGrid").show();
}

function hideGrid() {
    //$("#divGrid").remove();
    $("#divGrid").hide();
}

$("#txtSearch").keypress(function (e) {
    return alpha(e);
});

function View(e) {
    ClearViewData();

    try {
        var s = e.data.record.SchoolId;

        $.ajax({
            url: "/Web/GetSchoolView",
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

//Populate Zone in DropDown List
function GetZoneList() {
    try {
        hideGrid();
        $("#ddlZone").empty();
        $("#ddlZone").append(new Option("Select Zone", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "../../Common/GetZoneList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.ZoneName, item.ZoneId);
                    $("#ddlZone").append(opt);
                });
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
    }
}

//Populate District in DropDown List by Zone Id
function GetDistrictList(d) {
    try {
        hideGrid();
        $("#ddlDistrict").empty();
        $("#ddlDistrict").append(new Option("Select District", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "../../Common/GetDistrictList",
            data: { zi: d },
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.DistrictName, item.DistrictId);
                    $("#ddlDistrict").append(opt);
                });
                HideLoader();
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
    }
}

//Populate SubDivision in DropDown List by District Id
function GetSubDivisionList(d) {
    try {
        hideGrid();
        $("#ddlSubDivision").empty();
        $("#ddlSubDivision").append(new Option("Select Sub-Division", DefaultSetting.DefaultValEnc));
        $.ajax({
            url: "../../Common/GetSubDivisionList",
            data: { di: d },
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    var opt = new Option(item.SubDivisionName, item.SubDivisionId);
                    $("#ddlSubDivision").append(opt);
                });
                HideLoader();
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
    }
}

$("#ddlZone").change(function () {
    hideGrid();
    ShowLoader();
    GetDistrictList($("#ddlZone").val());
    $("#ddlSubDivision").empty();
    $("#GridSchoolDirectory").off('DOMSubtreeModified', chkMod);

});

$("#ddlDistrict").change(function () {
    hideGrid();
    ShowLoader();
    GetSubDivisionList($("#ddlDistrict").val());
    $("#GridSchoolDirectory").off('DOMSubtreeModified', chkMod);

});

$("#ddlSubDivision").change(function () {
    $("#txtSearch").val("");
    showGrid();
    $("#GridSchoolDirectory").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: "", searchType: "", sd: $("#ddlSubDivision").val() });
    ShowLoader();
});

function Search() {
    if ($.trim($("#txtSearch").val()) == "") {
        alert(School.Required.SchoolDirectory);
        $("#txtSearch").focus();
        return;
    }
    showGrid();
    $("#ddlZone").val(DefaultSetting.DefaultValEnc);
    $("#ddlDistrict").empty();
    $("#ddlSubDivision").empty();
    $("#GridSchoolDirectory").off('DOMSubtreeModified', chkMod);
    grid.reload({ searchString: $.trim($("#txtSearch").val()), searchType: $("#ddlSearchType").val(), sd: DefaultSetting.DefaultValEnc });
    ShowLoader();
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
