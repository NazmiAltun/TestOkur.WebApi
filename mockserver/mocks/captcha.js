"use strict";

const fs = require('fs');

module.exports = function(req, res) {
    fs.readFile('./mocks/captcha.png',(err,data) =>{
        if(err){
            console.log(err);
        }
        res.header('Text', 'iWr3').type('png').send(data);
    });
};
