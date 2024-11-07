



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

let isBotReplying = false; // Track if bot is replying
let isEditing = false; // Track if user is editing

// Toggle chat window open/close
let isChatOpen = false;

chatToggleBtn.addEventListener('click', () => {
    if (isChatOpen) {
        closeChat();
    } else {
        openChat();
    }
});

closeBtn.addEventListener('click', () => {
    closeChat();
});

function openChat() {
    chatbotContainer.classList.add('open');
    chatbotContainer.classList.remove('closed');
    chatToggleBtn.querySelector('i').classList.replace('fa-comment-alt', 'fa-times'); // Change icon to cross
    chatbotContainer.style.display = 'flex';  // Always display the chatbox container as flex
    isChatOpen = true;
}

function closeChat() {
    chatbotContainer.classList.add('closed');
    chatbotContainer.classList.remove('open');
    chatToggleBtn.querySelector('i').classList.replace('fa-times', 'fa-comment-alt'); // Change icon back to message
    chatbotContainer.style.display = 'flex';  // Always display the chatbox container as flex
    isChatOpen = false;
}

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
            <i class="delete-icon disabled" onclick="deleteMessage(${messageIndex})"></i>
        </div>
    `;
    chatBody.appendChild(userMsg);
    chatBody.scrollTop = chatBody.scrollHeight;

    messages.push(messageText);
};

// Function to add bot response to chat
const addBotResponse = (userMessageText, userMessageIndex) => {
    isBotReplying = true; // Bot is replying, disable user actions
    disableUserInput(); // Disable input while bot is replying

    showTypingIndicator();

    setTimeout(() => {
        let botMessageText;

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

            case 'how can I sell my car?':
                botMessageText = 'To sell your car, simply create an account on our website and fill in the necessary details about your vehicle, including its make, model, year, and price.';
                break;
            case 'how do I contact the seller?':
                botMessageText = 'You can contact the seller by clicking on the "Message Seller" button on the car listing page, which will send an email directly to the seller.';
                break;
            case 'is there a fee to list my car?':
                botMessageText = 'No, there are no fees to list your car on our platform. You can sell your car for free.';
                break;
            case 'how do I know my car is listed?':
                botMessageText = 'Once you submit your car details, you will receive a confirmation email. Your listing will then be visible to buyers on our website.';
                break;
            case 'what details do I need to provide to sell my car?':
                botMessageText = 'You will need to provide details such as the car make, model, year, mileage, condition, price, and any additional features. High-quality photos are also recommended.';
                break;
            case 'can I edit my car listing after it’s posted?':
                botMessageText = 'Yes, you can edit your car listing at any time by logging into your account and updating the relevant information.';
                break;
            case 'how do I know if a buyer is interested?':
                botMessageText = 'You will receive email notifications whenever a buyer sends a message expressing interest in your vehicle.';
                break;
            case 'can buyers negotiate the price?':
                botMessageText = 'Yes, buyers can express their interest in negotiating the price when they contact you through the messaging system.';
                break;
            case 'how can I promote my car listing?':
                botMessageText = 'Share your car listing on social media or with friends and family to increase visibility and attract more potential buyers.';
                break;
            case 'what should I do if I receive a suspicious message?':
                botMessageText = 'If you receive a suspicious message, please report it to our support team for investigation.';
                break;
            case 'what if I want to withdraw my car listing?':
                botMessageText = 'You can easily withdraw your car listing by logging into your account and selecting the option to remove your listing.';
                break;
            case 'how can I improve my car listing?':
                botMessageText = 'To improve your car listing, provide detailed descriptions, use high-quality images, and price your car competitively.';
                break;
            case 'how do I set a price for my car?':
                botMessageText = 'Research similar cars in your area to set a competitive price based on their condition, mileage, and market demand.';
                break;
            case 'is there a warranty on the car?':
                botMessageText = 'Our platform does not provide warranties. Any warranty or guarantee should be discussed directly between the buyer and seller.';
                break;
            case 'what if I have more questions?':
                botMessageText = 'Feel free to reach out to our support team or check our FAQ section for more information.';
                break;
            case 'can I sell multiple cars?':
                botMessageText = 'Yes, you can list multiple cars by creating separate listings for each vehicle.';
                break;
            case 'how long does it take to sell my car?':
                botMessageText = 'The time it takes to sell your car can vary based on demand and pricing, but being responsive to buyers can expedite the process.';
                break;
            case 'what if the buyer wants to test drive?':
                botMessageText = 'It is recommended to meet in a public place and supervise the test drive for safety. Discuss terms directly with the buyer.';
                break;
            case 'can I remove my contact information from the listing?':
                botMessageText = 'Your contact information will not be displayed publicly. Buyers can contact you through the messaging system.';
                break;
            case 'do you offer financing options?':
                botMessageText = 'We do not offer financing options directly; however, buyers can seek financing through their preferred lenders.';
                break;
            case 'is my information safe?':
                botMessageText = 'Yes, we take your privacy seriously and ensure that your personal information is protected according to our privacy policy.';
                break;
            case 'can I see buyer reviews?':
                botMessageText = 'Our platform does not have a review system for buyers, but you can gauge interest through their messages.';
                break;
            case 'what happens after I sell my car?':
                botMessageText = 'Once your car is sold, make sure to transfer the title and complete any necessary paperwork as per your local regulations.';
                break;
            case 'can I block a buyer?':
                botMessageText = 'If you encounter a problematic buyer, please report them to our support team for assistance.';
                break;
            case 'how can I get help with my listing?':
                botMessageText = 'You can contact our support team for assistance with any issues related to your listing.';
                break;
            case 'can I promote my listing on social media?':
                botMessageText = 'Yes, sharing your listing on social media platforms can help reach more potential buyers.';
                break;
            case 'is there a limit to how many photos I can upload?':
                botMessageText = 'You can upload up to 10 high-quality photos per listing to showcase your vehicle effectively.';
                break;
            case 'how can I improve my chances of selling?':
                botMessageText = 'Providing clear descriptions, attractive images, and being responsive to buyer inquiries can improve your chances of selling.';
                break;
            case 'what should I do if I’m not getting responses?':
                botMessageText = 'Consider adjusting your pricing or improving your listing details to attract more interest.';
                break;
            case 'are there any hidden fees?':
                botMessageText = 'There are no hidden fees associated with listing your car on our platform.';
                break;
            case 'can I ask for a deposit from the buyer?':
                botMessageText = 'Discuss any payment terms directly with the buyer, including the possibility of a deposit.';
                break;
            case 'how do I handle negotiations?':
                botMessageText = 'Be open to discussions and maintain a fair and respectful approach during negotiations with buyers.';
                break;
            case 'can I list a car that has a loan?':
                botMessageText = 'You can list a car with a loan, but ensure you discuss the payoff process with potential buyers.';
                break;
            case 'what if I need to sell my car quickly?':
                botMessageText = 'Consider pricing your car competitively and being responsive to inquiries to expedite the sale.';
                break;
            case 'how do I edit my account information?':
                botMessageText = 'Log into your account and navigate to your profile settings to edit your personal information.';
                break;
            case 'can I see my car listing history?':
                botMessageText = 'Yes, you can view your car listing history by accessing your account dashboard.';
                break;
            case 'what if I have a mechanical issue with my car?':
                botMessageText = 'It’s important to disclose any mechanical issues in your listing to maintain transparency with potential buyers.';
                break;
            case 'can I recommend a buyer to a friend?':
                botMessageText = 'Yes, you can share your buyer’s contact details with friends if you trust them and feel comfortable doing so.';
                break;
            case 'how do I update my payment information?':
                botMessageText = 'You can update your payment information in your account settings under the billing section.';
                break;
            case 'is there a way to track messages from buyers?':
                botMessageText = 'Yes, all messages from buyers will be visible in your account dashboard for easy tracking.';
                break;
            case 'how do I provide feedback on the platform?':
                botMessageText = 'We welcome feedback! Please contact our support team to share your thoughts and suggestions.';
                break;
            case 'are there any seasonal sales?':
                botMessageText = 'We do not offer seasonal sales, but keeping your listing updated can help attract buyers throughout the year.';
                break;
            case 'how do I know if my car is priced competitively?':
                botMessageText = 'Research similar cars in your area to determine if your pricing is aligned with the market.';
                break;
            case 'can I list a car that has been in an accident?':
                botMessageText = 'Yes, you can list a car that has been in an accident, but be sure to disclose that information in your listing.';
                break;
            case 'how do I handle lowball offers?':
                botMessageText = 'Politely decline lowball offers and consider countering with your desired price.';
                break;
            case 'what if a buyer wants to take my car to a mechanic?':
                botMessageText = 'It’s acceptable for buyers to have a mechanic inspect the car before finalizing the sale for added assurance.';
                break;
            case 'can I change my listing price after posting?':
                botMessageText = 'Yes, you can adjust the listing price at any time by editing your car details.';
                break;
            case 'is it safe to meet buyers?':
                botMessageText = 'Always meet in public places and consider bringing a friend for added safety when meeting potential buyers.';
                break;
            case 'can I ask for identification from the buyer?':
                botMessageText = 'It’s a good practice to verify the buyer’s identity before completing the sale.';
                break;
            case 'how can I promote trust with buyers?':
                botMessageText = 'Provide honest descriptions, clear photos, and be transparent about the car’s condition to build trust with buyers.';
                break;
            case 'what if I receive an offer I’m not happy with?':
                botMessageText = 'You can negotiate with the buyer or politely decline the offer if it doesn’t meet your expectations.';
                break;
            case 'can I sell a car that I bought on credit?':
                botMessageText = 'You can sell a car purchased on credit, but ensure to discuss the loan payoff with potential buyers.';
                break;
            case 'how can I create a standout listing?':
                botMessageText = 'Use high-quality images, write detailed descriptions, and highlight unique features to make your listing stand out.';
                break;
            case 'what if a buyer wants to pay in cash?':
                botMessageText = 'If the buyer prefers to pay in cash, make sure to conduct the transaction safely in a secure location.';
                break;
            case 'can I recommend this website to others?':
                botMessageText = 'Absolutely! We appreciate your referrals and encourage you to share your experience with others.';
                break;
            case 'is there a warranty on my listing?':
                botMessageText = 'There is no warranty provided on your listing. All sales are conducted directly between buyers and sellers.';
                break;
            case 'how do I verify a buyer’s identity?':
                botMessageText = 'Ask for valid identification and consider conducting the sale in a public place for added security.';
                break;
            case 'can I ask for a holding fee?':
                botMessageText = 'Discuss any payment terms directly with the buyer, including the possibility of a holding fee.';
                break;
            case 'how can I make my listing more attractive?':
                botMessageText = 'Use appealing images, accurate descriptions, and competitive pricing to attract more interest in your listing.';
                break;
            case 'can I block unwanted messages?':
                botMessageText = 'If you receive unwanted messages, please report them to our support team for assistance.';
                break;
            case 'how do I keep my listing updated?':
                botMessageText = 'Regularly log into your account to make necessary updates to your listing as required.';
                break;
            case 'are there any restrictions on listing?':
                botMessageText = 'Yes, ensure your listing complies with our guidelines, which prohibit illegal vehicles or fraud.';
                break;
            case 'can I list a car with a salvage title?':
                botMessageText = 'Yes, but you must disclose the salvage title in your listing for transparency.';
                break;
            case 'how often should I check my messages?':
                botMessageText = 'Regularly check your messages to respond promptly to potential buyers and keep the communication active.';
                break;
            case 'what if I want to change my email address?':
                botMessageText = 'You can change your email address in your account settings under profile preferences.';
                break;
            case 'how can I report a problem?':
                botMessageText = 'Please contact our support team to report any issues or concerns you may have.';
                break;
            case 'can I list a car that’s not in my name?':
                botMessageText = 'You can list a car not in your name, but ensure to disclose the ownership situation to potential buyers.';
                break;
            case 'how do I cancel my account?':
                botMessageText = 'You can cancel your account by contacting our support team for assistance.';
                break;
            case 'can I create multiple accounts?':
                botMessageText = 'We do not allow multiple accounts per individual to maintain integrity on our platform.';
                break;
            case 'how do I contact support?':
                botMessageText = 'You can contact our support team via the "Contact Us" section on our website for assistance.';
                break;
            case 'is there a customer support hotline?':
                botMessageText = 'Yes, we have a customer support hotline available during business hours for immediate assistance.';
                break;
            case 'can I access the website on mobile?':
                botMessageText = 'Yes, our website is mobile-friendly and can be accessed on any device for your convenience.';
                break;
            case 'how can I reset my password?':
                botMessageText = 'You can reset your password by clicking on the "Forgot Password?" link on the login page.';
                break;
            case 'can I change my username?':
                botMessageText = 'You can contact our support team to request a change to your username if needed.';
                break;
            case 'are there any regional restrictions?':
                botMessageText = 'Please check our guidelines for any regional restrictions that may apply to your listings.';
                break;
            case 'can I add a video to my listing?':
                botMessageText = 'Currently, we do not support video uploads, but you can include multiple images to showcase your car.';
                break;
            case 'how do I report inappropriate content?':
                botMessageText = 'If you encounter inappropriate content, please report it immediately to our support team for action.';
                break;
            case 'can I share my listing link?':
                botMessageText = 'Yes, you can share your listing link on social media or through email to attract more buyers.';
                break;
            case 'how do I view my listings?':
                botMessageText = 'You can view all your active listings by logging into your account and navigating to your dashboard.';
                break;
            case 'what payment methods do you accept?':
                botMessageText = 'Payment methods are determined between the buyer and seller; discuss options directly with the buyer.';
                break;
            case 'can I see stats on my listing?':
                botMessageText = 'Currently, we do not provide listing statistics, but you can track messages and inquiries in your account.';
                break;
            case 'how long will my listing stay active?':
                botMessageText = 'Your listing will remain active until you remove it or sell the vehicle, unless otherwise specified.';
                break;
            case 'what if I need to cancel a sale?':
                botMessageText = 'If you need to cancel a sale, communicate directly with the buyer and ensure to document the cancellation.';
                break;
            case 'can I add a warranty to my car sale?':
                botMessageText = 'If you offer a warranty, discuss it directly with potential buyers to establish terms and conditions.';
                break;
            case 'how do I protect myself from scams?':
                botMessageText = 'Always verify the buyer’s identity, meet in safe locations, and avoid sharing personal information.';
                break;
            case 'can I mark my listing as sold?':
                botMessageText = 'Yes, you can mark your listing as sold by editing the status in your account dashboard.';
                break;
            case 'how do I stay updated with site changes?':
                botMessageText = 'Subscribe to our newsletter or follow us on social media for the latest updates and changes to our platform.';
                break;
            case 'what if I have a question not listed here?':
                botMessageText = 'Feel free to reach out to our support team for assistance with any questions or concerns you may have.';
                break;


            case 'what is your name?':
                botMessageText = 'My name is Rahim Ali, and I\'m here to assist you!';
                break;
            case 'who are you?':
                botMessageText = 'I am the owner of this website, dedicated to helping both sellers and buyers.';
                break;
            case 'why did you create this website?':
                botMessageText = 'I created this website to make it easier for people to buy and sell cars.';
                break;
            case 'what is your experience?':
                botMessageText = 'I have extensive experience in car sales and marketing.';
                break;
            case 'do you own a car dealership?':
                botMessageText = 'No, this website serves as a platform where buyers and sellers can connect.';
                break;
            case 'what is your vision for this website?':
                botMessageText = 'My vision is to make this platform the most reliable and user-friendly for everyone.';
                break;
            case 'what types of cars do you sell?':
                botMessageText = 'We offer all kinds of cars, both new and used.';
                break;
            case 'what services do you provide on this website?':
                botMessageText = 'Our website features car listings, user profiles, and messaging services.';
                break;
            case 'how do you provide customer support?':
                botMessageText = 'We offer customer support via email and chat to assist you.';
                break;
            case 'how do you verify buyers and sellers?':
                botMessageText = 'We use identity verification and user ratings to ensure safety.';
                break;

            case 'can you sell cars yourself?':
                botMessageText = 'No, I only operate this platform; I don\'t sell cars myself.';
                break;
            case 'what is the listing process like?':
                botMessageText = 'Users need to create an account, fill in their car details, and publish their listing.';
                break;
            case 'do you take feedback from users?':
                botMessageText = 'Yes, we value user feedback to improve our platform.';
                break;
            case 'how do you handle customer reviews?':
                botMessageText = 'We encourage user reviews and ratings to enhance our services.';
                break;
            case 'do you have a social media presence?':
                botMessageText = 'Yes, we are active on social media to engage with our users.';
                break;

            case 'what do you like most about your website?':
                botMessageText = 'I love seeing people easily buy and sell cars through our platform.';
                break;
            case 'do you provide tips for users?':
                botMessageText = 'Absolutely! We share tips on buying and selling cars effectively.';
                break;
            case 'do you offer special promotions?':
                botMessageText = 'Yes, we occasionally provide special promotions and discounts.';
                break;
            case 'what is the security level of your website?':
                botMessageText = 'We have implemented various security features to protect user data.';
                break;
            case 'what type of feedback do you value most?':
                botMessageText = 'We prioritize feedback on user experience and customer service.';
                break;

            case 'have you faced any challenges running this website?':
                botMessageText = 'Yes, we encounter challenges, but there’s always a way to resolve them.';
                break;
            case 'what are your plans for the future?':
                botMessageText = 'My plan is to continually improve the website and make it more user-friendly.';
                break;
            case 'do you provide a tutorial for using the website?':
                botMessageText = 'Yes, we offer tutorials and guides to help users navigate the site.';
                break;
            case 'do you regularly update the website?':
                botMessageText = 'Yes, we maintain and update the website regularly with new features.';
                break;
            case 'do you support any charity projects?':
                botMessageText = 'Yes, we support local charity projects from time to time.';
                break;

            case 'what payment methods do you accept?':
                botMessageText = 'Payments are made between buyers and sellers, and we facilitate the transaction.';
                break;
            case 'are you focused on a specific region?':
                botMessageText = 'Yes, we have a focus on specific regions, but users can join from anywhere.';
                break;
            case 'do you promote any car enthusiast community?':
                botMessageText = 'Yes, we promote events and discussions for car enthusiasts.';
                break;
            case 'what do you enjoy most about selling cars?':
                botMessageText = 'I enjoy helping people find their dream cars or sell their vehicles.';
                break;
            case 'how long have you been in this business?':
                botMessageText = 'I have been in this business for several years, and it has taught me a lot.';
                break;

            case 'do you have a favorite car brand?':
                botMessageText = 'Yes, I’m particularly fond of luxury cars and their performance.';
                break;
            case 'do you attend car shows?':
                botMessageText = 'Yes, I enjoy attending car shows to stay updated on the latest trends.';
                break;
            case 'are you interested in car modifications?':
                botMessageText = 'Yes, I find car modifications and customization fascinating.';
                break;
            case 'what is your favorite car model?':
                botMessageText = 'My favorite car model is the Audi Q7.';
                break;
            case 'do you own a personal car?':
                botMessageText = 'Yes, I have a personal car that I use frequently.';
                break;

            case 'what is your favorite car color?':
                botMessageText = 'I prefer black cars; they always look stylish.';
                break;
            case 'do you enjoy road trips?':
                botMessageText = 'Yes, I love road trips as they allow me to explore new places.';
                break;
            case 'how do you maintain your car?':
                botMessageText = 'I ensure regular servicing and check-ups to keep my car in good condition.';
                break;
            case 'do you like vintage cars?':
                botMessageText = 'Yes, I appreciate the design and charm of vintage cars.';
                break;
            case 'are you into any particular car technologies?':
                botMessageText = 'Yes, I find electric car technology particularly interesting.';
                break;

            case 'have you ever participated in car racing?':
                botMessageText = 'No, but I enjoy watching car racing events.';
                break;
            case 'what is your car selling strategy?':
                botMessageText = 'I focus on competitive pricing and effective marketing strategies.';
                break;
            case 'what kind of advertising do you do?':
                botMessageText = 'We utilize online advertising and social media campaigns.';
                break;
            case 'do you talk to customers face-to-face?':
                botMessageText = 'Yes, I engage with customers to provide personalized assistance.';
                break;
            case 'what features do you think are most important for buyers?':
                botMessageText = 'I believe transparency, fair pricing, and good communication are crucial.';
                break;

            case 'how do you ensure user satisfaction?':
                botMessageText = 'We continuously gather feedback and improve our services based on it.';
                break;
            case 'do you have a loyalty program for returning users?':
                botMessageText = 'Yes, we offer rewards for our loyal users periodically.';
                break;
            case 'how can I contact you for support?':
                botMessageText = 'You can reach out to us via our contact page or email.';
                break;
            case 'do you provide a warranty for sold cars?':
                botMessageText = 'We do not provide warranties, but we encourage buyers to do thorough checks.';
                break;
            case 'what should I do if I face issues on your website?':
                botMessageText = 'Please contact our support team, and we\'ll help you resolve any issues.';
                break;

            case 'can I list multiple cars for sale?':
                botMessageText = 'Yes, you can list multiple cars as long as you have an account.';
                break;
            case 'what details do I need to provide for listing?':
                botMessageText = 'You need to provide car details, images, and your contact information.';
                break;
            case 'how long does it take to list a car?':
                botMessageText = 'Listing a car is quick and can be done in just a few minutes.';
                break;
            case 'what happens after I list my car?':
                botMessageText = 'Your car will be visible to potential buyers, and you can receive messages.';
                break;
            case 'do you have tips for first-time sellers?':
                botMessageText = 'Yes, we recommend being honest about your car’s condition and pricing fairly.';
                break;

            case 'how can I improve my car listing?':
                botMessageText = 'Use high-quality images and provide detailed descriptions to attract buyers.';
                break;
            case 'what if my car doesn’t sell?':
                botMessageText = 'You can adjust the price or update your listing to make it more appealing.';
                break;
            case 'can I edit my car listing after posting?':
                botMessageText = 'Yes, you can edit your listing anytime to update details or images.';
                break;
            case 'do you provide analytics for my listing?':
                botMessageText = 'Currently, we don’t provide analytics, but we’re working on adding this feature.';
                break;
            case 'how do I know if a buyer is serious?':
                botMessageText = 'Serious buyers usually ask detailed questions and respond quickly.';
                break;

            case 'do you allow test drives?':
                botMessageText = 'Test drives are arranged between the seller and the buyer.';
                break;
            case 'what should I include in my car description?':
                botMessageText = 'Include details like the make, model, year, mileage, and any special features.';
                break;
            case 'how can I get better visibility for my listing?':
                botMessageText = 'Consider sharing your listing on social media for wider reach.';
                break;
            case 'what are common mistakes to avoid when selling?':
                botMessageText = 'Avoid overpricing, providing insufficient details, and ignoring buyer inquiries.';
                break;
            case 'can I promote my listing?':
                botMessageText = 'We will offer promotional options in the future to increase visibility.';
                break;

            case 'what types of payments do you recommend?':
                botMessageText = 'We suggest secure payment methods like bank transfers or escrow services.';
                break;
            case 'is there a fee for listing my car?':
                botMessageText = 'Currently, there is no fee for listing cars on our platform.';
                break;
            case 'what should I do if I receive a low offer?':
                botMessageText = 'You can negotiate or decline the offer if it doesn’t meet your expectations.';
                break;
            case 'do you have any partnerships with other businesses?':
                botMessageText = 'Yes, we collaborate with local car services and finance companies.';
                break;
            case 'how can I stay updated about new features?':
                botMessageText = 'Subscribe to our newsletter for updates on new features and offers.';
                break;

            case 'can I share my listing on social media?':
                botMessageText = 'Yes, you can easily share your listing on various social media platforms.';
                break;
            case 'do you have an app for this service?':
                botMessageText = 'We are currently developing a mobile app for a better user experience.';
                break;
            case 'what is the best time to list my car?':
                botMessageText = 'Spring and summer are typically good times for selling cars.';
                break;
            case 'how do I handle buyers who want to negotiate?':
                botMessageText = 'Be open to discussions and decide your lowest acceptable price beforehand.';
                break;
            case 'do you offer tips for buying cars?':
                botMessageText = 'Yes, we provide a guide for buyers to make informed decisions.';
                break;

            case 'how do I ensure a smooth transaction?':
                botMessageText = 'Communicate clearly with the buyer and agree on payment terms beforehand.';
                break;
            case 'what should I do if I suspect fraud?':
                botMessageText = 'Report the issue immediately, and we’ll take appropriate action.';
                break;
            case 'do you provide legal advice?':
                botMessageText = 'No, we recommend consulting a legal expert for any legal advice.';
                break;
            case 'can I block users on the platform?':
                botMessageText = 'Yes, you can block users if you feel uncomfortable with their behavior.';
                break;
            case 'what is your refund policy?':
                botMessageText = 'Currently, we do not have a refund policy as listings are free.';
                break;

            case 'do you have a blog for tips and news?':
                botMessageText = 'Yes, we maintain a blog with tips, news, and updates in the automotive industry.';
                break;
            case 'how do you keep my data safe?':
                botMessageText = 'We use encryption and secure servers to protect user data.';
                break;
            case 'can I report inappropriate content?':
                botMessageText = 'Yes, you can report any inappropriate listings or user behavior.';
                break;
            case 'what if I forget my password?':
                botMessageText = 'You can easily reset your password through the login page.';
                break;
            case 'do you offer a referral program?':
                botMessageText = 'Yes, we have a referral program where you can earn rewards for referring new users.';
                break;
            default:
                botMessageText = "I'm sorry, I didn't understand that. Please try asking something else!";
                break;


        }

        hideTypingIndicator();

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

        botMessages[userMessageIndex] = botMessageText;
        enableUserInput(); // Enable input after bot reply
        isBotReplying = false; // Bot has replied

        // Enable delete button after bot response
        const deleteIcon = document.querySelector(`.message.user[data-index="${userMessageIndex}"] .delete-icon`);
        deleteIcon.classList.remove('disabled');
    }, 1500);
};

// Send message function
const sendMessage = () => {
    const message = userInput.value.trim();
    if (message === '' || isBotReplying) return; // Prevent sending if bot is replying

    disableUserInput(); // Disable input on message send

    if (isEditing) {
        updateMessage(message);
    } else {
        addUserMessage(message);
        userInput.value = '';

        setTimeout(() => {
            addBotResponse(message, messages.length - 1);
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
        event.preventDefault();
        sendMessage();
    }
});

// Update message function
const updateMessage = (updatedMessage) => {
    const updateBtn = document.getElementById('update-btn');
    if (!updateBtn) return;

    const index = parseInt(updateBtn.getAttribute('data-index'), 10);
    const userMsg = document.querySelector(`.message.user[data-index="${index}"] .text`);
    if (userMsg) {
        userMsg.textContent = updatedMessage;
        messages[index] = updatedMessage;

        const botMsg = document.querySelector(`.message.bot[data-index="${index}"]`);
        if (botMsg) botMsg.remove();

        // Regenerate bot response after update
        isBotReplying = true; // Disable user actions until regeneration is complete
        disableUserInput();

        setTimeout(() => {
            addBotResponse(updatedMessage, index);
        }, 1000);

        updateBtn.remove();
        sendBtn.style.display = 'inline-block';
        userInput.value = '';
        isEditing = false;
    }
};

// Edit message function
const editMessage = (index) => {
    if (isBotReplying) return; // Prevent editing if bot is replying

    const userMsg = document.querySelector(`.message.user[data-index="${index}"] .text`);
    const originalMessage = userMsg.textContent;
    userInput.value = originalMessage;

    const existingUpdateBtn = document.getElementById('update-btn');
    if (existingUpdateBtn) {
        existingUpdateBtn.remove();
    }

    sendBtn.style.display = 'none';

    const updateBtn = document.createElement('button');
    updateBtn.classList.add('send-btn');
    updateBtn.id = 'update-btn';
    updateBtn.setAttribute('data-index', index);
    updateBtn.textContent = 'Update';
    document.querySelector('.chatbot-input-container').appendChild(updateBtn);

    userInput.focus();
    chatBody.scrollTop = chatBody.scrollHeight;

    updateBtn.addEventListener('click', () => {
        const updatedMessage = userInput.value.trim();
        if (updatedMessage === '' || isBotReplying) return;

        updateMessage(updatedMessage);
    });

    isEditing = true;
};

// Delete message function
const deleteMessage = (index) => {
    if (isBotReplying) return; // Prevent deletion if bot is replying

    const userMsg = document.querySelector(`.message.user[data-index="${index}"]`);
    if (userMsg) userMsg.remove();

    const botMsg = document.querySelector(`.message.bot[data-index="${index}"]`);
    if (botMsg) botMsg.remove();

    messages[index] = null;
    botMessages[index] = null;
};

// Typing Indicator
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

