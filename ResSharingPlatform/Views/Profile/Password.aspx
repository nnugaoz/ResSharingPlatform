<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    作业互动平台-密码修改
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">


        $(function () {

        });

        function save() {

            $(":text").each(function () {
                $(this).val($.trim($(this).val())); //文本框前后去空格
            });

            if ($("#Password").val() == "") {
                layer.alert("请输入密码！", 5);
                return;
            }
            if ($("#Password1").val() == "") {
                layer.alert("请输入密码！", 5);
                return;
            }
            if ($("#Password").val() != $("#Password1").val()) {
                layer.alert("两次输入的密码不一样！", 5);
                return;
            }

            SavePassword();
        }

        //保存密码
        function SavePassword() {
            $("#ActionForm").ajaxSubmit({
                type: "post",  //提交方式  
                dataType: "json", //数据类型  
                url: "../../Profile/SavePassword", //请求url  
                success: function (data) { //提交成功的回调函数  
                    if (data.result == "success") {
                        layer.alert("保存成功！", 5);
                    } else {
                        layer.alert("保存失败！", 5);
                    }
                }
            });
            return false; //阻止表单自动提交事件   
        }

    </script>

    <div>
        <form id="ActionForm" class="form-horizontal ui-formwizard" action=""
        method="post">
        <div class="con-title">
            <h1 class="con-title-h1">
                密码修改</h1>
        </div>
        <div class="box">
            <div class="seach-box">
                <div class="seach-box-tit">
                    信息录入</div>
                <div class="seach-box-con">
                    <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                        <tr>
                            <td class="right" style="width: 140px;">
                                编号：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="text" id="Login_Name" name="Login_Name" readonly class="inputText" value="<%:ViewData["UserId"] %>" placeholder="请输入编号" />
                                <span style="color: Red;">*(登录用)</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                                密码：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="password" id="Password" name="Password" class="inputText" placeholder="请输入密码"
                                    value="" />
                                <span style="color: Red;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right" style="width: 140px;">
                                确认密码：
                            </td>
                            <td>
                                <span class="red">*</span>
                            </td>
                            <td>
                                <input type="password" id="Password1" name="Password1" class="inputText" placeholder="请输入密码"
                                    value="" />
                                <span style="color: Red;"></span>
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
