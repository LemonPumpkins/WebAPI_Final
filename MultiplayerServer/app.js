var io = require("socket.io")(process.env.PORT || 3000);
var shortid = require("shortid");
var mongoose = require("mongoose");

/*module.exports ={
   mongoURI:"mongodb://localhost:27017/highscore"
}*/
require('./models/User');
var User = mongoose.model('users');

console.log('connected');
//console.log(shortid.generate());

var players = [];

mongoose.connect("mongodb://localhost:27017/highscore", {
    useNewUrlParser:true,
    useUnifiedTopology:true
}).then(function(){
    console.log("Db connected")
}).catch(function(err){
    console.log(err);
})

io.on('connection', function(socket){

    var players = [];

    console.log('client connected');
    var thisClientId = shortid.generate();

    players.push(thisClientId);

    //spawn joined playewrs
    socket.broadcast.emit('spawn', {id:thisClientId});
    //Request active players positions
    socket.broadcast.emit('requestPos');

    players.forEach(function(playerId){

    if(playerId == thisClientId){
        return;
    }
        socket.emit('spawn', {id:playerId});
        console.log("spawn players");
    });

    socket.on('yolo', function(data){

        console.log("You only only yolo yolo");
        console.log(data);
    });

    socket.on("login", function(data){
        
        
    });
    socket.on("register", function(data){
        console.log(data.username + " " + data.password);
        var newUser={
            username:data.username,
            password:data.password,
            score:0
        }
        new User(newUser).save();
       // socket.emit("buildLeaders", "frog");
    });
    socket.on("leaderboard", function(){
        
        var usersArr = [];
       
        
        User.find(User.score).then(function(users){
           // usersArr = users;
       
            for (let index = 0; index < users.length; index++) {
                if(users[index].score > 0){
                    usersArr[index] = users[index];
                }
            }
            
            console.log(usersArr);
            socket.emit("PatMAgic", usersArr);
            //socket.emit("buildLeaders", users);
            
        });
        
        //console.log(data.username + " " + data.password);
    /*    var newUser={
            username:data.username,
            password:data.password,
            score:0
        }
        new User(newUser).save();*/
    });
    socket.on("updatePos", function(data){
        data.id = thisClientId;
        socket.broadcast.emit("updatePos", data);

    });

    socket.on('move', function(data){
        data.id = thisClientId;
        console.log("player is moving", JSON.stringify(data));
        socket.broadcast.emit('move');
    });


    socket.on('disconnect', function(){
        console.log("Player disconnected");

        players.splice(players.indexOf(thisClientId), 1);
        socket.broadcast.emit('disconnected', {id:thisClientId});

    });
})