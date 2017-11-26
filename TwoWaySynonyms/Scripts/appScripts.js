function grab(what){
    return document.querySelector(what)
}
let mainData = grab('#mainData')
let sysList = grab('#synonymList')
let ajaxForm = grab('#inputForm').querySelector('form')
let button = grab('#getList')

button.addEventListener('click', funcSubmit)
function funcSubmit(e) {
    button.disabled = true 
    if (ajaxForm.querySelector('#Term').value === "" || ajaxForm.querySelector('#Synonyms').value === "") {
        e.preventDefault()
        grab('#synonymList').innerHTML = "<h3 class='alert alert-danger'>Oba pola muszą być wypełnione</h3>"
        button.disabled = false
    }
    else if (parseFloat(ajaxForm.querySelector('#Term').value.length) > 50) {
        e.preventDefault()
        grab('#synonymList').innerHTML = "<h3 class='alert alert-danger'>Wyrażenie może mieć max 50 znaków</h3>"
        button.disabled = false
    }
    else {
        grab('#hidenSubmit').click();
    }    
}
function ajaxFailure() {
    mainData.classList.remove("panel-success")
    mainData.classList.remove("panel-primary")
    mainData.classList.add("panel-danger")
    button.disabled = false
}
function ajaxBegin() {   
    mainData.classList.remove("panel-success")
    mainData.classList.remove("panel-danger")
    if (!sysList.classList.contains("hide")) {
        sysList.classList.add("hide")
    }
    mainData.classList.add("panel-primary")
}
function ajaxSuccess() {    
    setTimeout(() => {
        mainData.classList.remove("panel-danger")
        mainData.classList.remove("panel-primary")
        mainData.classList.add("panel-success")
        if (sysList.classList.contains("hide")) {
            sysList.classList.remove("hide")
        }
    }, 200)
    grab('#Synonyms').value = ""
    grab('#Term').value = ""
    button.disabled = false
}