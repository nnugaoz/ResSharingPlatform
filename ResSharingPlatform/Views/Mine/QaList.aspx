<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<table style="border:0px;" id="scoreTable">     
    <tr>
        <td colspan="3" style="font-size:16px;border:0px;"><b>提问</b></td>
    </tr>                       
    <%if (ViewData["QaList"] != null)
    {
        List<ResSharingPlatform.Models.View_Qa> list = ViewData["QaList"] as List<ResSharingPlatform.Models.View_Qa>;
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            %>
            <tr>
                <td colspan='3' style="height:10px;"></td>
            </tr>
            <tr style="border:0px;border-bottom:1px dashed #b0b0b0;">
                <td>
                    <div style="width:115px;text-align:center;border:0px;" nowrap>
                    <%:string.IsNullOrEmpty(item.CREAT_NAME) ? "匿名" : item.CREAT_NAME%>
                    </div>
                </td>
                <td style="width:100%;border:0px;">
                    <div>
                        <div style="float:left">
                            <div style="font-size: 14px;word-wrap: break-word; word-break: normal;">
                                <%:item.QUESTION%>
                            </div>
                            <div style="font-size: 14px;word-wrap: break-word; word-break: normal">
                                <%if (item.ANSWER == null || item.ANSWER == "")
                                  {%>
                                  <a href="javascript:openAnswer('<%:item.ID %>');" >回复</a>
                                  <div style="display:none" id="divAnswer_<%:item.ID %>">
                                    <div style="float:left;">
                                        <textarea class="text-areas" style="width:728px;padding:10px;height:52px;" id="answer_<%:item.ID %>"></textarea>
                                    </div>
                                    <div style="width:100%;text-align:right">
                                        <input type="button" class="btn2" value="回复" onclick="answerQuest('<%:item.ID %>')"/>
                                    </div>
                                  </div>
                                <%}
                                  else
                                  { %>
                                  <div style="float:left;width:728px;padding:10px;background-color:#f9f9f9;border:1px #e3e3e3 solid;margin:6px 0px;border-radius:2px;">
                                    <div style="height:30px;">
                                        <span style="float:left;font-size: 12px;color: #7a8275;"><%:string.IsNullOrEmpty(item.MODIFY_NAME) ? "匿名" : item.MODIFY_NAME%></span>
                                        <span style="font-size: 12px;color: #b0b0b0;float:right;">
                                            <%:item.MODIFYTIME %>
                                        </span>
                                    </div>
                                    <div style="font-size: 12px;word-wrap: break-word; word-break: normal;">
                                        <%:item.ANSWER %>
                                    </div>
                                  </div>  
                                <%} %>
                            </div>
                        </div>
                     </div>
                </td>
                <td>
                        <div style="width:125px;text-align:right;">
                            <span style="font-size: 12px;color: #b0b0b0;">
                                <%:item.CREATETIME%>
                            </span>
                        </div>
                </td>
            </tr>
                                    
    <%
    }
    }%>
</table>

<div style="text-align: right;padding-top:10px;" class="yahoo">
    <%
        string linkpage = ViewData["qaLinkpage"] as string;
        Response.Write(linkpage);
    %>
</div>