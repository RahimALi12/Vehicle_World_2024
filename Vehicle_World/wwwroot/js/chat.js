const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

//connection.start().then(function () {
//    console.log("SignalR connected.");
//}).catch(function (err) {
//    return console.error(err.toString());
//});

// Start the connection
connection.start().catch(err => console.error(err.toString()));

function startChat(sellerId, buyerId) {
    // Logic to open the chat UI and connect the users
    // You could load a modal or redirect to a chat page
    openChatUI(sellerId, buyerId);
}

function openChatUI(sellerId, buyerId) {
    // Example: Display the chat modal with both IDs
    document.getElementById("chatModal").style.display = "block";

    // Send a message using the Hub
    //document.getElementById("sendButton").addEventListener("click", function () {
    //    const message = document.getElementById("messageInput").value;
    //    connection.invoke("SendMessage", buyerId, sellerId, message).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //    event.preventDefault();
    //});

    document.getElementById("sendButton").addEventListener("click", function (event) {
        const sellerId = document.getElementById("sellerId").value;  // Hidden field or part of the URL
        const message = document.getElementById("messageInput").value;

        // Send the message to the server
        connection.invoke("SendMessage", sellerId, message).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    });

}
// Extract sellerId from URL
const urlParams = new URLSearchParams(window.location.search);
const sellerId = urlParams.get('sellerId');

// Use sellerId in SignalR messages

// Receive messages
//connection.on("ReceiveMessage", function (message) {
//    const msg = document.createElement("div");
//    msg.textContent = message;
//    document.getElementById("chatMessages").appendChild(msg);
//});

connection.on("ReceiveMessage", function (sender, message) {
    const messageContainer = document.getElementById("messagesList");
    const messageItem = document.createElement("li");
    messageItem.textContent = sender + ": " + message;
    messageContainer.appendChild(messageItem);
});

