$(function () {
    $(".rating-control").on('click', function () {
        event.preventDefault();
        var obj = $(this);
        $("#statusValue").val($(this).hasClass("active") ? 0 : $(this).hasClass("like") ? 1 :
            $(this).hasClass("dislike") ? -1 : 0);
        $('#ratingValue').val(function (i, old) {
            console.log(old);
            if (obj.hasClass("active")) {
                if (obj.hasClass("like")) {
                    old--;
                } else if (obj.hasClass("dislike")) {
                    old++;
                }
            } else {
                if (obj.hasClass("like")) {
                    old++;
                    if ($(".dislike").hasClass("active")) {
                        old++;
                    }
                } else if (obj.hasClass("dislike")) {
                    old--;
                    if ($(".like").hasClass("active")) {
                        old--;
                    }
                }
            }
            return old;
        });

        $(this).toggleClass("active");

        if ($(this).hasClass("like")) {
            $('.dislike').removeClass("active");
        } else if ($(this).hasClass("dislike")) {
            $('.like').removeClass("active");
        } else {
            $('.rating-control').removeClass("active");
        }

        $("#ratingValueDisplay").html($('#ratingValue').val());
        submitRating();
    });

    function submitRating() {
        const form = $('#__AjaxAntiForgeryForm');
        const token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            url: $("#urlSubmit").val(),
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                userId: $('input[name="userId"]', form).val(),
                blogId: $('input[name="blogId"]', form).val(),
                status: $('input[name="status"]', form).val()
            },
            success: function (res) {
                console.log(res);
            }
        });
        setTimeout(
            function () {
                getRating1();
            }, 3000);
    }
    function getRating1() {
        $.ajax({
            url: $("#urlGetRating").val(),
            type: 'GET',
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
});