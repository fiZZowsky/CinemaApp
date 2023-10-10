// Sidebar
const toggler = document.querySelector(".btn");
toggler.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("collapsed");
});

// Calendar fields
$(document).ready(function () {
    var defaultDate = $('.calendar-date').first().data('date');
    selectDate(defaultDate);
});

var selectedDate = null;

$('.calendar-date').click(function () {
    var date = $(this).data('date');
    selectDate(date);
    updateMovieList();
});

function selectDate(date) {
    $('.calendar-date').removeClass('selected');

    $('[data-date="' + date + '"]').addClass('selected');

    selectedDate = date;
}

// Search input
var searchString = null;
$('#search-input').on('input', function () {
    searchString = $(this).val();
    updateMovieList(searchString);
});

// Function to update the movie list based on filters and sorting
function updateMovieList() {
    var selectedHalls = $('.hall-checkbox:checked').map(function () {
        return $(this).val();
    }).get().join(',');

    $.ajax({
        url: '/Movie/SortShowsList',
        data: {
            hallNumber: selectedHalls,
            repertoireDate: selectedDate,
            searchString: searchString
        },
        success: function (data) {
            $('#shows-list-partial').html(data);
        },
        error: function (xhr, status, error) {
            toastr["error"]("Something went wrong");
        }
    });
}

$('.hall-checkbox').change(updateMovieList);