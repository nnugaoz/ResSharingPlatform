<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
    <link href="../../Content/global.css" rel="stylesheet" type="text/css"/>
    <link href="../../Content/layout.css" rel="stylesheet" type="text/css"/>
    <link href="../../Content/bootstrap.css" rel="stylesheet" type="text/css"/> 
    <link href="../../Content/add.css" rel="stylesheet" type="text/css" />

    <script src="../../Scripts/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/js/focus.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.form.js" type="text/javascript"></script>

    
    <script src="../../Scripts/layer/layer.min.js" type="text/javascript"></script> 
    <script src="../../Content/js/template.js" type="text/javascript"></script>
    <script src="../../Content/js/appServers.js" type="text/javascript"></script>
    <style>
        .listbar
        {
	        z-index:9999;width:90%;position:absolute;left:20px;top:300px;text-align:center;
        }
    </style>
    <script>
       
        $(function () {
            getRoleList();
        });

        // 初始化角色数据列表
        function getRoleList() {
            $(".box").html(""); 
            $.ajax({
                type: 'post',
                url: "../../Role/get_role_user",
                dataType: 'json',
                success: function (data) {
                    var html = template('roleList', data);
                    $(".box").append(html);
                    //setColor();
                }
            });
        }

        //检索用户
        function searchuser() {
            $(".box").html("");
            $.ajax({
                type: 'post',
                url: "../../Role/search_user",
                dataType: 'json',
                data: { username: $("#username").val() },
                success: function (data) {
                    var html = template('searchlist', data);
                    alert(html);
                    $(".box").html(html);
                }
            });
        }


        function getUserList(id, obj) {

            if ($(obj).parent().parent().find(".userlist").is(":hidden")) {
                $(obj).parent().parent().find(".userlist").show();    //如果元素为隐藏,则将它显现
            } else {
                $(obj).parent().parent().find(".userlist").hide();     //如果元素为显现,则将其隐藏
                return;
            }

            $(document.body).append("<div class='listbar'><img src='../images/loading-0.gif' /></div>");
            $.ajax({
                type: 'post',
                url: "../../Role/get_role_user_list",
                dataType: 'json',
                data: { id: id },
                success: function (data) {
                    $(".listbar").remove();
                    var html = template('userlist', data);
                    $(obj).parent().parent().find(".userlist").html(html);
                    //setColor();
                }
            });
        }

        function setColor() {
            $(".box>ul>li").hover(function () {
                $(this).addClass("selected");
            }, function () {
                $(this).removeClass("selected");
            });

        }

        function addGroupUser(id) {
            window.parent.addGroupUser(id);
        }

        function addUserList(groupid,userid,username) {
            window.parent.addUserList(groupid, userid, username);
        }
        
    </script>
    <style>
        .selected
        {
            background-color: #0F6099;
        }
        .rolelist 
        {
            width:100%;
            
            line-height:30px;
            text-align:left;
            padding-left:0px;
            background-color: #e8f0f7;
            color: #555; 
            cursor: pointer;
        }
        
        .rolelist a 
        {
             
            line-height:30px;
            text-align:left; 
            color: #555; 
             padding-left:10px;
            text-decoration:none;
        }
        
         .role_userlist 
        {
            width:100%;
            
            line-height:30px;
            text-align:left;
            padding-left:20px;
            background-color: #8795A0;
            color: #555; 
        }
        
        .role_userlist a 
        {
             
            line-height:30px;
            text-align:left; 
            color: #fff; 
            padding-left:10px;
            text-decoration:none;
        }
        
        
         .rolelistgroup
        {
            width:100%;
            background: url("../../Images/arrow.png") no-repeat;
            background-position:5px 50%;  
        }
        
          .userlist
        {
            width:100%;
            background: url("../../Images/arrow.png") no-repeat;
            background-position:0% 50%;  
            display:none;
        }
        
        .rolelist span
        {
            text-align:right;
            background-color: #0F6099;
            color: #fff!important;
            padding: 2px 4px 4px;
            text-decoration: none;
            border-radius: 3px;
            position: absolute;
            right:5px;
        }
    </style>
      <!--角色列表-->
    <script id="roleList" type="text/html">
        <ul>
        {{each list as value}}
            
            <li class="rolelist">
                <div class="rolelistgroup">
                <a href="javascript:;" onclick="getUserList('{{=value.id}}',this)">{{=value.title}}({{=value.usercount}})</a><span onclick="addGroupUser('{{=value.id}}')">添加该组</span>
                </div>
                <div class="userlist"></div>
            </li>
        {{/each}}
        </ul>
    </script>
    <!--角色列表-->

     <!--人员列表-->
    <script id="userlist" type="text/html">
        <ul>
        {{each list as value}}
            <li class="role_userlist">
                <a href="javascript:;" onclick="addUserList('{{=id}}','{{=value.id}}','{{=value.name}}')">{{=value.name}}</a>
            </li>
        {{/each}}
        </ul>
    </script>
    <!--人员列表-->

     <!--检索人员列表-->
    <script id="searchlist" type="text/html">
        <ul>
        <li class="role_userlist">
                <a href="javascript:;" onclick="getRoleList()">返回</a> 
        </li>
        {{each list as value}}
            <li class="role_userlist">
                <a href="javascript:;" onclick="addUserList('{{=value.pagerole_id}}','{{=value.id}}','{{=value.name}}')">{{=value.name}}</a>
            </li>
        {{/each}}
        </ul>
    </script>
    <!--人员列表-->
</head>
<body style="min-width:100px;background-color: #F5F7FA">
  <div>
        <form id="actionform" class="form-horizontal ui-formwizard"  method="post">
 
            <div class="con-title" style="margin-bottom:0px;">
               <h1 class="con-title-h1">
                选择收件人</h1>
            <div class="con-title-btns">
               
            </div>
            
            </div>
            <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                    <tbody><tr>
                        <td class="right" style="width:140px;">
                        <input type="text" id="username" name="username" class="inputText" placeholder="请输入姓名" maxlength="255" style="width:140px;">
                        </td> 
                        <td> 
                            <input type="button" id="Button1" name="search" class="title-btn" value="检索" onclick="searchuser()">
                        </td>
                    </tr> 
                    
                </tbody></table>
            <div class="box" >
              
            </div>
        </form> 
    </div>
</body>
</html>
