<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>File</title>
    <link href="../../Content/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/flexpaper.css" rel="stylesheet" type="text/css" /> 
     <link href="../../Content/bootstrap.css" rel="stylesheet" type="text/css"/> 
     <script src="../../Scripts/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/uploadify/jquery.uploadify.js"></script>
     
    <script type="text/javascript">

        $(function () {
            $("#uploadFile").uploadify({
                'height': 28,
                //指定swf文件
                'swf': '../../Scripts/uploadify/uploadify.swf',
                //后台处理的页面
                'uploader': '/FileUpload/Upload',
                'cancelImg': '../../Images/uploadify-cancel.png',
                //按钮显示的文字
                'buttonText': '添加附件',
                //在浏览窗口底部的文件类型下拉菜单中显示的文本
                'fileTypeDesc': 'All Files',
                //允许上传的文件后缀
                //上传文件大小限制
                'fileSizeLimit': "50MB",
                'successTimeout': 9000,
                //允许上传的文件后缀
                'fileTypeExts': "<%=ViewData["format"] %>",
                //选择文件后自动上传
                'auto': true,
                //设置为true将允许多文件上传
                'multi': true,
                'removeCompleted': false,
                'overrideEvents': ['onDialogClose', 'onUploadError', 'onSelectError'],

                'onSelect': function (file) {

                },
                'onSelectError': function (file, errorCode, errorMsg) {
                    var msgText = "上传失败\n";
                    switch (errorCode) {
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

                    index = layer.msg(msgText, 1, 8, function result() {
                        layer.close(index);
                    });
                    return false;
                },

                'onUploadSuccess': function (file, data, response) {
                    //文件大小
                    var fileSize = Math.round(file.size / 1024);
                    var suffix = 'KB';
                    if (fileSize > 1000) {
                        fileSize = Math.round(fileSize / 1000);
                        suffix = 'MB';
                    }
                    var fileSizeParts = fileSize.toString().split('.');
                    fileSize = fileSizeParts[0];
                    if (fileSizeParts.length > 1) {
                        fileSize += '.' + fileSizeParts[1].substr(0, 2);
                    }
                    fileSize += suffix;
                    $("#" + file.id).find(".cancel").html("");
                    //$("#" + file.id).find(".fileName").html("<a href='javascript:viewOnline(\"" + data.split(",")[1] + "\",\"" + data.split(",")[2] + "\")'>" + file.name + "</a>(" + fileSize + ")");
                    if (data.split(",")[0] == "true") {
                        $("#" + file.id).find(".data").html(" - 上传成功");
                        $("#filename").val(file.name); 
                        $("#filepath").val(data.split(",")[3]); 
                    } else {
                        $("#" + file.id).find(".data").html(" - 上传失败");
                    }

                },

                'onQueueComplete': function (queueData) {
                   
                }

            });
        });

        function ruturnvalue(){
            window.parent.getFile($("#filename").val(),$("#filepath").val());
            // layer弹出层关闭
            var i = parent.layer.getFrameIndex();
            window.parent.layer.close(i);
        }
 
    </script>
</head>
<body style="background:none;margin-top: 5px;height:90%;background-color:rgb(238,238,238);">
 
  <div class="control-group">
            <label class="control-label" id="filetitle">请上传xls文件：</label>
            <div class="controls"> 
                <input type="hidden" id="filename" name="filename"/>
                <input type="hidden" id="filepath" name="filepath"/> 
                <input type="file" id="uploadFile" name="uploadFile"/>
                <input type="hidden" id="dfile" name="dfile"/>

            </div>
     </div>
      <div class="control-group">
    <label class="control-label"></label>

    <div class="controls" style="text-align:center;">
        <button type="button" onclick="ruturnvalue()" class="btn btn-success">确定</button>
    </div>
</div>
</body>
</html>
