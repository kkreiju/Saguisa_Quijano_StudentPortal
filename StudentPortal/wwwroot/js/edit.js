const idlimit = document.getElementById('idnumber');
const idlimit2 = document.getElementById('idnumber2');

if (idlimit) {
	idlimit.addEventListener('input', function () {
		if (idlimit.value.length > 9) {
			idlimit.value = idlimit.value.slice(0, 9)
		}
		if (idlimit.value < 1) {
			idlimit.value = ''
		}
	});
}

if (idlimit2) {
	idlimit2.addEventListener('input', function () {
		if (idlimit2.value.length > 9) {
			idlimit2.value = idlimit2.value.slice(0, 9)
		}
		if (idlimit2.value < 1) {
			idlimit2.value = ''
		}
	});
}

const input = document.getElementById('year');

input.addEventListener('input', function () {
	if (input.value.length > 1) {
		input.value = input.value.slice(0, 1)
	}
	if (input.value < 1 || input.value > 5) {
		input.value = ''

	}
});
