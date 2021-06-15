function HConfirmCustom(content, type, btn1, function1, btn2, function2, btn3, function3)
{
    try {

        
            document.body.scrollTop = 0; // For Safari
            document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
        
        //creating alert box
        var x = document.getElementById("H-Custom-Alert");
        x.innerHTML = '<div style="height: 100%; width: 100%; position: absolute; z-index: 100000; background: rgba(0,0,0,0.3);">'
            + '<div id="HAlertWindow" style = " min-height: 100px; width: 300px; margin: auto; background: white; margin-top: 15%; border-radius: 6px; box-shadow: 0 7px 8px -4px rgb(0 0 0 / 20%), 0 13px 19px 2px rgb(0 0 0 / 14%), 0 5px 24px 4px rgb(0 0 0 / 12%);    padding-bottom: 7px;" >'
            + '<div id="h-alert-header" class="" style=" height: 6px; background: yellow; border-radius: 6px 6px 0px 0px;"></div>'
            + '<div class="">'
            + '<div style=" margin: 1px; margin-right: 7px;">'
            + '<button type="button" class="close" aria-label="Close" id="h-alert-close">'
            + '<i class="fas fa-times" style="color:black;"></i>'
            + '</button>'
            + '</div>'
            + '</div>'
            + '<div style="width: 100%; min-height: 60px;">'
            + '<h3 style="width:98% ; overflow-wrap:break-word ; font-size:1.0rem; color:#887474;font-family: inherit;text-align: center;margin-top: 14px;padding: 10px;" id="h-alert-Content">'
            + 'No Content'
            + ' </h3>'
            + '</div>'
            + '<div style=" width: 100%; height: 30px;border-radius:0px 0px 6px 6px">'
            + '<input type="button" class=" " id="h-btn-3-alert" value="btn3" style="border-radius: 10px;float:right; border: none;color: white;margin: 4px;padding: 2px 10px 4px 10px; margin-right: 8px;display:none;"/>'
            + '<input type="button" class=" " id="h-btn-2-alert" value="btn2" style="border-radius: 10px;float:right; border: none;color: white;margin: 4px;padding: 2px 10px 4px 10px;margin-right: 8px;display:none;"/>'
            + '<input type="button" class="" id="h-btn-1-alert" value="btn1" style="border-radius: 10px;float:right;border: none;color: white;margin: 4px;padding: 2px 10px 4px 10px;margin-right: 8px;  display:none;"/>'
            + '</div>'
            + '</div >'
            + '</div >';
        document.getElementById('HAlertWindow').style.animationName = 'HAlertAnimationstart';
        document.getElementById('HAlertWindow').style.animationDuration = '0.5s';
        x.onclick = function () {
            //document.getElementById('HAlertWindow').style.animationName = 'HAlertAnimation';
            //document.getElementById('HAlertWindow').style.animationDuration = '0.5s';
        }
        //debugger;
        switch (type) {
            case 'success':
                document.getElementById('h-alert-header').style.background = '#057b05';
                document.getElementById('h-btn-1-alert').style.background = '#057b05';
                document.getElementById('h-btn-2-alert').style.background = '#057b05';
                document.getElementById('h-btn-3-alert').style.background = '#057b05';
                break;
            case 'warning':
                document.getElementById('h-alert-header').style.background = '#f1c020';
                document.getElementById('h-btn-1-alert').style.background = '#f1c020';
                document.getElementById('h-btn-2-alert').style.background = '#f1c020';
                document.getElementById('h-btn-3-alert').style.background = '#f1c020';
                break;
            case 'danger':
                document.getElementById('h-alert-header').style.background = 'red';
                document.getElementById('h-btn-1-alert').style.background = 'red';
                document.getElementById('h-btn-2-alert').style.background = 'red';
                document.getElementById('h-btn-3-alert').style.background = 'red';
                break;
            default:
                document.getElementById('h-alert-header').style.background = '#0086FF';
                document.getElementById('h-btn-1-alert').style.background = '#0086FF';
                document.getElementById('h-btn-2-alert').style.background = '#0086FF';
                document.getElementById('h-btn-3-alert').style.background = '#0086FF';
                break;

        }
        //debugger;
        document.getElementById('H-Custom-Alert').style.display = 'block';
        document.getElementById('h-alert-close').onclick = function () {
            x.innerHTML = '';
        }
        document.getElementById('h-alert-Content').innerHTML = content;
        if (btn1 != '') {
            document.getElementById('h-btn-1-alert').value = btn1;
            document.getElementById('h-btn-1-alert').style.display = 'block';
            document.getElementById('h-btn-1-alert').onclick = function () {
                x.style.display = 'none';
                x.innerHTML = '';
                function1();
            };
        }
        if (btn2 != '') {
            document.getElementById('h-btn-2-alert').value = btn2;
            document.getElementById('h-btn-2-alert').style.display = 'block';
            document.getElementById('h-btn-2-alert').onclick = function () {
                x.style.display = 'none';
                x.innerHTML = '';
                function2();
            };
        }
        if (btn3 != '') {
            document.getElementById('h-btn-3-alert').value = btn3;
            document.getElementById('h-btn-3-alert').style.display = 'block';
            document.getElementById('h-btn-3-alert').onclick = function () {
                x.style.display = 'none';
                x.innerHTML = '';
                function3();

            };
        }


    } catch (err) {
        alert('Jconfirm error ' + err);
    }
}