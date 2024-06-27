var dataTable;

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Category/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryName", "width": "30%" },
            { "data": "superCategoryName", "width": "30%" },
            {
                "data": "categoryID",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="text-center">
                            <a href="/Category/Upsert?id=${data}" class="btn btn-primary mx-2">Edit</a>
                            <a onClick=Delete("/Category/Delete/${data}") class="btn btn-danger mx-2">Delete</a>
                        </div>
                    `;
                },
                "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "No data found"
        },
        "width": "100%"
    });
}
$(document).ready(function () {
    loadDataTable();
});




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
