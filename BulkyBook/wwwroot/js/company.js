var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return '<a href="/Admin/Company/Upsert?id='+data+'"  class="mx-2 btn btn-primary" ><i class="bi bi-pencil-square"></i>Edit </a>'
                          //'<p>test</p>'
                           //'<div class="w-75 btn-group" role="group" ><a href="/Admin/Product/Upsert?id='+data+'"  class="mx-2 btn btn-primary" ><i class="bi bi-pencil-square"></i>Edit </a><a  class="mx-2 btn btn-danger"><i class="bi bi-trash-fill"></i>Delete</a></div>'
                },
                "width": "7.5%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return '<button onclick=Delete("/Admin/Company/Delete/'+data+'") class="mx-2 btn btn-danger"><i class="bi bi-trash-fill"></i>Delete</button>'
                },
                "width": "7.5%"
            }
        
         
        ]
    });
}

function Delete(url) {
    alert("delete");
    
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }

            })
        }
    })
}