<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    后台管理页面
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            GetScoreList();
            GetQaList();
        });

        // 提交表单、初始化、分页
        function GetScoreList() {
            $('#Sco_ActionForm').attr("action", "../../Upload/ScoreList");

            $('#Sco_ActionForm').ajaxSubmit(
                function (data) {
                    $("#sco_content").html(data);
                }
            );
            reFresh();
            setTimeout(GetScoreList, 10000);
        }

        // 提交表单、初始化、分页
        function GetQaList() {
            $('#Qa_ActionForm').attr("action", "../../Upload/QaList");

            $('#Qa_ActionForm').ajaxSubmit(
                function (data) {
                    $("#qa_content").html(data);
                }
            );
            setTimeout(GetQaList, 10000);
        }

        function reFresh() {
            $.ajax({
                type: "post",
                url: "../../Upload/Refresh",
                data: "resId=" + $("#resId").val(),
                dataType: "json",
                async: false,
                success: function (data) {
                    $('#grade').text(data["grade"]);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layer.alert(textStatus);
                    layer.alert(errorThrown);
                }
            });
        }

        function goBack() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.location.href = "../../Upload/Index?topage=" + $("#topage").val();
            } else {
                ExamLogin();
            }
        }

        function viewOnline(fileName) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.open("../../Upload/ViewOnline?fileName=" + fileName);
            } else {
                ExamLogin();
            }
        }

        function openPop(e, id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
                var y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
                $("#xingZQYTree").attr("style", "display:display;width:auto;z-index:9999");
                $("#xingZQYTree").attr("style", "top:" + y + "px;left:" + x + "px");

                $("#qaId").val(id);
            } else {
                ExamLogin();
            }
        }

        function closePop() {
            $("#xingZQYTree").attr("style", "display:none");
            $("#popAnswer").val("");
            $("#qaId").val("");
        }

        function answerQuest() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var id = $("#qaId").val();
                $.ajax({
                    type: "post",
                    url: "../../Upload/SaveAnswer",
                    data: "qaId=" + id + "&answer=" + $('#popAnswer').val(),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data != null && data != "") {
                            closePop();
                            GetQaList();
                        } else {
                            layer.alert("回复失败！", 8);
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layer.alert(textStatus);
                        layer.alert(errorThrown);
                    }
                });
            } else {
                ExamLogin();
            }
        }

        /**打开对比页面**/
        function ContrastFile(Aid) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.open("../../Upload/ContrastFile?Aid=" + Aid);
            }
            else {
                ExamLogin();
            }
        }

        function deleteScore(id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var lyr = layer.confirm("您确定要删除吗?", function () {
                    layer.close(lyr);
                    $.ajax({
                        type: "post",
                        url: "../../Upload/deleteScore",
                        data: "id=" + id,
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data) {
                                GetScoreList();
                            } else {
                                layer.alert("删除失败！", 8);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layer.alert(textStatus);
                            layer.alert(errorThrown);
                        }
                    });
                });
            } else {
                ExamLogin();
            }
        }

        function deleteQuestion(id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var lyr = layer.confirm("您确定要删除吗?", function () {
                    layer.close(lyr);
                    $.ajax({
                        type: "post",
                        url: "../../Upload/deleteQuestion",
                        data: "id=" + id,
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            if (data) {
                                GetQaList();
                            } else {
                                layer.alert("删除失败！", 8);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layer.alert(textStatus);
                            layer.alert(errorThrown);
                        }
                    });
                });
            } else {
                ExamLogin();
            }
        }
    </script>
    <div>
        <div class="con-title">
            <h1 class="con-title-h1">资源详情</h1>
        </div>
        <div class="box">
            <input type="hidden" id="topage" name="topage" value="<%:ViewData["topage"] %>" />
            <div class="seach-box">
                <div class="seach-box-tit">信息详情</div>
                <div class="seach-box-con">
                <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                <tr>
                    <td class="right" style="width:120px;">资源名称：</td>
                    <td>
                        <%if (ViewData["NAME"] != null)
                          {%>
                            <%:ViewData["NAME"]%>
                            <%}
                          else if ("2".Equals(ViewData["ORIGIN_FLG"]))
                          {%>
                            来自一体化课程
                            <%}
                          else if ("3".Equals(ViewData["ORIGIN_FLG"]))
                          {%>
                            来自精品课程
                            <%}
                          else if ("4".Equals(ViewData["ORIGIN_FLG"]))
                          {%>
                            来自互动平台
                            <%} %>
                        <input type="hidden" id="resId" name="resId" value="<%:ViewData["ID"] %>"/>
                    </td>
                </tr>
                <tr>
                    <td class="right">上传时间：</td>
                    <td><%:ViewData["UPLOAD_TIME"]%></td>
                </tr>
                <tr>
                    <td class="right">资源分类：</td>
                    <td><%:ViewData["TYPE_NAME"]%></td>
                </tr>
                <tr>
                    <td class="right">资源介绍：</td>
                    <td><%:ViewData["INTRODUCTION"]%></td>
                </tr>
                <tr>
                    <td class="right">标签：</td>
                    <td>
                        <div id="searchLabelDiv">
                            <%if (ViewData["LABEL"] != null)
                                {
                                    string[] labels = ViewData["LABEL"] as string[];
                                    for (int i = 0; i < labels.Length; i++)
                                    {
                            %>
                                <div class='label-queue-item'>
                                    <div class='labelName'>
                                    <%:labels[i] %>
                                    </div>
                                </div>
                                <%} %>
                            <%} %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="right">附件：</td>
                    <td>
                        <div id="queue" class="uploadify-queue">
                            <%if (ViewData["appendixList"] != null)
                                {
                                    int i = 0;
                                    foreach (var item in ViewData["appendixList"] as List<ResSharingPlatform.Models.T_Res_Appendix>)
                                    {
                                        var fileName = item.FILE_NAME;
                                        var ext = "";
                                        if (fileName != null && fileName.LastIndexOf('.') > 0)
                                        {
                                            ext = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.')).ToLower(); 
                                        }
                                        var saveName = item.ID + ext;
                                          
                                        var activeStart = Convert.ToDateTime(item.ACTIVE_TIME_START).ToString("yyyy/MM/dd");

                                        var activeEnd = Convert.ToDateTime(item.ACTIVE_TIME_END).ToString("yyyy/MM/dd");
                                        bool isForever = (Boolean)item.IS_FOREVER;
                                %>
                                    <div id="queue_<%:i %>" class="uploadify-queue-item">
                                        <span class="fileName">
                                            <a href='javascript:viewOnline("<%:saveName %>")'><%:fileName%></a>
                                            <input type="hidden" id="fileName_<%:i %>" name="fileName" value="<%:fileName%>" />
                                            <input type="hidden" id="saveName_<%:i %>" name="saveName" value="<%:saveName %>" />
                                            <input type="hidden" id="uploadTime_<%:i %>" name="uploadTime" value="<%:item.UPLOAD_TIME %>" />
                                        </span>
                                        <div style="width: 100%;">
                                            作者：<%:item.AUTHOR %>
                                            &nbsp;&nbsp;
                                            <%if (isForever)
                                              {%>
                                            永久有效
                                            <%}
                                              else
                                              { %>
                                              有效期：
                                            <%:activeStart %> - 
                                            <%:activeEnd %>
                                            <%} %>
                                            <input type="button" class="title-btn" value="版本对比" style="margin-left:50px;" onclick="ContrastFile('<%:item.ID %>')"/>
                                        </div>
                                    </div>
                            <%
                                i++;
                                    }
                                } %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="right">资源评分：</td>
                    <td id="grade"><%:ViewData["GRADE"]%></td>
                </tr>
                <tr>
                    <td class="right">评价：</td>
                    <td>
                        <div>
                            <form id="Sco_ActionForm" action="" class="form-horizontal ui-formwizard" method="post">
                                <input type="hidden" id="sco_res_Id" name="sco_res_Id" value="<%:ViewData["ID"] %>"/>
                                <div id="sco_content">
                                </div>
                            </form>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="right">提问：</td>
                    <td>
                        <div>
                            <form id="Qa_ActionForm" action="" class="form-horizontal ui-formwizard" method="post">
                                <input type="hidden" id="qa_res_Id" name="qa_res_Id" value="<%:ViewData["ID"] %>"/>
                                <div id="qa_content">
                                </div>
                            </form>
                        </div>
                    </td>
                </tr>
            </table>
                </div>
            </div>
        <div class="sub-box" style="text-align:center;">
            <input type="button" class="sub-btn" value="返回" onclick="goBack()"/>
        </div>
        </div>
    </div>
    <div id="xingZQYTree" class="pop"
		style="display: none; overflow: auto; position: absolute; background-color: #FFFFFF; border: 1px solid #0099CC; padding-left: 0px">
		<div id="pop_title" class="pop_title">
			<div class="pop_title_left" style="font-size: 14px">
				回复内容
                <input type="hidden" id="qaId" name="qaId"/>
			</div>
			<div class="pop_title_right">
				<label title="关闭此窗口" onclick="closePop()">
					&nbsp;X&nbsp;
				</label>
			</div>
		</div>
		<div class="pop_content">
			<table style="width:100%;">
				<tr>
					<td>回复内容：&nbsp;&nbsp;</td>
					<td>
                        <textarea id="popAnswer" name="popAnswer" rows="5" cols="80"></textarea>
                    </td>
				</tr>
			</table>
			<div style="width:100%;text-align:center">
				<input type="button" name="" value="提交" class="btn btn-primary btn-sm"
					onclick="answerQuest()" />
				<input type="button" name="" value="取消" class="btn btn-primary btn-sm"
					onclick="closePop()" />
			</div>
		</div>
	</div>
</asp:Content>
