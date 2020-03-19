<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    在线预览
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        window.onload = function () {
            $.ajax({
                type: "post",
                url: "../../Upload/ToView",
                data: "fileName=" + $("#fileName").val(),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        if (data["success"] == "true") {
                            $("#savePath").val(data["savePath"]);
                            $("#filePath").val(data["filePath"]);
                            $("#pageCount").val(data["pageCount"]);
                        } else {
                            $("#waitting").html("<h2>" + data["savePath"] + "</h2>");

                            $("#filePath").val(data["filePath"]);
                            $("#pageCount").val(data["pageCount"]);

                            $("#btnDownload").attr("style", "display:none");
                        }
                    }
                }
            });

            if ($('#extName').val() != "other") {
                
                var file = $("#savePath").val();
                var ext = $("#extName").val();
                var pageCount = $("#pageCount").val();

                if (file != null && file != "") {
                    $("#waitting").html("");
                    if (ext == ".swf") {
                        $('#documentViewer1').attr("style", "width:100%;height:650px;display:display");
                        $('#documentViewer1').FlexPaperViewer({
                            config: {
                                /**分页加载**/
                                SWFFile: "{" + file + "/[*,0].swf," + pageCount + "}",
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
                                WMode: 'transparent',
                                localeChain: 'zh_CN'
                            }
                        });
                    } else if (ext == ".flv") {
                        $("#ckPlayer1").attr("style", "width:100%;height:100%;display:display");
                        var flashvars = {
                            f: file,
                            c: '0',
                            p: 0
                        };
                        var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always', wmode: "transparent" };
                        var video = [file + '->video/mp4'];
                        CKobject.embed('../../Scripts/ckplayer/ckplayer.swf', 'ckPlayer1', 'ckplayer_ckPlayer', '800', '500', false, flashvars, video, params);
                    } else if (ext == ".jpg") {
                        $("#picView1").attr("src", file);
                    }
                    $("#btnDownload").attr("style", "display:display");
                }
            } else {
                $("#waitting").html("<h2>该文件不支持在线预览</h2>");
                $("#btnDownload").attr("style", "display:display");
            }
        }

        function checkDownload() {
            $.ajax({
                type: "post",
                url: "../../Upload/CheckFileIsExist",
                data: "fileName=" + $("#filePath").val(),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data["isExist"] == true) {
                        download();
                    } else {
                        layer.alert("您下载的文件不存在！", 8);
                    }
                }
            });
        }

        function download() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                var form = $("<form>"); //定义一个form表单
                form.attr("style", "display:none");
                form.attr("target", "");
                form.attr("method", "post");
                form.attr("action", "../../Upload/DownLoad");
                var input1 = $("<input>");
                input1.attr("type", "hidden");
                input1.attr("id", "fileName");
                input1.attr("name", "fileName");
                input1.attr("value", $("#filePath").val());
                $("body").append(form); //将表单放置在web中
                form.append(input1);
                form.submit(); //表单提交
                form.remove();
            } else {
                ExamLogin();
            }
        }

        
    </script>

    <div>
        <div class="con-title">
            <h1 class="con-title-h1">预览</h1>
        </div>
        <div class="box">
        <%if (ViewData["extName"].ToString() == "error")
          { %>
            <p><%:ViewData["message"] %></p>
            <br />
            <br />
            <div class="sub-box"  style="text-align:center">
                <input type="button" class="sub-btn" value="关闭" onclick=""/>
            </div>
        <%}
          else
          { %>
            <input type="hidden" id="fileName" value="<%:ViewData["fileName"] %>"/>
            <input type="hidden" id="filePath" value="<%:ViewData["filePath"] %>"/>
            <input type="hidden" id="savePath" value="<%:ViewData["savePath"] %>"/>
            <input type="hidden" id="extName" value="<%:ViewData["extName"] %>"/>
            <input type="hidden" id="pageCount" name="pageCount"/>

            <div id="waitting"><h2>加载中...</h2></div>

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
                <div style="overflow:auto">
                    <image id="picView1" style="display:block; vertical-align:top; "></image>
                </div>
            <%} %>
        
            <br />
            <br />
            <div class="sub-box"  style="text-align:center">
                <%if ("1".Equals(ViewData["DownLoadAthority"]))
                  { %>
                    <input type="submit" class="sub-btn" id="btnDownload" value="下载" onclick="checkDownload()"/>
                <%} %>
                <input type="button" class="sub-btn" value="关闭" onclick="window.close()"/>
            </div>
        <%} %>
        </div>
        
    </div>
</asp:Content>
