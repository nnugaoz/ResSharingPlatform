<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    添加教师
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
                }
            });
        }

        function save() {
            if ($.trim($("#Login_Name").val()) == "") {
                layer.alert("请输入教师编号！", 5);
                return;
            }

            if ($.trim($("#Password").val()) == "") {
                layer.alert("请输入密码！", 5);
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

            if ($.trim($("#DataRole").val()) == "") {
                layer.alert("请选择数据权限！", 5);
                return;
            }

            checkUser();
        }

        //检测重复用户名
        function checkUser() {

            $.ajax({
                type: 'post',
                url: "../../Teacher/checkUser",
                data: { username: $.trim($("#Login_Name").val()) },
                dataType: 'json',
                success: function (data) {
                    if (data.result == true) {
                        layer.alert("该教师编号已经存在！", 5);
                        return;
                    } else {
                        saveUser()
                    }
                }
            });
        }

        //保存用户
        function saveUser() {
            $("#ActionForm").ajaxSubmit({
                type: "post",  //提交方式  
                dataType: "json", //数据类型  
                url: "../../Teacher/Save", //请求url  
                success: function (data) { //提交成功的回调函数  
                    if (data.result == "success") {
                        layer.alert("保存成功！", 1);
                    } else {
                        layer.alert("保存失败！", 5);
                    }
                }
            });
            return false; //阻止表单自动提交事件   
        }

        //添加xls源数据
        function addXls() {

            if ($.trim($("#schoolId").val()) == "") {
                layer.alert("请选择学校！", 5);
                return;
            }

            if ($.trim($("#roleId").val()) == "") {
                layer.alert("请选择角色！", 5);
                return;
            }

            if ($.trim($("#DataRole").val()) == "") {
                layer.alert("请选择数据权限！", 5);
                return;
            }

            if ($.trim($("#filepath").val()) == "") {
                layer.alert("请先选择要导入的xls文件！", 5);
                return;
            }
            $(document.body).append("<div class='listbar'><img src='../images/loading-0.gif' /></div>");

            $("#ActionForm").ajaxSubmit({
                type: "post",  //提交方式  
                dataType: "json", //数据类型  
                url: "../../Teacher/addXls", //请求url  
                success: function (data) { //提交成功的回调函数  
                    if (data.result == "success") {
                        layer.alert("导入成功！", 1);
                    } else {
                        layer.alert("导入失败！", 5);
                    }
                    $(".listbar").remove();
                }
            });
            return false; //阻止表单自动提交事件   
        }

        function selectFile() {

            //iframe层-禁滚动条 
            $.layer({
                type: 2,
                title: ["选择文件", true], //不显示默认标题栏
                //shade: [1], //显示遮罩
                area: ["400px", "300px"],
                iframe: { src: "../../FileUpload/File" }
            });
        }

        function getFile(filename, filepath) {
            $("#filepath").val(filepath);
            $("#filename").html(filename);
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
        <form id="ActionForm" class="form-horizontal ui-formwizard" action="../../Teacher/Save"
        method="post">
        <div class="con-title">
            <h1 class="con-title-h1">
                添加教师信息</h1>
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
                                <input type="text" id="Login_Name" name="Login_Name" class="inputText" placeholder="请输入教师编号" />
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
                                <input type="password" id="Password" name="Password" class="inputText" placeholder="请输入密码"
                                    value="000000" />
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
                                <input type="text" id="Name" name="Name" class="inputText" placeholder="请输入教师姓名" />
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
                            <td class="right">
                                数据权限：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <select style="width: 200px;" name="DataRole" id="DataRole">
                                    <option value="">请选择数据权限</option>
                                    <option value="0">全部数据</option>
                                    <option value="1">授权数据</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">
                                通过xls导入：
                            </td>
                            <td>
                            </td>
                            <td>
                                <input type="hidden" id="filepath" name="filepath" />
                                <input type="button" id="Button1" name="search" class="title-btn" value="选择文件" onclick="selectFile()">
                                <input type="button" id="Button2" name="search" class="title-btn" value="开始上传" onclick="addXls()"><span
                                    style="color: Red;">*(手动输入和xls导入只可选择其一)</span> <span id="filename"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">
                                模板下载：
                            </td>
                            <td>
                                <span class="red"></span>
                            </td>
                            <td>
                                <a href="../../xls/teacherlist.xls">批量导入教师信息模板</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="sub-box" style="text-align: center;">
                <input type="button" class="sub-btn" value="保存" onclick="save();" />
            </div>
        </div>
        </form>
    </div>
</asp:Content>
