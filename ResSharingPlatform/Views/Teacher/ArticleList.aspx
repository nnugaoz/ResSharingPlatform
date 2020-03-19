<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <script type="text/javascript">

        //初始化
        $(function () {

            // 提交表单、初始化、分页
            GetInitList();

        });

        // 提交表单、初始化、分页
        function GetInitList() {
            app.ajaxForm("../../Teacher/SearchArticle", acticleCallBack);
        }

        function acticleCallBack(data) {
            var html = template('ArticleList', data);
            $("#div_content").html(html);
        }

        function preview(id, name) {
            var arr = name.split('.');
            app.redirect("../../Upload/ViewOnline?fileName=" + id + "." + arr[1]);
        }

    </script>
    <!--资源列表-->
    <script id="ArticleList" type="text/html">
<table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
    <thead>
        <tr>
            <th width="50px">
                序号
            </th>
            <th width="400px">
                文章
            </th>
            <th>
                更新时间
            </th>
        </tr>
    </thead>
    <tbody id="tbody">
       {{each list as value}}
    <tr> 
        <td>{{=value.id0}}</td>
        <td>
            <a style="text-decoration: none;" target="_blank" href="{{value.protocol}}{{value.domain}}{{value.flag}}{{value.port}}{{value.context_path}}/{{value.channel_path}}/{{value.content_id}}{{value.dynamic_suffix}}" title="资源详情">{{=value.title}}</a>
        </td>
        <td>{{=value.sort_date}}</td>
    </tr>
{{/each}}
    </tbody>
</table> 
<div style="text-align: right;" class="yahoo">
   {{=linkpage}}
</div>

    </script>
    <!--资源列表-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    后台管理页面
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="actionform" action="" class="form-horizontal ui-formwizard" method="post"
    novalidate="novalidate" runat="server">
    <input type="hidden" id="userid" name="userid" value="<%:ViewData["userid"] %>" />
    <div>
        <div class="con-title">
            <h1 class="con-title-h1">
                文章一览</h1>
            <div class="con-title-btns">
            </div>
        </div>
        <div class="box">
            <div id="div_content">
            </div>
        </div>
    </div>
    </form>
</asp:Content>
