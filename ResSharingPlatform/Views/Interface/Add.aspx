<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    添加教师
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .listbar
    {
	    z-index:9999;width:100%;position:absolute;left:20px;top:360px;text-align:center;
    }
    </style>
    <script type="text/javascript">

     
 
        function save() {
            if ($.trim($("#Name").val()) == "") {
                layer.alert("请输入接口名称！", 5);
                return;
            }

            if ($.trim($("#Url").val()) == "") {
                layer.alert("请输入接口地址！", 5);
                return;
            }

            app.ajaxForm("../../Interface/Save", saveCallBack);
            
        }

        function saveCallBack(data) {
            if (data.result == "success") {
                layer.alert("保存成功", 1);
            } else {
                layer.alert("保存失败", 5);
            }
        }

        function reback() {
            window.location.href = "../../Interface/List";
        }

    </script>
 
    <div>
        <form id="actionform" class="form-horizontal ui-formwizard"method="post">
            <input type="hidden" value="<%:ViewData["id"] %>" name="id" />
            <div class="con-title">
                <h1 class="con-title-h1">编辑接口信息</h1>
            </div>
            <div class="box">
                <div class="seach-box">
                <div class="seach-box-tit">信息录入</div>
                <div class="seach-box-con">
                <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                    <tr>
                        <td class="right" style="width:140px;">接口名：</td>
                        <td><span class="red">*</span></td>
                        <td>
                             <input type="text" id="Name" name="Name" value="<%:ViewData["Name"] %>" class="inputText" placeholder="请输入接口名" />
                            <span style="color:Red;">*</span>
                        </td>
                    </tr>
                 
                    <tr>
                        <td class="right" style="width:140px;">姓名：</td>
                        <td><span class="red">*</span></td>
                        <td>
                           <input type="text" id="Url" name="Url" value="<%:ViewData["Url"] %>" style="width:600px;" class="inputText" placeholder="请输入接口地址"/>
                        </td>
                    </tr>
                   
                    
                    
                </table>
            </div>
            </div>

            <div class="sub-box" style="text-align:center;">
                <input type="button" class="sub-btn" value="保存" onclick="save();"/> 
                 <input type="button" class="sub-btn" value="返回" onclick="reback();"/> 
            </div>
            </div>
        </form> 
    </div>
</asp:Content>
