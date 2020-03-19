<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<table style="border:0px;" id="scoreTable">          
    <%if (ViewData["scoList"] != null)
    {
        List<ResSharingPlatform.Models.View_Score> list = ViewData["scoList"] as List<ResSharingPlatform.Models.View_Score>;
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
                            
            string pos = "0px";
            switch (Convert.ToString(item.GRADE))
            {
                case "0.0": pos = "0px"; break;
                case "0.5": pos = "-11px"; break;
                case "1.0": pos = "-22px"; break;
                case "1.5": pos = "-33px"; break;
                case "2.0": pos = "-44px"; break;
                case "2.5": pos = "-55px"; break;
                case "3.0": pos = "-66px"; break;
                case "3.5": pos = "-77px"; break;
                case "4.0": pos = "-88px"; break;
                case "4.5": pos = "-99px"; break;
                case "5.0": pos = "-110px"; break;
                default: pos = "0px"; break;
            }
                            
            %>
            <tr style="border:0px;border-bottom:1px solid #b0b0b0;">
                <td>
                    <div style="width:100px;text-align:center;border:0px;" nowrap>
                    <%:item.CREATE_NAME%>
                    </div>
                </td>
                <td style="width:600px;border:0px;">
                    <div>
                        <div style="float:left">
                            <div style="font-size: 14px;word-wrap: break-word; word-break: normal;">
                                <%:item.REVIEW%>
                            </div>
                            <span style="font-size: 12px;color: #b0b0b0;">
                                <%:item.CREATETIME%>
                            </span>
                        </div>
                    </div>
                </td>
                <td style="width:125px;text-align:right;">
                    <div style="width:100%">
                        <div style="width:20px;float:left;">&nbsp;</div>
		                <div id="Div1" class="rated" style="display:block;width:105px;float:left;">
			                <div id="Div2" class="small_stars" style="background-position:0px <%:pos %>;display: block;">&nbsp;</div>
			                <div id="Div3" class="small_rating" style="display: block;"><%:item.GRADE %>分</div>
		                </div>		
	                    <input type="hidden" id="Hidden2" name="rating_output" value="<%:item.GRADE %>" />
                    </div>
                </td>
                <td style="width:50px;">
                    <a href="javascript:deleteScore('<%:item.ID %>')" style="margin-left:20px;vertical-align:middle;" title="删除"><image src="../../Images/delete.png"></image></a>
                </td>
            </tr>
                                    
    <%
    }
    }%>
</table>

<div style="text-align: right;" class="yahoo">
    <%
        string linkpage = ViewData["scoLinkpage"] as string;
        Response.Write(linkpage);
    %>
</div>