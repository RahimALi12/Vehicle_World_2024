


//var iphone= $('#iphone');

//var layer= $('#main');

//layer.mousemove(function(e){
//  var ivalueX= (e.pageX * -1 / 30);
//  var ivalueY= (e.pageY * -1 / 30);
//  console.log('ok');
//  iphone.css('transform', 'translate3d('+ivalueX+'px,'+ivalueY+'px, 0)');
//});















// function tes() {
//   var element = document.body;
//   element.classList.toggle("bg-dark");
//   element.classList.toggle("text-light");
//   element.style.transition = "all 1s";

//   var jumbotron = document.getElementById("jumbo");
//   jumbotron.classList.toggle("dark-light");
//   jumbotron.style.transition = "all 1s";







// };

// const container = document.getElementById('js-container');
// const setValue = (property, value) => {
//     container.style.setProperty(`--${property}`, value);

// };

// const setValueFromLocalStorage = property => {
//     let value = localStorage.getItem(property);
//     setValue(property, value);
// };

// const setTheme = options => {
//     for (let option of Object.keys(options)) {
//         const property = option;
//         const value = options[option];



















//const d = document;
//const button = d.querySelector('.toogleBtn');

//let darkModeState = false;


//const useDark = window.matchMedia("(prefers-color-scheme: dark)");


//function toggleDarkMode(state) {
//  document.documentElement.classList.toggle("dark-mode", state);
//  darkModeState = state;
//}

//function setDarkModeLocalStorage(state) {
//  localStorage.setItem("dark-mode", state);
//}


//toggleDarkMode(useDark.matches);
//toggleDarkMode(localStorage.getItem("dark-mode") == "true");


//useDark.addListener((evt) => toggleDarkMode(evt.matches));


//button.addEventListener("click", () => {
//  darkModeState = !darkModeState;

//  toggleDarkMode(darkModeState);
//  setDarkModeLocalStorage(darkModeState);
//});



















//$(".toggle-password").click(function() {
//  $(this).toggleClass("bi bi-eye-fill bi bi-eye-slash-fill");
//  input = $(this).parent().find("input");
//  if (input.attr("type") == "password") {
//      input.attr("type", "text");
//  } else {
//      input.attr("type", "password");
//  }
//});

























//const allHoverImages = document.querySelectorAll('.hover-container div img');
//const imgContainer = document.querySelector('.img-container');

//window.addEventListener('DOMContentLoaded', () => {
//    allHoverImages[0].parentElement.classList.add('active');
//});

//allHoverImages.forEach((image) => {
//    image.addEventListener('mouseover', () =>{
//        imgContainer.querySelector('img').src = image.src;
//        resetActiveImg();
//        image.parentElement.classList.add('active');
//    });
//});

//function resetActiveImg(){
//    allHoverImages.forEach((img) => {
//        img.parentElement.classList.remove('active');
//    });
//}








var btn = $('#btn-back-to-top');

$(window).scroll(function () {
    if ($(window).scrollTop() > 70) {
        btn.addClass('show');
    } else {
        btn.removeClass('show');
    }
});

btn.on('click', function (e) {
    e.preventDefault();
    $('html, body').animate({ scrollTop: 0 }, '300');
});























let messages = [];
let botMessages = [];
const chatToggleBtn = document.getElementById('chat-toggle-btn');
const chatbotContainer = document.querySelector('.chatbot-container');
const closeBtn = document.getElementById('close-btn');
const sendBtn = document.getElementById('send-btn');
const userInput = document.getElementById('user-input');
const chatBody = document.getElementById('chat-body');




// Toggle chat window open/close
let isChatOpen = false;

chatToggleBtn.addEventListener('click', () => {
    if (isChatOpen) {
        closeChat();
    } else {
        openChat();
    }
});

// Close Button event listener
closeBtn.addEventListener('click', () => {
    closeChat();
});

function openChat() {
    chatbotContainer.classList.add('open');
    chatbotContainer.classList.remove('closed');

    chatToggleBtn.querySelector('i').classList.replace('fa-comment-alt', 'fa-times'); // Change icon to cross
    isChatOpen = true;
}

function closeChat() {
    chatbotContainer.classList.add('closed');
    chatbotContainer.classList.remove('open');

    chatToggleBtn.querySelector('i').classList.replace('fa-times', 'fa-comment-alt'); // Change icon back to message
    isChatOpen = false;
}



// Toggle chat window
chatToggleBtn.addEventListener('click', () => {
    chatbotContainer.style.display = 'flex';
    chatToggleBtn.style.display = 'flex';
});

// Close chat window
closeBtn.addEventListener('click', () => {
    chatbotContainer.style.display = 'none';
    chatToggleBtn.style.display = 'flex';
});

// Function to format the current time
const getCurrentTime = () => {
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
};

// Save messages to local storage
const saveMessagesToLocalStorage = () => {
    localStorage.setItem('chatMessages', JSON.stringify({
        userMessages: messages,
        botMessages: botMessages
    }));
};

// Function to add user message to chat
const addUserMessage = (messageText) => {
    const messageIndex = messages.length;
    const userMsg = document.createElement('div');
    userMsg.classList.add('message', 'user');
    userMsg.setAttribute('data-index', messageIndex);

    userMsg.innerHTML = `
        <div style="margin-top: 10px; margin-right: -20px; color: #464646; font-size: 13px;">You</div> 
        <div class="text">${messageText}</div>
        <div class="message-time">${getCurrentTime()}</div>
        <div class="message-icons">
            <i class="edit-icon" onclick="editMessage(${messageIndex})"></i>
            <i class="delete-icon" onclick="deleteMessage(${messageIndex})"></i>
        </div>
    `;
    chatBody.appendChild(userMsg);
    chatBody.scrollTop = chatBody.scrollHeight;

    messages.push(messageText);
};

// Function to add bot response to chat with custom messages
const addBotResponse = (userMessageText, userMessageIndex) => {
    showTypingIndicator(); // Show typing indicator

    setTimeout(() => {
        let botMessageText;

        // Check for specific user inputs and respond accordingly
        switch (userMessageText.toLowerCase()) {
            case 'hello':
                botMessageText = 'Hello, how can I assist you?';
                break;
            case 'hi':
                botMessageText = 'Hi, how are you?';
                break;
            case 'how are you?':
                botMessageText = 'I am just a bot, but thanks for asking!';
                break;
            case 'what is your name?':
                botMessageText = 'I am your friendly chatbot, here to assist you!';
                break;
            default:
                botMessageText = "I'm sorry, I didn't understand that.";
                break;
        }

        hideTypingIndicator(); // Hide typing indicator

        const botMessage = document.createElement('div');
        botMessage.classList.add('message', 'bot');
        botMessage.setAttribute('data-index', userMessageIndex);

        botMessage.innerHTML = `
          <div style="align-items: flex-start;">
            <img src="web/assets/img/logo.png" alt="Bot Avatar" class="bot-avatar">
            <div>
              <div class="text">${botMessageText}</div>
              <div class="message-time">${getCurrentTime()}</div>
            </div>
          </div>
        `;

        chatBody.appendChild(botMessage);
        chatBody.scrollTop = chatBody.scrollHeight;

        // Store the bot's response in the botMessages array
        botMessages[userMessageIndex] = botMessageText;

        // Enable the send button and input field again
        enableUserInput();
    }, 1500); // Adjust delay to match typing effect duration
};

// Function to handle sending or updating a message
const sendMessage = () => {
    const message = userInput.value.trim();
    if (message === '') return;

    disableUserInput(); // Disable input when the message is sent

    if (isEditing) {
        // Handle updating an existing message
        updateMessage(message);
    } else {
        // Add the user message
        addUserMessage(message);
        userInput.value = '';

        // Add bot response after 1 second delay
        setTimeout(() => {
            addBotResponse(message, messages.length - 1); // Pass user message text and index
        }, 1000);
    }
};

// Disable user input
const disableUserInput = () => {
    sendBtn.disabled = true;
    userInput.disabled = true;
};

// Enable user input
const enableUserInput = () => {
    sendBtn.disabled = false;
    userInput.disabled = false;
};

// Send Button Click Event
sendBtn.addEventListener('click', sendMessage);

// Enter Key Press Event Listener
userInput.addEventListener('keypress', (event) => {
    if (event.key === 'Enter') {
        event.preventDefault(); // Prevent the default action (new line)
        sendMessage(); // Call the sendMessage function
    }
});

// Function to handle updating an existing message
const updateMessage = (updatedMessage) => {
    const updateBtn = document.getElementById('update-btn');
    if (!updateBtn) return;

    // Find and update the user message
    const index = parseInt(updateBtn.getAttribute('data-index'), 10);
    const userMsg = document.querySelector(`.message.user[data-index="${index}"] .text`);
    if (userMsg) {
        userMsg.textContent = updatedMessage;
        messages[index] = updatedMessage;

        // Remove the old bot response
        const botMsg = document.querySelector(`.message.bot[data-index="${index}"]`);
        if (botMsg) {
            botMsg.remove();
        }

        // Generate a new bot response based on the updated message
        setTimeout(() => {
            addBotResponse(updatedMessage, index);
        }, 1000);

        // Restore send button and remove update button
        updateBtn.remove();
        sendBtn.style.display = 'inline-block';
        userInput.value = '';
        isEditing = false; // Reset editing mode
    }
};

// Edit Message Function
const editMessage = (index) => {
    const userMsg = document.querySelector(`.message.user[data-index="${index}"] .text`);
    const originalMessage = userMsg.textContent;
    userInput.value = originalMessage; // Set input value to original message

    // If there's an existing update button, remove it first
    const existingUpdateBtn = document.getElementById('update-btn');
    if (existingUpdateBtn) {
        existingUpdateBtn.remove();
    }

    // Replace send button with update button
    sendBtn.style.display = 'none';

    // Create the update button
    const updateBtn = document.createElement('button');
    updateBtn.classList.add('send-btn');
    updateBtn.id = 'update-btn';
    updateBtn.setAttribute('data-index', index); // Store the index of the message being edited
    updateBtn.textContent = 'Update';
    document.querySelector('.chatbot-input-container').appendChild(updateBtn);

    // Focus on the input field and scroll to the bottom of the chat
    userInput.focus();
    chatBody.scrollTop = chatBody.scrollHeight;

    // Update message functionality
    updateBtn.addEventListener('click', () => {
        const updatedMessage = userInput.value.trim();
        if (updatedMessage === '') return;

        updateMessage(updatedMessage);
    });

    // Set editing mode
    isEditing = true;
};

// Delete Message Function
const deleteMessage = (index) => {
    // Delete user message
    const userMsg = document.querySelector(`.message.user[data-index="${index}"]`);
    if (userMsg) userMsg.remove();

    // Delete bot response
    const botMsg = document.querySelector(`.message.bot[data-index="${index}"]`);
    if (botMsg) botMsg.remove();

    messages[index] = null;
    botMessages[index] = null;
};

// Typing Indicator Functions
const showTypingIndicator = () => {
    const typingIndicator = document.createElement('div');
    typingIndicator.classList.add('typing-indicator');
    typingIndicator.innerHTML = '<div class="dot"></div><div class="dot"></div><div class="dot"></div>';
    chatBody.appendChild(typingIndicator);
    chatBody.scrollTop = chatBody.scrollHeight;
};

const hideTypingIndicator = () => {
    const typingIndicator = document.querySelector('.typing-indicator');
    if (typingIndicator) typingIndicator.remove();
};

let isEditing = false;
