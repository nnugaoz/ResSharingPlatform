<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="list-box2">
    <%
        List<ResSharingPlatform.Models.AppendixByLike_Result> videoList = ViewData["reslist"] as List<ResSharingPlatform.Models.AppendixByLike_Result>;
        int videoCount = Convert.ToInt32(ViewData["videoCount"].ToString());
        for (int i = 0; i < videoCount; i++)
        {
            int count = i * 5 + 5;
            if (count > videoList.Count)
            {
                count = videoList.Count;
            }
    %>
    <ul>
        <% 
            for (int j = i * 5; j < count; j++)
            {
                ResSharingPlatform.Models.AppendixByLike_Result item = videoList[j];
                string fileName = item.FILE_NAME;
                string ext = "";
                if(item.FILE_NAME != null && item.FILE_NAME.LastIndexOf('.') > 0)
                {
                    fileName = item.FILE_NAME.Substring(0, item.FILE_NAME.LastIndexOf('.'));
                    ext = item.FILE_NAME.Substring(item.FILE_NAME.LastIndexOf('.'), item.FILE_NAME.Length - item.FILE_NAME.LastIndexOf('.')).ToLower();
                }
        %>
        <li>
            <a href='javascript:viewOnline("<%:item.ID%>","<%:ext %>","<%:item.RES_ID %>")' title="<%: fileName %> ">
                <img class="flv_img" src="../../Mine/ShowImg?id=<%:item.ID %>" alt="<%: fileName %>" />
                <p class="info2">
                    <%:ResSharingPlatform.Common.CommonUtil.StringTruncat(fileName, 12, "...")%>
                </p>
                <p class="info2">作者：<%:item.AUTHOR == null ? "" : item.AUTHOR%></p>
                <p class="info2" style="text-align:left">上传时间:<%:item.UPLOAD_TIME%></p>
                <p class="info2" style="width:85px; float:left; padding-right:2px;">
                    <%if (item.PAGE_VIEW_NUM != null)
                      { %>
                        <%:item.PAGE_VIEW_NUM%>次浏览
                    <%}else{ %>
                        0次浏览
                    <%} %>
                </p>
                <p class="info2" style="width:85px; float:right; padding-right:2px;">
                    <%if (item.DOWNLOAD_NUM != null)
                      { %>
                        <%:item.DOWNLOAD_NUM%>次下载
                    <%}else{ %>
                        0次下载
                    <%} %>
                </p>
                <!--<p class="info2">
                   上传时间：<%:Convert.ToDateTime(item.UPLOAD_TIME.ToString()).ToString("yyyy-MM-dd")%>
                </p>-->
            
            </a>
        </li>
        <%} %>
    </ul>
    <%} %>
</div>
<div style="text-align: right;" class="yahoo">
    <%
        string linkpage = ViewData["linkpage"] as string;
        Response.Write(linkpage);
    %>
</div>
