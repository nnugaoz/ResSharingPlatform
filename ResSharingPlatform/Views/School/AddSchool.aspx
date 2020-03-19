<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    学校编辑
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">

    <%--JS初始化--%>
    <script type="text/javascript">
    var zNodes;

    $(function () {
        //初始化取数据
        GetSchoolInfo();
    });
    </script>

    <%--Ajax数据交互--%>
    <script type="text/javascript">
        // 初始化取数据
        function GetSchoolInfo() {
            $.ajax({
                type: 'post',
                url: "../../School/GetSchoolInfo",
                data: { SchoolId: $("#txt_RecordId").val() },
                dataType: 'json',
                success: function (data) {
                    if (data.result != "Nodata") {
                        var html = template('SchoolInfo', data);
                        $("#tbody").empty();
                        $("#tbody").append(html);
                    }
                }
            });
        }
    </script>

    <%--页面按钮响应事件--%>
    <script type="text/javascript">
        // 提交
        function do_save() {
            $(":text").each(function () {
                $(this).val($.trim($(this).val())); //文本框前后去空格
            });
            var txt_SchoolName = $("#txt_SchoolName").val();
            if (txt_SchoolName == "") {
                layer.alert("请填写学校名", 10);
                return false;
            }
            var txt_RecordId = $("#txt_RecordId").val();
            if ($("#txt_RecordId").val() == "") {
                ajax_add(txt_SchoolName);
            } else {
                ajax_edit(txt_RecordId, txt_SchoolName);
            }
        }
        function ajax_add(txt_SchoolName) {
            $.ajax({
                type: 'post',
                url: "../../School/DoAddSchool",
                data: { SchoolName: txt_SchoolName },
                async: false,
                dataType: 'json',
                success: function (data) {
                    if (data.result == "success") { 
                        layer.alert("提交成功", 2);
                        do_return();
                    }
                    else { 
                        layer.alert("系统出错", 10);
                    }
                }
            });
            return false;
        }
        function ajax_edit(txt_RecordId, txt_SchoolName) {
            $.ajax({
                type: 'post',
                url: "../../School/DoEditSchool",
                data: { RecordId: txt_RecordId, SchoolName: txt_SchoolName },
                async: false,
                dataType: 'json',
                success: function (data) {
                    if (data.result == "success") { 
                        layer.alert("修改成功", 9);
                        do_return();
                    }
                    else { 
                        layer.alert("系统出错", 2);
                    }
                }
            });
            return false;
        }

        // 返回
        function do_return() {
            location.href = "../School/List?t=" + new Date().getTime();
        }
    </script>
    <!--角色列表-->
    <script id="SchoolInfo" type="text/html">
    {{each list as value}}
        <tr>
            <td>
                <input id="txt_RecordId" type="hidden" value="{{=value.recordid}}"/>
                <input id="txt_SchoolName" type="text" value="{{=value.schoolname}}"/>
            </td>
        </tr>
    {{/each}}
    </script>
    <!--角色列表-->

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="graup">
            <input type="button" value="提交" onclick="do_save()" class="btn btn-primary btn-sm"/>
            &nbsp;&nbsp;
            <input type="button" value="返回" onclick="do_return()" class="btn btn-primary btn-sm"/>
        </div>
        <table class="table table-bordered table-striped with-check">
            <thead>
                <tr>
                    <th width="300px">
                        学校
                    </th>
                </tr>
            </thead>
            <tbody id="tbody">
                <tr>
                    <td><input id="txt_RecordId" type="hidden" value="<%=ViewData["SchoolId"] %>"/><input id="txt_SchoolName" type="text"/></td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
