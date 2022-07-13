document.getElementById("menu-btn").onclick = function() {
    var x = document.getElementById("sidebar");
    if (x.style.display === 'none')
        x.style.display = 'flex';
    else
        x.style.display = 'none';
}

let expand_more = document.querySelectorAll("#sidebar span.expand");
expand_more.forEach(function(item) {
    item.onclick = function() {
        let subList = item.parentElement.parentElement.querySelector(".sub-list");
        if (subList) {
            if (subList.style.display === "flex" || subList.style.display === '') {
                subList.style.display = "none";
                item.innerText = "chevron_left";
            } else {
                subList.style.display = "flex";
                item.innerText = "expand_more";
            }
        }
    }
});
document.getElementById("minimizer").onclick = function() {
    var x = document.querySelectorAll("#sidebar .nav-link .nav-item a");
    if (x) {
        let sidebar = document.getElementById("sidebar");

        x.forEach(function(item) {
            if (item.style.display === "none") {
                item.style.display = "flex";
            } else {
                item.style.display = "none";
            }
        });
        if (sidebar.style.width === "unset")
            sidebar.style.width = "300px";
        else
            sidebar.style.width = "unset";
        let chevIcon = this.querySelector("span");
        chevIcon.style.display = "block";
        if (chevIcon.innerText.includes("left"))
            chevIcon.innerText = "chevron_right";
        else
            chevIcon.innerText = "chevron_left";
    }

}
document.querySelector(".nav-item.account a").addEventListener("click", e => {
    e.preventDefault;
    document.querySelector('.nav-item.account .dropdown').classList.toggle("d-none");
});