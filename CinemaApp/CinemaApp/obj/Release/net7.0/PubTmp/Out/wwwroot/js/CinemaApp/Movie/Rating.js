$(document).ready(function () {

    const RenderRatings = (comments, container, currUserId, isAdmin) => {
        container.empty();
        for (const comment of comments) {
            const ratingStars = Array(5).fill('<span class="bi bi-star"></span>');
            for (let i = 0; i < 5; i++) {
                if (i < comment.rateValue) {
                    ratingStars[i] = '<span class="bi bi-star-fill"></span>';
                } else if (i + 0.5 === comment.rateValue) {
                    ratingStars[i] = '<span class="bi bi-star-half"></span>';
                }
            }

            const deleteButton = (currUserId != '' && (comment.createdByUserId == currUserId || isAdmin === "True")) == true
                ? `<button class="btn btn-danger delete-rating" data-comment-id="${comment.id}" data-created-by-user-id="${comment.createdByUserId}">Delete</button>`
                : '';

            console.log(deleteButton);

            container.append(`
            <div class="d-flex flex-column mb-3" style="border: 1px solid #495057; border-radius: 15px; background: radial-gradient(circle at 100px 100px, rgba(255,255,255,0.3) 0%, rgba(255,255,255,0) 100%); padding: 1rem;">
                <div class="d-flex align-items-center card-header">
                    <div style="margin-right:1rem;">${comment.createdBy}</div>
                    <div class="rating-stars">${ratingStars.join('')}</div>
                </div>
                <div class="card-body">
                    <h5 class="card-title">${comment.comment}</h5>
                    ${deleteButton}
                </div>
            </div>
            `);
        }
    }

    const LoadRatings = () => {
        const container = $("#comments");
        const movieId = container.data("movieId");
        const currUserId = container.data("currUserId");
        const isAdmin = container.data("isAdmin");

        $.ajax({
            url: `/CinemaApp/${movieId}/MovieRatings`,
            type: 'get',
            success: function (data) {
                if (!data.length) {
                    container.html("There are no ratings for this movie");
                } else {
                    RenderRatings(data, container, currUserId, isAdmin);
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

    $("#comments").on("click", ".delete-rating", function () {
        const commentId = $(this).data("comment-id");
        const movieId = $("#comments").data("movieId");
        const createdByUserId = $(this).data("createdByUserId");

        $.ajax({
            url: `/CinemaApp/${movieId}/DeleteRating/${commentId}/${createdByUserId}`,
            type: 'DELETE',
            success: function (data) {
                toastr["success"]("Successfully deleted your rating.");
                LoadRatings();
            },
            error: function () {
                toastr["error"]("An error occurred while deleting your rating.");
            }
        });
    });
});
