<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>注册软件</title>
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
        }
        body
        {
            padding: 5px;
            font: 12px Verdana, Arial, Tahoma;
            color: #404040;
            background: url(../../Content/tips/bodybg.jpg) repeat;
        }
        img
        {
            border-style: none;
        }
        ul
        {
            list-style-type: none;
        }
        li
        {
            list-style-image: none;
        }
        input, textarea
        {
            line-height: 16px;
            padding: 2px;
        }
        .main
        {
            position: absolute;
            width: 700px;
            height: 220px;
            top: 50%;
            left: 50%;
            margin-left: -350px;
            margin-top: -120px;
            padding: 4px;
            background: url(../../Content/tips/main_bg.png) repeat;
        }
        .content
        {
            width: 698px;
            height: 218px;
            border: 1px #b7bebf solid;
            background: #fafaf6;
        }
        .content p.tit
        {
            height: 48px;
            background: #f2f8ff url(../../Content/tips/tip32.png) no-repeat 10px center;
            border-bottom: 1px #d6e7f3 solid;
            padding-left: 48px;
            line-height: 48px;
            font-size: 14px;
            font-weight: bold;
        }
        .content p.con
        {
            padding: 2px 15px;
            height: 22px;
            line-height: 22px;
        }
        .txt_input
        {
            background: #fff;
            padding: 0px 3px;
            line-height: 22px;
            height: 22px;
            border: 1px #b6b7b9 solid;
            width: 650px;
            color: #03C;
            font-size: 12px;
        }
        .com
        {
            position: absolute;
            top: 230px;
            width: 700px;
            text-align: center;
            height: 40px;
            line-height: 40px;
            font-size: 14px;
            color: #56606a;
        }
        a
        {
            text-decoration: none;
        }
    </style>
</head>
<body>
    <div class="main">
        <div class="content">
            <p class="tit">
                你的软件使用期已过</p>
            <p class="con" style="margin-top: 8px;">
                序列号：</p>
            <p class="con">
                <input type="text" readonly="readonly" value="<%= ViewData["Message"] %>" class="txt_input" /></p>
            <p class="con">
                激活码：</p>
            <p class="con">
                <input id="key" type="text" class="txt_input" /></p>
            <p class="con" style="padding-top: 12px;">
                <input id="register" type="button" value="激 活" /></p>
        </div>
        <div class="com">
            <a target="_blank" href="http://www.ntchst.com">春晖软件提供技术支持</a> 联系电话：0513-85291122</div>
    </div>
</body>
<script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="../../Scripts/layer/layer.min.js" type="text/javascript"></script>
<script language="javascript">
    $(function () {
        var olayer;
        $("#register").click(function () {
            $.get("../../Register/Register", { key: $("#key").val(), r: Math.random }, function (obj) {
                if (obj == "ok") {
                    window.location = "../../Home/Index";
                } else {
                    olayer = layer.alert("激活失败！", function () {
                        layer.close(olayer);
                    });
                }
            });
        });
    });
</script>
</html>
