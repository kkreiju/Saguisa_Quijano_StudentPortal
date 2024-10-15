let edpCodes = [];

function addNewRow(schedule) {
    const subjCode = schedule.subjCode;
    const course = schedule.course;

    const matchedSubject = subjects.find(subject => subject.subjCode == subjCode && subject.subjCourseCode == course);

    // Retrieve values from the matched schedule object
    const edpCode = schedule.edpCode;
    const time = formatTime(schedule.startTime) + " - " + formatTime(schedule.endTime);
    const days = schedule.days;
    const room = schedule.room;
    const category = matchedSubject.subjCategory;
    const units = matchedSubject.subjUnits;

    const table = document.getElementById("enrollmenttable");
    const tbody = table.querySelector("tbody");

    const newRow = tbody.insertRow();

    newRow.insertCell().textContent = edpCode;
    newRow.insertCell().textContent = subjCode;
    newRow.insertCell().textContent = time;
    newRow.insertCell().textContent = days;
    newRow.insertCell().textContent = room;
    newRow.insertCell().textContent = category;
    newRow.insertCell().textContent = units;
    subjectscount++;
    unitscount += units;
    updateCount();

    // Create and add the button in the last cell
    const buttonCell = newRow.insertCell();
    const button = document.createElement('button');
    button.className = "btn btn-danger"; // Add bootstrap
    button.id = "deletebutton";
    button.textContent = "Delete"; // Label the button

    // Add click event to the button
    button.addEventListener('click', function () {
        // Find the index of the matched value in the array
        const index = edpCodes.indexOf(edpCode.toString());

        // Remove the matched value from the array
        if (index > -1) {
          edpCodes.splice(index, 1);
        }

        newRow.remove();
        subjectscount--;
        unitscount -= units;
        updateCount();
    });

    // Append the button to the cell
    buttonCell.appendChild(button);
}

function searchSchedules() {
    // Get the value entered in the input field
    const edpCodeInput = document.getElementById('edpcode').value;

    // Check if the input EDPCode exists in the schedules list
    const matchedSchedule = schedules.find(schedule => schedule.edpCode == edpCodeInput);

    // Handle success or error
    if (matchedSchedule) {
        checkDuplicateEDP(matchedSchedule, edpCodeInput);
    } else {
        alert("EDP Code not found!");
    }
}

function updateCount() {
    const table = document.getElementById("enrollmenttable");
    const thead = table.querySelector("thead");
    const firstRow = thead.querySelector("tr");
    const footer = document.getElementById("totalunits");

    //Update Units
    document.getElementById("totalunits").innerHTML = "Total Units: " + unitscount.toString();

    // Add Options into THead
    if (!search) {
        search = true;

        // Create a new `th` element for the "Options" column
        const optionsTh = document.createElement("th");
        optionsTh.textContent = "Options";

        // Insert the new `th` element into the first row of the `thead`
        firstRow.appendChild(optionsTh);
        footer.colSpan = 8;
    }
    else if (search && subjectscount == 0) {
        // Removes "Options" when there is no schedules listed
        search = false;
        firstRow.removeChild(firstRow.children[7]);
        footer.colSpan = 7; 
    }

    if (unitscount == 0) {
        document.getElementById("enrollmentbutton").disabled = true;
    }
    else {
        document.getElementById("enrollmentbutton").disabled = false;
    }
}

function checkDuplicateEDP(schedule, EDPCode) {
    // Get all the table rows
    let rows = document.querySelectorAll("table tr");

    // If there are no schedules in the table
    if (rows.length === 2) {
        addNewRow(schedule);
        rows = document.querySelectorAll("table tr");
        edpCodes.push(EDPCode);
        return;
    }

    if (edpCodes.includes(EDPCode)) {
        alert(EDPCode.toString() + " is duplicate.");
        return;
    }
    else {
        checkConflictTime(schedule);

        if (!conflict) {
            addNewRow(schedule);
            edpCodes.push(EDPCode);
        }
        else {
            alert("Schedule Conflict with EDP Code " + conflictEDPCode.toString());
        }
    }
}

let conflict = false;
let conflictEDPCode;

function checkConflictTime(schedule) {
    // Get all the table rows
    let rows = document.querySelectorAll("table tr");

    // Get the time of the schedule to be added
    const startTime = schedule.startTime;
    const endTime = schedule.endTime;

    // Iterate through each row
    for (let i = 1; i < rows.length; i++) {

        // Get the time of the schedule in the row
        const rowSchedule = schedules.find(schedule => schedule.edpCode == rows[1].cells[0].textContent);
        const rowStartTime = rowSchedule.startTime;
        const rowEndTime = rowSchedule.endTime;

        // Convert the time values to hours
        const startTimeHours = convertToHours(startTime);
        const endTimeHours = convertToHours(endTime);
        const rowStartTimeHours = convertToHours(rowStartTime);
        const rowEndTimeHours = convertToHours(rowEndTime);

        // Check if the time of the schedule to be added conflicts with the time of the schedule in the row
        if (startTime >= rowStartTime && startTime <= rowEndTime || endTime >= rowStartTime && endTime <= rowEndTime) {
            conflict = true;
            checkConflictDay(schedule, rowSchedule);
            return;
        }
    }
    conflict = false;
}

function checkConflictDay(schedule, rowSchedule) {
    let scheduleDays = [];
    let rowScheduleDays = [];
    scheduleDays = getSelectedDays(schedule.days).split(",");
    rowScheduleDays = getSelectedDays(rowSchedule.days).split(",");

    // Removes the extra value whitespace [""]
    scheduleDays.pop();
    rowScheduleDays.pop();

    for (let day of scheduleDays) {
        if (rowScheduleDays.includes(day)) {
            conflict = true;
            conflictEDPCode = rowSchedule.edpCode;
            return;
        }
    }
    conflict = false;
}

function convertToHours(time) {
    const [hours, minutes] = time.split(':');
    return parseInt(hours) + parseInt(minutes) / 60;
}


function enrollStudent() {
    const idnumber = document.getElementById('idnumber').value;

    const student = enrollees.find(e => e.id == idnumber);

    if (student == null) {
        // Get all the table rows
        let rows = document.querySelectorAll("table tr");
        let data = [];

        // Iterate through each row
        for (let i = 1; i < rows.length - 1; i++) {
            var rowData = {
                edpCode: rows[i].cells[0].textContent,
            };
            data.push(rowData);
        }

        // Send data to the server
        fetch('/Enrollment/EnrollStudent?idnumber=' + idnumber + '&units=' + unitscount, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.text())
            .then(() => { window.location.href = '/Enrollment/Entry'; document.getElementById("prompt").style = 'display: block;'; document.getElementById("prompttext").innerHTML = 'Successfully Enrolled.'; })
            .then(() => alert('Successfully Enrolled.'))
            .catch((error) => alert('Error, please try again.'));
    }
    else {
        alert("Student is already enrolled!");
        return;
    }
}
