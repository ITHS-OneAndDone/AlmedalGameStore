var dataTable;
/*function that calls load data table*/
$(document).ready(function () {
    loadDataTable();
});
/* loads dataTable ajax */
function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": {
            "url": "/Admin/Customer/GetAll"
        },
        "columns": [
            { "data": "id", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                              <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Customer/Details?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Details</a>
                    </div>
                           `
                },
                "width": "15%",

            }
        ]
    });
}