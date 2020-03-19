<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <script language="javascript" type="text/javascript" src="../../Scripts/jquery.form.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">

        //初始化
        $(function () {
       
            var condition = null;//<%=Session["condition"] %>;

            if (condition != null) {
                $("#resource").val(condition["resource"]);
                $("#type").combotree("setValue", condition["type"]);
                $("#uploadTime").val(condition["uploadTime"]);
                $("#label").val(condition["label"]);
                $("#orderBy").val(condition["orderBy"]);
                $("#origin").val(condition["origin"]);
                $("#originSelect").val(condition["origin"]);
                $("#pagecurrent2").val(condition["pagecurrent"]);

                //链接样式
                for (var i = 1; i < 6; i++) {
                    if (i == condition["orderBy"]) {
                        $("#" + condition["orderBy"]).attr("color", "red");
                    } else {
                        $("#" + i).removeAttr("color");
                    }
                }
            }
            
            //未删除（0）
            $("#delFlg").val("0");
            // 提交表单、初始化、分页
            GetInitList();

            var originHid = $("#origin").val();
            $("#originSelect option[value='" + originHid + "']").attr("selected", true);
        });

        //检索 排序
        function doSearch(orderBy) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                //排序方法
                $("#orderBy").val(orderBy);
                //默认第一页
                $("#pagecurrent").val("");

                GetInitList();

                //链接样式
                for (var i = 1; i < 6; i++) {
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

        //上传
        function goUpload(id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                $("#resId").val(id);
                $("#ActionForm").attr("action", "../../Upload/Upload");
                $("#ActionForm").submit();
            } else {
                ExamLogin();
            }
        }

        // 提交表单、初始化、分页
        function GetInitList() {
       
            $('#ActionForm').attr("action", "../../Upload/SearchList");

            $('#ActionForm').ajaxSubmit(
                function (data) {
                    $("#div_content").html(data);
                }
            );

            return false;
        }

        //删除资源
        function deleteRes(id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var lyr = layer.confirm("您确定要删除吗?", function () {
                    layer.close(lyr);
                    $("#pagecurrent2").val($("#currentPageNumber").val());
                    $('#ActionForm').attr("action", "../../Upload/Delete");
                    $('#ActionForm').ajaxSubmit({
                        data: { id: id },
                        success: function (data) {
                            layer.alert("删除成功！", 9);
                            $("#div_content").html(data);
                        }
                    }
                    );
                });
                return false;
            } else {
                ExamLogin();
            }
        }

        //来源 下拉框事件
        function doOriginChange() {
            var sel = $('#originSelect').val();
            $("#origin").val(sel);
        }

        function selectAll(obj) {
            $("[name='tableCheck']").each(function () {
                if ($(obj).attr("checked") == "checked") {
                    $(this).attr("checked", "checked");
                    $(this).next().val("1");
                } else {
                    $(this).removeAttr("checked");
                    $(this).next().val("0");
                }
            });
        }

        function selectOne(obj) {
            if ($(obj).attr("checked") == "checked") {
                $(obj).next().val("1");
            } else {
                $(obj).next().val("0");
            }
        }

        function check(type) {
            var checked = true;
            var i = 0;
            $("[name='tableCheck']").each(function () {
                if ($(this).attr("checked") == "checked") {
                    i++;
                }
            });

            if (i == 0) {
                layer.alert("请选择资源！", 5);
                checked = false;
            }

            if (type == "examine") {
                $("[name='tableCheck']").each(function (index) {
                    if ($(this).attr("checked") == "checked") {
                        if ($(this).next().next().val() == null || $(this).next().next().val() == "") {
                            layer.alert("请设置第" + (index + 1) + "行资源分类！", 5);
                            checked = false;
                            return false;
                        }
                    }
                });
            }
            return checked;
        }

        //批量审核
        function batchExamine() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                if (!check("examine")) {
                    return;
                }
                examineHtml();
            } else {
                ExamLogin();
            }
        }

        function examineHtml() {
            var html = "<div style='width:300px;'><table style='width:100%'><tr>"
                     + "<td style='width:80px;text-align:right;'>审核：</td><td><span class='red'>*</span></td>"
                     + "<td>"
                     + "<input type='radio' name='status' id='exaStatus1' checked='checked' value='1' />通过&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                     + "<input type='radio' name='status' id='exaStatus2' value='2' />不通过"
                     + "</td></tr><tr><td style='width:80px;text-align:right;'>精品：</td><td><span class='red'>*</span></td><td>"
                     + "<input type='radio' name='excellentFlg' id='Radio1' checked='checked' value='0' />普通&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                     + "<input type='radio' name='excellentFlg' id='Radio2' value='1' />精品"
                     + "</td></tr></table>"
                     + "<div style='text-align:center;height:40px;padding-top:5px;'><input type='button' class='btn btn-primary btn-sm' value='审核' id='btnExamine'/></div></div>";

            $.layer({
                type: 1,
                title: [
                                '批量审核',
                                'background:#2B2E37; height:40px; color:#fff; border:none;'
                                ],
                border: [0],
                area: ['auto', 'auto'],
                page: {
                    html: html
                }
            });
            $("#btnExamine").click(function () {
                $("[name='status']").each(function () {
                    if ($(this).attr("checked") == "checked") {
                        $("#hidStatus").val($(this).val());
                    }
                });

                $("[name='excellentFlg']").each(function () {
                    if ($(this).attr("checked") == "checked") {
                        $("#hidExcellentFlg").val($(this).val());
                    }
                });

                $("#ActionForm").attr("action", "../../Upload/BatchExamine");
                $('#ActionForm').submit();
            });
            return html;
        }

        //批量删除
        function batchDelete() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var lyr = layer.confirm("您确定要删除吗?", function () {
                    layer.close(lyr);
                    if (!check("delete")) {
                        return;
                    }
                    $("#pagecurrent2").val($("#currentPageNumber").val());
                    $("#ActionForm").attr("action", "../../Upload/BatchDelete");
                    $('#ActionForm').ajaxSubmit({
                        success: function (data) {
                            layer.alert("删除成功！", 9);
                            $("#div_content").html(data);
                        }
                    }
                    );
                });
            } else {
                ExamLogin();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    后台管理页面
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="ActionForm" action="" class="form-horizontal ui-formwizard" method="post"
    novalidate="novalidate" runat="server">
    <input type="hidden" id="resId" name="resId" value="" />
    <input type="hidden" id="orderBy" name="orderBy" value="<%:ViewData["orderBy"] %>" />
    <input type="hidden" id="delFlg" name="delFlg" value="<%:ViewData["delFlg"] %>" />
    <input type="hidden" id="origin" name="origin" value="<%:ViewData["origin"] %>" />
    <input type="hidden" id="AddAthority" name="AddAthority" value="<%:ViewData["AddAthority"] %>" />
    <input type="hidden" id="EditAthority" name="EditAthority" value="<%:ViewData["EditAthority"] %>" />
    <input type="hidden" id="DelAthority" name="DelAthority" value="<%:ViewData["DelAthority"] %>" />
    <input type="hidden" id="ExamineAthority" name="ExamineAthority" value="<%:ViewData["ExamineAthority"] %>" />
    <input type="hidden" id="topage" name="topage" value="<%:ViewData["topage"] %>" />
    <input id="pagecurrent2" type="hidden" name="pagecurrent2" />
    <input type="hidden" id="hidExcellentFlg" name="ExcellentFlg" />
    <input type="hidden" id="hidStatus" name="Status" />
    <div>
        <div class="con-title">
            <h1 class="con-title-h1">
                资源一览</h1>
            <div class="con-title-btns">
                <%if ("1".Equals(ViewData["AddAthority"]))
                  { %>
                <input type="button" id="Button1" name="search" class="title-btn" value="上传" onclick="goUpload()" />
                <%} %>
                <%if (ViewData["topage"] != null && (ViewData["topage"].ToString() == "pending" || ViewData["topage"].ToString() == "unqualified"))
                  {
                      if (ViewData["topage"].ToString() == "pending")
                      {
                %>
                <%if ("1".Equals(ViewData["ExamineAthority"]))
                  { %>
                <input type="button" id="Button2" name="search" class="title-btn" value="批量审核" onclick="batchExamine()" />
                <%} %>
                <%} %>
                <%if ("1".Equals(ViewData["DelAthority"]))
                  { %>
                <input type="button" id="Button3" name="search" class="title-btn" value="批量删除" onclick="batchDelete()" />
                <%} %>
                <%} %>
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
                            <td>
                                上传时间：
                            </td>
                            <td>
                                标签：
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
                            <td style="width: 150px;">
                                <input id="uploadTime" name="uploadTime" type="text" size="20" class="inputText"
                                    style="width: 140px;" value="<%:ViewData["uploadTime"] %>" onclick="WdatePicker()" />
                            </td>
                            <td style="width: 150px;">
                                <input type="text" id="label" name="label" class="inputText" style="width: 140px;"
                                    value="<%:ViewData["label"] %>" />
                            </td>
                           
                            <td>
                                <input type="button" id="searchBtn" name="searchBtn" class="seach-btn" value="查询"
                                    onclick="doSearch()" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div>
                <%if (ViewData["topage"] == null || ViewData["topage"].ToString() == "" || ViewData["topage"].ToString() == "checked")
                  { %>
                排序 &nbsp;&nbsp; <a href="javascript:doSearch('1')"><font id="1">评价次数</font></a>&nbsp;&nbsp;
                <a href="javascript:doSearch('2')"><font id="2">最受好评</font></a>&nbsp;&nbsp; <a href="javascript:doSearch('3')">
                    <font id="3">最多浏览</font></a>&nbsp;&nbsp; <a href="javascript:doSearch('4')"><font
                        id="4">最多下载</font></a>&nbsp;&nbsp; <a href="javascript:doSearch('5')"><font id="5">最新上传</font></a>&nbsp;&nbsp;
                <br />
                <%} %>
                <br />
            </div>
            <div id="div_content">
            </div>
        </div>
    </div>
    </form>
</asp:Content>
