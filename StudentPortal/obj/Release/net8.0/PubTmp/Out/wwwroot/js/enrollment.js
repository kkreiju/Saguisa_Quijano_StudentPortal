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
        addNewRow(schedule);
        edpCodes.push(EDPCode);
    }
}
