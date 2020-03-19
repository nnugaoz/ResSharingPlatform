<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    后台管理页面
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function goBack() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                $("#form-wizard").attr("action","../../Upload/deleteFile");
                $("#form-wizard").submit();
                //window.location.href = "../../Upload/Index?topage=" + $("#topage").val();
            }else {
                 ExamLogin();
             }
        }

        $(function () {
            try
            {
            $("#typeId").combotree("setValue","<%:ViewData["TYPE_ID"] %>");
            $("#xingZQYTree").css("display", "none");
            InitUpdateFile();
            $("#uploadFile").uploadify({
                'height': 28,
                //指定swf文件
                'swf': '../../Scripts/uploadify/uploadify.swf',
                //后台处理的页面
                'uploader': '/Upload/UploadPdf',
                'cancelImg': '../../Images/uploadify-cancel.png',
                //按钮显示的文字
                'buttonText': '添加附件',
                //在浏览窗口底部的文件类型下拉菜单中显示的文本
                'fileTypeDesc': "<%:ViewData["fileTypeName"] %>",
                //允许上传的文件后缀
                'fileTypeExts': "<%:ViewData["fileType"] %>",
                //上传文件大小限制
                'fileSizeLimit': "<%:ViewData["fileSize"] %>",
                //选择文件后自动上传
                'auto': true,
                //设置为true将允许多文件上传
                'multi': true,
                //是否自动将已完成任务从队列中删除，如果设置为false则会一直保留此任务显示。
                'removeCompleted': false,
                //返回结果的超时时间60000s
                'successTimeout': 60000,
                'overrideEvents': ['onDialogClose', 'onSelectError'],
                'onSelectError': function (file, errorCode, errorMsg) {
                    var msgText = "上传失败\n";
                    switch (errorCode) {
                        case SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED:
                            msgText += "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
                            break;
                        case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                            msgText += "文件大小超过限制( " + this.settings.fileSizeLimit + " )";
                            break;
                        case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                            msgText += "文件大小为0";
                            break;
                        case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                            msgText += "文件格式不正确，仅限 " + this.settings.fileTypeExts;
                            break;
                        default:
                            msgText += "错误代码：" + errorCode + "\n" + errorMsg;
                    }
                    layer.alert(msgText, 8);
                },

                'onUploadSuccess': function (file, data, response) {
                    if (response == true) {
                        if (data != null && data.split(",")[0] == "true") {
                            
                            $("#" + file.id).find(".fileName").append(
                              "<input type='hidden' id='dataType_" + file.id + "' name='dataType' value=\"add\"/>"
                            + "<input type='hidden' id='fileName_" + file.id + "' name='fileName' value=\"" + file.name + "\"/>"
                            + "<input type='hidden' id='saveName_" + file.id + "' name='saveName' value=\"" + data.split(",")[1] + "\"/>"
                            + "<input type='hidden' id='uploadTime_" + file.id + "' name='uploadTime' value='' />"
                            + "<input type='hidden' id='fileType_" + file.id + "' name='fileType' value=\"" + data.split(",")[2] + "\"/>"
                            + "<input type='hidden' id='fileUrl_" + file.id + "' name='fileUrl' value=\"" + data.split(",")[3] + "\"/>");
                            $("#" + file.id).find(".data").html(" - 上传成功");

                            var div = $("<div>");
                            div.attr("style", "width:100%");
                            div.append("作者：<input type='text' id='author_'" + file.id + " name='author' style='width:80px;margin-right:5px' value='' maxlength='20'/>");
                            div.append("<input type='checkbox' onclick='isForeverClick(this)'/>永久有效  ")
                            div.append("<span id='activeTitle_" + file.id + "' style='margin-left:5px'>有效期：</span>");
                            var input1 = $("<input>");
                            input1.attr("type", "text");
                            input1.attr("id", "activeStart_" + file.id);
                            input1.attr("name", "activeStart");
                            input1.attr("style","width:80px;");
                            input1.click(function(){
                                WdatePicker();
                            });
                            var nowDate = new Date();
                            var dateFormat = nowDate.format("yyyy/MM/dd");
                            input1.val(dateFormat);
                            div.append(input1);
                            div.append("<span> - </span>");

                            var input2 = $("<input>");
                            input2.attr("type", "text");
                            input2.attr("id", "activeEnd_" + file.id);
                            input2.attr("name", "activeEnd");
                            input2.attr("style","width:80px;");
                            input2.click(function(){
                                WdatePicker();
                            });
                            input2.val(dateFormat);
                            div.append(input2);

                            div.append("<input type='hidden' id='isForever_" + file.id + "' name='isForever' value='0'/>");

                            var input3 = $("<input>");
                            input3.attr("type", "hidden");
                            input3.attr("id", "uploadTime_" + file.id);
                            input3.attr("name", "uploadTime");
                            div.append(input3);

                            $("#" + file.id).find(".data").after(div);
                        } else {
                            $("#" + file.id).find(".data").html(" - " + data.split(",")[1]);
                            $('#' + file.id).find('.uploadify-progress-bar').css('width', '1px');
                        }
                    } else {
                        $('#' + file.id).find('.uploadify-progress-bar').css('width', '1px');
                        $("#" + file.id).find(".data").html(" - 上传失败");
                    }
                }
            });
            }
            catch(e)
            {
                layer.alert(e, 8);
            }
        });

        function viewOnline(fileName) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.open("../../Upload/ViewOnline?fileName=" + fileName);
            }else {
                 ExamLogin();
             }
        }

        function check(){
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var resourceName = $("#resourceName").val();
                if(resourceName == null || resourceName == "") {
                    layer.alert("请输入资源名称！", 5);
                    $("#resourceName").focus();
                    return false;
                }
                if($("#typeId").combotree("getValue") == null || $("#typeId").combotree("getValue") == "") {
                    layer.alert("请选择资源分类！", 5);
                    $("#typeId").focus();
                    return false;
                }
                if($.trim($("#searchLabelDiv").html()) == "") {
                    layer.alert("请选择标签！", 5);
                    return false;
                }
                if($.trim($("#queue").html()) == "" && $.trim($("#uploadFile-queue").html()) == "") {
                    layer.alert("请选择附件！", 5);
                    return false;
                }
                var hasList = false;
                if($.trim($("#queue").html()) != "") {
                    $(".uploadify-queue-item").each(function() {
                        if($(this).css("display") != "none") {
                            hasList = true;
                            return false;
                        }
                    });
                    if(!hasList) {
                        layer.alert("请选择附件！", 5);
                        return false;
                    }
                }

                var IsAuthor = true;
                $("[name='author']").each(function(index){
                    if($(this).val() == null || $(this).val() == ""){
                        layer.alert("请输入第" + (index + 1) + "个附件的作者！",5);
                        $(this).focus();
                        IsAuthor = false;
                        return false;
                    }
                });
                if(!IsAuthor){
                    return false;
                }
                
                var hasTime = true;
                $("[name='activeStart']").each(function(index) {
                    if($(this).next().next().next().val() == "1") {
                        return false;
                    }
                    if($(this).val() == null || $(this).val() == "") {
                        layer.alert("请选择第" + (index + 1) + "个附件的有效开始时间！",5);
                        $(this).focus();
                        hasTime = false;
                        return false;
                    }
                    if($(this).next().next().val() == null || $(this).next().next().val() == "") {
                        layer.alert("请选择第" + (index + 1) + "个附件的有效结束时间！",5);
                        $(this).next().focus();
                        hasTime = false;
                        return false;
                    }

                    if($(this).val() > $(this).next().next().val()) {
                        layer.alert("第" + (index + 1) + "个附件的有效开始时间应小于有效结束时间！",5);
                        $(this).focus();
                        hasTime = false;
                        return false;
                    }

                    var now = new Date();
                    if($(this).next().next().val() < now.format("yyyy/MM/dd")) {
                        layer.alert("第" + (index + 1) + "个附件的有效结束时间应大于当前时间！",5);
                        $(this).next().focus();
                        hasTime = false;
                        return false;
                    }
                });
                if(!hasTime) {
                    return false;
                }

            }else {
                 ExamLogin();
                 return false;
             }
        }

        function onDeleteFile(i) {
            $("#dataType_" + i).val("delete");
            $("#queue_" + i).hide(500);
        }

        /**更新附件**/
        function InitUpdateFile()
        {
            $("input[id^=updateFile_]").uploadify({
                'width': 100,
                'height': 28,
                //指定swf文件
                'swf': '../../Scripts/uploadify/uploadify.swf',
                //后台处理的页面
                'uploader': '/Upload/UploadPdf',
                'cancelImg': '../../Images/uploadify-cancel.png',
                //按钮显示的文字
                'buttonText': '更新附件',
                //在浏览窗口底部的文件类型下拉菜单中显示的文本
                'fileTypeDesc': "<%:ViewData["fileTypeName"] %>",
                //允许上传的文件后缀
                'fileTypeExts': "<%:ViewData["fileType"] %>",
                //上传文件大小限制
                'fileSizeLimit': "<%:ViewData["fileSize"] %>",
                //选择文件后自动上传
                'auto': true,
                //设置为true将允许多文件上传
                'multi': false,
                //是否自动将已完成任务从队列中删除，如果设置为false则会一直保留此任务显示。
                'removeCompleted': false,
                //返回结果的超时时间60000s
                'successTimeout': 60000,
                'overrideEvents': ['onDialogClose', 'onSelectError'],
                'onSelectError': function (file, errorCode, errorMsg) {
                    var msgText = "上传失败\n";
                    switch (errorCode) {
                        case SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED:
                            msgText += "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
                            break;
                        case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                            msgText += "文件大小超过限制( " + this.settings.fileSizeLimit + " )";
                            break;
                        case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                            msgText += "文件大小为0";
                            break;
                        case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                            msgText += "文件格式不正确，仅限 " + this.settings.fileTypeExts;
                            break;
                        default:
                            msgText += "错误代码：" + errorCode + "\n" + errorMsg;
                    }
                    layer.alert(msgText);
                },

                'onUploadSuccess': function (file, data, response) {
                    if (response == true) {
                        if (data != null && data.split(",")[0] == "true") {
                            var fileId = file.id;
                            var i = fileId.split("_")[1];
                            var OldFileId = $("#fileId_" + i).val();//更新后 将原来的资源附件ID放在dataType中传至后台
                            $("#queue_" + i).find(".fileName").html(
                                file.name+"(" + GetFileSize(file.size) + ")"
                            + "<input type='hidden' id='dataType_" + i + "' name='dataType' value=\"" + OldFileId + "\"/>"
                            + "<input type='hidden' id='fileName_" + i + "' name='fileName' value=\"" + file.name + "\"/>"
                            + "<input type='hidden' id='saveName_" + i + "' name='saveName' value=\"" + data.split(",")[1] + "\"/>"
                            + "<input type='hidden' id='uploadTime_" + i + "' name='uploadTime' value='' />"
                            + "<input type='hidden' id='fileType_" + i + "' name='fileType' value=\"" + data.split(",")[2] + "\"/>"
                            + "<input type='hidden' id='fileUrl_" + i + "' name='fileUrl' value=\"" + data.split(",")[3] + "\"/>");
                            
                        } else {
                            layer.alert("更新失败！", 8)
                        }
                    } else {
                        layer.alert("更新失败！", 8)
                    }
                }
            });
        }
        /**文件大小-字节转换**/
        function GetFileSize(size)
        {
            var fileSize = Math.round(size / 1024);
            var suffix = 'KB';
            if (fileSize > 1000) {
                fileSize = Math.round(fileSize / 1000);
                suffix = 'MB';
            }else if(fileSize < 1){
                fileSize = Math.round(size);
                suffix = 'B';
            }
            var fileSizeParts = fileSize.toString().split('.');
            fileSize = fileSizeParts[0];
            if (fileSizeParts.length > 1) {
                fileSize += '.' + fileSizeParts[1].substr(0, 2);
            }
            fileSize += suffix;
            return fileSize;
        }

        /**打开对比页面**/
        function ContrastFile(Aid){
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.open("../../Upload/ContrastFile?Aid=" + Aid);
            }
            else {
                ExamLogin();
            }
        }
    </script>
    <div>
        <form id="form-wizard" class="form-horizontal ui-formwizard" action="../../Upload/Save" method="post">
            <input type="hidden" id="topage" name="topage" value="<%:ViewData["topage"] %>" />
            <div class="con-title">
                <h1 class="con-title-h1">上传资源</h1>
            </div>
            <div class="box">
                <div class="seach-box">
                <div class="seach-box-tit">信息录入</div>
                <div class="seach-box-con">
                <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                    <tr>
                        <td class="right" style="width:140px;">资源名称：</td>
                        <td><span class="red">*</span></td>
                        <td>
                            <%if (ViewData["NAME"] != null || "1".Equals(ViewData["ORIGIN_FLG"]))
                              {%>
                                    <input type="text" id="resourceName" name="resourceName" class="inputText" value="<%:ViewData["NAME"] %>" maxlength="20"/>
                              <%}
                              else if ("2".Equals(ViewData["ORIGIN_FLG"]))
                              {%>
                                来自一体化课程
                                <input type="hidden" id="resourceName" name="resourceName" value="来自一体化课程"/>
                              <%}
                              else if ("3".Equals(ViewData["ORIGIN_FLG"]))
                              {%>
                                来自精品课程
                                <input type="hidden" id="resourceName" name="resourceName" value="来自精品课程"/>  
                              <%}
                              else if ("4".Equals(ViewData["ORIGIN_FLG"]))
                              {%>
                                来自互动平台
                                <input type="hidden" id="resourceName" name="resourceName" value="来自互动平台"/>  
                              <%}%>
                            <input type="hidden" id="originFlg" name="originFlg" class="inputText" value="<%:ViewData["ORIGIN_FLG"] %>"/>
                            <input type="hidden" id="resId" name="resId" value="<%:ViewData["ID"] %>"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">资源分类：</td>
                        <td><span class="red">*</span></td>
                        <td>
                            <select class="easyui-combotree" class="inputText" style="width:200px;" url='../../Upload/GetResType?type=combox' name="typeId" id="typeId"></select>
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
                            <%--<font color="red">注：用逗号隔开</font>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            附件：
                            <div style='height: 28px;font-size:12px;color: #b0b0b0;'>
                                文件类型：<%:ViewData["fileTypeName"] %>
                                <br />
                                文件大小：<%:ViewData["fileSize"] %>
                            </div>
                        </td>
                        <td><span class="red">*</span></td>
                        <td>
                            <input type="file" id="uploadFile" name="uploadFile"/>
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

                                          var activeStart = "";
                                          if (item.ACTIVE_TIME_START != null)
                                          {
                                              activeStart = Convert.ToDateTime(item.ACTIVE_TIME_START).ToString("yyyy/MM/dd");
                                          }

                                          var activeEnd = "";
                                          if (item.ACTIVE_TIME_END != null)
                                          {
                                              activeEnd = Convert.ToDateTime(item.ACTIVE_TIME_END).ToString("yyyy/MM/dd");
                                          }
                                          bool isForever = (Boolean)item.IS_FOREVER;
                                 %>
                                        <div id="queue_<%:i %>" class="uploadify-queue-item">
                                            <input type="hidden" id="fileId_<%:i %>" name="OldFileID" value="<%:item.ID %>" />
                                            <div class="cancel">
                                                <a href="javascript:onDeleteFile('<%:i %>')">X</a>
                                            </div>
                                            <span class="fileName">
                                                <a href='javascript:viewOnline("<%:saveName %>")'><%:fileName%></a>
                                                <input type="hidden" id="dataType_<%:i %>" name="dataType" value="update" />
                                                <input type="hidden" id="fileName_<%:i %>" name="fileName" value="<%:fileName%>" />
                                                <input type="hidden" id="saveName_<%:i %>" name="saveName" value="<%:saveName %>" />
                                                <input type="hidden" id="uploadTime_<%:i %>" name="uploadTime" value="<%:item.UPLOAD_TIME %>" />
                                                <input type="hidden" id="fileType_<%:i %>" name="fileType" value="<%:item.TYPE_FLG %>" />
                                                <input type="hidden" id="fileUrl_<%:i %>" name="fileUrl" value="<%:item.FILE_URL %>" />
                                            </span>
                                            <div style="width: 100%;height:40px;">
                                                作者：<input type="text" id="author_<%:i %>" name="author" style="width:80px;margin-right:5px" value="<%:item.AUTHOR %>" maxlength="20"/>
                                                <input type="checkbox" onclick="isForeverClick(this)" <%if(isForever) {%>checked<%} %> />永久有效
                                                <span id="activeTitle_<%:i %>" style="margin-left:5px;<%if(isForever) {%>display:none<%} %>">有效期：</span>
                                                <input type="text" id="activeStart_<%:i %>" name="activeStart" style="width:80px;<%if(isForever) {%>display:none<%} %>" value="<%:activeStart %>" onclick="WdatePicker()"/>
                                                <span <%if(isForever) {%>style="display:none"<%} %>> - </span>
                                                <input type="text" id="activeEnd_<%:i %>" name="activeEnd" style="width:80px;<%if(isForever) {%>display:none<%} %>" value="<%:activeEnd %>" onclick="WdatePicker()"/>
                                                <input type="hidden" id="isForever_<%:i %>" name="isForever" value="<%if(isForever) {%>1<%} else { %>0<%} %>"/>
                                                <input type="button" class="title-btn" value="版本对比" style="margin-left:10px;" onclick="ContrastFile('<%:item.ID %>')"/>
                                            </div>
                                            <input type="file" id="updateFile_<%:i %>" name="updateFile"/>
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
                        <td><textarea id="note" name="note" style="border:1px solid #cdcdcd;" rows="5" cols="100"><%:ViewData["NOTE"]%></textarea></td>
                    </tr>
                </table>
            </div>
            </div>

            <div class="sub-box" style="text-align:center;">
                <input type="submit" class="sub-btn" value="保存" onclick="return check();"/>
                <input type="button" class="sub-btn" style="margin-left:100px;" value="返回" onclick="goBack()"/>
            </div>
            </div>
        </form> 
    </div>
</asp:Content>
