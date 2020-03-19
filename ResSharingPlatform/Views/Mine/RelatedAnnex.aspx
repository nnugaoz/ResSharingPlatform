<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<table class="wenkuTable">
    <% 
        foreach (var item in ViewData["reslist"] as List<ResSharingPlatform.Models.View_Appendix>)
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
                    <div class="wenkuName-left" style="float:left;width:160px; font:14px '微软雅黑'; padding-bottom:8px; line-height:32px;">
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
                            <a href='javascript:viewOnline("<%:item.ID%>","<%:ext %>","<%:item.RES_ID %>")'><%:fileName.Length > 8 ? fileName.Substring(0, 7) + "..." : fileName%></a>
                            <input type="hidden" id="Hidden1" name="app_id" value="<%:item.ID%>" />
                            <input type="hidden" id="Hidden2" name="res_id" value="<%:item.RES_ID%>" />
                        </div>
                    </div>
                    <div class="wenkuName-right" style="float:right;width:60px;">
                    <%if (item.GRADE != null)
                      { %>
                        <%:item.GRADE%>
                    <%}else{ %>
                        0
                    <%} %>分
                    </div>
                </div>
                <div class="wenkuEnd" style="float:left;"> 
                    <%if (item.PAGE_VIEW_NUM != null)
                      { %>
                        <%:item.PAGE_VIEW_NUM %>次浏览
                    <%}else{ %>
                        0次浏览
                    <%} %>
                    | 作者：<%:item.AUTHOR == null ? "" : item.AUTHOR%>
                    | 贡献者：<%:item.USER_NAME %>
                </div>
            </td>
        </tr>
    <%} %>
</table>
