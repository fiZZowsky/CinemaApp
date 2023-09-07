$(document).ready(function () {
    const seating = document.querySelector('.seating');
    const selectedSeatsList = document.getElementById('selected-seats-list');
    const numberOfHall = document.getElementById('hallNumber').value;
    const startTime = document.getElementById('startTime').value;
    let selectedSeatNumbers = [];
    let selectedRowNumbers = [];
    var numRows;
    var numSeatsPerRow;

    function getHallData() {
        $.ajax({
            url: `/Ticket/GetHallData/${numberOfHall}`,
            type: 'get',
            success: function (data) {
                numRows = data.rows;
                numSeatsPerRow = data.seats;

                getUnavailableSeats();
                createSeatingPlan();
            },
            error: function () {
                toastr['error']("Something went wrong")
            }
        })
    }

    function getUnavailableSeats() {
        // Change datetime format to yyyy-MM-ddTHH:mm:ss
        const parts = startTime.split(' ');
        const datePart = parts[0];
        const timePart = parts[1];

        const dateComponents = datePart.split('.');
        const day = dateComponents[0];
        const month = dateComponents[1];
        const year = dateComponents[2];

        const timeComponents = timePart.split(':');
        const hour = timeComponents[0];
        const minute = timeComponents[1];
        const second = timeComponents[2];

        isoStartTime = `${year}-${month}-${day}T${hour}:${minute}:${second}`;

        $.ajax({
            url: `/Ticket/GetNotAvailableSeats?hallNumber=${numberOfHall}&startTime=${isoStartTime}`,
            type: 'get',
            dataType: 'json',
            success: function (data) {
                markUnavailableSeats(data);
            },
            error: function () {
                toastr['error']("Something went wrong");
            }
        });
    }

    function markUnavailableSeats(unavailableSeats) {
        for (const seat of unavailableSeats) {
            const row = seat.rowNumber - 1;
            const seatNumber = seat.number - 1;

            const seatElement = document.querySelector(`.seat[data-row="${row}"][data-seat-number="${seatNumber}"]`);

            if (seatElement) {
                seatElement.classList.remove('available', 'selected');
                seatElement.classList.add('unavailable');
            }
        }
    }

    function handleSeatClick(seat) {
        const isAvailable = seat.classList.contains('available');

        if (!isAvailable) {
            return;
        }

        const isSelected = seat.classList.contains('selected');
        const row = parseInt(seat.dataset.row);
        const seatNumber = parseInt(seat.dataset.seatNumber);

        if (!isSelected) {
            seat.classList.add('selected');
            selectedSeatNumbers.push({ row: row + 1, seatNumber: seatNumber + 1 });
        } else {
            seat.classList.remove('selected');
            const seatIndex = selectedSeatNumbers.findIndex(s => s.row === row + 1 && s.seatNumber === seatNumber + 1);
            if (seatIndex !== -1) {
                selectedSeatNumbers.splice(seatIndex, 1);
            }
        }

        updateSelectedSeats();
    }


    function createSeatingPlan() {
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

        const normalPriceSeats = parseInt($("#normalPriceSeats").val());
        const reducedPriceSeats = parseInt($("#reducedPriceSeats").val());
        if (isNaN(normalPriceSeats) || isNaN(reducedPriceSeats) || (normalPriceSeats + reducedPriceSeats) !== selectedSeatNumbers.length) {
            toastr["error"]("Selected incorrect number of ticket types.");
            return;
        }

        assignListsToInput();
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

    getHallData();
    getUnavailableSeats();
    updateSelectedSeats();
});
