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

    $("#showNotifications").hover(function () {
        //debugger;
        $("#ShopDataDiv").show();
        $(".shopStatus_Div").show();
    }, function () {
        $("#ShopDataDiv").hide();
        $(".shopStatus_Div").hide();
    });

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
setInterval(function () {
    rightsideclickevent();
},10000);
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
    if (checkUserSignIn()) {
        if (cartDiv.css('display') == 'block') {
            cartDiv.removeClass('animate__bounceInRight');
            cartDiv.addClass('animate__animated animate__bounceOutRight');
            setTimeout(function () {
                cartDiv.hide();
            },1000);
           
        } else {
            cartDiv.show();
            var loadCart = $("#LoadCartData");
            loadCart.html('<div class="Loader"></div>');
            cartDiv.removeClass('animate__bounceOutRight');
            cartDiv.addClass('animate__animated animate__bounceInRight');
            var url = "/Items/BuyItemView?Userid=" + $('#CheckLogin').val();
            $.get(url, function (data)
            {
                debugger;
                loadCart.html('');
                loadCart.html(data);
                
            });
        }
            
        } else {

            Swal.fire({
                position: 'top-end',
                icon: 'warning',
                title: 'Please SignIn first !',
                showConfirmButton: false,
                timer: 1500
            });
       
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
                 
                   Swal.fire({
                       position: 'top-end',
                       icon: 'success',
                       title: 'Item Added To cart !',
                       showConfirmButton: false,
                       timer: 1500
                   });
                   location.reload();
               } else {

                   
                   Swal.fire({
                       position: 'top-end',
                       icon: 'warning',
                       title: 'Can t add to cart this item',
                       showConfirmButton: false,
                       timer: 1500
                   });
               }
               debugger
                    }
                };
                $.ajax(options);
    } else {
        
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please SignIn first !',
            showConfirmButton: false,
            timer: 1500
        });
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
      
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please enter Name.',
            showConfirmButton: false,
            timer: 1500
        });
        return;
    }
    if (document.getElementById('ProfileUserEmailAddress').value == "")
    {
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please enter Email Address.',
            showConfirmButton: false,
            timer: 1500
        });
       
        return;
    }
    if (document.getElementById('ProfileUserContact').value == "")
    {
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please enter Contact.',
            showConfirmButton: false,
            timer: 1500
        });
       
        return;
    }
    if (document.getElementById('ProfileUserUsername').value == "")
    {
        
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please enter Username.',
            showConfirmButton: false,
            timer: 1500
        });
       
        return;
    }
    if (document.getElementById('ProfileUserAddress').value == "")
    {
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please enter Address.',
            showConfirmButton: false,
            timer: 1500
        });
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
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'User profile updated successfully.',
                    showConfirmButton: false,
                    timer: 1500
                });
                $(".close").trigger("click");
                window.location.reload();
            }
            else {
                Swal.fire({
                    position: 'top-end',
                    icon: 'warning',
                    title: 'Profile not updated.',
                    showConfirmButton: false,
                    timer: 1500
                });
                return;
            }
        },
        error: function () {
            debugger;
            alert("Error");
        }
    });
}

function rightsideclickevent() {


    for (var i = 0; i < $('.slider').length; i++) {
        if ($('.slider:eq(' + i + ')').attr('data-status') == 'on') {
            var slider1 = $('.slider:eq(' + i + ')');
            var sliderno = slider1.attr('data-number');
            if (sliderno == "3") {
                for (var i = 0; i < $('.slider').length; i++) {
                    if ($('.slider:eq(' + i + ')').attr('data-number') == '1') {
                        $('.slider').hide();
                        $('.slider').attr('data-status', 'off');
                        $('.slider:eq(' + i + ')').show();
                        $('.slider:eq(' + i + ')').attr('data-status', 'on');
                        $('.slider:eq(' + i + ')').attr('class', 'container-fluid slider');
                        $('.slider:eq(' + i + ')').addClass('animate__animated animate__slideInRight');
                        
                    }
                }
            }
            else {
                var slidernoint = parseInt(sliderno);
                slidernoint = slidernoint + 1;
                slidernoint = slidernoint.toString();
                for (var i = 0; i < $('.slider').length; i++) {
                    if ($('.slider:eq(' + i + ')').attr('data-number') == slidernoint) {
                        $('.slider').hide();
                        $('.slider').attr('data-status', 'off');
                        $('.slider:eq(' + i + ')').show();
                        $('.slider:eq(' + i + ')').attr('data-status', 'on');
                        $('.slider:eq(' + i + ')').attr('class', 'container-fluid slider');
                        $('.slider:eq(' + i + ')').addClass('animate__animated animate__slideInRight');
                    }
                }
            }
        }
    }
}
function leftsideclickevent() {
    for (var i = 0; i < $('.slider').length; i++) {
        if ($('.slider:eq(' + i + ')').attr('data-status') == 'on') {
            var slider1 = $('.slider:eq(' + i + ')');
            var sliderno = slider1.attr('data-number');
            if (sliderno == "1") {
                for (var i = 0; i < $('.slider').length; i++) {
                    if ($('.slider:eq(' + i + ')').attr('data-number') == '3') {
                        $('.slider').hide();
                        $('.slider').attr('data-status', 'off');
                        $('.slider:eq(' + i + ')').show();
                        $('.slider:eq(' + i + ')').attr('data-status', 'on');
                        $('.slider:eq(' + i + ')').attr('class', 'container-fluid slider');
                        $('.slider:eq(' + i + ')').addClass('animate__animated animate__slideInLeft');
                    }
                }
            }
            else {
                var slidernoint = parseInt(sliderno);
                slidernoint = slidernoint - 1;
                slidernoint = slidernoint.toString();
                for (var i = 0; i < $('.slider').length; i++) {
                    if ($('.slider:eq(' + i + ')').attr('data-number') == slidernoint) {
                        $('.slider').hide();
                        $('.slider').attr('data-status', 'off');
                        $('.slider:eq(' + i + ')').show();
                        $('.slider:eq(' + i + ')').attr('data-status', 'on');
                        $('.slider:eq(' + i + ')').attr('class', 'container-fluid slider');
                        $('.slider:eq(' + i + ')').addClass('animate__animated animate__slideInLeft');

                    }
                }
            }
        }
    }
}

function getTrackingHistory(Shopid) {
    debugger;
    
    if (checkUserSignIn()) {
        var trackingView = $('#History-container');
        console.log(Shopid);
        var url = '/user/getHistoryofUser?shopid=' + Shopid;
        $.get(url, function (data) {
            debugger;
            trackingView.html(data);
            trackingView.show();
        });
    } else {
        Swal.fire({
            position: 'top-end',
            icon: 'warning',
            title: 'Please SignIn first !',
            showConfirmButton: false,
            timer: 1500
        });
    }
}
function showTrackingPackage() {
    debugger;
    $('.loadingMod').show();
    var trackingView = $('#TrackPackageContainer');
    var trackingid = $('#TrackingValue').val();
    trackingView.hide();
    if (trackingid != null) {
        var url = '/user/showTrackingPackage?TrackingId=' + trackingid;
        $.get(url, function (data)
        {
            debugger;
            $('.loadingMod').hide();
            trackingView.html(data);
            trackingView.show();
        });
    }
}
