$(document).ready(function () {
    loadManufacturerDropbox();
    loadSuperCategoryDropbox();
});

$(document).ready(function () {
    $('#SuperCategoryDropdown').change(function () {
        var superCategoryId = $(this).val();
        $.ajax({
            url: '/Product/GetSubCategories/' + superCategoryId,
            type: "GET",
            data: { superCategoryId: superCategoryId },
            success: function (data) {
                $('#SubCategoryDropdown').empty().append('<option disabled selected>Select Sub Category</option>');
                $.each(data.data, function (index, item) {
                    $('#SubCategoryDropdown').append($('<option>', {
                        value: item.categoryID,
                        text: item.name
                    }));
                });
            }
        });
    });
});


function loadManufacturerDropbox() {
    $.ajax({
        url: '/Product/GetManufacturers',
        type: "GET",
        success: function (data) {
            $('#ManufacturerDropdown').empty().append('<option disabled selected>Select Manufacturer</option>');
            $.each(data.data, function (index, item) {
                $('#ManufacturerDropdown').append($('<option>', {
                    value: item.manufacturerID,
                    text: item.name
                }));
            });

        }
    })  
}


