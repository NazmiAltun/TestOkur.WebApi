function eachWitIndex(context, start, end, options) {
    var ret = "";

    for (var i = start, j = end; i < j && i < context.length; i++) {
        ret = ret + options.fn(context[i]);
    }
    return ret;
}

function formatTwoDecimalPlaces(number) {
    return Number(number).toFixed(2);
}

function formatOneDecimalPlace(number) {
    return Number(number).toFixed(1);
}

function getQuestionClass(result) {
    if (result === 1 || result === 4) {
        return 'wrong';
    }
    if (result === 3) {
        return 'empty';
    }
    return 'correct';
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

function ifTooManySections(secondaryLessons, lessons, opts) {
    if (secondaryLessons || lessons.length > 5) {
        return opts.fn(this);
    } else {
        return opts.inverse(this);
    }
}

function lookup(context, key) {
    return context[key];
}