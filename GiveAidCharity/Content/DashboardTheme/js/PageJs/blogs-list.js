$(function () {
    const advance = $("#advance").val();
    // ReSharper disable  CoercedEqualsUsing
    if (advance == 1) {
        $("#searchCollapseAdvance").addClass("show");
    } else if (advance == 0) {
        $("#searchCollapseAdvance").removeClass("show");
    }



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
    $("#title").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Api/AjaxFindByBlogTitle",
                type: "POST",
                dataType: "json",
                data: { title: request.term },
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
