$(function () {
    const advance = $("#advance").val();
    // ReSharper disable  CoercedEqualsUsing
    if (advance == 1) {
        $("#searchCollapseAdvance").addClass("show");
    } else if (advance == 0) {
        $("#searchCollapseAdvance").removeClass("show");
    }




    $("#nav-list-tab").click(function () {
        $("#view").val(0);
    });

    $("#nav-chart-tab").click(function () {
        $("#view").val(1);
    });

    $("#searchCollapseTrigger").click(function () {
        if ($("#advance").val() == 0) {
            $("#advance").val(1);
        } else if ($("#advance").val() == 1) {
            $("#advance").val(0);
        }
    });

    const dataStatus = $("#status").val();
    $("#statusSelect").val(dataStatus);

    $("#statusSelect").on("change",
        function () {
            const data = $(this).val();
            $("#status").val(data);
            console.log($("#status").val());
        });
    $(".sortData").click(function () {
        const sortByData = $(this).data("sort");
        $('input[name="sortBy"]').val(sortByData);
        const directData = $(this).data("direct");
        $('input[name="direct"]').val(directData);
        console.log(directData);
        $("#productForm").submit();
    });
    $("#nameProject").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Api/AjaxFindByProjectName",
                type: "POST",
                dataType: "json",
                data: { nameProject: request.term },
                success: function (data) {
                    console.log(request);
                    console.log(response);
                    console.log(data);
                    response($.map(data,
                        function (item) {
                            return { label: item, value: item };
                        }));
                }
            });
        }
    });
});

$(function () {
    // init start time and end time;
    var startDate = new Date();
    startDate.setFullYear(startDate.getFullYear() - 1);
    var endDate = new Date();

    // check start and end parameter
    const startPara = $("#startTime").val();
    if (startPara != null && startPara !== "") {
        startDate = new Date(startPara);
    }

    const endPara = $("#endTime").val();
    if (endPara != null && endPara !== "") {
        endDate = new Date(endPara);
    }

    $("#dateFilter").daterangepicker({
        autoUpdateInput: true,
        startDate: startDate,
        endDate: endDate,
        locale: {
            cancelLabel: "Clear"
        }
    });

    $("#dateFilter").on("apply.daterangepicker",
        function (ev, picker) {
            const start = picker.startDate.format("YYYY-MM-DD");
            const end = picker.endDate.format("YYYY-MM-DD");
            $("#startTime").val(start);
            $("#endTime").val(end);
            console.log(start + " " + end);
            console.log(startPara + " " + endPara);
        });



});



$(function () {
    if ($("#project_chart").length) {
        var projectName = [];
        var currentFund = [];
        $.ajax({
            url: "/Api/GetProjectData",
            type: "GET",
            success: function (res) {
                if (res != null) {
                    console.log("Yes");
                    for (let i = 0; i < res.listProject.length; i++) {
                        projectName[i] = res.listProject[i].name;
                        currentFund[i] = res.listProject[i].currentFund;
                    }
                }
                console.log(projectName);
                const projectData = {
                    labels: projectName,
                    datasets: [{
                        data: currentFund,
                        backgroundColor: [
                            "rgba(255, 99, 132, 0.2)",
                            "rgba(54, 162, 235, 0.2)",
                            "rgba(255, 206, 86, 0.2)",
                            "rgba(75, 192, 192, 0.2)",
                            "rgba(153, 102, 255, 0.2)",
                            "rgba(255, 159, 64, 0.2)",
                            "rgba(255, 99, 132, 0.2)",
                            "rgba(54, 162, 235, 0.2)",
                            "rgba(255, 206, 86, 0.2)",
                            "rgba(75, 192, 192, 0.2)",
                            "rgba(153, 102, 255, 0.2)",
                            "rgba(255, 159, 64, 0.2)"
                        ],
                        borderColor: [
                            "rgba(255,99,132,1)",
                            "rgba(54, 162, 235, 1)",
                            "rgba(255, 206, 86, 1)",
                            "rgba(75, 192, 192, 1)",
                            "rgba(153, 102, 255, 1)",
                            "rgba(255, 159, 64, 1)",
                            "rgba(255,99,132,1)",
                            "rgba(54, 162, 235, 1)",
                            "rgba(255, 206, 86, 1)",
                            "rgba(75, 192, 192, 1)",
                            "rgba(153, 102, 255, 1)",
                            "rgba(255, 159, 64, 1)"
                        ],
                        borderWidth: 1,
                        fill: false
                    }]
                };
                const projectOptions = {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    legend: {
                        display: false
                    },
                    elements: {
                        point: {
                            radius: 0
                        }
                    }
                };
                const projectChartCanvas = $("#project_chart").get(0).getContext("2d");
                // This will get the first returned node in the jQuery collection.
                // ReSharper disable once UnusedLocals
                const projectChart = new window.Chart(projectChartCanvas, {
                    type: "bar",
                    data: projectData,
                    options: projectOptions
                });
            }
        });
    };
});