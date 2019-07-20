function getQuestionClass(result) {
	if (result == 1 || result == 4) {
		return 'wrong';
	}
	if (result == 3) {
		return 'empty';
	}
	return 'correct';
}