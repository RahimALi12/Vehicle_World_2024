//document.addEventListener("DOMContentLoaded", function () {
//    const toggleButton = document.getElementById("toggleDarkMode");
//    const carIcon = document.getElementById("carIcon");

//    // Check if dark mode is already enabled in local storage
//    if (localStorage.getItem("darkMode") === "enabled") {
//        document.body.classList.add("dark-mode");
//        carIcon.src = "web/assets/img/lightmode.png"; // Dark mode icon
//    }

//    toggleButton.addEventListener("click", function () {
//        if (document.body.classList.contains("dark-mode")) {
//            document.body.classList.remove("dark-mode");
//            localStorage.setItem("darkMode", "disabled");
//            carIcon.src = "web/assets/img/darkmode.png"; // Light mode icon
//        } else {
//            document.body.classList.add("dark-mode");
//            localStorage.setItem("darkMode", "enabled");
//            carIcon.src = "web/assets/img/lightmode.png"; // Dark mode icon
//        }
//    });
//});







document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("toggleDarkMode");
    const carIcon = document.getElementById("carIcon");

    // Path to the car images (adjust paths if needed)
    const lightModeCarImage = "/web/assets/img/darkmodeblack.png"; // Image for light mode
    const darkModeCarImage = "/web/assets/img/darkmodewhite.png"; // Image for dark mode

    // Check if dark mode is already enabled in local storage
    if (localStorage.getItem("darkMode") === "enabled") {
        document.body.classList.add("dark-mode");
        carIcon.src = darkModeCarImage; // Set the dark mode car image
    } else {
        carIcon.src = lightModeCarImage; // Set the light mode car image
    }

    toggleButton.addEventListener("click", function () {
        // Add smooth transition effect when toggling between modes
        if (document.body.classList.contains("dark-mode")) {
            document.body.classList.remove("dark-mode");
            localStorage.setItem("darkMode", "disabled");
            carIcon.src = lightModeCarImage; // Switch to light mode car image
        } else {
            document.body.classList.add("dark-mode");
            localStorage.setItem("darkMode", "enabled");
            carIcon.src = darkModeCarImage; // Switch to dark mode car image
        }
    });
});

