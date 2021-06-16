// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function ()
{
    //ShowAllItems();
/*    $("#Expand").click(function ()
    {
        debugger;
        $("#dropdown").slideToggle();
    });*/


    $("#profilebtn").click(function ()
    {
        debugger;
        $("#profile").slideToggle();
    });

    $("#Subcategories").change(function ()
    {
        debugger;
        var selectedText = $(this).find("option:selected").text();
        window.open('/Categories/GetSubCategoriesItem?id=' + this.value + "&&name=" + selectedText, "_self");
       /* $.ajax({
            url: "/Items/GetSubCategoriesItem",
            data: { id: this.value },
            contentType: "application/json",
            success: function (responce)
            {
                debugger;
                var html = '';
                $(responce).each(function (index, value)
                {
                    debugger;
                    *//*html += '<div onclick="buy('+value["id"]+');"  class="col-sm-3 cardstyle" id=' + value["id"] + '>'
                        + '<table>'
                        + '<tr><td style="padding-left: 50px;"><img src=' + value["image"] + '/></td></tr>'
                        + '<tr><td><b>Item Name :</b></td><td>' + value["name"] + '</td></tr>'
                        + '<tr><td>Weight : </td><td>' + value["weight"] + '</td></tr>'
                        + '<tr><td>Price : </td><td>' + value["price"] + '</td></tr>'
                        + '</table></div>'*//*

                    html += '<div class="col-sm-4" onclick="buy('+value["id"]+');">'
                   +'<div class="best_shoes">'
                        +'<p class="best_text">'+value["name"]+'</p>'
                      +'<div class="shoes_icon"><img src='+value["image"]+'/></div>'
                        +'<div class="star_text">'
                            +'<div class="left_part">'
                                +'<ul>'
                                    +'<li><a href="#"><img src="/Content/Template/images/star-icon.png"/></a></li>'
                                    +'<li><a href="#"><img src="~/Content/Template/images/star-icon.png"/></a></li>'
                                    +'<li><a href="#"><img src="~/Content/Template/images/star-icon.png"/></a></li>'
                                    +'<li><a href="#"><img src="~/Content/Template/images/star-icon.png"/></a></li>'
                                    +'<li><a href="#"><img src="~/Content/Template/images/star-icon.png"/></a></li>'
                                +'</ul>'
                            +'</div>'
                            +'<div class="right_part">'
                                +'<div class="shoes_price">$ <span style="color: #ff4e5b;">'+value["price"]+'</span></div>'
                            +'</div>'
                        +'</div>'
                        +'</div>'
                        +'</div>'

                });
                $("#allcategories").html(html);
            }
        });*/
    });

/*    $("#itemSearchResult").keyup(function () {
        debugger;
        if ($(this).val().trim() == "") {
            $("#searchResultID").hide();
        } else {
            $("#searchResultID").show();
            $.get("/Home/SearchItems?Name=" + $(this).val(), function (r) {
                //update ui with results
                $("#searchResultID").html(r);
            });
        }
        
    });*/

    $("#SignInbtn").hover(function () {
        $("#SignIn").show();
    }, function () {
            $("#SignIn").hide();
    });

    $("#Aboutbtn").hover(function () {
        $("#About").show();
    }, function () {
            $("#About").hide();
    });

    $("#Categories").hover(function () {
        $("#categoriesDropDiv").show();
    }, function () {
            $("#categoriesDropDiv").hide();
    });

    $('.showItemsOfSubCategories').click(function ()
    {
        debugger;
        var SCateId = $(this).find($('.GetSubCategorieId')).val();
        var name = $(this).find($('.GetSubCategorieName')).val();;
        $.get("/Categories/getItemsByCategories?id=" + SCateId + "&&name=" + name, function (r)
        {
            //update ui with results
            $("#item_View").html(r);
        });
            $('html,body').animate({
                scrollTop: $("#item_View").offset().top
            },
                'fast');
    });


    $("#LocationType").hover(function () {
        $("#LocationTypeDropDiv").show();
    }, function () {
        $("#LocationTypeDropDiv").hide();
    });

});
$('.sideNavtab').click(function () {
    $('.sideNavtab').css('border-left', '0px solid');
    $(this).css('border-left', '4px solid');

});
var SearchItemsFlag = 0;
function SearchItems() {
    $('.searchEveryThing');
    if (SearchItemsFlag == 0) {
        $('.searchEveryThing').css('display', 'block');
        SearchItemsFlag = 1;
    } else {
        $('.searchEveryThing').css('display', 'none');
        SearchItemsFlag = 0;
    }
}
$('#searchItemsInshop').keyup(function () {
    if ($('#searchItemsInshop').val() == "") {
        $("#searchResultView").hide();
        $("#NormalView").show();
    } else {
        $("#searchResultView").show();
        $("#NormalView").hide();
        $.ajax({
            type: "GET",
            url: "/Items/SearchItemBySubCategories?key=" + $('#searchItemsInshop').val() + "&&Sid=" + $("#GetSubCategoryId").val(),
            success: function (responce) {
                debugger;
                $('#searchResultView').html('');
                $('#searchResultView').html(responce);
            }
        });
    }
});
//                                               javascript                                  // 
function loadLocationType(type)
{
    debugger;
    window.open('/Home/Index?type=' + type, "_self");
}

function loadSubCategories(cate, name)
{
    debugger;
    window.open('/Categories/GetSubCategoriesByCategory?id='+cate+"&&name="+name , "_self");
}

function loadCategories(location, shopname, shopid) {
    debugger;
    window.open('/Categories/GetSubCategories?location=' + location + "&&shopname=" + shopname + "&&shopid=" + shopid, "_self");
}

function RegisterAccount(userType) {
    debugger;
    window.open('/Login/SabiRegister?type=' + userType, "_self");
}

function Search() {
    debugger;
    var val1 = $('#Searchid').val();

    if (val1 == "") {
        $('#Searchdiv').hide();
        return;
    }

    var url = '/Items/SearchItem?id=' + $("#Searchid").val();
    $.get(url, function (data) {
        $('#Searchdiv').html(data);
        $('#Searchdiv').show();
    });
}

function LoadProfile() {
    debugger;
    $("#openUserProfileModal").trigger("click");
}
function checkUserSignIn() {
    if ($('#CheckLogin').val() == "0") {
        return false;
    }
    return true;
}

function BuyItemView() {
    debugger;
    var cartDiv = $(".AddtoCartDiv");
    cartDiv.show();
    var loadCart = $("#LoadCartData");
    if (checkUserSignIn()) {
        var url = "/Items/BuyItemView?Userid=" + $('#CheckLogin').val();
        $.get(url, function (data) {

            debugger;
            loadCart.html(data);
        });
    } else {
        Swal.fire('Please SignIn first !','','warning')
    }
  
}
function AddToCart(itemId) {
    debugger
    if (checkUserSignIn()) {
       var options = {
                    type: "POST",
           url: '/Items/AddtoCart?itemId=' + itemId + "&&userID=" + $('#CheckLogin').val() ,
                    async: false,
                    cache: false,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
           success: function (response) {
               if (response == "ok") {
                   Swal.fire('Item Added To cart !', '', 'success');
               } else {

                   Swal.fire('Can t add to cart this item', '', 'warning');
               }
               debugger
                    }
                };
                $.ajax(options);
    } else {
        Swal.fire('Please SignIn first !','','warning')
    }
  
}
function showItemDetails(itemId) {
    var itemView = $('#itemViewFromAllSide');
    var trigger = $('#ItemViewTrigger');
    trigger.trigger('click');
    
    var url = '/Items/ItemView?itemID=' + itemId;
    $.get(url, function (data) {
        itemView.html(data);
        itemView.show();
    });
}
function UpdateUserProfileInfo() {
    debugger;
    if (document.getElementById('ProfileUserName').value == "")
    {
      
        Swal.fire('Please enter Name.', '','warning');
        return;
    }
    if (document.getElementById('ProfileUserEmailAddress').value == "")
    {
        Swal.fire('Please enter Email Address.', '', 'warning');
       
        return;
    }
    if (document.getElementById('ProfileUserContact').value == "")
    {
        Swal.fire('Please enter Contact.', '', 'warning');
       
        return;
    }
    if (document.getElementById('ProfileUserUsername').value == "")
    {
        Swal.fire('Please enter Username.', '', 'warning');
       
        return;
    }
    if (document.getElementById('ProfileUserAddress').value == "")
    {
        Swal.fire('Please enter Address.', '', 'warning');
      
        return;
    }

    f = new FormData($("#updateUserProfileForm")[0]);

    $.ajax({
        type: "POST",
        async: "true",
        data: f,
        url: "/User/UpdateUserProfile",
        processData: false,
        contentType: false,
        success: function (data) {
            debugger;
            if (data == "success")
            {
                Swal.fire('User profile updated successfully.', '','success');
                $(".close").trigger("click");
                window.location.reload();
            }
            else {
                Swal.fire('Profile not updated.', '', 'warning');
                return;
            }
        },
        error: function () {
            debugger;
            alert("Error");
        }
    });
}

