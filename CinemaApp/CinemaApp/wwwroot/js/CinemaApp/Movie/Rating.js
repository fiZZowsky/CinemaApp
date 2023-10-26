$(document).ready(function () {

    const RenderRatings = (comments, container) => {
        container.empty();
        console.log(comments);
        for (const comment of comments) {
            const ratingStars = Array(5).fill('<span class="bi bi-star"></span>');
            for (let i = 0; i < 5; i++) {
                if (i < comment.rateValue) {
                    ratingStars[i] = '<span class="bi bi-star-fill"></span>';
                } else if (i + 0.5 === comment.rateValue) {
                    ratingStars[i] = '<span class="bi bi-star-half"></span>';
                }
            }

            container.append(`
        <div class="d-flex flex-column mb-3" style="border: 1px solid #495057; border-radius: 15px; background: rgb(255,255,255);
             background: radial-gradient(circle at 100px 100px, rgba(255,255,255,0.3) 0%, rgba(255,255,255,0) 100%); padding: 1rem;">
            <div class="rating-stars">${ratingStars.join('')}</div>
            <div class="card-body">
                <h5 class="card-title">${comment.comment}</h5>
            </div>
        </div>
    `);
        }
    }

    const LoadRatings = () => {
        const container = $("#comments");
        const movieId = container.data("movieId");
        $.ajax({
            url: `/CinemaApp/${movieId}/MovieRatings`,
            type: 'get',
            success: function (data) {
                if (!data.length) {
                    container.html("There are no ratings for this movie");
                } else {
                    RenderRatings(data, container);
                }
            },
            error: function () {
                toastr["error"]("An error occurred while loading comments.")
            }
        })
    }

    LoadRatings();

    $("#CreateRatingPartial form").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (data) {
                toastr["success"]("Successfully added new rating.")
                LoadRatings();
            },
            error: function () {
                toastr["error"]("Something went wrong.")
            }
        })
    });
});