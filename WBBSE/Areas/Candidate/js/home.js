function GetCandidateDetails() {
    try {
        $("#divCandidateStatus").empty();
        $("#tblCandidateData").empty();
        $("#tblResultData").empty();
        $.ajax({
            url: "/CandidateHome/GetCandidateDetails",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (data.Err == 0) {
                    $("#divCandidateStatus").append(data.Eligibility);
                    $("#tblCandidateData").append(data.CandidateData);
                    $("#tblResultData").append(data.ResultData);
                }
                else {
                    $("#divCandidateData").empty();
                    $("#divCandidateData").append('<div class="row m-5"><div class="col-12 text-center">' + Candidate.Info.DashBoardErrMsg + '</div></div>');
                    $("#divResultData").empty();
                    $("#divResultData").append('<div class="row m-5"><div class="col-12 text-center">' + Candidate.Info.DashBoardErrMsg + '</div></div>');
                }
                $("#divCandidateStatus").LoadingOverlay("hide");
                $("#pills-tabContent").LoadingOverlay("hide");
            },
            error: function (jqXHR, exception) {
                console.log(jqXHR.responseText);
                $("#divCandidateStatus").LoadingOverlay("hide");
                $("#pills-tabContent").LoadingOverlay("hide");
                $("#divCandidateData").empty();
                $("#divCandidateData").append('<div class="row m-5"><div class="col-12 text-center">' + Candidate.Info.DashBoardErrMsg + '</div></div>');
                $("#divResultData").empty();
                $("#divResultData").append('<div class="row m-5"><div class="col-12 text-center">' + Candidate.Info.DashBoardErrMsg + '</div></div>');
            }
        });
    }
    catch (err) {
        console.log(err);
        $("#divCandidateStatus").LoadingOverlay("hide");
        $("#pills-tabContent").LoadingOverlay("hide");
        $("#divCandidateData").empty();
        $("#divCandidateData").append('<div class="row m-5"><div class="col-12 text-center">' + Candidate.Info.DashBoardErrMsg + '</div></div>');
        $("#divResultData").empty();
        $("#divResultData").append('<div class="row m-5"><div class="col-12 text-center">' + Candidate.Info.DashBoardErrMsg + '</div></div>');
    }
}
$(document).ready(function () {
    $("#liAdminHome").addClass('active');
    $("#divCandidateStatus").LoadingOverlay("show");
    $("#pills-tabContent").LoadingOverlay("show");
    GetCandidateDetails();
});
