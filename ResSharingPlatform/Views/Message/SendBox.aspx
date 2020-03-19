<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    发信
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .listbar
        {
            z-index: 9999;
            width: 100%;
            position: absolute;
            left: 20px;
            top: 360px;
            text-align: center;
        }
        .role_userlist
        {
            float: left;
            text-align: center;
            line-height: 20px;
            background-color: #0F6099;
            color: #fff !important;
            padding-left: 5px;
            padding-right: 5px;
            margin-right: 5px;
            margin-top: 5px;
            border-radius: 3px;
        }
        
        .role_userlist a
        {
            font-size: 12px;
            color: #fff !important;
        }
    </style>
    <script type="text/javascript">


        $(function () {
            initUser();
        });

        function initUser() { 
            if("<%=ViewData["groupid"] %>"!="")
            {
                addUserList("<%=ViewData["groupid"] %>", "<%=ViewData["userid"] %>", "<%=ViewData["username"] %>");
            }
        }


        function backSendList() {
            window.location.href = "../../Message/SendList";
        }

        function selectUser() {
            //iframe层-禁滚动条 
            $.layer({
                type: 2,
                title: ["选择收件人", true], //不显示默认标题栏
                //shade: [1], //显示遮罩
                area: ["260px", "700px"],
                shift: 'left', //从左动画弹出
                iframe: { src: "../../Message/selectUser" }
            });
        }

        function addGroupUser(id) {
            $(document.body).append("<div class='listbar'><img src='../images/loading-0.gif' /></div>");
            $.ajax({
                type: 'post',
                url: "../../Role/get_role_user_list",
                dataType: 'json',
                data: { id: id },
                success: function (data) {
                    
                    $(".listbar").remove();
                    var html = template('userlist', data);
                    if ($("#group_" + data.id).length > 0) {
                        $("#group_" + data.id).remove();
                    }
                    $("#usergroup").append(html);
                }
            });
        }

        function deleteUser(id) {
            $("#user_"+id).remove();
        }

        function addUserList(groupid,userid,username) {
            var data = {
                id: groupid,
                list: [{'id':userid,'name':username}]
            };

            if ($("#user_" + userid).length > 0) {
                return;
            }


            if ($("#group_" + groupid).length > 0) {
                var html = template('singleuser', data);
                $("#group_" + groupid).append(html);
            } else {
                var html = template('userlist', data);
                $("#usergroup").append(html);
            }

        }

        function save() {

            if ($("#usergroup:has(ul)").length == 0) {
                layer.alert("请选择收件人！", 5);
                return;
            }   

            if ($.trim($("#Msg_Title").val()) == "") {
                layer.alert("请输入主题！", 5);
                return;
            }

            if ($.trim($("#Msg_Content").val()) == "") {
                layer.alert("请输入内容！", 5);
                return;
            }

            app.ajaxForm("../../Message/SaveSendMsg", saveCallBack); 
        }

        function saveCallBack(data) {
            if (data.result == "success") {
                layer.msg('发送成功', 2, 1, function(){
                    app.redirect("../../Message/SendList");
                });
                //app.msg("发送成功！",10);
                //layer.alert("发送成功！", 10);
                //app.redirect("../../Message/SendList");
            } else {
                layer.alert("发送失败！", 5);
            }
        }

    </script>
    <!--人员列表-->
    <script id="userlist" type="text/html">
        <ul id="group_{{id}}">
        {{each list as value}}
            <li class="role_userlist" id="user_{{=value.id}}" >
                <a href="javascript:;" onclick="addUserList('{{=value.id}}')">{{=value.name}}</a>
                <span onclick="deleteUser('{{=value.id}}')" style="cursor:pointer;"><img src="../../Images/user_delete.png" style="width:16px;heigth:16px;"/></span>
                <input type="hidden" name="tousergroup" value="{{=value.id}}"/>
            </li>
        {{/each}}
        </ul>
    </script>
    <!--人员列表-->
    <!--单个人员人员-->
    <script id="singleuser" type="text/html"> 
        {{each list as value}}
            <li class="role_userlist" id="user_{{=value.id}}" >
                <a href="javascript:;" onclick="addUserList('{{=value.id}}')">{{=value.name}}</a>
                <span onclick="deleteUser('{{=value.id}}')" style="cursor:pointer;"><img src="../../Images/user_delete.png" style="width:16px;heigth:16px;"/></span>
                <input type="hidden" name="tousergroup" value="{{=value.id}}"/>
            </li>
        {{/each}}
        </ul>
    </script>
    <!--人员列表-->
    <div>
        <form id="actionform" class="form-horizontal ui-formwizard" method="post">
        <div class="con-title">
            <h1 class="con-title-h1">
                发件</h1>
            <div class="con-title-btns">
                <input type="button" id="Button3" name="search" class="title-btn" value="返回" onclick="backSendList()" />
            </div>
        </div>
        <div class="box">
            <div class="seach-box">
                <div class="seach-box-tit">
                    请选择收件人后发送</div>
                <div class="seach-box-con">
                    <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                        <tr>
                            <td class="right" style="width: 140px;">
                                收件人：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <div id="usergroup" style="width: 815; height: 100px; overflow-x: hidden; overflow-y: auto;">
                                </div>
                                <input type="button" id="Button1" name="search" class="title-btn" value="选择收件人" onclick="selectUser()">
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                                主题：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="text" id="Msg_Title" name="Msg_Title" class="inputText" maxlength="255"
                                    style="width: 450px;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                                公文类型：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <select name="Type">
                                    <option value="0">一般公文</option>
                                    <option value="1">自定义类</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <textarea style="width: 550px; height: 300px;" id="Msg_Content" name="Msg_Content"></textarea>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="sub-box" style="text-align: center;">
                <input type="button" class="sub-btn" value="发送" onclick="save();" />
            </div>
        </div>
        </form>
    </div>
</asp:Content>
