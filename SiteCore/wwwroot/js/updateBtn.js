var isButton = true;
function HiddenBtnClick() {
    isButton = false;
    document.getElementById("UpdateBtn").click();
    isButton = true;
};


function ShowLoader() {
    if (isButton === false) return;
    const Result = document.getElementById('Result');
    const LoadingBar = document.getElementById('LoadingBar');
    Result.style.display = 'none';
    LoadingBar.style.display = 'block';
}
function HideLoader() {
    if (isButton === false) return;
    const Result = document.getElementById('Result');
    const LoadingBar = document.getElementById('LoadingBar');
    Result.style.display = 'block';
    LoadingBar.style.display = 'none';
}
function Err(xhr, textStatus, errorThrown) {
    alert('Ошибка запроса');
    HideLoader();
}