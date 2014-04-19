$(document).ready(function () {
    $("#add").hide();
    //$("#customersTable").hide();
    
    $("#addCustomer").click(function() {
        $("#add").slideToggle();
    });
    
    $("#getCustomers").click(function () {
        $("#customersTable").slideToggle();
    });
    
    //$("#getCustomers").click(function () {
    //    $.ajax({
    //        url: 'http://localhost:17016/api/customer/GetCustomers',
    //        type: 'get',
    //        contentType: "application/json; charset=utf-8",
    //        async: true,
    //        success: function (data) {
    //            alert(data.success);
    //        },
    //        error: function (x, y, z) {
    //            alert(x + '\n' + y + '\n' + z);
    //        }
    //    });
    //});
});