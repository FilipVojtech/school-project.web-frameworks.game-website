// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/**
 * Toggle showing contents of a password field
 * @param buttonEl The button element being clicked
 * @param fieldSelector The password field being toggled
 */
function togglePassword(buttonEl, fieldSelector) {
    const passwordField = $(fieldSelector)[0];
    if (passwordField.type === "password") {
        buttonEl.innerHTML = "<i class=\"bi bi-eye-slash\"></i>";
        passwordField.type = ("text");
    } else {
        buttonEl.innerHTML = "<i class=\"bi bi-eye\"></i>";
        passwordField.type = ("password");
    }
}
