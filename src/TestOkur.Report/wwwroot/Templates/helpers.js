function getQuestionClass(result) {
	if (result === 1 || result === 4) {
		return 'wrong';
	}
	if (result === 3) {
		return 'empty';
	}
	return 'correct';
}
function formatTwoDecimalPlaces(number) {
    return number.toFixed(2);
}
function formatOneDecimalPlace(number) {
    return number.toFixed(1);
}
function getStatusClass(status) {
    if (status === 0) {
        return 'pending';
    }
    if (status === 3 || status === 2) {
        return 'error';
    }
    return '';
}