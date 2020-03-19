<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>版本对比</title>
    <link href="../../Content/main.css" rel="stylesheet" type="text/css"/>
    <link href="../../Content/add.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/flexpaper/flexpaper.js" type="text/javascript"></script>
    <script src="../../Scripts/flexpaper/flexpaper_handlers.js" type="text/javascript"></script>
    <script src="../../Scripts/ckplayer/ckplayer.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
            var msg = $("#msg_hid").val();
            var ext = $("#extName_hid").val();
            var filePath = $("#fileUrl_hid").val();
            var readPath = $("#readUrl_hid").val();
            var pageCount = $("#pageCount_hid").val();
            if (msg == "OK") {
                if (ext == ".swf") {
                    $('#documentViewer1').attr("style", "width:100%;height:500px;display:display");
                    $('#documentViewer1').FlexPaperViewer({ 
                        config: {
                                /**分页加载**/
                                SWFFile: "{" + readPath + "/[*,0].swf," + pageCount + "}",
                                Scale: 0.6,
                                ZoomTransition: 'easeOut',
                                ZoomTime: 0.5,
                                ZoomInterval: 0.2,
                                FitPageOnLoad: true,
                                FitWidthOnLoad: true,
                                FullScreenAsMaxWindow: false,
                                ProgressiveLoading: false,
                                MinZoomSize: 0.2,
                                MaxZoomSize: 5,
                                SearchMatchAll: false,
                                InitViewMode: 'Portrait',
                                RenderingOrder: 'flash',
                                StartAtPage: '',
                                ViewModeToolsVisible: true,
                                ZoomToolsVisible: true,
                                NavToolsVisible: true,
                                CursorToolsVisible: true,
                                SearchToolsVisible: true,
                                WMode: 'window',
                                localeChain: 'zh_CN'
                            }
                        });
                } else if (ext == ".flv") {
                    $("#ckPlayer1").attr("style", "width:100%;height:100%;display:display");
                    var flashvars = {
                        f: readPath,
                        c: '0',
                        p: 0
                    };
                    var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always', wmode: "transparent" };
                    var video = [readPath + '->video/mp4'];
                    CKobject.embed('../../Scripts/ckplayer/ckplayer.swf', 'ckPlayer1', 'ckplayer_ckPlayer', '600', '500', false, flashvars, video, params);
                } else if (ext == ".jpg") {
                    $("#picView1").attr("src", readPath);
                } else {
                    $("#info").html("该文件不支持在线预览！");
                }
            }
            //一旦有旧的版本 就展示信息
            if ($("#selVersion").val() != null && $("#selVersion").val() != "") {
                GetVersionInfo($("#selVersion").val());
            }
            //选择版本 展示信息
            $("#selVersion").change(function () {
                GetVersionInfo($("#selVersion").val());
            });
        });

        /**获取版本信息**/
        function GetVersionInfo(Oid) {
            $.ajax({
                type: "post",
                url: "../../Upload/VersionInfo",
                data: "Oid=" + Oid,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        if (data["success"] == "true") {
                            ShowVersioninfo(data["ext"], data["fileUrl"], data["readUrl"], data["pageCount"]);
                        } else {
                            var msg = data["MSG"];
                            $("#rightConnect").html("<h2>" + msg + "</h2>");
                        }
                    }
                }
            });
        }
        
        /**展示版本信息**/
        function ShowVersioninfo(ext, fileUrl, readUrl, pageCount) {
            if (ext == ".swf") {
                var html = '<div id="documentViewer2" class="flexpaper_viewer" style="display:none"></div>';
                $("#rightConnect").html(html);
                $('#documentViewer2').attr("style", "width:100%;height:500px;display:display");
                $('#documentViewer2').FlexPaperViewer({ 
                    config: {
                            /**分页加载**/
                            SWFFile: "{" + readUrl + "/[*,0].swf," + pageCount + "}",
                            Scale: 0.6,
                            ZoomTransition: 'easeOut',
                            ZoomTime: 0.5,
                            ZoomInterval: 0.2,
                            FitPageOnLoad: true,
                            FitWidthOnLoad: true,
                            FullScreenAsMaxWindow: false,
                            ProgressiveLoading: false,
                            MinZoomSize: 0.2,
                            MaxZoomSize: 5,
                            SearchMatchAll: false,
                            InitViewMode: 'Portrait',
                            RenderingOrder: 'flash',
                            StartAtPage: '',
                            ViewModeToolsVisible: true,
                            ZoomToolsVisible: true,
                            NavToolsVisible: true,
                            CursorToolsVisible: true,
                            SearchToolsVisible: true,
                            WMode: 'window',
                            localeChain: 'zh_CN'
                        }
                    });
            } else if (ext == ".flv") {
                var html = '<div id="ckPlayer2" style="display:none"></div>';
                $("#rightConnect").html(html);
                $("#ckPlayer2").attr("style", "width:100%;height:100%;display:display");
                var flashvars = {
                    f: readUrl,
                    c: '0',
                    p: 0
                };
                var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always', wmode: "transparent" };
                var video = [readUrl + '->video/mp4'];
                CKobject.embed('../../Scripts/ckplayer/ckplayer.swf', 'ckPlayer2', 'ckplayer_ckPlayer', '600', '500', false, flashvars, video, params);
            } else if (ext == ".jpg") {
                var html = '<div style="height:520px;overflow:auto"><image id="picView2" style="display:block; vertical-align:top; "></image></div>';
                $("#rightConnect").html(html);

                $("#picView2").attr("src", readUrl);
            } else {
                $("#rightConnect").html("<h2>该文件不支持在线预览！</h2>");
            }
        }
    </script>
</head>
<body>
    <div style="width: 1280px;margin: 0 auto;padding-top: 0px;">
    <div class="con-title">
        <h1 class="con-title-h1">版本对比</h1>
    </div>
    <input type="hidden" id="msg_hid" value="<%: ViewData["MSG"]%>" />
    <input type="hidden" id="extName_hid" value="<%: ViewData["extName"]%>" />
    <input type="hidden" id="fileUrl_hid" value="<%: ViewData["fileUrl"]%>" />
    <input type="hidden" id="readUrl_hid" value="<%: ViewData["readUrl"]%>" />
    <input type="hidden" id="pageCount_hid" value="<%: ViewData["pageCount"]%>" />
    <%if (ViewData["MSG"] == "OK")
      {%>
        <div id="leftDiv" style="float:left;width:635px;">
            <div id="leftTitle" style="height:30px;">
                <a style="margin-left:10px;"><%:ViewData["fileName"] %></a>
                <a id="info" style="margin-left:50px;"></a>
            </div>
            <div id="leftConnect">
                <%if (ViewData["extName"].ToString() == ".swf")
                  {%>
                <div id="documentViewer1" class="flexpaper_viewer" style="display:none"></div>
                <%}
                  else if (ViewData["extName"].ToString() == ".flv")
                  {%>
                <div id="ckPlayer1" style="display:none"></div>
                <%}
                  else if (ViewData["extName"].ToString() == ".jpg")
                  { %>
                <div style="height:520px;overflow:auto">
                    <image id="picView1" style="display:block; vertical-align:top; "></image>
                </div>
                <%} %>
            </div>
        </div>
        <div id="rightDiv" style="float:right;width:635px;">
            <div id="rightTitle" style="height:30px;">
                对比版本：
                <select id="selVersion" name="selVersion" class="inputText" style="width:200px;">
                    <%if (ViewData["VersionList"] != null)
                      {
                          foreach (var item in ViewData["VersionList"] as List<ResSharingPlatform.Models.VersionModel>)
                          {
                              var selectName = "V" + item.Version + "—" + System.IO.Path.GetFileNameWithoutExtension(item.Name);
                              %>
                                <option value="<%: item.ID %>"><%: selectName%></option>   
                              <%
                          }
                          %>
                    <%} %>
                </select>
            </div>
            <div id="rightConnect">
                
            </div>
        </div>
    <%}
      else
      { %>
        <p><%:ViewData["MSG"]%></p>
    <%} %>
    </div>
</body>
</html>
