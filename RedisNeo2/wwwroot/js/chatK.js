var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButtonKorisnik").disabled = true;

connection.on("NBP_Chat", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {

    connection.invoke("re").then(function (u) {
        document.getElementById("userNPBk").innerText = u;
    });

    document.getElementById("sendButtonKorisnik").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("ngo_user").value;
//    var message = document.getElementById("messageInput").value;
//    const messageString = JSON.stringify(message);
//    connection.invoke("SendMessage", user, messageString).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

document.getElementById("sendButtonKorisnik").addEventListener("click", function (event) {
    var user = document.getElementById("userNPBk").innerText;
    //var user = document.getElementById("korisnik_user").innerHTML;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

