// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// This function runs automatically on startup.
(function () {
    // Use the jQuery function .get to make an AJAX get call to the specified url
    // Read more about jQuery.get here: https://api.jquery.com/jquery.get/
    $.get("https://localhost:44330/api/blogposts", function (data, status) {
        console.log("jQuery status: ", status);
        console.log("jQuery data:", data);
    });

    // You can also use the native JavaScript function 'fetch'
    // Read more about fetch here: https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
    fetch("https://localhost:44330/api/blogposts")
        .then((response) => {
            console.log("fetch status: ", response.status);
            return response.json();
        }).then((data) => {
            console.log("fetch data: ", data);
        });
})();