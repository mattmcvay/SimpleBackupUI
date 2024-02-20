const deletButton = document.getElementById('DeleteBTN');

deletButton.addEventListener('click', DeleteBackupLocation())
{
    var id = deletButton.dataset.id;
};

function DeleteBackupLocation(id) {

    
    var myData = { Id: id };
    $.ajax({
        type: 'POST',
        data: myData,
        url: '/Home/DeleteBackupLocation',
    });
};
