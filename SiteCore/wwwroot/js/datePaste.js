/*
 * <input type="date" datePaste="true" name="DR" required autocomplete="off"/>
 */
function parceDate(val) {
    let parts;
    if (val.includes('-')) {
        parts = val.split('-');
    }
    if (val.includes('.')) {
        parts = val.split('.');
    }
    if (parts) {
        if (parts.length === 3) {
            if (parts[0].length === 4) {
                return `${parts[0]}-${parts[1]}-${parts[2]}`;
            }
            else
                return `${parts[2]}-${parts[1]}-${parts[0]}`;
        }
    }
    return "";
}

document.querySelectorAll('[datePaste="true"]').forEach(el => {
    // register double click event to change date input to text input and select the value
    el.addEventListener('dblclick', () => {
        el.type = "text";

        // After changing input type with JS .select() wont work as usual
        // Needs timeout fn() to make it work
        setTimeout(() => {
            el.select();
        });
    });

    // register the focusout event to reset the input back to a date input field
    el.addEventListener('focusout', () => {
        el.value = parceDate(el.value);
        el.type = "date";
    });
});