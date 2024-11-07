const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.start().then(function () {
    console.log("SignalR connected.");
}).catch(function (err) {
    return console.error(err.toString());
});

function startChat(sellerId, buyerId) {
    // Logic to open the chat UI and connect the users
    // You could load a modal or redirect to a chat page
    openChatUI(sellerId, buyerId);
}

function openChatUI(sellerId, buyerId) {
    // Example: Display the chat modal with both IDs
    document.getElementById("chatModal").style.display = "block";

    // Send a message using the Hub
    document.getElementById("sendButton").addEventListener("click", function () {
        const message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", buyerId, sellerId, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}

// Receive messages
connection.on("ReceiveMessage", function (message) {
    const msg = document.createElement("div");
    msg.textContent = message;
    document.getElementById("chatMessages").appendChild(msg);
});
