
const wTranMax = document.querySelector("#d"),
    rTranList = document.querySelector(".transaction-list");

rTranList.style.maxWidth = `${wTranMax.getBoundingClientRect().width}px`;

const showerList = document.querySelectorAll(".show-more");

document.addEventListener("DOMContentLoaded", e => {

    showerList.forEach(shower => {

        const parentTop = shower.parentElement.nextElementSibling.offsetTop
        const childTop = shower.parentElement.nextElementSibling.lastElementChild.offsetTop

        if (parentTop != childTop) {
            shower.classList.remove("no-show")
        }

    })
})
document.addEventListener("change", e => {
    showerList.forEach(shower => {

        const parentTop = shower.parentElement.nextElementSibling.offsetTop
        const childTop = shower.parentElement.nextElementSibling.lastElementChild.offsetTop

        if (parentTop != childTop) {
            shower.classList.remove("no-show")
        }

    })
})
function changer(a) {
    var x = document.getElementById(a);
    x.click();


}

function AddChecked() {
    var status = document.getElementById("labelstatus");
    if (status == "Activate") {
        document.getElementById("statuscheck").cheked = true;
    }
}
function show(btn, e, px) {
    btn.text = btn.text == "Show more"? "Show less": "Show more"
    if (e.style.height == px) {
        e.style.height = "fit-content"
    } else {
        e.style.height =px
    }
}
