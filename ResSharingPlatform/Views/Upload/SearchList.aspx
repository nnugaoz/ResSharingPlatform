<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
    <thead>
        <tr>
            <%if (ViewData["topage"] != null && (ViewData["topage"].ToString() == "pending" || ViewData["topage"].ToString() == "unqualified"))
              { %>
                <th style="text-align:center"><input type="checkbox" onclick="selectAll(this)"/></th>
            <%} %>
            <th>资源名称</th>
            <th>资源分类</th>
            <th>上传时间</th>
            <%if (ViewData["topage"] == null || ViewData["topage"].ToString() == "" || ViewData["topage"].ToString() == "checked")
              { %>
            <th>评价数</th>
            <th>好评百分比</th>
            <th>浏览量</th>
            <th>下载量</th>
            <%} %>
            <th>创建人</th>
            <th>状态</th>
            <th class="center">操作</th>
        </tr>
    </thead>
    <tbody>
        <%
    foreach (var item in ViewData["resources"] as List<ResSharingPlatform.Models.View_Resource>)
            {
                double pes = 0;
                if (item.PRAISE_PRE != null)
                {
                    pes = Convert.ToDouble(item.PRAISE_PRE) * 100;
                } 
        %>
        <tr>
            <%if (ViewData["topage"] != null && (ViewData["topage"].ToString() == "pending" || ViewData["topage"].ToString() == "unqualified"))
              { %>
            <td style="text-align:center">
                <input type="checkbox" name="tableCheck" onclick="selectOne(this)"/>
                <input type="hidden" name="hidIsSelect"/>
                <input type="hidden" name="tableType" value="<%:item.TYPE_ID %>"/>
                <input type="hidden" name="resIds" value="<%:item.ID %>"/>
            </td>
            <%} %>
            <td>
                <%
                if (item.NAME != null)
                { 
                %>
                    <%:Html.ActionLink(ResSharingPlatform.Common.CommonUtil.StringTruncat(item.NAME, 8, "..."), "Detail", new
                        {
                            resId = item.ID,
                            resource = ViewData["resource"],
                            type = ViewData["type"],
                            uploadTime = ViewData["uploadTime"],
                            label = ViewData["label"],
                            orderBy = ViewData["orderBy"],
                            delFlg = ViewData["delFlg"],
                            origin = ViewData["origin"],
                            pagecurrent = ViewData["pagecurrent"],
                            topage = ViewData["topage"]
                        }, new { @title = item.NAME })%>
                <%}
                else if ("2".Equals(item.ORIGIN_FLG))
                { 
                 %>
                    <%:Html.ActionLink("来自一体化课程", "Detail", new
                        {
                            resId = item.ID,
                            resource = ViewData["resource"],
                            type = ViewData["type"],
                            uploadTime = ViewData["uploadTime"],
                            label = ViewData["label"],
                            orderBy = ViewData["orderBy"],
                            delFlg = ViewData["delFlg"],
                            origin = ViewData["origin"],
                            pagecurrent = ViewData["pagecurrent"],
                            topage = ViewData["topage"]
                        }, new { @title = item.NAME })%>
                <%}
                else if ("3".Equals(item.ORIGIN_FLG))
                { 
                 %>
                    <%:Html.ActionLink("来自精品课程", "Detail", new
                        {
                            resId = item.ID,
                            resource = ViewData["resource"],
                            type = ViewData["type"],
                            uploadTime = ViewData["uploadTime"],
                            label = ViewData["label"],
                            orderBy = ViewData["orderBy"],
                            delFlg = ViewData["delFlg"],
                            origin = ViewData["origin"],
                            pagecurrent = ViewData["pagecurrent"],
                            topage = ViewData["topage"]
                        }, new { @title = item.NAME })%>
                <%}
                else if ("4".Equals(item.ORIGIN_FLG))
                { 
                 %>
                    <%:Html.ActionLink("来自互动平台", "Detail", new
                        {
                            resId = item.ID,
                            resource = ViewData["resource"],
                            type = ViewData["type"],
                            uploadTime = ViewData["uploadTime"],
                            label = ViewData["label"],
                            orderBy = ViewData["orderBy"],
                            delFlg = ViewData["delFlg"],
                            origin = ViewData["origin"],
                            pagecurrent = ViewData["pagecurrent"],
                            topage = ViewData["topage"]
                        }, new { @title = item.NAME })%>
                <%}
                  %>
                &nbsp;
            </td>
            <td>
                <%:item.TYPE_NAME %>&nbsp;
            </td>
            <td>
                <%:item.UPLOAD_TIME %>&nbsp;
            </td>
            <%if (ViewData["topage"] == null || ViewData["topage"].ToString() == "" || ViewData["topage"].ToString() == "checked")
              { %>
            <td>
                <%if (item.REVIEW_NUM != null)
                  { %>
                    <%:item.REVIEW_NUM%>
                <%}
                  else
                  { %>
                  0
                <%} %>
                次
            </td>
            <td>
                <%:pes%>%
            </td>
            <td>
                <%if (item.PAGE_VIEW_NUM != null)
                  { %>
                    <%:item.PAGE_VIEW_NUM%>
                <%}
                  else
                  { %>
                  0
                <%} %>
                次
            </td>
            <td>
                <%if (item.DOWNLOAD_NUM != null)
                  { %>
                    <%:item.DOWNLOAD_NUM%>
                <%}
                  else
                  { %>
                    0
                <%} %>
                次
            </td>
            <%} %>
            <td><%:item.CREATE_NAME %></td>
            <td>
                <%  if (item.STATUS == "0")
                    {
                %>
                待审核
                <%}
                  else if (item.STATUS == "1")
                  {
                %>
                <font color="green">审核通过</font>
                <%}
                  else if (item.STATUS == "2")
                  { 
                %>
                <font color="red">审核不通过</font>
                <%} %>
                &nbsp;
            </td>
            <td class="center">
                <%  if (item.STATUS == "0")
                    {
                        if ("1".Equals(ViewData["ExamineAthority"]))
                        {
                %>
                            <%:Html.ActionLink("审核", "Examine", new {   id = item.ID, 
                                                                          resource = ViewData["resource"],
                                                                          type = ViewData["type"],
                                                                          uploadTime = ViewData["uploadTime"],
                                                                          label = ViewData["label"],
                                                                          orderBy = ViewData["orderBy"],
                                                                          delFlg = ViewData["delFlg"],
                                                                          origin = ViewData["origin"],
                                                                          pagecurrent= ViewData["pagecurrent"],
                                                topage = ViewData["topage"]})%>&nbsp;&nbsp;
                                                              
                <%      }
                    }
                    if("1".Equals(ViewData["EditAthority"]))
                    {   
                %>
                        <%:Html.ActionLink("修改", "Upload", new { resId = item.ID, 
                                                                        resource = ViewData["resource"],
                                                                        type = ViewData["type"],
                                                                        uploadTime = ViewData["uploadTime"],
                                                                        label = ViewData["label"],
                                                                        orderBy = ViewData["orderBy"],
                                                                        delFlg = ViewData["delFlg"],
                                                                        origin = ViewData["origin"],
                                                                        pagecurrent= ViewData["pagecurrent"],
                                            topage = ViewData["topage"]})%>&nbsp;&nbsp;
                <%
                    }
                %>
                <% if (item.STATUS == "0" || item.STATUS == "2")
                   {
                       if ("1".Equals(ViewData["DelAthority"]))
                       {  
                %>
                            <a href="javascript:#" onclick = "return deleteRes('<%:item.ID %>')">删除</a>&nbsp;&nbsp;
                <%      }
                   }
                %>
                &nbsp;
            </td>
        </tr>
        <%} %>
    </tbody>
</table>
<input id="currentPageNumber" type="hidden" value="<%:ViewData["pagecurrent"] %>"/>
<div style="text-align: right;" class="yahoo">
    <%
        string linkpage = ViewData["linkpage"] as string;
        Response.Write(linkpage);
    %>
</div>