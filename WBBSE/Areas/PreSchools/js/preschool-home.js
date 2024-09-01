$("#liAdminHome").addClass('active');
$(document).ready(function () {
    $("#divProfile").LoadingOverlay("show");
    Stat1View();
    getUnreadNotice();
});
function Stat1View() {
    try {
        $("#divProfile").empty();
        $.ajax({
            url: "/PreSchoolHome/GetPreSchoolProfileDashBoardData",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#divProfile").append(data.ProfileData);
                $("#divProfile").LoadingOverlay("hide");
            },
            error: function (error) {
                console.log(error.responseText);
                $("#divProfile").LoadingOverlay("hide");
                $("#divProfile").html(GeneralMsg.Error.SchoolProfileData);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
        $("#divProfile").LoadingOverlay("hide");
        $("#divProfile").html(GeneralMsg.Error.SchoolProfileData);
    }
}
function getUnreadNotice() {
    try {
        $("#divNoticeData").empty();
        $.ajax({
            url: "/PreSchoolHome/GetUnreadNoticeList",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#divNoticeAfter").after(data.Notice);
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
