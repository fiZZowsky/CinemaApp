$(document).ready(function () {
    const seating = document.querySelector('.seating');
    const selectedSeatsList = document.getElementById('selected-seats-list');
    let selectedSeatNumbers = [];
    let selectedRowNumbers = [];

    function handleSeatClick(seat) {
        const isSelected = seat.classList.contains('selected');
        const row = parseInt(seat.dataset.row);
        const seatNumber = parseInt(seat.dataset.seatNumber);

        if (!isSelected) {
            seat.classList.add('selected');
            seat.classList.remove('available');
            selectedSeatNumbers.push(seatNumber + 1);
            selectedRowNumbers.push(row + 1);
        } else {
            seat.classList.remove('selected');
            seat.classList.add('available');

            selectedSeatNumbers = selectedSeatNumbers.filter(num => num !== seatNumber + 1);
            selectedRowNumbers = selectedRowNumbers.filter(num => num !== row + 1);
        }

        updateSelectedSeats();
    }

    function createSeatingPlan() {
        const numRows = 8;
        const numSeatsPerRow = 6;

        for (let row = 0; row < numRows; row++) {
            const rowElement = document.createElement('div');
            rowElement.classList.add('row');

            for (let seatNumber = 0; seatNumber < numSeatsPerRow; seatNumber++) {
                const seat = document.createElement('div');
                seat.classList.add('seat', 'available');
                seat.dataset.row = row;
                seat.dataset.seatNumber = seatNumber;

                seat.addEventListener('click', () => handleSeatClick(seat));

                rowElement.appendChild(seat);
            }

            seating.appendChild(rowElement);
        }
    }

    function updateSelectedSeats() {
        selectedSeatsList.innerHTML = '';

        for (let i = 0; i < selectedSeatNumbers.length; i++) {
            const li = document.createElement('li');
            li.textContent = `Rząd ${selectedRowNumbers[i]}, Miejsce ${selectedSeatNumbers[i]}`;
            selectedSeatsList.appendChild(li);
        }
    }

    function assignListsToInput() {
        const seatsInput = document.getElementById("selected-seats");
        const rowsInput = document.getElementById("selected-rows");

        seatsInput.value = selectedSeatNumbers.join(',');
        rowsInput.value = selectedRowNumbers.join(',');
    }

    $("#CreateTicketModal form").submit(function (event) {
        event.preventDefault();

        assignListsToInput();

        console.log(selectedSeatNumbers);
        console.log(selectedRowNumbers);

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (data) {
                toastr["success"]("Bought new ticket")
            },
            error: function (data) {
                toastr["error"]("Something went wrong")
            }
        });
    });

    createSeatingPlan();
    updateSelectedSeats();
});
