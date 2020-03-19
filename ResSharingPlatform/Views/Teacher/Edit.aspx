<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑教师信息
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
    </style>
    <script type="text/javascript">


        $(function () {
            getRoleList();
            getSchoolList()
            $("#DataRole").val($("#userdatarole").val());
        });

        // 初始化角色数据列表
        function getRoleList() {
            $.ajax({
                type: 'post',
                url: "../../Role/GetList",
                data: { title: "" },
                dataType: 'json',
                success: function (data) {
                    var html = template('roleList', data);
                    $("#roleId").append(html);
                    $("#roleId").val($("#role").val());
                }
            });
        }

        // 初始化学校数据列表
        function getSchoolList() {
            $.ajax({
                type: 'post',
                url: "../../Teacher/GetSchoolList",
                dataType: 'json',
                success: function (data) {
                    var html = template('schoolList', data);
                    $("#schoolId").append(html);
                    $("#schoolId").val($("#school").val());
                }
            });
        }

        function back() {
            app.redirect("../../Teacher/List");
        }

        function save() {
            if ($.trim($("#Login_Name").val()) == "") {
                layer.alert("请输入教师编号！", 5);
                return;
            }
            if ($.trim($("#Name").val()) == "") {
                layer.alert("请输入教师姓名！", 5);
                return;
            }
            if ($.trim($("#schoolId").val()) == "") {
                layer.alert("请选择学校！", 5);
                return;
            }
            if ($.trim($("#roleId").val()) == "") {
                layer.alert("请选择角色！", 5);
                return;
            }
            saveUser();
        }

        //保存用户
        function saveUser() {
            app.ajaxForm("../../Teacher/EditSave", saveResultCallBack);
        }

        function saveResultCallBack(data) {
            if (data.result == "success") {
                layer.alert("保存成功！", 1, back());
                //back();
            } else {
                layer.alert("保存失败！", 5);
            }
        }

    </script>
    <!--学校列表-->
    <script id="schoolList" type="text/html">
    {{each list as value}}
        <option value="{{=value.recordid}}">{{=value.schoolname}}</option>
    {{/each}}
    </script>
    <!--学校列表-->
    <!--角色列表-->
    <script id="roleList" type="text/html">
    {{each list as value}}
        <option value="{{=value.id}}">{{=value.title}}</option>
    {{/each}}
    </script>
    <!--角色列表-->
    <div>
        <form id="actionform" class="form-horizontal ui-formwizard" method="post">
        <input type="hidden" id="userid" name="userid" value="<%:ViewData["userid"] %>" />
        <input type="hidden" id="role" name="role" value="<%:ViewData["PageRole_ID"] %>" />
        <input type="hidden" id="school" name="school" value="<%:ViewData["SchoolId"] %>" />
        <input type="hidden" id="userdatarole" name="userdatarole" value="<%:ViewData["DataRole"] %>" />

        <div class="con-title">
            <h1 class="con-title-h1">
                编辑教师信息</h1>
        </div>
        <div class="box">
            <div class="seach-box">
                <div class="seach-box-tit">
                    信息录入</div>
                <div class="seach-box-con">
                    <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                        <tr>
                            <td class="right" style="width: 140px;">
                                教师编号：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="text" id="Login_Name" name="Login_Name" readonly value="<%:ViewData["userid"] %>"
                                    class="inputText" placeholder="请输入教师编号" />
                                <span style="color: Red;">*(登录用)</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                                教师密码：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="password" id="Password" name="Password" class="inputText" placeholder="请输入密码" />
                                <span style="color: Red;">*(初始密码为000000)</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                                姓名：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="text" id="Name" name="Name" value="<%:ViewData["Name"] %>" class="inputText"
                                    placeholder="请输入教师姓名" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">
                                所属学校：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <select style="width: 200px;" name="schoolId" id="schoolId">
                                    <option value="">请选择学校</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">
                                角色：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <select style="width: 200px;" name="roleId" id="roleId">
                                    <option value="">请选择角色</option>
                                </select>
                            </td>
                        </tr>
                         <tr>
                        <td class="right">数据权限：</td>
                        <td><span class="red">*</span></td>
                        <td>
                            <select style="width:200px;"  name="DataRole" id="DataRole">
                                <option value="">请选择数据权限</option>
                                <option value="0">全部数据</option>
                                <option value="1">授权数据</option>
                            </select>
                            
                        </td>
                    </tr>
                    </table>
                </div>
            </div>
            <div class="sub-box" style="text-align: center;">
                <input type="button" class="sub-btn" value="保存" onclick="save();" />
                <input type="button" class="sub-btn" value="返回" onclick="back();" />
            </div>
        </div>
        </form>
    </div>
</asp:Content>
