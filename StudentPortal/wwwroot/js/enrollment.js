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
        const rowIndex = newRow.rowIndex - 1; // Adjust for header row
        console.log(edpCodes.pop(edpCode));
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
        if (!checkConflictTime(schedule)) {
            addNewRow(schedule);
            edpCodes.push(EDPCode);
        }
    }
}

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
            alert("Schedule with EDP Code: " + rows[i].cells[0].textContent);
            return true;
        }
    }
    return false;
}

function convertToHours(time)
    const [hours, minutes] = time.split(':');
    return parseInt(hours) + parseInt(minutes) / 60;
}
