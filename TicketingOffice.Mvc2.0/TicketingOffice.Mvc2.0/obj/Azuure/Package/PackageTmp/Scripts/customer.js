$(document).ready(function () {
    
    $("#add").hide();
    $("#divAdd").hide();
    $("#divUpdate").hide();
    $("#divGet").hide();
    $("#customersTable").hide();
    
    $("#addCustomer").click(function () {
        $("#divAdd").slideToggle();
    });
    
    $("#updateCustomer").click(function () {
        $("#divUpdate").slideToggle();
    });

    $("#lastCustomers").click(function () {
        window.open('LastCustomer.html', '_newtab');
    });
    
    //var valuesAddress = "http://localhost:17682/api/customer/";
    //var valuesAddress = "http://ticketingofficejonathan.azurewebsites.net/api/customer/";
    var valuesAddress = "http://jholmticketingoffice.azurewebsites.net/api/customer";
        $("#getCustomers").click(function () {
            if ($("#divGet").is(":hidden")) {
                $("#divGet").addClass("hahaha");
                $.ajax({
                    url: valuesAddress,
                    type: "GET",
                    success: function (result) {
                        var tr = null;
                        for (var x = 0; x < result.length; x++) {
                            tr = $('<tr/>');
                            tr.append("<td>" + result[x].FirstName + "</td>");
                            tr.append("<td>" + result[x].LastName + "</td>");
                            tr.append("<td>" + result[x].Address + "</td>");
                            tr.append("<td>" + result[x].BirthDate + "</td>");
                            tr.append("<td>" + result[x].CellNumber + "</td>");
                            tr.append("<td>" + result[x].Country + "</td>");
                            tr.append("<td>" + result[x].City + "</td>");
                            tr.append("<td>" + result[x].Email + "</td>");
                            tr.append("<td>" + result[x].PhoneNumber + "</td>");
                            tr.append("<td>" + result[x].ReductionCode + "</td>");
                            $('table').append(tr);
                        }
                        $("#divGet").slideDown();
                    },
                    error: function(textStatus) {
                        $("#divGet").text(textStatus);
                        $("#divGet").slideToggle();
                    }
                });
            } else {
                $("#getCustomers").click(function () {
                    $("#divGet").slideUp();
                    $("#tableGet").find("tr:gt(0)").remove();

                });
            }
        });
});