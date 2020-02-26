$(function () {

    // Remove pro banner on close
    document.querySelector("#bannerClose").addEventListener("click", function () {
        document.querySelector("#proBanner").classList.add("d-none");
    });

    window.Chart.defaults.global.legend.labels.usePointStyle = true;

    if ($("#serviceSaleProgress").length) {
        var serviceSaleProgressbar = new window.ProgressBar.Circle(window.serviceSaleProgress, {
            color: "url(#gradient)",
            // This has to be the same size as the maximum width to
            // prevent clipping
            strokeWidth: 8,
            trailWidth: 8,
            easing: "easeInOut",
            duration: 1400,
            text: {
                autoStyleContainer: false
            },
            from: { color: "#aaa", width: 6 },
            to: { color: "#57c7d4", width: 6 }
        });

        serviceSaleProgressbar.animate(.65);  // Number from 0.0 to 1.0
        serviceSaleProgressbar.path.style.strokeLinecap = "round";
        let linearGradient = '<defs><linearGradient id="gradient" x1="0%" y1="0%" x2="100%" y2="0%" gradientUnits="userSpaceOnUse"><stop offset="20%" stop-color="#da8cff"/><stop offset="50%" stop-color="#9a55ff"/></linearGradient></defs>';
        serviceSaleProgressbar.svg.insertAdjacentHTML("afterBegin", linearGradient);
    }
    if ($("#productSaleProgress").length) {
        var productSaleProgressbar = new window.ProgressBar.Circle(window.productSaleProgress, {
            color: "url(#productGradient)",
            // This has to be the same size as the maximum width to
            // prevent clipping
            strokeWidth: 8,
            trailWidth: 8,
            easing: "easeInOut",
            duration: 1400,
            text: {
                autoStyleContainer: false
            },
            from: { color: "#aaa", width: 6 },
            to: { color: "#57c7d4", width: 6 }
        });

        productSaleProgressbar.animate(.6);  // Number from 0.0 to 1.0
        productSaleProgressbar.path.style.strokeLinecap = "round";
        let linearGradient = '<defs><linearGradient id="productGradient" x1="0%" y1="0%" x2="100%" y2="0%" gradientUnits="userSpaceOnUse"><stop offset="40%" stop-color="#36d7e8"/><stop offset="70%" stop-color="#b194fa"/></linearGradient></defs>';
        productSaleProgressbar.svg.insertAdjacentHTML("afterBegin", linearGradient);
    }
    if ($("#points-chart").length) {
        var ctxpointschart = document.getElementById("points-chart").getContext("2d");

        var gradientStrokeVioletpointschart = ctxpointschart.createLinearGradient(0, 0, 0, 181);
        gradientStrokeVioletpointschart.addColorStop(0, "rgba(218, 140, 255, 1)");
        gradientStrokeVioletpointschart.addColorStop(1, "rgba(154, 85, 255, 1)");

        // ReSharper disable once UnusedLocals
        var myChartpointschart = new window.Chart(ctxpointschart,
            {
                type: "bar",
                data: {
                    labels: [1, 2, 3, 4, 5, 6, 7, 8],
                    datasets: [
                        {
                            label: "North Zone",
                            borderColor: gradientStrokeVioletpointschart,
                            backgroundColor: gradientStrokeVioletpointschart,
                            hoverBackgroundColor: gradientStrokeVioletpointschart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "South Zone",
                            borderColor: "#e9eaee",
                            backgroundColor: "#e9eaee",
                            hoverBackgroundColor: "#e9eaee",
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 20]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 10
                                },
                                gridLines: {
                                    drawBorder: false,
                                    display: false
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "#eeeeee"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                barPercentage: 0.7
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
    }
    if ($("#events-chart").length) {
        var ctxeventschart = document.getElementById("events-chart").getContext("2d");

        var gradientStrokeBlueeventschart = ctxeventschart.createLinearGradient(0, 0, 0, 181);
        gradientStrokeBlueeventschart.addColorStop(0, "rgba(54, 215, 232, 1)");
        gradientStrokeBlueeventschart.addColorStop(1, "rgba(177, 148, 250, 1)");

        // ReSharper disable once UnusedLocals
        var myCharteventschart = new window.Chart(ctxeventschart,
            {
                type: "bar",
                data: {
                    labels: [1, 2, 3, 4, 5, 6, 7, 8],
                    datasets: [
                        {
                            label: "Domestic",
                            borderColor: gradientStrokeBlueeventschart,
                            backgroundColor: gradientStrokeBlueeventschart,
                            hoverBackgroundColor: gradientStrokeBlueeventschart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "International",
                            borderColor: "#e9eaee",
                            backgroundColor: "#e9eaee",
                            hoverBackgroundColor: "#e9eaee",
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 20]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 10
                                },
                                gridLines: {
                                    drawBorder: false,
                                    display: false
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "#eeeeee"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                barPercentage: 0.7
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
    }
    if ($("#visit-sale-chart").length) {
        window.Chart.defaults.global.legend.labels.usePointStyle = true;
        var ctxvisitsalechart = document.getElementById("visit-sale-chart").getContext("2d");

        var gradientStrokeVioletvisitsalechart = ctxvisitsalechart.createLinearGradient(0, 0, 0, 181);
        gradientStrokeVioletvisitsalechart.addColorStop(0, "rgba(218, 140, 255, 1)");
        gradientStrokeVioletvisitsalechart.addColorStop(1, "rgba(154, 85, 255, 1)");
        var gradientLegendVioletvisitsalechart = "linear-gradient(to right, rgba(218, 140, 255, 1), rgba(154, 85, 255, 1))";

        var gradientStrokeBluevisitsalechart = ctxvisitsalechart.createLinearGradient(0, 0, 0, 360);
        gradientStrokeBluevisitsalechart.addColorStop(0, "rgba(54, 215, 232, 1)");
        gradientStrokeBluevisitsalechart.addColorStop(1, "rgba(177, 148, 250, 1)");
        var gradientLegendBluevisitsalechart = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

        var gradientStrokeRedvisitsalechart = ctxvisitsalechart.createLinearGradient(0, 0, 0, 300);
        gradientStrokeRedvisitsalechart.addColorStop(0, "rgba(255, 191, 150, 1)");
        gradientStrokeRedvisitsalechart.addColorStop(1, "rgba(254, 112, 150, 1)");
        var gradientLegendRedvisitsalechart = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

        var myChartvisitsalechart = new window.Chart(ctxvisitsalechart,
            {
                type: "bar",
                data: {
                    labels: ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG"],
                    datasets: [
                        {
                            label: "CHN",
                            borderColor: gradientStrokeVioletvisitsalechart,
                            backgroundColor: gradientStrokeVioletvisitsalechart,
                            hoverBackgroundColor: gradientStrokeVioletvisitsalechart,
                            legendColor: gradientLegendVioletvisitsalechart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "USA",
                            borderColor: gradientStrokeRedvisitsalechart,
                            backgroundColor: gradientStrokeRedvisitsalechart,
                            hoverBackgroundColor: gradientStrokeRedvisitsalechart,
                            legendColor: gradientLegendRedvisitsalechart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 40]
                        },
                        {
                            label: "UK",
                            borderColor: gradientStrokeBluevisitsalechart,
                            backgroundColor: gradientStrokeBluevisitsalechart,
                            hoverBackgroundColor: gradientStrokeBluevisitsalechart,
                            legendColor: gradientLegendBluevisitsalechart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [70, 10, 30, 40, 25, 50, 15, 30]
                        }
                    ]
                },
                options: {
                    responsive: true,
                    legend: false,
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < chart.data.datasets.length; i++) {
                            text.push(`<li><span class="legend-dots" style="background:${chart.data.datasets[i].legendColor}"></span>`);
                            if (chart.data.datasets[i].label) {
                                text.push(chart.data.datasets[i].label);
                            }
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 20,
                                    max: 80
                                },
                                gridLines: {
                                    drawBorder: false,
                                    color: "rgba(235,237,242,1)",
                                    zeroLineColor: "rgba(235,237,242,1)"
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "rgba(235,237,242,1)"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                categoryPercentage: 0.5,
                                barPercentage: 0.5
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
        $("#visit-sale-chart-legend").html(myChartvisitsalechart.generateLegend());
    }
    if ($("#visit-sale-chart-dark").length) {
        window.Chart.defaults.global.legend.labels.usePointStyle = true;
        var ctxvisitsalechartdark = document.getElementById("visit-sale-chart-dark").getContext("2d");

        var gradientStrokeVioletvisitsalechartdark = ctxvisitsalechartdark.createLinearGradient(0, 0, 0, 181);
        gradientStrokeVioletvisitsalechartdark.addColorStop(0, "rgba(218, 140, 255, 1)");
        gradientStrokeVioletvisitsalechartdark.addColorStop(1, "rgba(154, 85, 255, 1)");
        var gradientLegendVioletvisitsalechartdark = "linear-gradient(to right, rgba(218, 140, 255, 1), rgba(154, 85, 255, 1))";

        var gradientStrokeBluevisitsalechartdark = ctxvisitsalechartdark.createLinearGradient(0, 0, 0, 360);
        gradientStrokeBluevisitsalechartdark.addColorStop(0, "rgba(54, 215, 232, 1)");
        gradientStrokeBluevisitsalechartdark.addColorStop(1, "rgba(177, 148, 250, 1)");
        var gradientLegendBluevisitsalechartdark = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

        var gradientStrokeRedvisitsalechartdark = ctxvisitsalechartdark.createLinearGradient(0, 0, 0, 300);
        gradientStrokeRedvisitsalechartdark.addColorStop(0, "rgba(255, 191, 150, 1)");
        gradientStrokeRedvisitsalechartdark.addColorStop(1, "rgba(254, 112, 150, 1)");
        var gradientLegendRedvisitsalechartdark = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

        var myChartvisitsalechartdark = new window.Chart(ctxvisitsalechartdark,
            {
                type: "bar",
                data: {
                    labels: ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG"],
                    datasets: [
                        {
                            label: "CHN",
                            borderColor: gradientStrokeVioletvisitsalechartdark,
                            backgroundColor: gradientStrokeVioletvisitsalechartdark,
                            hoverBackgroundColor: gradientStrokeVioletvisitsalechartdark,
                            legendColor: gradientLegendVioletvisitsalechartdark,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "USA",
                            borderColor: gradientStrokeRedvisitsalechartdark,
                            backgroundColor: gradientStrokeRedvisitsalechartdark,
                            hoverBackgroundColor: gradientStrokeRedvisitsalechartdark,
                            legendColor: gradientLegendRedvisitsalechartdark,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 40]
                        },
                        {
                            label: "UK",
                            borderColor: gradientStrokeBluevisitsalechartdark,
                            backgroundColor: gradientStrokeBluevisitsalechartdark,
                            hoverBackgroundColor: gradientStrokeBluevisitsalechartdark,
                            legendColor: gradientLegendBluevisitsalechartdark,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [70, 10, 30, 40, 25, 50, 15, 30]
                        }
                    ]
                },
                options: {
                    responsive: true,
                    legend: false,
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < chart.data.datasets.length; i++) {
                            text.push(`<li><span class="legend-dots" style="background:${chart.data.datasets[i].legendColor}"></span>`);
                            if (chart.data.datasets[i].label) {
                                text.push(chart.data.datasets[i].label);
                            }
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 20,
                                    max: 80
                                },
                                gridLines: {
                                    drawBorder: false,
                                    color: "#322f2f",
                                    zeroLineColor: "#322f2f"
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "rgba(235,237,242,1)"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                categoryPercentage: 0.5,
                                barPercentage: 0.5
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
        $("#visit-sale-chart-legend-dark").html(myChartvisitsalechartdark.generateLegend());
    }
   
    if ($("#inline-datepicker").length) {
        $("#inline-datepicker").datepicker({
            enableOnReadonly: true,
            todayHighlight: true
        });
    }

});