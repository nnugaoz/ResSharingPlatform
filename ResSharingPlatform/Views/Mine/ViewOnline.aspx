<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    在线预览
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            try {
                var ie = ! -[1, ];
                if (ie) { $("#IE_DIV").css({ "height": "100px" }) }
            }
            catch (e) {
            }
            GetInitList();
            GetQaList();
            GetScoreList();

            var rethtml = '<%=Html.ActionLink("首页", "Index", "Home")%>';
            rethtml += $("#txtLink").val(); 
            $("#hLink").html(rethtml);
        });

        // 提交表单、初始化、分页
        function GetScoreList() {
            $('#Sco_ActionForm').attr("action", "../../Mine/ScoreList");
            $('#Sco_ActionForm').ajaxSubmit(
                function (data) {
                    $("#sco_content").html(data);
                }
            );
        }

        // 提交表单、初始化、分页
        function GetQaList() {
            $('#Qa_ActionForm').attr("action", "../../Mine/QaList");
            $('#Qa_ActionForm').ajaxSubmit(
                function (data) {
                    $("#qa_content").html(data);
                }
            );
        }

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
                            $("#waitting").html(data["savePath"]);
                            $("#filePath").val(data["filePath"]);
                            $("#pageCount").val(data["pageCount"]);
                            $("#btnDownload").attr("style", "display:none");
                            $("#btnCollect").attr("style", "display:none");
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
                        $('#documentViewer').attr("style", "width:100%;height:520px;display:display");
                        $('#documentViewer').FlexPaperViewer({
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
                                SearchMatchAll: true,
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
                        $("#ckPlayer").attr("style", "width:100%;height:100%;display:display;text-align:center");
                        var flashvars = {
                            f: file,
                            c: '0',
                            p: 0
                        };
                        var params = { bgcolor: '#FFF', allowFullScreen: true, allowScriptAccess: 'always', wmode: "transparent" };
                        var video = [file + '->video/mp4'];
                        CKobject.embed('../../Scripts/ckplayer/ckplayer.swf', 'ckPlayer', 'ckplayer_ckPlayer', '750', '520', false, flashvars, video, params);
                    } else if (ext == ".jpg") {
                        $("#picView").attr("src", file);
                    }
                    $("#btnDownload").attr("style", "display:display");
                    $("#btnCollect").attr("style", "display:display");
                } else {
                    $("#waitting").html("<h2>文件不存在<h2>");
                    $("#btnDownload").attr("style", "display:none");
                    $("#btnCollect").attr("style", "display:none");
                }
            } else {
                $("#waitting").text("该文件不支持在线预览");
                $("#btnDownload").attr("style", "display:display");
                $("#btnCollect").attr("style", "display:display");
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
            var form = $("<form>"); //定义一个form表单
            form.attr("style", "display:none");
            form.attr("target", "");
            form.attr("method", "post");
            form.attr("action", "../../Mine/DownLoad");
            var input1 = $("<input>");
            input1.attr("type", "hidden");
            input1.attr("id", "fileName");
            input1.attr("name", "fileName");
            input1.attr("value", $("#filePath").val());
            var input2 = $("<input>");
            input2.attr("type", "hidden");
            input2.attr("id", "resId");
            input2.attr("name", "resId");
            input2.attr("value", $("#res_Id").val());
            var input3 = $("<input>");
            input3.attr("type", "hidden");
            input3.attr("id", "appId");
            input3.attr("name", "appId");
            input3.attr("value", $("#app_Id").val());
            $("body").append(form); //将表单放置在web中
            form.append(input1);
            form.append(input2);
            form.append(input3);
            form.submit(); //表单提交
            form.remove();
        }

        //清除
        function starsClear() {
            $('#rated').hide();
            $('#rating_btns').fadeIn();
            $('#rating_on').fadeIn();
            $("#rating_output").val("not rated");
        }

        //评价
        function evaluated() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                if ($("#rating_output").val() == "not rated" || $("#rating_output").val() == "") {
                    layer.alert("请先评分！", 5);
                    return;
                }
                if ($("#resReview").val() == "") {
                    layer.alert("请输入你的评论！", 5);
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "../../Mine/IsScore",
                    data: "resId=" + $("#res_Id").val() + "&appId=" + $('#app_Id').val(),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data) {
                            layer.alert("您已经评论过了！", 5);
                        } else {
                            $.ajax({
                                type: "post",
                                url: "../../Mine/SaveScore",
                                data: "resId=" + $("#res_Id").val() + "&appId=" + $('#app_Id').val() + "&grade=" + $("#rating_output").val() + "&review=" + $("#resReview").val(),
                                dataType: "json",
                                async: false,
                                success: function (data) {
                                    if (data) {
                                        starsClear();
                                        $("#resReview").val("");
                                        GetScoreList();
                                        $("div[score]").each(function () {
                                            $(this).hide();
                                        });
                                    } else {
                                        layer.alert("评价失败！", 8);
                                    }
                                }
                            });
                        }
                    }
                });
                
            } else {
                ExamLogin();
            }
        }

        //提问
        function toQuestion() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                if ($("#question").val() == "") {
                    layer.alert("请输入你的提问！", 5);
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "../../Mine/SaveQa",
                    data: "resId=" + $("#res_Id").val() + "&appId=" + $('#app_Id').val() + "&question=" + $("#question").val(),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data == true) {
                            $("#question").val("");
                            GetQaList();
                        } else {
                            layer.alert("提问失败！", 8);
                        }
                    }
                });
            } else {
                ExamLogin();
            }
        }

        function openAnswer(id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                if ($("#divAnswer_" + id).css("display") == "none") {
                    $("#divAnswer_" + id).attr("style", "display:block;");
                } else {
                    $("#divAnswer_" + id).attr("style", "display:none");
                }
            } else {
                ExamLogin();
            }
        }

        function answerQuest(id) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                if ($("#answer_" + id).val() == "") {
                    layer.alert("请输入你的回复！", 5);
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "../../Upload/SaveAnswer",
                    data: "qaId=" + id + "&answer=" + $('#answer_' + id).val(),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data != null && data != "") {
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

        /**收藏资源附件 2014-12-18 5920 **/
        function CollectionThis() {
            if ($("#app_Id").val() == "") { return; }
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                $.ajax({
                    type: "post",
                    url: "../../Mine/CollectFile",
                    data: "AID=" + $("#app_Id").val(),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data == "0") {
                            layer.alert("收藏失败！", 8);
                        }
                        else if (data == "2") {
                            $("#btnCollect").hide();
                            $("#btnCannelCollect").show();
                            layer.alert("您已收藏！", 1);
                        }
                        else if (data == "3") {
                            ExamLogin();
                        }
                        else if (data == "1") {
                            $("#btnCollect").hide();
                            $("#btnCannelCollect").show();
                            layer.alert("收藏成功！", 9);
                        }
                    }
                });
            }
            else {
                ExamLogin();
            }
        }
        /**取消收藏**/
        function CannelCollectionThis() {
            $.ajax({
                type: "post",
                url: "../../Mine/CannelMyCollect",
                data: "AID=" + $("#app_Id").val(),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data) {
                        $("#btnCollect").show();
                        $("#btnCannelCollect").hide();
                    }
                    else {
                        layer.alert("删除失败！", 8)
                    }
                }
            });
        }

        /**获取相关资源**/
        function GetInitList() {
            $('#ActionForm').attr("action", "../../Mine/RelatedAnnex");
            $('#ActionForm').ajaxSubmit(
                function (data) {
                    $("#div_content").html(data);
                }
            );
            return false;
        }

        /**点击资源分类导航**/
        function doSearch2(id, belong) {
            window.location.href = "../../Home/ListByType?typeId=" + id + "&belong=" + belong;
        }
    </script>

    <div class="list main-box mt10" style="margin-top:0px;padding-right:5px;">
        <input type="hidden" id="txtLink" value="<%: ViewData["retStr"]%>"/>
        <h1 id="hLink">
            <%: Html.ActionLink("首页", "Index", "Home")%>
        </h1>
    <form id="ActionForm" action="" method="post" >
        <%if (ViewData["extName"].ToString() == "error")
          { %>
            <p><%:ViewData["message"] %></p>
            <br />
            <br />
            <div style="width:100%;text-align:center">
                <input type="button" class="btn2" value="关闭" onclick=""/>
            </div>
        <%}
          else
          { %>
            <input type="hidden" id="fileName" name="fileName" value="<%:ViewData["fileName"] %>"/>
            <input type="hidden" id="filePath" value="<%:ViewData["filePath"] %>"/>
            <input type="hidden" id="savePath" value="<%:ViewData["savePath"] %>"/>
            <input type="hidden" id="extName" name="extName" value="<%:ViewData["extName"] %>"/>
            <input type="hidden" id="res_Id" name="res_Id" value="<%:ViewData["resId"] %>"/>
            <input type="hidden" id="app_Id" name="app_Id" value="<%:ViewData["appId"] %>"/>
            <input type="hidden" id="pageCount" name="pageCount"/>
        </form>
            <div style="height:570px;">
            <div id="leftDiv" style="width:750px;float:left;">
                <h1 style="display: block; line-height:32px; padding:10px 0px 5px;">
                    <%if (ViewData["extName"] == ".swf")
                          {
                              if (ViewData["AnnexType"] == "0")
                              {
                        %>
                                <image src="../../Images/office/word.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                            <%}
                              else if (ViewData["AnnexType"] == "1")
                              {%>
                                <image src="../../Images/office/excel.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                            <%}
                              else if (ViewData["AnnexType"] == "2")
                              { %>
                                <image src="../../Images/office/powerpoint.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                            <%}
                              else if (ViewData["AnnexType"] == "3")
                              { %>
                                <image src="../../Images/office/pdf.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                            <%}
                              else if (ViewData["AnnexType"] == "4")
                              { %>
                                  <image src="../../Images/office/txt.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                            <%}
                              else
                              { %>
                                <image src="../../Images/office/unkonw.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                            <%} %>
                        <%}
                      else if (ViewData["extName"] == ".flv")
                          { %>
                            <image src="../../Images/office/film.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                        <%}
                      else if (ViewData["extName"] == ".jpg")
                      { %>
                            <image src="../../Images/office/pic.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                        <%}
                      else
                      { %>
                            <image src="../../Images/office/unkonw.png" style="display:block; width:24px; height:24px; float:left; margin-top:4px; margin-right:8px; vertical-align:middle; "></image>
                        <%} %>
                    <span id="doc-tittle-5" style=" color:#333; font: 24px '微软雅黑'; line-height:32px;"><%:ViewData["AnnexName"]%></span>
                </h1>
                <div class="wenkuEnd" style="padding-bottom:10px;">
                     贡献者：<%:ViewData["UserName"]%> | 上传时间：<%:ViewData["UploadTime"]%> | 作者：<%:ViewData["Author"] %> | 标签：<%:ViewData["Tag"]%> <br />
                     <%:ViewData["Grage"]%>分 | <%:ViewData["Review_num"]%>次评价 | <%:ViewData["Page_view_num"]%>次浏览 | <%:ViewData["Download_num"]%>次下载
                </div>
                <div id="waitting" style="font-size:14px"><p>加载中...</p></div>
                <%if (ViewData["extName"].ToString() == ".swf")
                  {%>
                    <div id="documentViewer" class="flexpaper_viewer" style="display:none"></div>
                <%}
                  else if (ViewData["extName"].ToString() == ".flv")
                  {%>
                    <div id="ckPlayer" style="display:none"></div>
                <%}
                  else if (ViewData["extName"].ToString() == ".jpg")
                  { %>
                    <div style="overflow:auto">
                        <image id="picView" style="display:block; vertical-align:top; "></image>
                    </div>
                <%} %>
            </div>
            <div id="rightDiv" style="width:243px; font:12px '微软雅黑'; padding:10px; float:right; margin-top:15px; background-color:rgb(251,251,251); border:1px #e5e5e5 solid;">
                <p style="font:16px  '微软雅黑';color:#333; line-height:32px; padding-top:5px; background-color:#e5e5e5; padding-left:10px;">相关资源：</p>
                <div id="div_content" style="padding-top:0px;"></div>
            </div>
            </div>
            <div style="width:100%; clear:both; padding-top:15px; ">
                <div style="font-size:16px;float:left" score>
                    <b>您的评论</b>&nbsp;&nbsp;
                </div>
                <div id="score" style="float:left" score>
	                <div id="rating_cont" class="rating_cont">		
		                <div id="rating_btns" class="rating_btns">
			                <ul>
				                <li style="width: 5px;">0.0</li>
				                <li>0.5</li>
				                <li>1.0</li>
				                <li>1.5</li>
				                <li>2.0</li>
				                <li>2.5</li>
				                <li>3.0</li>
				                <li>3.5</li>
				                <li>4.0</li>
				                <li>4.5</li>
				                <li>5.0</li>
			                </ul>
		                </div>	
		                <div id="rating_on" class="rating_on">&nbsp;</div>
		                <div id="rated" class="rated">
			                <div id="large_stars" class="large_stars">&nbsp;</div>
			                <div id="rating" class="rating">not rated</div>
			                <div style="height:23px;line-height: 23px;font-size: 14px;padding-left: 3px;padding-top: 3px; ">分&nbsp;&nbsp;</div>
			                <div id="rate_edit" class="rate_edit" style="color:Blue;">重新评分</div>
		                </div>		
	                </div>
	                <input type="hidden" id="rating_output" name="rating_output" value="not rated" />
                </div> 
                <div style="width:100%; clear:both; padding-top:10px; " score>
                    <textarea class="text-areas" id="resReview"></textarea>
                </div>
                <div style="float:left; width:150px;" score><span class="red">每人只能发表一次评论！</span></div>
                <div style="width:100%;text-align:right;padding-top:5px; " score>
                    <input type="button" class="btn2" value="发表评论" onclick="evaluated()"/>
                </div>
                <div style="height:10px;"></div>
                <form id="Sco_ActionForm" action="" class="form-horizontal ui-formwizard" method="post">
                    <input type="hidden" id="sco_app_Id" name="sco_app_Id" value="<%:ViewData["appId"] %>"/>
                    <div id="sco_content" style="background-color:#fff;">
                    </div>
                </form>
            </div>
            <div style="height:10px;"></div>
            <div style="width:100%">
                <div style="font-size:16px;float:left">
                    <b>您的提问</b>&nbsp;&nbsp;
                </div>
                <div style="width:100%;clear:both; padding-top:10px;">
                    <textarea style="width:100%"  class="text-areas" id="question"></textarea>
                </div>
                <div style="width:100%;text-align:right;padding-top:5px;">
                    <input type="button" class="btn2" value="提问" onclick="toQuestion()"/>
                </div>
                <div style="height:10px;"></div>
                <form id="Qa_ActionForm" action="" class="form-horizontal ui-formwizard" method="post">
                    <input type="hidden" id="qa_app_Id" name="qa_app_Id" value="<%:ViewData["appId"] %>"/>
                    <div id="qa_content" style="background-color:#fff;">
                    </div>
                </form>
            </div>
        <%} %>
    </div>
    <div id="pop" class="pop">
	    <div id="popHead" class="popContent"> 
            <%if ("1".Equals(ViewData["IsShowCollection"]))
              { %>
              <input id="btnCollect" type="button" class="btn2" style="margin-right:50px;" value="收藏资源" onclick="CollectionThis()" />
              <input id="btnCannelCollect" type="button" class="btn2" style="margin-right:50px;display:none;" value="取消收藏" onclick="CannelCollectionThis();" />
            <%} %>
            <%if ("1".Equals(ViewData["DownLoadAthority"]))
              { %>
		    <input type="submit" class="btn2" id="btnDownload" value="下载" onclick="checkDownload()"/>
            <%} %>
	    </div>
    </div>
</asp:Content>
