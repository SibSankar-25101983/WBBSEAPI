var config, config1;
var ctx, ctx1, myNewChart;
$("#liAdminHome").addClass('active');
$(document).ready(function () {
    $("#divProfile").LoadingOverlay("show");
    //$("#divStat").LoadingOverlay("show");
    Stat1View();
    //Stat2View();
    getUnreadNotice();
});
function Stat1View() {
    try {
        $("#divProfile").empty();
        $.ajax({
            url: "/SchoolHome/GetStat1View",
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
function Stat2View() {
    try {
        $.ajax({
            url: "/SchoolHome/GetStat2View",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (parseInt(data.err) != 0) {
                    $("#divStat").LoadingOverlay("hide");
                    $("#canvas-holder").html(GeneralMsg.Error.DashboardStatistics);
                    $("#canvas-holder1").html(GeneralMsg.Error.DashboardStatistics);
                }
                else {
                    config = {
                        type: 'doughnut',
                        data: {
                            datasets: [{
                                data: data.RegistrationStatValue,
                                backgroundColor: data.RegistrationColors,
                                label: data.RegistrationLabel
                            }],
                            labels: data.RegistrationStatValueLabel
                        },
                        options: {
                            responsive: true,
                            legend: {
                                display: true,
                                position: 'bottom'
                            },
                            title: {
                                display: true,
                                text: data.RegistrationLabel,
                                position: 'top',
                                fontSize: 20
                            },
                            animation: {
                                animateScale: true,
                                animateRotate: true
                            },
                            events: ['mousemove', 'mouseout', 'click', 'touchstart', 'touchmove']
                        }
                    };
                    config1 = {
                        type: 'pie',
                        data: {
                            datasets: [{
                                data: data.RegistrationApprovalStatValue,
                                backgroundColor: data.RegistrationApprovalColors,
                                label: data.RegistrationApprovalLabel
                            }],
                            labels: data.RegistrationApprovalStatValueLabel
                        },
                        options: {
                            responsive: true,
                            legend: {
                                display: true,
                                position: 'bottom'
                            },
                            title: {
                                display: true,
                                text: data.RegistrationApprovalLabel,
                                position: 'top',
                                fontSize: 20
                            },
                            animation: {
                                animateScale: true,
                                animateRotate: true
                            }
                        }
                    };
                    ctx = document.getElementById('chartArea').getContext('2d');
                    window.myDoughnut = new Chart(ctx, config);
                    myNewChart = new Chart(ctx, config);

                    ctx1 = document.getElementById('chartArea1').getContext('2d');
                    myNewChart = new Chart(ctx1, config1);
                }
                $("#divStat").LoadingOverlay("hide");
            },
            error: function (error) {
                console.log(error.responseText);
                $("#divStat").LoadingOverlay("hide");
                $("#canvas-holder").html(GeneralMsg.Error.DashboardStatistics);
                $("#canvas-holder1").html(GeneralMsg.Error.DashboardStatistics);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
        $("#divStat").LoadingOverlay("hide");
        $("#canvas-holder").html(GeneralMsg.Error.DashboardStatistics);
        $("#canvas-holder1").html(GeneralMsg.Error.DashboardStatistics);
    }
}
function getUnreadNotice() {
    try {
        $("#divNoticeData").empty();
        $.ajax({
            url: "/SchoolHome/GetUnreadNoticeList",
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
