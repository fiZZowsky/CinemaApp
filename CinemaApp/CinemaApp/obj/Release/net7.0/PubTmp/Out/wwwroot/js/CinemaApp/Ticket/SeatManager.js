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
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                numRows = data.rows;
                numSeatsPerRow = data.seats;

                getUnavailableSeats();
                createSeatingPlan();
            },
            error: function () {
                toastr['error']("Something went wrong");
            }
        })
    }

    function getUnavailableSeats() {
        // Change datetime format to yyyy-MM-ddTHH:mm:ss
        const parts = startTime.split(' ');
        const datePart = parts[0];
        const timePart = parts[1];

        const date = new Date(datePart);
        const year = date.getFullYear();
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');

        const timeComponents = timePart.split(':');
        const hour = timeComponents[0];
        const minute = timeComponents[1];
        const second = timeComponents[2];

        const isoStartTime = `${year}-${month}-${day}T${hour}:${minute}:${second}`;

        $.ajax({
            url: `/Ticket/GetNotAvailableSeats?hallNumber=${numberOfHall}&startTime=${isoStartTime}`,
            type: 'get',
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
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
            selectedSeatNumbers.push(seatNumber + 1);
            selectedRowNumbers.push(row + 1);
        } else {
            seat.classList.remove('selected');
            const seatIndex = selectedSeatNumbers.indexOf(seatNumber + 1);
            const rowIndex = selectedRowNumbers.indexOf(row + 1);

            if (seatIndex !== -1) {
                selectedSeatNumbers.splice(seatIndex, 1);
            }

            if (rowIndex !== -1) {
                selectedRowNumbers.splice(rowIndex, 1);
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
            const paragraph = document.createElement('p');
            paragraph.textContent = `${selectedRowNumbers[i]} row ${selectedSeatNumbers[i]} seat`;
            selectedSeatsList.appendChild(paragraph);
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
        const sum = normalPriceSeats + reducedPriceSeats;

        if (selectedSeatNumbers.length < 1) {
            toastr["error"]("No seat in the room was chosen.");
            return;
        }

        if (isNaN(normalPriceSeats) || isNaN(reducedPriceSeats) || (sum != selectedSeatNumbers.length)) {
            toastr["error"]("Please enter valid numbers for normal and reduced price seats.");
            return;
        }

        assignListsToInput();
        $.ajax({
            url: `/Ticket/CreateCheckoutSession`,
            type: 'POST',
            data: $(this).serialize(),
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                window.location.href = data.sessionUrl;
            },
            error: function (data) {
                toastr["error"]("Something went wrong");
            }
        });
    });

    getHallData();
    getUnavailableSeats();
    updateSelectedSeats();
});