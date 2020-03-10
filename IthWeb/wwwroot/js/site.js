// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// This function runs automatically on startup.
(function () {
    // Use the jQuery function .get to make an AJAX get call to the specified url
    $.get("https://localhost:44330/api/blogposts", function (data, status) {
        console.log("data:", data);
        console.log("status: ", status);
    });
})()