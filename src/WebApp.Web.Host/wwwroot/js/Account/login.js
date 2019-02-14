$(document).ready(function () {
    var page = {
        init: function () {
            page.autoSize();
            page.initControl();
            page.eventBind();
        },
        autoSize: function () {
            var h = $("#right").height();
            $("#right_header").outerHeight((h - $("#right_footer").outerHeight()) / 2 - 50);
            $("#right_main").outerHeight((h - $("#right_footer").outerHeight()) / 2 + 50);
        },
        eventBind: function () {
            //页面Resize
            $(window).resize(function () {
                page.autoSize();
            });
   
            $('#username').focus();

            $('#LoginButton').click(function (e) {
                e.preventDefault();
                if ($('#username').val()=="") {
                    abp.notify.success('用户名不允许为空');
                    $('#username').focus();
                    return;
                }
                if ($('#password').val() == "") {
                    abp.notify.success('用户密码不允许为空');
                    $('#username').focus();
                    return;
                }
                abp.ui.setBusy(
                    null,
                    abp.ajax({
                        url: abp.appPath + 'api/TokenAuth/GetTicket',
                        type: 'POST',
                        data: JSON.stringify({
                            userNameOrEmailAddress: $('#username').val(),
                            password: $('#password').val(),
                            rememberClient: $('#rememberMe').is(':checked'),
                            clientId: $("#hdClientId").val(),
                            clientState: $("#hdClientState").val(),
                            returnUrl: $("#hdReturnUrl").val()
                        })
                        //data: JSON.stringify({
                        //    userNameOrEmailAddress: $('#username').val(),
                        //    password: $('#password').val()  
                        //})
                    }).done(function (data) {
                        abp.notify.success('success', '登录成功');
                        localStorage.setItem("serverState", data);
                        console.log(data);
                        location.href = $("#hdReturnUrl").val() + "?serverState=" + data + "&clientState=" + $("#hdClientState").val();
                    }).fail(function (data) {
                        console.log(data);
                        abp.ajax.showError(data.responseJSON.error);
                    })
                );
            });


            //绑定Enter事件
            document.onkeydown = function (_event) {
                _event = (_event) ? _event : ((window.event) ? window.event : "");
                var k = _event.keyCode ? _event.keyCode : _event.which;
                if (k == 13) { //判断是否为回车
                    $('#LoginButton').trigger("click");
                }
            };

        },
        initControl: function () {
            //选中更改图标
            $("#username").focus(function () {
                $(".icon-user").css("background-image", "url(../images/user_sel.png)")
            })
            $("#username").blur(function () {
                $(".icon-user").css("background-image", "url(../images/user.png)")
            })
            $("#password").focus(function () {
                $(".icon-password").css("background-image", "url(../images/lock_sel.png)")
            })
            $("#password").blur(function () {
                $(".icon-password").css("background-image", "url(../images/lock.png)")
            })
        }
    };
    //页面初始化
    page.init();
});