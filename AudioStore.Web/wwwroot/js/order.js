var dataTable

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Order/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "customerName", "width": "20%" },
            { "data": "orderDate", "defaultContent": "", "width": "20%" },
            { "data": "orderStatus", "defaultContent": "", "width": "20%" },
            { "data": "orderTotal", "width": "20%" },
            {
                "data": "orderID",
                "render": function (data, type, row, meta) {
                    return `
                            <div class="text-center">
                                <a href="/Order/Edit?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                <a onClick=Delete("/Order/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                            </div>
                        `;
                }, "width": "20%"
            }
        ]
    })
}


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}