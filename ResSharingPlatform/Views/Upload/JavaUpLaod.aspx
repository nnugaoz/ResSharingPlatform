<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	大文件断点续传
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                $(".index").css("float", "none");
                $("#validityDate").hide();
            }
            else {
                ExamLogin();
            }
        });

        /**每次选择文件等于开始上传**/
        function reStartUpload() {
            $("#validityDate").hide();
            $("#startDate").val("");
            $("#endDate").val("");
        }
        /**所有文件上传结束**/
        function uploadOver() {
            $("#validityDate").show();
        }

        /**单文件上传成功返回路径地址**/
        function upload(filePath, fileName) {
            var html = '<input name="uploadFilePath" type="hidden" value="' + filePath + '" />';
            html += '<input name="uploadFileName" type="hidden" value="' + fileName + '" />';
            $("#uploadPath").append(html);
        }

        /**提交前验证**/
        function check() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var txtName = $("#resourceName").val();
                if (txtName == "") {
                    layer.alert("请输入资源名称！", 5);
                    $("#resourceName").focus();
                    return false;
                }
                var txtType = $("#typeId").combotree("getValue");
                if (txtType == "") {
                    layer.alert("请选择资源分类！", 5);
                    $("#typeId").focus();
                    return false;
                }
                if ($("input[name='uploadFilePath']").length < 1) {
                    layer.alert("请选择附件！", 5);
                    return false;
                }
                if ($("#author").val() == "") {
                    layer.alert("请输入作者！", 5);
                    $("#author").focus();
                    return false;
                }
                if ($("#isForever").val() != "1") {
                    if ($("#startDate").val() == "" || $("#endDate").val() == "") {
                        layer.alert("请设置有效期！", 5);
                        return false;
                    }
                }
                return true;
            }
            else {
                ExamLogin();
                return false;
            }
        }

        /**提交**/
        function submitData() {
            if (check()) {
                $("#FormSubmit").ajaxSubmit(
                function (data) {
                    eval("ret=" + data + ";");
                    if (ret) {
                        location.href = "../../Upload/Index";
                    }
                    else {
                        layer.alert("保存失败！", 8);
                    }
                }
            );
            }
        }
    </script>
    
    <div id="main">
        <input id="ext" type="hidden" value="<%:ViewData["fileExt"] %>" />
            <div class="con-title">
                <h1 class="con-title-h1">大文件断点续传</h1>
            </div>
            <div class="box">
                <form id="FormSubmit" action="../../Upload/JavaUpLoadSubmit" method="post">
                <div class="seach-box">
                    <div class="seach-box-tit">信息录入</div>
                    <div class="seach-box-con">
                        <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                            <tr>
                                <td class="right" style="width:140px;">资源名称：</td>
                                <td><span class="red">*</span></td>
                                <td>
                                    <input id="resourceName" name="resourceName" type="text" class="inputText" maxlength="20"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">资源分类：</td>
                                <td><span class="red">*</span></td>
                                <td>
                                    <select id="typeId" name="typeId" class="easyui-combotree" class="inputText" style="width:200px;" url="../../Upload/GetResType?type=combox"></select>
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
                                    <div id="searchLabelDiv" style="width:100%"></div>
                                    <div id="imgDiv" style="width:100%;float:left">
                                        <image alt="添加标签" src="../../Images/add.png" class="label-img" onclick="addTags();"></image>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                    附件：
                                    <div style='height: 28px;font-size:12px;color: #b0b0b0;'>
                                        文件大小：<%:ViewData["fileSize"] %>G
                                        <br />
                                        <span style="color:red">注：大文件断点续传只支持IE10以上版本的浏览器</span>
                                    </div>
                                </td>
                                <td><span class="red">*</span></td>
                                <td>
                                    <div style="border:1px solid #cdcdcd;">
                                    <div id="i_select_files"></div>
                                    <div id="i_stream_files_queue" style="width:100%;"></div>
                                    <div style="padding:10px;">
	                                <input type="button" onclick="_t.upload();" class="seach-btn" value="开始上传" />
                                    <input type="button" onclick="_t.stop();" class="seach-btn" value="停止上传" />
                                    <input type="button" onclick="_t.cancel();" class="seach-btn" value="取消上传" />
                                    </div>
                                    <div id="i_stream_message_container" class="stream-main-upload-box" style="overflow: auto; width:100%;height:0px;visibility:hidden;"></div>
                                        <div id="validityDate" style="margin-left:10px;margin-bottom:10px;">
                                            作者：
                                            <input type="text" id="author" name="author" class="inputText" style="width:80px;" maxlength="20"/>
                                            <input type="checkbox" onclick="isForeverClick(this)"/>永久有效
                                            <span id="activeTitle" style="margin-left:5px;">有效期：</span>
                                            <input id="startDate" name="startDate" type="text" class="inputText" style="width:80px;" onclick="WdatePicker({dateFmt:'yyyy/MM/dd'})"/>
                                            <span> - </span>
                                            <input id="endDate" name="endDate" type="text" class="inputText" style="width:80px;" onclick="WdatePicker({dateFmt:'yyyy/MM/dd'})"/>
                                            <input type="hidden" id="isForever" name="isForever" value="0"/>
                                        </div>
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
                <div id="uploadPath" style="width:100%;height:0px;visibility:hidden;"></div>
                </form>
                <div class="sub-box" style="text-align:center;">
                    <input type="button" class="sub-btn" value="保存" onclick="submitData()"/>
                    <input type="button" class="sub-btn" style="margin-left:100px;" value="返回" onclick="javascript:history.back(-1);"/>
                </div>
            </div>
    </div>
    <script type="text/javascript" src="<%:ViewData["javaUploadUrl"] %>stream/js/stream-v1.js"></script>
    <!--JAVAUPLOAD-->
    <script type="text/javascript">
        /**
        * 配置文件（如果没有默认字样，说明默认值就是注释下的值）
        * 但是，on*（onSelect， onMaxSizeExceed...）等函数的默认行为
        * 是在ID为i_stream_message_container的页面元素中写日志
        */
        var config = {
            browseFileId: "i_select_files", /** 选择文件的ID, 默认: i_select_files */
            browseFileBtn: "<div style='text-align:center;'>请选择文件</div>", /** 显示选择文件的样式, 默认: `<div>请选择文件</div>` */
            dragAndDropArea: "i_select_files", /** 拖拽上传区域，Id（字符类型"i_select_files"）或者DOM对象, 默认: `i_select_files` */
            dragAndDropTips: "<span>把文件(文件夹)拖拽到这里</span>", /** 拖拽提示, 默认: `<span>把文件(文件夹)拖拽到这里</span>` */
            filesQueueId: "i_stream_files_queue", /** 文件上传容器的ID, 默认: i_stream_files_queue */
            filesQueueHeight: 100, /** 文件上传容器的高度（px）, 默认: 450 */
            messagerId: "i_stream_message_container", /** 消息显示容器的ID, 默认: i_stream_message_container */
            multipleFiles: true, /** 多个文件一起上传, 默认: false */
            autoUploading: false, /** 选择文件后是否自动上传, 默认: true */
            //autoRemoveCompleted : true, /** 是否自动删除容器中已上传完毕的文件, 默认: false */
            maxSize: <%:ViewData["javaUploadSize"] %>, /** 单个文件的最大大小，默认:2G */
            //retryCount : 5, /** HTML5上传失败的重试次数 */
            postVarsPerFile: { /** 上传文件时传入的参数，默认: {} */
                prefix: '<%:ViewData["UserID"] %>'
            },
            swfURL: '<%:ViewData["javaUploadUrl"] %>stream/swf/FlashUploader.swf', /** SWF文件的位置 */
            tokenURL: '../../stream/tk', /** 根据文件名、大小等信息获取Token的URI（用于生成断点续传、跨域的令牌） */
            frmUploadURL: '<%:ViewData["javaUploadUrl"] %>stream/fd;', /** Flash上传的URI */
            uploadURL: '<%:ViewData["javaUploadUrl"] %>stream/upload', /** HTML5上传的URI */
            //simLimit: 200, /** 单次最大上传文件个数 */
            extFilters: <%=ViewData["fileExt"] %>, /** 允许的文件扩展名, 默认: [] */
            //extFilters: [".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".flv", ".mp4", ".wmv"],
            onSelect: function(list) { reStartUpload();}, /** 选择文件后的响应事件 */
            onMaxSizeExceed: function(size, limited, name) {layer.alert('您选择的文件过大！',5)}, /** 文件大小超出的响应事件 */
            onFileCountExceed: function(selected, limit) {layer.alert('您选择的文件过多！',5)}, /** 文件数量超出的响应事件 */
            onExtNameMismatch: function(name, filters) {layer.alert('请上传扩展名在'+$("#ext").val()+'中的文件！',5)}, /** 文件的扩展名不匹配的响应事件 */
            onCancel: function (file) { layer.alert('Canceled:  ' + file.name,1); }, /** 取消上传文件的响应事件 */
            onComplete: function (file) { eval("var t = " + file.msg); upload(t.path,file.name); }, /** 单个文件上传完毕的响应事件 */
            onQueueComplete: function() { uploadOver();}, /** 所有文件上传完毕的响应事件 */
            onUploadError: function(status, msg) {layer.alert('上传失败！原因为'+msg,8)} /** 文件上传出错的响应事件 */
            //onDestroy: function() {alert('onDestroy')} /** 文件上传出错的响应事件 */
        };
        var _t = new Stream(config);
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <link href="<%:ViewData["javaUploadUrl"] %>stream/css/stream-v1.css" rel="stylesheet" type="text/css" />
</asp:Content>
