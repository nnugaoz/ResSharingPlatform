<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<table style="border:0px;" id="scoreTable">                     
    <%if (ViewData["QaList"] != null)
    {
        List<ResSharingPlatform.Models.View_Qa> list = ViewData["QaList"] as List<ResSharingPlatform.Models.View_Qa>;
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            %>
            <tr style="border:0px;border-bottom:1px solid #b0b0b0;">
                <td>
                    <div style="width:100px;text-align:center;border:0px;" nowrap>
                    <%:item.CREAT_NAME%>
                    </div>
                </td>
                <td style="width:600px;border:0px;">
                    <div>
                        <div style="float:left">
                            <div style="font-size: 14px;word-wrap: break-word; word-break: normal;">
                                <%:item.QUESTION%>
                            </div>
                            <div style="font-size: 14px;word-wrap: break-word; word-break: normal">
                                <%if (item.ANSWER == null || item.ANSWER == "")
                                  {%>
                                  <a href="javascript:void(0);" onclick="openPop(event,'<%:item.ID %>'); return false;">回复</a>
                                  <div style="display:none" id="divAnswer_<%:item.ID %>">
                                    <div style="float:left;">
                                        <textarea rows="2" cols="120" style="margin:5px" id="answer_<%:item.ID %>"></textarea>
                                    </div>
                                    <div style="width:80px;text-align:center;float:left;">
                                        <input type="button" class="btn btn-primary btn-sm" style="margin:10px" value="回复" onclick="answerQuest('<%:item.ID %>')"/>
                                    </div>
                                  </div>
                                <%}
                                  else
                                  { %>
                                  <div style="background-color:#f1f1f1;">
                                    <div style="margin:0px 20px 0px 30px;">
                                        <span><b style="font-size: 13px;"><%:item.MODIFY_NAME %></b></span>
                                        <br />
                                        <div style="font-size: 13px;word-wrap: break-word; word-break: normal;">
                                            <%:item.ANSWER %>
                                        </div>
                                        <span style="font-size: 12px;color: #b0b0b0;">
                                            <%:item.MODIFYTIME %>
                                        </span>
                                    </div>
                                  </div>  
                                <%} %>
                            </div>
                        </div>
                     </div>
                </td>
                <td style="width:125px;text-align:right;">
                    <span style="font-size: 12px;color: #b0b0b0;">
                        <%:item.CREATETIME%>
                    </span>
                </td>
                <td style="width:50px;">
                    <a href="javascript:deleteQuestion('<%:item.ID %>')" style="margin-left:20px;vertical-align:middle;" title="删除"><image src="../../Images/delete.png"></image></a>
                </td>
            </tr>
                                    
    <%
    }
    }%>
</table>

<div style="text-align: right;" class="yahoo">
    <%
        string linkpage = ViewData["qaLinkpage"] as string;
        Response.Write(linkpage);
    %>
</div>