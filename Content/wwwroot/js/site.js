// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function ()
{
    ShowAllItems();
    $("#Expand").click(function ()
    {
        debugger;
        $("#dropdown").slideToggle();
    });

    $("#SignInbtn").click(function ()
    {
        debugger;
        $("#SignIn").slideToggle();
    });

    $("#profilebtn").click(function ()
    {
        debugger;
        $("#profile").slideToggle();
    });

    $("#Subcategories").change(function ()
    {
        debugger;
        $.ajax({
            url: "/Items/GetSubCategoriesItem",
            data: { id: this.value },
            contentType: "application/json",
            success: function (responce) {
                debugger;
                var html = '';
                $(responce).each(function (index, value) {
                    debugger;
                    html += '<div onclick="buy(1);" style="margin:5px;text-align:center;display: grid;cursor: pointer;" class="col-sm-2" style="background-color:lavender;" id=' + value["id"] + '>'
                        + '<img src=' + value["image"] + '/><b>' + value["name"] + '</b></div >'
                });
                $("#allcategories").html(html);
            }
        });
    });
    $("#itemSearchResult").keyup(function () {
        debugger;
        if ($(this).val().trim() == "") {
            $("#searchResultID").hide();
        } else {
            $("#searchResultID").show();
        }
        
    });
});
