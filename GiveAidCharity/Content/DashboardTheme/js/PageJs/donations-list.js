$(document).ready(function () {
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
    const methodStatus = $("#method").val();
    $("#statusSelect").val(dataStatus);
    $("#methodSelect").val(methodStatus);

    $("#statusSelect").on("change",
        function () {
            const data = $(this).val();
            $("#status").val(data);
            console.log($("#status").val());
        });
    $("#methodSelect").on("change",
        function () {
            const data = $(this).val();
            $("#method").val(data);
            console.log($("#method").val());
        });

    $(".sortData").click(function () {
        const sortByData = $(this).data("sort");
        $('input[name="sortBy"]').val(sortByData);
        const directData = $(this).data("direct");
        $('input[name="direct"]').val(directData);
        console.log(directData);
        $("#productForm").submit();
    });

    const minAmount = $("#minAmount").val();
    const maxAmount = $("#maxAmount").val();
    $("#slider-range").slider({
        range: true,
        min: 0,
        max: 100,
        values: [minAmount, maxAmount],
        slide: function (event, ui) {
            $("#amount").html(`$${ui.values[0]} - $${ui.values[1]}`);
            $("#minAmount").val(ui.values[0]);
            $("#maxAmount").val(ui.values[1]);
        }
    });
    $("#amount")
        .html(`$${$("#slider-range").slider("values", 0)} - $${$("#slider-range").slider("values", 1)}`);

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
            //loadDonationChart();
        });


    var datetimeChart = [];
    var amount = [];
    var paypal = [];
    var vnPay = [];
    var directBank = [];
    var namePaymentMethod = [];
    var valuePaymentMethod = [];
    var totalPaymentMethod = 0;

    function loadDonationChart() {
        if ($("#curve_chart").length) {
            datetimeChart = [];
            paypal = [];
            amount = [];
            $.ajax({
                url: "/Api/GetDonations",
                data: {
                    fromDate: $("#startTime").val(),
                    toDate: $("#endTime").val()
                },
                success: function (res) {
                    console.log(res);

                    for (let k = 0; k < res.paypalPerMonth.length; k++) {
                        paypal.push(res.paypalPerMonth[k].Amount);

                    }

                    for (let k = 0; k < res.directBankPerMonth.length; k++) {
                        directBank.push(res.directBankPerMonth[k].Amount);
                    }

                    for (let k = 0; k < res.vnPayPerMonth.length; k++) {
                        vnPay.push(res.vnPayPerMonth[k].Amount);
                    }

                    for (let k = 0; k < res.amountPerMonth.length; k++) {
                        amount.push(res.amountPerMonth[k].Amount);
                        const dateChart = res.amountPerMonth[k].Month + "/" + res.amountPerMonth[k].Year;

                        datetimeChart.push(dateChart);
                    }



                    drawChart();
                }
            });
        }
    }

    if ($("#traffic-chart").length) {
        $.ajax({
            url: "/Api/GetPaymentMethod",
            success: function (res) {
                for (let i = 0; i < res.paymentMethod.length; i++) {

                    if (res.paymentMethod[i].PaymentMethod === 0) {
                        namePaymentMethod.push("Paypal");
                    } else if (res.paymentMethod[i].PaymentMethod === 1) {
                        namePaymentMethod.push("VnPay");
                    } else if (res.paymentMethod[i].PaymentMethod === 2) {
                        namePaymentMethod.push("DirectBankTransfer");
                    } else {
                        namePaymentMethod.push(res.paymentMethod[i].PaymentMethod);
                    }

                    valuePaymentMethod.push(res.paymentMethod[i].Quantity);
                    totalPaymentMethod += res.paymentMethod[i].Quantity;
                }
                const ctxColor = document.getElementById("traffic-chart").getContext("2d");
                const colorGradientStrokeBlue = ctxColor.createLinearGradient(0, 0, 0, 181);
                colorGradientStrokeBlue.addColorStop(0, "rgba(54, 215, 232, 1)");
                colorGradientStrokeBlue.addColorStop(1, "rgba(177, 148, 250, 1)");
                const colorGradientLegendBlue =
                    "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

                const colorGradientStrokeRed = ctxColor.createLinearGradient(0, 0, 0, 50);
                colorGradientStrokeRed.addColorStop(0, "rgba(255, 191, 150, 1)");
                colorGradientStrokeRed.addColorStop(1, "rgba(254, 112, 150, 1)");
                const colorGradientLegendRed =
                    "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

                const colorGradientStrokeGreen = ctxColor.createLinearGradient(0, 0, 0, 300);
                colorGradientStrokeGreen.addColorStop(0, "rgba(6, 185, 157, 1)");
                colorGradientStrokeGreen.addColorStop(1, "rgba(132, 217, 210, 1)");
                const colorGradientLegendGreen =
                    "linear-gradient(to right, rgba(6, 185, 157, 1), rgba(132, 217, 210, 1))";

                var trafficChartData = {
                    datasets: [
                        {
                            data: valuePaymentMethod,
                            backgroundColor: [
                                colorGradientStrokeBlue,
                                colorGradientStrokeGreen,
                                colorGradientStrokeRed
                            ],
                            hoverBackgroundColor: [
                                colorGradientStrokeBlue,
                                colorGradientStrokeGreen,
                                colorGradientStrokeRed
                            ],
                            borderColor: [
                                colorGradientStrokeBlue,
                                colorGradientStrokeGreen,
                                colorGradientStrokeRed
                            ],
                            legendColor: [
                                colorGradientLegendBlue,
                                colorGradientLegendGreen,
                                colorGradientLegendRed
                            ]
                        }
                    ],

                    // These labels appear in the legend and in the tooltips when hovering different arcs
                    labels: namePaymentMethod
                };
                const trafficChartOptions = {
                    responsive: true,
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    },
                    legend: false,
                    // ReSharper disable once UnusedParameter
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < trafficChartData.datasets[0].data.length; i++) {
                            console.log();
                            text.push(
                                `<li><span class="legend-dots" style="background:${trafficChartData.datasets[0]
                                    .legendColor[i]}"></span>`);
                            if (trafficChartData.labels[i]) {
                                text.push(trafficChartData.labels[i]);
                            }
                            text.push(`<span class="float-right">
                                        ${new Number(trafficChartData.datasets[0].data[i] / totalPaymentMethod * 100)
                                    .toPrecision(3)}
                                    %</span>`);
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    }
                };
                const trafficChartCanvas = $("#traffic-chart").get(0).getContext("2d");
                const trafficChart = new window.Chart(trafficChartCanvas,
                    {
                        type: "doughnut",
                        data: trafficChartData,
                        options: trafficChartOptions
                    });
                $("#traffic-chart-legend").html(trafficChart.generateLegend());
            }
        });

    }

    function drawChart() {
        window.Highcharts.chart("curve_chart",
            {
                title: {
                    text: "List Donation"
                },

                yAxis: {
                    title: {
                        text: ""
                    }
                },

                xAxis: [
                    {
                        categories: datetimeChart,
                        labels: {
                            formatter: function () {
                                return `<span style="color:blue;">${this.value}</span>`;
                            },
                            step: 2
                        }
                    }
                ],

                legend: {
                    layout: "vertical",
                    align: "right",
                    verticalAlign: "middle"
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        }
                    }
                },

                series: [
                    {
                        name: "Online Banking",
                        data: directBank
                    },
                    {
                        name: "VNpay",
                        data: vnPay
                    },
                    {
                        name: "Paypal",
                        data: paypal
                    }, {
                        name: "Total",
                        data: amount
                    }
                ],

                responsive: {
                    rules: [
                        {
                            condition: {
                                maxWidth: 500
                            },
                            chartOptions: {
                                legend: {
                                    layout: "horizontal",
                                    align: "center",
                                    verticalAlign: "bottom"
                                }
                            }
                        }
                    ]
                }
            });
    }


    loadDonationChart();
});