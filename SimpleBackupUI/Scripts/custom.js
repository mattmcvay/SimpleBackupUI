const saveButton = document.getElementById('saveBTN');

saveButton.addEventListener('click', function () {
    var source = document.getElementById('source').value;
    var destination = document.getElementById('destination').value;

        var myData = { Source: source, Destination: destination };
        $.ajax({
            type: 'POST',
            data: myData,
            url: '/Home/UpdateSourceAndDestination',
        });   
});




function myFunction() {
        // Get the checkbox
        var checkBox = document.getElementById("myCheck");
        // Get the output text
        var checkBoxText = document.getElementById("checkBoxText");
        // If the checkbox is checked, display the output text
        if (checkBox.checked == true) {
    checkBoxText.innerText = "Service status: On";
            var checkBoxValue = 'On';
        }
        else if (checkBox.checked == false) {
    checkBoxText.innerText = "Service status: Off";
            checkBoxValue = "Off"
        }
        console.log(checkBoxValue);

        var currentStatus = {ServiceStatus: checkBoxValue }

        $.ajax({
    type: 'POST',
            data: currentStatus,
            url: '/Home/UpdateServceStatus',
        });
    }
  




