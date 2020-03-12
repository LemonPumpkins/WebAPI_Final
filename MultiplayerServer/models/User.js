var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var UserSchema = new Schema({
    username:{
        type:String,
        required:true
    },
    password:{
        type:String,
        required:true
    },
    score:{
        type:Number,
        default:0
    }
});

mongoose.model('users', UserSchema)