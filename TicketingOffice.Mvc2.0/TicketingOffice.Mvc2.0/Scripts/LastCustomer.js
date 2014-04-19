/// <reference path="jquery-1.9.1.min.js" />
/// <reference path="jquery.signalR-2.0.0.min.js" />
/// <reference path="/signalr/hubs" />


$(function() {
    var customerHub = $.connection.customerHub;
    
    customerHub.client.broadcastMessage = function (values, time) {
        var firstname = "Waiting...";
        var lastname = null;
        var address = null;
        var birth = null;
        var city = null;
        var phone = null;
        var cell = null;
        var reduction = null;
        var email = null;
        var country = null;

        $("#Customers").append("<li>Customer: " + firstname + "</li>");

        $.each(values, function (index, value) {
            if (index == "FirstName") {
                firstname = value;
            }
            if (index == "LastName") {
                lastname = value;
            }
            if (index == "Address") {
                address = value;
            }
            if (index == "BirthDate") {
                birth = value;
            }
            if (index == "CellNumber") {
                cell = value;
            }
            if (index == "Country") {
                country = value;
            }
            if (index == "City") {
                city = value;
            }
            if (index == "Email") {
                email = value;
            }
            if (index == "PhoneNumber") {
                phone = value;
            }
            if (index == "ReductionCode") {
                reduction = value;
            }
        });
        
        $("#tableGet > tbody:last").append("<tr>" +
            "<td>" + firstname + "</td>" +
            "<td>" + lastname + "</td>" +
             "<td>" + address + "</td>" +
             "<td>" + birth + "</td>" +
             "<td>" + cell + "</td>" +
             "<td>" + country + "</td>" +
             "<td>" + city + "</td>" +
             "<td>" + email + "</td>" +
             "<td>" + phone + "</td>" +
             "<td>" + reduction + "</td>" +
            "<td>" + time + "</td>" +
            "</tr>");
        var postaddress = "/customer/addcustomer";
        $.ajax({
            url: postaddress,
            type: "POST",
            data: values,
            success: function () {
                $("#divAdd").slideUp();
                $("#Customers").append("<li>Customer: " + firstname + " " + time + "</li>");
            },
            error: function (data) {
                alert("something got wrong!" + data);
            }
        });
    };
    $.connection.hub.logging = true;
    $.connection.hub.start().done(function () {
        $("#buttonAdd").click(function () {
            var $inputs = $('#formAdd :input');
            var values = {};
            $inputs.each(function () {
                values[this.name] = $(this).val();
            });
            var now = new Date();
            customerHub.server.send(values, now);
        });
    });
});