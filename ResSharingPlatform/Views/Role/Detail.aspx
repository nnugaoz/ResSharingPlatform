<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    详情
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
            $.get("../../Role/GetDetail?t=" + new Date().getTime(), { id: select_id }, function (obj) {
                try {
                    eval("ret=" + obj + ";");
                    $("#tbody").empty();
                    if (ret[0].HaveData == "true") {
                        var html = "";
                        var t = ret[1].Table;
                        for (var i = 0; i < t.length; i++) {
                            html = ""
                                + "<tr>"
                                + "<td>" + t[i].Title + "</td>"
                                + "<td>" + t[i].Remark + "</td>"
                                + "</tr>"
                            $("#tbody").append(html);
                        }
                    } else {
                        $("#tbody").append("<td colspan=\"2\">暂无数据</td>");
                    }

                    if (ret[2].HaveData == "true") {
                        zNodes = ret[3].Table;
                        $.fn.zTree.init($("#tdTree"), setting, zNodes);
                        setCheck();
                    }
                } catch (e) {
                    alert("获取数据出错！");
                    return false;
                }
            });
        }
    </script>

    <%--页面按钮响应事件--%>
    <script type="text/javascript">
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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box">
        <div class="graup">
            <input type="button" value="返回" onclick="do_return()" class="btn btn-primary btn-sm"/>
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
                    <td colspan="2">暂无数据</td>
                </tr>
            </tbody>
        </table>
        <div id="tree" style="width:99%;margin:20px 0px 20px 0px;line-height:30px;">
            <ul id="tdTree" class="ztree" style="width: 99%; height: 250px; overflow-y: scroll;border:1px solid #000;">
            </ul>
        </div>
    </div>
</asp:Content>
