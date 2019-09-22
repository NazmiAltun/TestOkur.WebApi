function eachWitIndex(context, start, end, options) {
    var ret = "";

    for (var i = start, j = end; i < j && i < options.length; i++) {
        ret = ret + options.fn(context[i]);
    }
    return ret;
}

function formatTwoDecimalPlaces(number) {
    return number.toFixed(2);
}

function formatOneDecimalPlace(number) {
    return number.toFixed(1);
}

function getQuestionClass(result) {
    if (result == 1 || result == 4) {
        return 'wrong';
    }
    if (result == 3) {
        return 'empty';
    }
    return 'correct';
}