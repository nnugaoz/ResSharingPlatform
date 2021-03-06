﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    页面角色-编辑
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">
    <link href="../../Content/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.ztree.core-3.5.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ztree.excheck-3.5.js" type="text/javascript"></script>

    <%--JS初始化--%>
    <script type="text/javascript">
        var select_id = "";
        var zNodes;

        $(function () {
            get_url_para();
            //初始化取数据
            getInitList();
        });
    </script>

    <%--Ajax数据交互--%>
    <script type="text/javascript">
        // 初始化数据列表
        function getInitList() {
            $.get("../../Role/GetEdit?t=" + new Date().getTime(), { id: select_id }, function (obj) {
                try {
                    eval("ret=" + obj + ";");
                    if (ret[0].HaveData == "true") {
                        if (ret[0].HaveData == "true") {
                            var t = ret[1].Table;
                            $("#txt_title").val(t[0].Title);
                            $("#txt_remark").val(t[0].Remark);
                        }
                    }

                    if (ret[2].HaveData == "true") {
                        zNodes = ret[3].Table;
                        $.fn.zTree.init($("#tdTree"), setting, zNodes);
                        setCheck();
                    }
                } catch (e) {
                    layer.alert("获取数据出错", 10);
                    return false;
                }
            });
        }

        // 数据提交
        function ajax_save(txt_title, txt_remark, ids) {
            $.post("../../Role/SaveEdit?t=" + new Date().getTime(), { id: select_id, title: txt_title, remark: txt_remark, menus: ids }, function (obj) {
                try {
                    eval("ret=" + obj + ";");
                    if (ret[0].Save == "true") {
                        layer.msg("提交成功", 1, 10, function result() {
                            do_return();
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
        // 提交
        function do_save() {
            var txt_title = $("#txt_title").val();
            if (txt_title == "") {
                layer.alert("请填写角色名称", 10);
                return false;
            }

            var txt_remark = $("#txt_remark").val();

            var zTree = $.fn.zTree.getZTreeObj("tdTree");
            nodes = zTree.getCheckedNodes(true); // 选中的节点
            if (nodes.length == 0) {
                layer.alert("请选择页面权限", 10);
                return false;
            }

            var ids = "";
            for (var i = 0; i < nodes.length; i++) {
                ids += nodes[i].id + ",";
            }

            ajax_save(txt_title, txt_remark, ids);
        }

        // 返回
        function do_return() {
            location.href = "../Role/List?t=" + new Date().getTime();
        }
    </script>

    <%--树--%>
    <script type="text/javascript">
        var setting = {
            check: {
                enable: true
            },
            data: {
                simpleData: {
                    enable: true
                }
            }
        };

        function setCheck() {
            var zTree = $.fn.zTree.getZTreeObj("tdTree"),
			py = "p",
			sy = "s",
			pn = "p",
			sn = "s",
			type = { "Y": py + sy, "N": pn + sn };
            zTree.setting.check.chkboxType = type;
        }
    </script>

    <%--其他--%>
    <script type="text/javascript">
        // 获取Url传值
        function get_url_para() {
            var url = document.URL;
            var para = "";
            if (url.lastIndexOf("?") > 0) {
                para = url.substring(url.lastIndexOf("?") + 1, url.length);
                var arr = para.split("&");
                para = "";
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].split("=")[0] == "id") {
                        select_id = arr[i].split("=")[1];
                    }
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="graup">
            <input type="button" value="提交" onclick="do_save()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="返回" onclick="do_return()" class="btn btn-primary btn-sm" />
        </div>
        <table class="table table-bordered table-striped with-check">
            <thead>
                <tr>
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
                    <td>
                        <input id="txt_title" type="text" />
                    </td>
                    <td>
                        <input id="txt_remark" type="text" />
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="tree" style="width:99%;height:600px;overflow:hidden;margin:20px 0px 20px 0px;line-height:30px;">
            <ul id="tdTree" class="ztree" style="width: 99%; height: 250px; overflow-y: scroll;border:1px solid #000;">
            </ul>
        </div>
    </div>
</asp:Content>
