<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    页面角色-编辑
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">

    <%--JS初始化--%>
    <script type="text/javascript">
        var select_id = "";

        $(function () {
            //初始化取数据
            getInitList();
        });
    </script>

    <%--Ajax数据交互--%>
    <script type="text/javascript">
        // 初始化数据列表
        function getInitList() {
            $.ajax({
                type: 'post',
                url: "../../PointRule/GetRule",
                dataType: 'json',
                success: function (data) {
                    var html = template('PointRule', data);
                    $("#tbody").empty();
                    $("#tbody").append(html);
                }
            });
        }
        function ajax_edit(txt_PointRuleName, txt_Point, txt_RecordId) {
            $.ajax({
                type: 'post',
                url: "../../PointRule/EditRule",
                data: { RecordId: txt_RecordId, PointRuleName: txt_PointRuleName, Point: txt_Point },
                async: false,
                dataType: 'json',
                success: function (data) {
                    if (data.result == "success") {
                        alert("修改成功");
                        window.location.reload();
                    }
                    else {
                        alert("系统出错");
                    }
                }
            });
            return false;
        }
        function ajax_add(txt_PointRuleName, txt_Point) {
            $.ajax({
                type: 'post',
                url: "../../PointRule/AddRule",
                data: { PointRuleName: txt_PointRuleName, Point: txt_Point },
                async: false,
                dataType: 'json',
                success: function (data) {
                    if (data.result == "success") {
                        alert("增加成功");
                        window.location.reload();
                    }
                    else {
                        alert("系统出错");
                    }
                }
            });
            return false;
        }
    </script>

    <%--页面按钮响应事件--%>
    <script type="text/javascript">
        // 提交
        function do_save() {
            $(":text").each(function () {
                $(this).val($.trim($(this).val())); //文本框前后去空格
            });
            var txt_PointRuleName = $("#txt_PointRuleName").val();
            if (txt_PointRuleName == "") {
                layer.alert("请填写积分规则", 10);
                return false;
            }
            var txt_Point = $("#txt_Point").val();
            if (txt_Point == "") {
                layer.alert("请填写积分", 10);
                return false;
            }
            var z = /^[0-9]*$/;
            if (!z.test($("#txt_Point").attr("value"))) {
                layer.alert("积分只能输入数字!");
                return false;
            }
            var txt_RecordId = $("#txt_RecordId").val();
            if (txt_RecordId == "") {
                //insert
                ajax_add(txt_PointRuleName, txt_Point);
            } else {
                //update
                ajax_edit(txt_PointRuleName, txt_Point, txt_RecordId);
            }
        }
    </script>
    <!--积分规则-->
    <script id="PointRule" type="text/html">
    {{each list as value}}
        <tr onclick="select_one(this, '{{=value.id}}');" ondblclick="do_detail('{{=value.id}}');" style="cursor:pointer;">
            <td>
                <input id="txt_RecordId" type="hidden" value="{{=value.recordid}}"/>
                <input id="txt_PointRuleName" type="text" value="{{=value.pointrulename}}" maxlength="200"/>
            </td>
            <td>
                <input id="txt_Point" type="text" value="{{=value.point}}" maxlength = "8"/>
            </td>
        </tr>
    {{/each}}
    </script>
    <!--积分规则-->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="graup">
            <input type="button" value="提交" onclick="do_save()" class="btn btn-primary btn-sm" />
        </div>
        <table class="table table-bordered table-striped with-check">
            <thead>
                <tr>
                    <th width="300px">
                        积分规则
                    </th>
                    <th>
                        积分
                    </th>
                </tr>
            </thead>
            <tbody id="tbody">
                <tr>
                    <td>
                        <input id="txt_RecordId" type="hidden" value=""/>
                        <input id="txt_PointRuleName" type="text" value="" maxlength="200"/>
                    </td>
                    <td>
                        <input id="txt_Point" type="text" value="" maxlength = "8"/>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
