<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%--Ajax数据交互--%>
<script type="text/javascript">
    GetInitList();
    // 初始化数据列表
    function GetInitList() {
        $.ajax({
            type: 'post',
            url: "../../School/GetSchoolList",
            data: { SchoolName: txt_SchoolName },
            dataType: 'json',
            success: function (data) {
                //alert(data.result);
                //alert(data.pagecurrent);
                //alert(data.linkpage);
                var html = template('SchoolList', data);
                $("#tbody").empty();
                $("#tbody").append(html);

                $("#currentPageNumber").val(data.pagecurrent);
                $(".yahoo").html(data.linkpage);
            }
        });
    }
</script>

<!--学校列表-->
<script id="SchoolList" type="text/html">
{{each list as value}}
    <tr>
        <td><input type="checkbox" name="checkitem" id="checkitem" value="{{=value.recordid}}" /></td>
        <td>{{=value.id0}}</td>
        <td>{{=value.schoolname}}</td>
        <td>{{=value.createtime}}</td>
        <td>
            <a style="text-decoration: none;" onclick="UpdateSchool('{{=value.recordid}}');" title="修改">修改</a>
            <a style="text-decoration: none;" onclick="DeleteSchool('{{=value.recordid}}','{{=value.schoolname}}');" title="删除">删除</a>
        </td>
    </tr>
{{/each}}
</script>
<!--学校列表-->

<table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
    <thead>
        <tr>
            <th width="30px">
                
            </th>
            <th width="50px">
                序号
            </th>
            <th width="300px">
                学校名称
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
        <tr>
            <td colspan="5" style="cursor:pointer;">
                暂无数据
            </td>
        </tr>
    </tbody>
</table>
<input id="currentPageNumber" type="hidden" value="<%:ViewData["pagecurrent"] %>"/>
<div style="text-align: right;" class="yahoo">
    <%
        string linkpage = ViewData["linkpage"] as string;
        Response.Write(linkpage);
    %>
</div>