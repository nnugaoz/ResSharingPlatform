<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	资源平台--资源搜索
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            GetInitList();
        });

        function GetInitList() {
            $('#ActionForm').attr("action", "../../Mine/SearchList");

            $('#ActionForm').ajaxSubmit(
                function (data) {
                    $("#div_content").html(data);
                }
            );
        }
    </script>
   <div class="list main-box mt10">
        <h1>
            <%: Html.ActionLink("首页", "Index", "Home")%> > <a href="javascript:doSearch()" class="ml5 mr5">搜索</a>
        </h1>
        <div class="tabtitle mt10">
            排序 &nbsp;&nbsp;
            <a class="curalink" href="javascript:doSearch('5')"><font id = "5">最新上传</font></a>&nbsp;&nbsp;
            <a href="javascript:doSearch('1')"><font id = "1">评价次数</font></a>&nbsp;&nbsp; 
            <a href="javascript:doSearch('2')"><font id = "2">最受好评</font></a>&nbsp;&nbsp;
            <a href="javascript:doSearch('3')"><font id = "3">最多浏览</font></a>&nbsp;&nbsp; 
            <a href="javascript:doSearch('4')"><font id = "4">最多下载</font></a>&nbsp;&nbsp; 
        </div>

        <form id="ActionForm" action="" class="form-horizontal ui-formwizard" method="post">
            <input type="hidden" id="af_keyword" name="af_keyword" value="<%:ViewData["keyword"] %>"/>
            <input type="hidden" id="orderBy" name="orderBy" value="<%:ViewData["orderBy"] %>"/>
            <input type="hidden" id="fileType" name="fileType" value="<%:ViewData["radioValue"] %>" />
            <div id="div_content">
            </div>
        </form>
   </div>
</asp:Content>


