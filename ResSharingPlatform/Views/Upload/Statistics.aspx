<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    统计报表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <script type="text/javascript">
        $(function () {
            init();
        })

        function init() {
            $("#ActionForm").attr("action", "../../Upload/StatisticsList");

            $('#ActionForm').ajaxSubmit(
                function (data) {
                    try {
                        eval("ret=" + data + ";");
                        tableInit(ret);
                    } catch (e) {
                        MsgErr("获取数据出错！");
                    }
                }
            );
        }

        function tableInit(ret) {
            $("#sBody").empty();
            if (ret.HaveData == "true") {
                var item = ret.list;
                var html = "";
                for (var i = 0; i < item.length; i++) {
                    if (item[i].CREATE_NAME != null) {
                        html += ""
                            + "<tr>"
                            + "<td>" + item[i].CREATE_NAME + "</td>"
                            + "<td style='text-align:right'>" + item[i].allFile + "</td>"
                            + "<td style='text-align:right'>" + (item[i].PRAISE_PRE * 100) + "%</td>"
                            + "<td style='text-align:right'>" + item[i].VIEW_NUM + "</td>"
                            + "<td style='text-align:right'>" + item[i].DOWNLOAD_NUM + "</td>"
                            + "</tr>";
                    }
                }
                $("#sBody").append(html);
            } else {
                $("#sBody").append("<tr><td colspan=\"5\">暂无数据</td></tr>");
            }
        }

        function doSearch(orderBy) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                //排序方法
                $("#orderBy").val(orderBy);

                init();

                //链接样式
                for (var i = 1; i < 5; i++) {
                    if (i == orderBy) {
                        $("#" + orderBy).attr("color", "red");
                    } else {
                        $("#" + i).removeAttr("color");
                    }
                }
            } else {
                ExamLogin();
            }
        }

        function goChart() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.location.href = "../../Upload/Chart";
            } else {
                ExamLogin();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="ActionForm" action="" class="form-horizontal ui-formwizard" method="post"
    novalidate="novalidate" runat="server">
    <div>
        <input type="hidden" id="orderBy" name="orderBy" value="<%:ViewData["orderBy"] %>" />
        <div class="con-title">
            <h1 class="con-title-h1">
                统计报表</h1>
            <div class="con-title-btns">
                <input type="button" id="Button1" name="search" class="title-btn" value="折线图" onclick="goChart()" />
            </div>
        </div>
        <div class="box">
            <div>
                排序 &nbsp;&nbsp; <a href="javascript:doSearch('1')"><font id="1">上传最多</font></a>&nbsp;&nbsp;
                <a href="javascript:doSearch('2')"><font id="2">最受好评</font></a>&nbsp;&nbsp; <a href="javascript:doSearch('3')">
                    <font id="3">最多浏览</font></a>&nbsp;&nbsp; <a href="javascript:doSearch('4')"><font
                        id="4">最多下载</font></a>&nbsp;&nbsp;
                <br />
            </div>
            <div id="div_content">
                <table id="statisticsTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
                    <thead>
                        <tr>
                            <th width='20%'>
                                姓名
                            </th>
                            <th width='20%'>
                                上传资源总数
                            </th>
                            <th width='20%'>
                                好评率
                            </th>
                            <th width='20%'>
                                浏览量
                            </th>
                            <th width='20%'>
                                下载量
                            </th>
                        </tr>
                    </thead>
                    <tbody id="sBody">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
