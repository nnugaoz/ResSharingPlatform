<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	资源平台-页面角色列表
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
        var select_id = "";
        var txt_title = "";

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
                url: "../../Role/GetList",
                data: { title: txt_title },
                dataType: 'json',
                success: function (data) {
                    var html = template('RoleList', data);
                    $("#tbody").empty();
                    $("#tbody").append(html);
                }
            });
        }

        // 删除数据
        function ajax_delete() {
            $.post("../../Role/SaveDelete?t=" + new Date().getTime(), { id: select_id }, function (obj) {
                try {
                    eval("ret=" + obj + ";");
                    if (ret[0].Save == "true") {
                        layer.msg("提交成功", 1, 10, function result() {
                            location.href = "../Role/List?t=" + new Date().getTime();
                        });
                    } else {
                        layer.alert("提交失败", 10);
                        return false;
                    }
                } catch (e) {
                    layer.alert("提交失败", 10);
                    return false;
                }
            });
        }
    </script>
    <%--页面按钮响应事件--%>
    <script type="text/javascript">
        // 查询
        function do_search() {
            select_id = "";
            txt_title = $("#txt_title").val();
            //初始化取数据
            getInitList();
        }

        // 双击
        // 进入详细页面
        function do_detail(id) {
            select_id = id;
            location.href = "../Role/Detail?t=" + new Date().getTime() + "&id=" + id;
        }

        // 新增
        function do_add() {
            location.href = "../Role/Add?t=" + new Date().getTime();
        }

        // 编辑
        function do_edit() {
            if (select_id == "") {
                layer.alert("请选择数据", 10);
                return false;
            }
            location.href = "../Role/Edit?t=" + new Date().getTime() + "&id=" + select_id;
        }

        // 删除
        function do_delete() {
            if (select_id == "") {
                layer.alert("请选择数据", 10);
                return false;
            }

            // 删除确认
            $.layer({
                shade: [0],
                area: ['auto', 'auto'],
                dialog: {
                    msg: '确认删除该数据？',
                    btns: 2,
                    type: 4,
                    btn: ['确认', '取消'],
                    yes: function () {
                        ajax_delete();
                        return false;
                    }, no: function () {
                        layer.msg('删除取消', 1, 1);
                        return false;
                    }
                }
            });
        }
    </script>
    <%--其他--%>
    <script type="text/javascript">
        // 单击
        // 选中行
        function select_one(obj, id) {
            select_id = id;
            $(".tr_select").removeClass("tr_select");
            $(obj).addClass("tr_select");
        }
    </script>
    <!--角色列表-->
    <script id="RoleList" type="text/html">
    {{each list as value}}
        <tr onclick="select_one(this, '{{=value.id}}');" ondblclick="do_detail('{{=value.id}}');" style="cursor:pointer;">
            <td>{{=value.rowid}}</td>
            <td>{{=value.title}}</td>
            <td>{{=value.remark}}</td>
        </tr>
    {{/each}}
    </script>
    <!--角色列表-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="graup">
            角色名称 &nbsp;&nbsp;
            <input id="txt_title" type="text" />
            &nbsp;&nbsp;
            <input type="button" value="查询" onclick="do_search()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="新增" onclick="do_add()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="编辑" onclick="do_edit()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="删除" onclick="do_delete()" class="btn btn-primary btn-sm" />
        </div>
        <table class="table table-bordered with-check">
            <thead>
                <tr>
                    <th width="50px">
                        序号
                    </th>
                    <th width="300px">
                        角色名称
                    </th>
                    <th>
                        描述
                    </th>
                </tr>
            </thead>
            <tbody id="tbody">
                <tr>
                    <td colspan="3" style="cursor:pointer;">
                        暂无数据
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
