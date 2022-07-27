var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("pending");
        }
        else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
                
            }
        }
    }
    
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },

            {
                "data": "id",
                "render": function (data) {
                    return '<a href="/Admin/Order/Details?orderId='+data+'"  class="mx-2 btn btn-primary" ><i class="bi bi-pencil-square"></i>Edit </a>'
                          //'<p>test</p>'
                           //'<div class="w-75 btn-group" role="group" ><a href="/Admin/Product/Upsert?id='+data+'"  class="mx-2 btn btn-primary" ><i class="bi bi-pencil-square"></i>Edit </a><a  class="mx-2 btn btn-danger"><i class="bi bi-trash-fill"></i>Delete</a></div>'
                },
                "width": "5%"
            },
          
         
        ]
    });
}
