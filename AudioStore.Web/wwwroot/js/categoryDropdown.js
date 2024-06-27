$(document).ready(function () {
    loadSuperCategoryDropbox();
});

function loadSuperCategoryDropbox() {
    $.ajax({
        url: '/Product/GetSuperCategories',
        type: "GET",
        success: function (data) {
            $('#SuperCategoryDropdown').empty().append('<option disabled selected>Select Category</option>');
            $('#SuperCategoryDropdown').append($('<option>', {
                value: "",
                text: 'None'
            }));
            $.each(data.data, function (index, item) {
                $('#SuperCategoryDropdown').append($('<option>', {
                    value: item.categoryID,
                    text: item.name
                }));
            });
        }
    })
}