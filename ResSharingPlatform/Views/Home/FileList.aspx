<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<table class="wenkuTable">
    <% 
        foreach (var item in ViewData["appList"] as List<ResSharingPlatform.Models.AppendixByLike_Result>)
        {
            string fileName = "";
            string ext = "";
            if (item.FILE_NAME != null)
            {
                fileName = System.IO.Path.GetFileNameWithoutExtension(item.FILE_NAME);
                ext = System.IO.Path.GetExtension(item.FILE_NAME).ToLower();
            }

            string fileType = "0";
            if (item.TYPE_FLG == "0")
            {
                fileType = ResSharingPlatform.Common.Constant.GetDocType(ext);
            }
        %>
        <tr>
            <td>
                <div class="wenkuName">
                    <div class="wenkuName-left">
                        <%if (item.TYPE_FLG == "0")
                          {
                              if (fileType == "0")
                              {
                        %>
                                <image src="../../Images/office/word.png"></image>
                            <%}
                              else if (fileType == "1")
                              {%>
                                <image src="../../Images/office/excel.png"></image>
                            <%}
                              else if (fileType == "2")
                              { %>
                                <image src="../../Images/office/powerpoint.png"></image>
                            <%}
                              else if (fileType == "3")
                              { %>
                                <image src="../../Images/office/pdf.png"></image>
                            <%}
                              else if (fileType == "4")
                              { %>
                                  <image src="../../Images/office/txt.png"></image>
                            <%}
                              else
                              { %>
                                <image src="../../Images/office/unkonw.png"></image>
                            <%} %>
                        <%}
                          else if (item.TYPE_FLG == "1")
                          { %>
                            <image src="../../Images/office/film.png"></image>
                        <%}
                          else if (item.TYPE_FLG == "2")
                          { %>
                            <image src="../../Images/office/pic.png"></image>
                        <%}
                          else
                          { %>
                            <image src="../../Images/office/unkonw.png"></image>
                        <%} %>
                        <div>
                            <a href='javascript:viewOnline("<%:item.ID%>","<%:ext %>","<%:item.RES_ID %>")'><%:fileName%></a>
                            <input type="hidden" id="Hidden1" name="app_id" value="<%:item.ID%>" />
                            <input type="hidden" id="Hidden2" name="res_id" value="<%:item.RES_ID%>" />
                        </div>
                    </div>
                    <div class="wenkuName-right">
                    <%if (item.GRADE != null)
                      { %>
                        <%:item.GRADE%>
                    <%}else{ %>
                        0
                    <%} %>分</div>
                </div>
                <div class="wenkuInt">
                    资源名称：<%:item.NAME %> <br />
                    资源介绍：<%:item.INTRODUCTION %>
                </div>
                <div class="wenkuEnd">
                    <%:item.UPLOAD_TIME %> | 
                    <%if (item.PAGE_VIEW_NUM != null)
                      { %>
                        <%:item.PAGE_VIEW_NUM %>次浏览
                    <%}else{ %>
                        0次浏览
                    <%} %>
                    | 
                    <%if (item.DOWNLOAD_NUM != null)
                      { %>
                        <%:item.DOWNLOAD_NUM%>次下载
                    <%}else{ %>
                        0次下载
                    <%} %> 
                    | 作者：<%:item.AUTHOR == null ? "" : item.AUTHOR%>
                    | 贡献者：<%:item.USER_NAME %>
                    | 标签：<%:item.LABEL == null ? "" : item.LABEL.Replace(","," ")%>
                </div>
            </td>
        </tr>
    <%} %>
</table>
<div style="text-align: right;" class="yahoo">
    <%
        string linkpage = ViewData["linkpage"] as string;
        Response.Write(linkpage);
    %>
</div>
