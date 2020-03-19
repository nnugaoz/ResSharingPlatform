<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#menu4").children().addClass("cur");
            $("#orderBy").val("5");
            $("#userId").val("002");
            GetInitList();

            //模糊条件回车
            $('#like').bind('keypress', function (event) {
                if (event.keyCode == "13") {
                    btnSearch_Click();
                    return false;
                }
            });
        });

        // 提交表单、初始化、分页
        function GetInitList() {
            $('#ActionForm').attr("action", "../../Mine/MyResList");

            $('#ActionForm').ajaxSubmit(
                function (data) {
                    $("#div_content").html(data);
                }
            );

            return false;
        }

        function btnSearch_Click() {
            $("#pagecurrent").val(1);
            GetInitList();
        }
	</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	资源平台-我的文库
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <form id="ActionForm" action="" method="post" style="height:0px;">
    <input type="hidden" id="pagecurrent" name="pagecurrent" value="<%:ViewData["pagecurrent"] %>"/>
    <input type="hidden" id="orderBy" name="orderBy" value="<%:ViewData["orderBy"] %>"/>
    <input type="hidden" id="userId" name="userId" value="<%:ViewData["userId"] %>"/>

    <div class="list main-box mt10" style="margin-top:0px;">
        <h1>
            <%: Html.ActionLink("首页", "Index", "Home")%> > <a href="javascript:doSearch()" class="ml5 mr5">我的文库</a>
        </h1>
        <div class="tabtitle mt10" style="font-size:14px;margin-top:0px;">
            排序 &nbsp;&nbsp;
            <a class="curalink" href="javascript:doSearch('5')"><font id = "5">最新上传</font></a>&nbsp;&nbsp;
            <a href="javascript:doSearch('1')"><font id = "1">评价次数</font></a>&nbsp;&nbsp; 
            <a href="javascript:doSearch('2')"><font id = "2">最受好评</font></a>&nbsp;&nbsp;
            <a href="javascript:doSearch('3')"><font id = "3">最多浏览</font></a>&nbsp;&nbsp; 
            <a href="javascript:doSearch('4')"><font id = "4">最多下载</font></a>&nbsp;&nbsp;
            <input id="like" name="like" type="text" style="height:24px; line-height:20px; border:1px solid #cdcdcd; width:200px;margin-left:100px;" />
            <input type="button" id="btnSearch" class="btnSeach" value="搜索" onclick="btnSearch_Click()" /> 
        </div>
        <div id="div_content" style="padding-top:5px;">
        </div>
    </div>
    </form>
</asp:Content>
