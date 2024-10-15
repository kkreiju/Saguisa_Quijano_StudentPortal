// Select all input fields of type="number"
const numberInputs = document.querySelectorAll('input[type="number"]');

// Loop through each number input and apply the event listener
numberInputs.forEach(input => {
    // Restrict input to digits only
    input.addEventListener('input', function (e) {
        this.value = this.value.replace(/[^0-9]/g, ''); // Replace non-numeric characters
    });

    // Prevent non-numeric paste
    input.addEventListener('paste', function (e) {
        const pastedData = e.clipboardData.getData('text');
        if (!/^\d+$/.test(pastedData)) {
            e.preventDefault(); // Prevent paste if it's not only numbers
        }
    });

    // Optional: Prevent non-numeric key inputs (e.g., block "e", "+", "-")
    input.addEventListener('keydown', function (e) {
        // Allow: backspace, delete, tab, escape, enter, arrows, digits
        if (
            [46, 8, 9, 27, 13].indexOf(e.keyCode) !== -1 || // Allow special keys: delete, backspace, etc.
            (e.keyCode >= 35 && e.keyCode <= 40) || // Arrow keys
            (e.keyCode >= 48 && e.keyCode <= 57) || // Numbers on the main keyboard (0-9)
            (e.keyCode >= 96 && e.keyCode <= 105)   // Numbers on the numpad (0-9)
        ) {
            return; // Allow these keys
        }

        // Allow Ctrl+A, Ctrl+C, Ctrl+V (key combinations)
        if ((e.ctrlKey || e.metaKey) && (e.key === 'a' || e.key === 'c' || e.key === 'v')) {
            return; // Allow Ctrl+A, Ctrl+C, Ctrl+V
        }
        e.preventDefault(); // Prevent all other keypresses
    });
});

// Day abbreviations
const dayAbbreviations = {
    MON: 'M',
    TUE: 'T',
    WED: 'W',
    THU: 'TH',
    FRI: 'F',
    SAT: 'S'
};

// All combinations of day abbreviations
const abbreviationMap = {
    'M': 'MON',
    'T': 'TUE',
    'W': 'WED',
    'TH': 'THU',
    'F': 'FRI',
    'S': 'SAT',
    // 2 days combinations
    'M,T': 'MT',
    'M,W': 'MW',
    'M,TH': 'MTH',
    'M,F': 'MF',
    'M,S': 'MS',
    'T,W': 'TW',
    'T,TH': 'TTH',
    'T,F': 'TF',
    'T,S': 'TS',
    'W,TH': 'WTHU',
    'W,F': 'WF',
    'W,S': 'WS',
    'TH,F': 'THF',
    'TH,S': 'THS',
    'F,S': 'FS',
    // 3 days combinations
    'M,T,W': 'MTW',
    'M,T,TH': 'MTTH',
    'M,T,F': 'MTF',
    'M,T,S': 'MTS',
    'M,W,TH': 'MWTH',
    'M,W,F': 'MWF',
    'M,W,S': 'MWS',
    'M,TH,F': 'MTHF',
    'M,TH,S': 'MTHS',
    'M,F,S': 'MFS',
    'T,W,TH': 'TWTH',
    'T,W,F': 'TWF',
    'T,W,S': 'TWS',
    'T,TH,F': 'TTHF',
    'T,TH,S': 'TTHS',
    'T,F,S': 'TFS',
    'W,TH,F': 'WTHF',
    'W,TH,S': 'WTHS',
    'W,F,S': 'WFS',
    'TH,F,S': 'THFS'
};

const weekdayOrder = ['M', 'T', 'W', 'TH', 'F', 'S'];

// Concatenates days selected
function updateSelectedDays() {
    const checkboxes = document.querySelectorAll('.form-check-input');

    // Get checked days and map them to abbreviations
    const selectedDays = Array.from(checkboxes)
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);

    // Sort based on weekday order (Mon -> Sat)
    selectedDays.sort((a, b) => {
        return weekdayOrder.indexOf(a) - weekdayOrder.indexOf(b);
    });

    // Map sorted days to their abbreviations
    const selectedAbbreviations = selectedDays.map(day => dayAbbreviations[day]);

    // Join abbreviations into a string for lookup
    const joinedDays = selectedAbbreviations.join(',');

    // Get the abbreviation from the map, or fallback to the concatenated value
    const abbreviation = abbreviationMap[joinedDays] || selectedAbbreviations.join(',');

    // Store the result in the hidden field
    document.getElementById('days').value = abbreviation;
}

function getSelectedDays(days) {
    // Break the abbreviation into individual day values
    const dayAbbreviations = [];

    // Handle TH (Thursday) separately since it's 2 characters
    let thursdayremover = days;
    if (days.includes('TH')) {
        dayAbbreviations.push('TH');
        thursdayremover = thursdayremover.replace('TH', '');
    }

    // Handle the rest of the characters (M, T, W, F, S)
    for (let char of thursdayremover) {
        if (char !== 'H') { // Skip 'H' since we already handled 'TH'
            dayAbbreviations.push(char);
        }
    }

    let alldays = [];

    // Map the abbreviations to their full day names and check the corresponding boxes
    dayAbbreviations.forEach(abbreviation => {
        const dayName = abbreviationMap[abbreviation];   
        alldays += dayName + ",";
    });

    return alldays;
}


// Add event listener to all checkboxes
document.querySelectorAll('.form-check-input').forEach(checkbox => {
    checkbox.addEventListener('change', updateSelectedDays);
});

const submitBtn = document.getElementById('submitBtn');
const checkboxes = document.querySelectorAll('.form-check-input');
const tooltipMessage = document.getElementById('tooltipMessage');

// Add event listener for the submit button
submitBtn.addEventListener('click', function (event) {
    // Check if any checkbox is selected
    let isAnyChecked = false;
    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            isAnyChecked = true;
        }
    });

    // If no checkbox is checked, alert the user and prevent form submission
    if (!isAnyChecked) {
        tooltipMessage.style.display = 'block';
        daysform.scrollIntoView({
            behavior: 'smooth',  // Smooth scrolling
            block: 'start'       // Scroll to the top of the element
        });
        event.preventDefault(); // Prevent the form from submitting
    }
    else {
        tooltipMessage.style.display = 'none';
    }
});

function formatTime(timeString) {
    const [hours, minutes, seconds] = timeString.split(':');
    const formattedHours = parseInt(hours, 10) % 12 || 12;
    const formattedMinutes = minutes.padStart(2, '0');
    const amPm = hours < 12 ? 'AM' : 'PM';

    return `${formattedHours}:${formattedMinutes} ${amPm}`;
}