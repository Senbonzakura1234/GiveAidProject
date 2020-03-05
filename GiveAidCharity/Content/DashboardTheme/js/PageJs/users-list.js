$(function () {
    $(".sortData").click(function () {
        const sortByData = $(this).data("sort");
        $('input[name="sortBy"]').val(sortByData);
        const directData = $(this).data("direct");
        $('input[name="direct"]').val(directData);
        console.log(directData);
        $("#productForm").submit();
    });

    $(function() {
        $('[data-toggle="tooltip"]').tooltip();
    });
});