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
    var categoriesFlag = 0;
    $("#SignInbtn").click(function ()
    {
        
        debugger;
        if (categoriesFlag == 0)
        {
            $("#SignIn").show(100);
            categoriesFlag = 1;

        } else
        {
            categoriesFlag = 0
            $("#SignIn").hide(100);
        }
       // $("#SignIn").slideToggle();
    });

    var categoriesFlag2 = 0;
    $("#Aboutbtn").click(function ()
    {
        
        debugger;
        if (categoriesFlag2 == 0)
        {
            $("#About").show(100);
            categoriesFlag2 = 1;

        } else
        {
            categoriesFlag2 = 0
            $("#About").hide(100);
        }
       // $("#SignIn").slideToggle();
    });

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
    var categoriesFlag = 0;
    $("#Categories").click(function ()
    {
        debugger;
        if (categoriesFlag == 0)
        {
            $("#categoriesDropDiv").show(100);
            categoriesFlag = 1;
        } else {
            categoriesFlag = 0
            $("#categoriesDropDiv").hide(100);
        }
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
});

//                                               javascript                                  // 
function loadSubCategories(cate, name)
{
    debugger;
    window.open('/Categories/GetSubCategoriesByCategory?id='+cate+"&&name="+name , "_self");
}

function loadCategories(categoryId, shopname, shopid) {
    debugger;
    window.open('/Categories/GetSubCategories?categoryId=' + categoryId + "&&shopname=" + shopname + "&&shopid=" + shopid, "_self");
}

  