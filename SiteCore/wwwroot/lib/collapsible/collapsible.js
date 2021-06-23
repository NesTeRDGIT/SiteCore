
var coll = document.getElementsByClassName("collapsible-header");
var i;
for (i = 0; i < coll.length; i++) {
    coll[i].onclick = collapsibleheaderClick;
   // coll[i].addEventListener("click", collapsibleheaderClick);
}

function collapsibleheaderClick () {
    this.classList.toggle("active");
};
