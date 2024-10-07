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

// Pre Load in Schedule Edit

// Function to pre-check checkboxes based on the value of the hidden input
function preCheckDays() {
    //console.log("working")

    const daysValue = document.getElementById('days').value;

    // Break the abbreviation into individual day values
    const dayAbbreviations = [];

    // Handle TH (Thursday) separately since it's 2 characters
    if (daysValue.includes('TH')) {
        dayAbbreviations.push('TH');
    }

    // Handle the rest of the characters (M, T, W, F, S)
    for (let char of daysValue) {
        if (char !== 'H') { // Skip 'H' since we already handled 'TH'
            dayAbbreviations.push(char);
        }
    }

    // Map the abbreviations to their full day names and check the corresponding boxes
    dayAbbreviations.forEach(abbreviation => {
        const dayName = abbreviationMap[abbreviation];

        // Find the checkbox with the value corresponding to the day name and check it
        const checkbox = document.querySelector(`input[value="${dayName}"]`);
        if (checkbox) {
            checkbox.checked = true;
        }
    });
}
