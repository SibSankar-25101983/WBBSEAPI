var config, config1;
var ctx, ctx1, myNewChart;
$("#liAdminHome").addClass('active');
$(document).ready(function () {
    //$("#dvStat1").LoadingOverlay("show");
    $("#dvStat2").LoadingOverlay("show");
    //Stat1View();
    Stat2View();
});
function Stat1View() {
    try {
        $.ajax({
            url: "/AdminHome/GetStat1View",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (parseInt(data.err) != 0) {
                    $("#dvStat1").LoadingOverlay("hide");
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
                $("#dvStat1").LoadingOverlay("hide");
            },
            error: function (error) {
                console.log(error.responseText);
                $("#dvStat1").LoadingOverlay("hide");
                $("#canvas-holder").html(GeneralMsg.Error.DashboardStatistics);
                $("#canvas-holder1").html(GeneralMsg.Error.DashboardStatistics);
            }
        });
    }
    catch (err) {
        console.log(err.responseText);
        $("#dvStat1").LoadingOverlay("hide");
        $("#canvas-holder").html(GeneralMsg.Error.DashboardStatistics);
        $("#canvas-holder1").html(GeneralMsg.Error.DashboardStatistics);
    }
}

function Stat2View() {
    try {

        $.ajax({
            url: "/AdminHome/GetStat2View",
            type: "GET",
            dataType: "json",
            contentType: "application/json",
            async: true,
            success: function (data) {
                $("#dvStat2").append(data.View);
                $("#dvStat2").LoadingOverlay("hide");
            },
            error: function (error) {
                alert(OperationError());
                console.log(error.responseText);
                $("#dvStat2").LoadingOverlay("hide");
            }
        });
    }
    catch (err) {
        alert(OperationError());
        console.log(err.message);
        $("#dvStat2").LoadingOverlay("hide");
    }
}
