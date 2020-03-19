<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
    <link href="../../Content/comit/css/login.css" rel="stylesheet" type="text/css" />

    <script src="../../Scripts/js/jquery-1.8.2.min.js" type="text/javascript"></script> 
    <script src="../../Scripts/jquery.form.js" type="text/javascript"></script>  
    <script src="../../Scripts/layer/layer.min.js" type="text/javascript"></script> 
    <script src="../../Content/js/template.js" type="text/javascript"></script>
    <script src="../../Content/js/appServers.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#username").focus();
            $("#username").bind('keydown', function (event) {

                if (event.keyCode == "13") {

                    if ($("#username").val() == "") {
                        layer.msg("请输入用户名", 1, 9, function result() {
                            $("#username").focus();
                        });
                        return false;
                    }
                    else {
                        $("#password").focus();
                    }
                }
            });

            $("#password").bind('keydown', function (event) {
                if (event.keyCode == "13") {

                    if ($("#password").val() == "") {
                        layer.msg("请输入密码", 1, 9, function result() {
                            $("#password").focus();
                        });
                        return false;

                    } else {
                        login();
                    }
                }

            });
        });
      
        function login() {
            $.ajax({
                type: 'post',
                url: "../../Login/loginAPI",
                data: { userid: $("#username").val(), password: $("#password").val() },
                dataType: 'json',
                success: function (data) {
                    if (data.Login == true) {
                        layer.load("登录中...", 20);
                        //其它系统登录
                        sysLogin(data);
                    } else {
                        layer.alert("用户名或密码错误！", 5);
                    }

                }
            });
        }

//    function goHome() {
//        window.clearTimeout(t1);
//        layer.closeAll();
//        window.location.href = "<%=ViewData["url"] %>";
//    }
      var queue = new Array(); //接口队列
      var schoolid;
      var schoolname;
      function sysLogin(data) {
        //接口地址
        var strursl=data.url;
        schoolid=data.schoolid;
        schoolname=data.schoolname;
        
        //接口队列
        queue = strursl.split(";"); //字符分割
        //从队列第一个开始
        app.jspon(queue[0],0,$("#username").val(),$("#password").val(),data.Name,data.schoolid,data.schoolname,resultCallBack);
      }

      function resultCallBack(data,name){ 
         var queuecount=parseInt(data.flag)+1; 
         if(parseInt(queuecount)<queue.length){
                app.jspon(queue[queuecount],queuecount,$("#username").val(),$("#password").val(),name,schoolid,schoolname,resultCallBack);
         }else{
            layer.closeAll();
            window.location.href = "<%=ViewData["url"] %>";
         }


//        if(data.flag=="true")
//        {
//            layer.closeAll();
//            window.location.href = "<%=ViewData["url"] %>";
//        }
      }

     
    </script>
</head>
<body>
<div class="logintop"> <span>欢迎登录</span> </div>
<div class="loginbody">
  <div class="loginbox">
    <ul>
      <li>
        <input name="username" id="username"  type="text" class="loginuser" placeholder="请输入用户名" />
      </li>
      <li>
        <input name="password" id="password" type="password" class="loginpwd" placeholder="请输入密码" />
      </li>
      <li>
        <input type="button" class="loginbtn" value="登录"  onclick="login()" />
       
      </li> 
    </ul>
  </div>
</div> 
   
</body>
</html>
