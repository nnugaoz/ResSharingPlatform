<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RecLink" runat="server">
    <script language="javascript" type="text/javascript" src="../../Scripts/jquery.form.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#typeId").combotree("setValue", '<%:ViewData["TYPE_ID"]%>');

            $("#xingZQYTree").css("display", "none");

        });

        //必须项目验证
        function doCheck() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var resourceName = $("#resourceName").val();
                if ($("#originFlg").val() != "2") {
                    if (resourceName == "") {
                        layer.alert("请输入资源名称！", 5);
                        return false;
                    }
                }
                var typeId = $("#typeId").combo('getValue');
                if (typeId == "") {
                    layer.alert("请选择资源分类！", 5);
                    return false;
                }
                if ($.trim($("#searchLabelDiv").html()) == "") {
                    layer.alert("请选择标签！", 5);
                    return false;
                }

                var isReady = true;
                $.ajax({
                    type: "post",
                    url: "../../Upload/checkFileIsReady",
                    data: "resId=" + $("#resId").val(),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data["isReady"] == "false") {
                            
                            isReady = false;
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layer.alert(textStatus);
                        layer.alert(errorThrown);
                    }
                });
                if (!isReady) {
                    layer.alert("文件还未处理好，暂不能审核！", 5);
                    return false;
                }
            } else {
                ExamLogin();
                return false;
            }

        }

        //返回按钮
        function goBackList() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                //$('#actionForm').attr("action", "../../Upload/DoBack");
                //$('#actionForm').submit();
                window.location.href = "../../Upload/Index?topage=" + $("#topage").val();
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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
	资源审核
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    

     <div>
        <form id="actionForm" class="form-horizontal ui-formwizard" action="../../Upload/DoExamine" onsubmit="return doCheck()" method="post" runat="server">
            <input type="hidden" id="orderBy" name="orderBy" value="<%:ViewData["orderBy"] %>" />
            <input type="hidden" id="delFlg" name="delFlg" value="<%:ViewData["delFlg"] %>" />
            <input type="hidden" id="pagecurrent" name="pagecurrent" value="<%:ViewData["pagecurrent"] %>" />
            <input type="hidden" id="resource" name="resource" value="<%:ViewData["resource"] %>" />
            <input type="hidden" id="type" name="type" value="<%:ViewData["type"] %>" />
            <input type="hidden" id="upload_Time" name="upload_Time" value="<%:ViewData["uploadTime"] %>" />
            <input type="hidden" id="labels" name="labels" value="<%:ViewData["label"] %>" />
            <input type="hidden" id="origin" name="origin" value="<%:ViewData["origin"] %>" />
            <input type="hidden" id="topage" name="topage" value="<%:ViewData["topage"] %>" />
            <div class="con-title">
                <h1 class="con-title-h1">资源审核</h1>
            </div>
            <div class="box">
                <div class="seach-box">
                <div class="seach-box-tit">信息审核</div>
                <div class="seach-box-con">
                <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                    <tr>
                        <td class="right" style="width:120px;">资源名称：</td>
                        <td><span class="red">*</span></td>
                        <td>
                        <%if (ViewData["NAME"] != null)
                          { %>
                            <input type="text" id="resourceName" name="resourceName" value="<%:ViewData["NAME"] %>"/>
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
                            <input type="hidden" id="originFlg" name="originFlg" value="<%:ViewData["ORIGIN_FLG"] %>"/>
                            <input type="hidden" id="resId" name="resId" value="<%:ViewData["ID"] %>"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">上传时间：</td>
                        <td></td>
                        <td><%:ViewData["UPLOAD_TIME"]%></td>
                    </tr>
                    <tr>
                        <td class="right">创建人：</td>
                        <td></td>
                        <td><%:ViewData["CREATE_NAME"]%></td>
                    </tr>
                    <tr>
                        <td class="right">资源分类：</td>
                        <td><span class="red">*</span></td>
                        <td>
                            <select class="easyui-combotree" style="width:200px;" url='../../Upload/GetResType?type=combox' name="typeId" id="typeId"/>
                            <!--<input type="button" class="btn btn-primary btn-sm" value="新建" onclick="openPop(event)"/>-->
                        </td>
                    </tr>
                    <tr>
                        <td class="right">资源介绍：</td>
                        <td></td>
                        <td><textarea id="introduction" name="introduction" style="border:1px solid #cdcdcd;" rows="5" cols="100"><%:ViewData["INTRODUCTION"]%></textarea></td>
                    </tr>
                    <tr>
                        <td class="right">标签：</td>
                        <td><span class="red">*</span></td>
                        <td>
                            <div id="searchLabelDiv" style="width:100%">
                                <%if (ViewData["LABEL"] != null)
                                  {
                                      string[] labels = ViewData["LABEL"] as string[];
                                      for (int i = 0; i < labels.Length; i++)
                                      {
                                %>
                                    <div class='label-queue-item'>
                                        <div class='labelName'>
                                        <%:labels[i] %>
                                        <input type='hidden' name='savelabel' value='<%:labels[i] %>'/>
                                        </div>
                                        <div class='delete'>
                                            <image src='../../Images/uploadify-cancel.png' onclick='deleteLabel(this)' style='cursor:pointer;'></image>
                                        </div>
                                    </div>
                                    <%} %>
                                <%} %>
                            </div>
                            <div id="imgDiv" style="width:100%;float:left">
                                <image alt="添加标签" src="../../Images/add.png" class="label-img" onclick="addTags();"></image>
                            </div>
                        </td>
                    </tr>
                   <tr>
                        <td class="right">附件：</td>
                        <td></td>
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
                                              ext = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.')); 
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
                        <td class="right">备注：</td>
                        <td></td>
                        <td>
                            <textarea id="note" name="note" style="border:1px solid #cdcdcd;" rows="5" cols="100" readonly="readonly" style="background-color:#E1E0E0"><%:ViewData["NOTE"]%></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">审核：</td>
                        <td><span class="red">*</span></td>
                        <td>
                            <input type="radio" name="status" id="exaStatus1" checked="checked" value="1" />通过&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" name="status" id="exaStatus2" value="2" />不通过
                        </td>
                    </tr>
                    <tr>
                        <td class="right">精品：</td>
                        <td></td>
                        <td>
                            <input type="radio" name="excellentFlg" id="Radio1" checked="checked" value="0" />普通&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" name="excellentFlg" id="Radio2" value="1" />精品
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </div>

            <div class="sub-box" style="text-align:center;">
                <input type="submit" class="sub-btn" value="审核"/>
                <input type="button" class="sub-btn" value="返回" onclick="goBackList()"/>
            </div>
            </div>
        </form> 
    </div>
</asp:Content>
