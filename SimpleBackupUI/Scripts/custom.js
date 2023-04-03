

const saveButton = document.getElementById('saveBTN');



saveButton.addEventListener('click', showClickedMessage);


function showClickedMessage(e) {  // Always pass in E
    //e.preventDefault();

    var source = document.getElementById('source').value;
    var destination = document.getElementById('destination').value;

    var myData = { Source: source, Destination: destination }; 
    $.ajax({
        type: 'POST',
        data: myData, 
        url: '/Home/UpdateSourceAndDestination',
    });

   
}