window.blazr_setEditorExitCheck = function (show) {
    if (show) {
        window.addEventListener("beforeunload", blazr_showExitDialog);
    }
    else {
        window.removeEventListener("beforeunload", blazr_showExitDialog);
    }
}

window.blazr_showExitDialog = function (event) {
    event.preventDefault();
    event.returnValue = "There are unsaved changes on this page.  Do you want to leave?";
}
