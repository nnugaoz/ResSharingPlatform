<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    资源平台-学校列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <style type="text/css">
        .tr_select
        {
            background: #0066ff;
        }
    </style>
    <%--JS初始化--%>
    <script type="text/javascript"> 
        $(function () { 
            //初始化取数据
            GetInitList();
        });
    </script>
    <%--Ajax数据交互--%>
    <script type="text/javascript"> 
        // 初始化数据列表
        function GetInitList() {
            app.ajaxForm("../../Interface/GetInterfaceList", resultCallBack);
        }

        function resultCallBack(data) {
            var html = template('interfaceList', data);
            $("#div_list").html(html);
        }
      
        // 删除数据
        function ajax_delete(id) {
            $.post("../../Interface/SaveDelete?t=" + new Date().getTime(), { id: id }, function (obj) {
                try {
                    eval("ret=" + obj + ";"); 
                    if (ret[0].Save == "true") {
                        layer.alert("删除成功", 1);
                        //初始化取数据
                        GetInitList();
                    } else {
                        layer.alert("删除失败", 10);
                        return false;
                    }
                } catch (e) {
                    layer.alert("删除失败", 10);
                    return false;
                }
            });
        }

        function add() {
            window.location.href = "../../Interface/Add";
        }
    </script>
    
     
    <!--接口列表-->
<script id="interfaceList" type="text/html">
<table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
    <thead>
        <tr>
           
            <th width="50px">
                序号
            </th>
            <th width="300px">
                接口名
            </th>
            <th>
                更新时间
            </th>
            <th>
                操作
            </th>
        </tr>
    </thead>
    <tbody id="tbody">
       {{each list as value}}
    <tr> 
        <td>{{=value.id0}}</td>
        <td>{{=value.name}}</td>
        <td>{{=value.createdate}}</td>
        <td>
            <a style="text-decoration: none;" href="../../Interface/Add?id={{=value.interface_id}}" title="修改">修改</a>
            <a style="text-decoration: none;" onclick="ajax_delete('{{=value.interface_id}}');" title="删除">删除</a>
        </td>
    </tr>
{{/each}}
    </tbody>
</table> 
<div style="text-align: right;" class="yahoo">
   {{=linkpage}}
</div>

</script>
<!--接口列表-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="actionform" action="" class="form-horizontal ui-formwizard" method="post">
      <div class="con-title">
            <h1 class="con-title-h1">
                接口一览</h1>
            <div class="con-title-btns"> 
                <input type="button" id="Button1" name="search" class="title-btn" value="添加" onclick="add()" />
                
            </div>
        </div>
    <div class="box">
        
        <div id="div_list"></div>

    </div>
    </form>
</asp:Content>
