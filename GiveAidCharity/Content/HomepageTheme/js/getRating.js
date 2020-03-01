$(function () {
    function getRating() {
        $.ajax({
            url: $("#urlGetRating").val(),
            type: "GET",
            data: {
                blogId: $('input[name="blogId"]').val()
            },
            success: function (res) {
                console.log(res.rating);
                if (res.rating == null) {
                    $("#ratingValue").val(0);
                    $("#ratingValueDisplay").html(0);
                } else {
                    $("#ratingValue").val(res.rating);
                    $("#ratingValueDisplay").html(res.rating);
                }
            }
        });
    }
    getRating();
    window.setInterval(function () {
        $.ajax({
            url: $("#urlGetRating").val(),
            type: "GET",
            data: {
                blogId: $('input[name="blogId"]').val()
            },
            success: function (res) {
                console.log(res.rating);
                if (res.rating == null) {
                    $("#ratingValue").val(0);
                    $("#ratingValueDisplay").html(0);
                } else {
                    $("#ratingValue").val(res.rating);
                    $("#ratingValueDisplay").html(res.rating);
                }
            }
        });
    }, 20000);
});