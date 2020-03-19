<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@  Import   Namespace="System.Web.Script.Serialization"   %>
<%@  Import   Namespace="System.Collections"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    资源平台
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var html = "";
        $(function () {
            $("#menu1").children().addClass("cur");

            var list = '<%=ViewData["typeTree"]%>';
            var treeList = eval(list);
            //GetJsonData(treeList,0);
            //$("#TreeDiv").html(html);
            
            //加载树
            $.fn.zTree.init($("#tdTree"), setting, treeList);
            var treeObj = $.fn.zTree.getZTreeObj("tdTree");
            treeObj.expandAll(true);
        });

        function toListByType(id, belong) {
            window.location.href = "../../Home/ListByType?typeId=" + id + "&belong=" + belong;
        }

        //递归json tree
        function GetJsonData(tree, n) {
            for (var i = 0; i < tree.length; i++) {
                if (tree[i].children == "") {
                    html += '<div  style="padding:3px 0px;padding-left:' + n * 15 + 'px;"><p class="carrow" style="padding-right:5px;"></p><a style="font-size:14px; color:green;" href=\'javascript:toListByType(\"' + tree[i].id + '\",\"' + tree[i].belong + '\")\'>' + fucking(tree[i].text) + '</a></div>';
                }
                else {
                    html += '<div  style="padding:3px 0px;padding-left:' + n * 15 + 'px;"><p class="parrow" style="padding-right:5px;"></p><a style="font-size:14px;color:#333;" href=\'javascript:toListByType(\"' + tree[i].id + '\",\"' + tree[i].belong + '\")\'>' + fucking(tree[i].text) + '</a></div>';
                    GetJsonData(tree[i].children, n + 1);
                }
            }
        }
        //字符串太长截取
        function fucking(str) {
            if (str.length > 8)
                return str.substring(0, 8) + "...";
            else
                return str;
        }

        var TreeID = "";
        /**选择树节点**/
        function zTreeOnClick(event, treeId, treeNode) {
            TreeID = treeId;
            //每次只能选择一个
            var treeObj = $.fn.zTree.getZTreeObj(TreeID);
            treeObj.selectNode(treeNode);
            SelectTreeNode();
        }
        /**获取Node的ID**/
        function GetNodeID() {
            var ret = "";
            if (TreeID != "") {
                var treeObj = $.fn.zTree.getZTreeObj(TreeID);
                var nodes = treeObj.getSelectedNodes();
                ret = nodes[0].id;
            }
            return ret;
        }
        /**获取Node的PID**/
        function GetNodePID() {
            var ret = "";
            if (TreeID != "") {
                var treeObj = $.fn.zTree.getZTreeObj(TreeID);
                var nodes = treeObj.getSelectedNodes();
                ret = nodes[0].pId;
            }
            return ret;
        }
        /**选择分类节点**/
        function SelectTreeNode() {
            toListByType(GetNodeID(), GetNodePID());
        }
	</script>
    <%--树--%>
    <script type="text/javascript">
        var setting = {
            callback: {
                onClick: zTreeOnClick
            },
            check: {
                enable: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            }
        };

        function setCheck() {
            var zTree = $.fn.zTree.getZTreeObj("tdTree"),
			py = "p",
			sy = "s",
			pn = "p",
			sn = "s",
			type = { "Y": py + sy, "N": pn + sn };
            zTree.setting.check.chkboxType = type;
        }
    </script>

   <div class="top" style=" height:300px; padding:10px 0px;">
    	 <img alt="" style="display:block;width:1020px; height:300px;" src="../../Images/xue.jpg" />
    </div>
    
    <div class="main-box" style=" padding-bottom:10px;">
    	<div class="left-box">
            <div class="main-title">
            	<h4>精品文档</h4>
               	<%: Html.ActionLink("更多>>", "Library", "Mine")%>
            </div>
            <div class="list-box">
                <%
                    List<ResSharingPlatform.Models.View_Res_Appendix> docList = ViewData["docList"] as List<ResSharingPlatform.Models.View_Res_Appendix>;
                    int docCount = Convert.ToInt32(ViewData["docCount"].ToString());
                    for (int i = 0; i < docCount; i++)
                    {
                        int count = i * 4 + 4;
                        if (count > docList.Count)
                        {
                            count = docList.Count;
                        }
                %>
                    <ul>
                        <%for (int j = i * 4; j < count; j++)
                          {
                              ResSharingPlatform.Models.View_Res_Appendix app = docList[j];
                              string fileName = System.IO.Path.GetFileNameWithoutExtension(app.FILE_NAME);
                              int viewNum = 0;
                              if (app.PAGE_VIEW_NUM != null)
                              {
                                  viewNum = Convert.ToInt32(app.PAGE_VIEW_NUM);
                              }
                              string ext = System.IO.Path.GetExtension(app.FILE_NAME).ToLower();
                        %>
                            <li>
                    	        <a href='javascript:viewOnline("<%:app.APP_ID%>","<%:ext %>","<%:app.ID %>")' title="<%: fileName + "\n作者：" + (app.AUTHOR == null ? "" : app.AUTHOR) + "\n上传时间：" + app.UPLOAD_TIME %> ">
                        	        <img alt="<%:fileName%>" src="../../Mine/ShowImg?id=<%:app.APP_ID %>" class="flv_img"/>
                                    <p class="info1" style="text-align:left;padding-left:5px;padding-bottom:5px"><%:ResSharingPlatform.Common.CommonUtil.StringTruncat(fileName, 12, "...")%></p>
                                    <p class="info2" style="text-align:left;padding-left:5px;padding-bottom:5px">作者:<%:app.AUTHOR == null ? "" : app.AUTHOR%></p>
                                    <p class="info2" style="text-align:left;padding-left:5px;padding-bottom:5px">上传时间:<%:app.UPLOAD_TIME%></p>

                                    <p class="info2" style="text-align:left;width:85px; float:left; padding-right:2px;;padding-left:5px">
                                        <%if (app.PAGE_VIEW_NUM != null)
                                          { %>
                                            <%:app.PAGE_VIEW_NUM%>次浏览
                                        <%}else{ %>
                                            0次浏览
                                        <%} %>
                                    </p>
                                    <p class="info2" style="text-align:left;width:85px; float:right; padding-right:2px;">
                                        <%if (app.DOWNLOAD_NUM != null)
                                          { %>
                                            <%:app.DOWNLOAD_NUM%>次下载
                                        <%}else{ %>
                                            0次下载
                                        <%} %>
                                    </p>
                                </a>
                            </li>
                        <%} %>
                    </ul>
                <%} %>
            </div>
            <div class="main-title">
            	<h4>优秀视频</h4>
               	<%: Html.ActionLink("更多>>", "Video", "Mine")%>
            </div>
            <div class="list-box">
                <%
                    List<ResSharingPlatform.Models.View_Res_Appendix> videoList = ViewData["videoList"] as List<ResSharingPlatform.Models.View_Res_Appendix>;
                    int videoCount = Convert.ToInt32(ViewData["videoCount"].ToString());
                    for (int i = 0; i < videoCount; i++)
                    {
                        int count = i * 4 + 4;
                        if (count > videoList.Count)
                        {
                            count = videoList.Count;
                        }
                %>
                    <ul>
                        <%for (int j = i * 4; j < count; j++)
                          {
                              ResSharingPlatform.Models.View_Res_Appendix app = videoList[j];
                              string fileName = System.IO.Path.GetFileNameWithoutExtension(app.FILE_NAME);
                              int viewNum = 0;
                              if (app.PAGE_VIEW_NUM != null)
                              {
                                  viewNum = Convert.ToInt32(app.PAGE_VIEW_NUM);
                              }
                              string ext = System.IO.Path.GetExtension(app.FILE_NAME).ToLower();
                        %>
                            <li>
                    	        <a href='javascript:viewOnline("<%:app.APP_ID%>","<%:ext %>","<%:app.ID %>")' title="<%: fileName + "\n作者：" + (app.AUTHOR == null ? "" : app.AUTHOR) + "\n上传时间：" + app.UPLOAD_TIME %> ">
                        	        <img alt="<%:fileName%>" src="../../Mine/ShowImg?id=<%:app.APP_ID %>" class="flv_img"/>
                                    <p class="info1" style="text-align:left;padding-left:5px;padding-bottom:5px"><%:ResSharingPlatform.Common.CommonUtil.StringTruncat(fileName, 12, "...")%></p>
                                    <p class="info2" style="text-align:left;padding-left:5px;padding-bottom:5px">作者:<%:app.AUTHOR == null ? "" : app.AUTHOR%></p>
                                    <p class="info2" style="text-align:left;padding-left:5px;padding-bottom:5px">上传时间:<%:app.UPLOAD_TIME%></p>
                                    
                                    <p class="info2" style="text-align:left;width:85px; float:left; padding-right:2px;padding-left:5px">
                                        <%if (app.PAGE_VIEW_NUM != null)
                                          { %>
                                            <%:app.PAGE_VIEW_NUM%>次浏览
                                        <%}else{ %>
                                            0次浏览
                                        <%} %>
                                    </p>
                                    <p class="info2" style="text-align:left;width:85px; float:right; padding-right:2px;">
                                        <%if (app.DOWNLOAD_NUM != null)
                                          { %>
                                            <%:app.DOWNLOAD_NUM%>次下载
                                        <%}else{ %>
                                            0次下载
                                        <%} %>
                                    </p>
                                </a>
                            </li>
                        <%} %>
                    </ul>
                <%} %>
            </div>
        </div>
        <div class="right-box" style="height:1010px;">
            <div class="right" style="height:990px;">
                <div class="main-title">
                    <h4>资源分类</h4>
                    <a href="javascript:toListByType('','')">更多&gt;&gt;</a>
                </div>
                <div style="width:100%;height:970px;overflow:auto;" id="TreeDiv">
                    <ul id="tdTree" class="ztree">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
