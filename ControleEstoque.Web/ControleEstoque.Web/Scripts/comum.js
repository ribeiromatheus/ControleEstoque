﻿function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

$(document)
    .on('keydown', 'input[type="number"]', (e) => {
        if (e.key === 'e') {
            e.preventDefault();
        }
    })