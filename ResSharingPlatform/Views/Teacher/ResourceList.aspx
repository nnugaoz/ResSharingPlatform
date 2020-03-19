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
            app.ajaxForm("../../Teacher/SearchList", resourceCallBack);
        }

        function resourceCallBack(data) {
            var html = template('ResourceList', data);
            $("#div_content").html(html);
        }

        function preview(id, name) {
            var arr = name.split('.');
            app.redirect("../../Upload/ViewOnline?fileName=" + id + "." + arr[1]);
        }

    </script>
    <!--资源列表-->
    <script id="ResourceList" type="text/html">
<table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
    <thead>
        <tr>
            <th width="50px">
                序号
            </th>
            <th width="400px">
                资源名
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
            <a style="text-decoration: none;" onclick="preview('{{=value.id}}','{{=value.file_name}}')" title="资源详情">{{=value.file_name}}</a>
        </td>
        <td>{{=value.createtime}}</td>
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
                资源一览</h1>
            <div class="con-title-btns">
            </div>
        </div>
        <div class="box">
            <div class="seach-box">
                <div class="seach-box-tit">
                    检索</div>
                <div class="seach-box-con">
                    <table border="0" cellpadding="0" cellspacing="0" class="seach-tab">
                        <tr>
                            <td>
                                资源名称：
                            </td>
                            <td>
                                资源分类：
                            </td>
                            <td style="display: none;">
                                上传时间：
                            </td>
                            <td>
                                标签：
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <input type="text" id="resource" name="resource" class="inputText" style="width: 140px;"
                                    value="<%:ViewData["resource"] %>" />
                            </td>
                            <td style="width: 150px;">
                                <select class="easyui-combotree" name="type" id="type" style="width: 140px;" url="../../Upload/GetResType?type=combox">
                                </select>
                            </td>
                            <td style="width: 150px; display: none;">
                                <input id="uploadTime" name="uploadTime" type="text" size="20" class="inputText"
                                    style="width: 140px;" value="<%:ViewData["uploadTime"] %>" onclick="WdatePicker()" />
                            </td>
                            <td style="width: 150px;">
                                <input type="text" id="label" name="label" class="inputText" style="width: 140px;"
                                    value="<%:ViewData["label"] %>" />
                            </td>
                            <td>
                                <input type="button" id="searchBtn" name="searchBtn" class="seach-btn" value="查询"
                                    onclick="GetInitList()" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="div_content">
            </div>
        </div>
    </div>
    </form>
</asp:Content>
