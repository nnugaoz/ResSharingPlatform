<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    发信
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .listbar
    {
	    z-index:9999;width:100%;position:absolute;left:20px;top:360px;text-align:center;
    }
    .role_userlist
    {
        float:left;
        text-align:center;
        line-height:20px;
        background-color: #0F6099;
        color: #fff!important;
        padding-left: 5px; 
        padding-right: 5px; 
        margin-right:5px;
        margin-top:5px;
        border-radius: 3px; 
    }
    
    .role_userlist a
    { 
        font-size:12px;
        color: #fff!important; 
    }
  
     .tablec
    {
        border-collapse: collapse;
        border: none;
        width: 100%;
    }
    .tablec td
    {
        background:#fff;
        border: solid #000 1px;
    }
    </style>
    <script type="text/javascript">


        $(function () {
            getReadUserList();
            getUnReadUserList();

            if($("#msgtype").val()=="1"){
                getTable();
            }
        });

        function getTable(){
            $.ajax({
                type: 'post',
                url: "../../Message/get_table_list",
                dataType: 'json',
                data: { mid: "<%=ViewData["mid"]%>" },
                success: function (data) {
                    var html = template('tablelist', data);
                    $("#tablegroup").append(html);

                    if("<%=ViewData["from"] %>"=="send"){
                        $("#addform").hide();
                    }
                }
            });
        }

        function getReadUserList() {
            $.ajax({
                type: 'post',
                url: "../../Message/get_read_user_list",
                dataType: 'json',
                data: { mid: "<%=ViewData["mid"]%>" },
                success: function (data) {
                    var html = template('readlist', data);
                    $("#readgroup").append(html);
                }
            });
        }

        function getUnReadUserList() {
            $.ajax({
                type: 'post',
                url: "../../Message/get_unread_user_list",
                dataType: 'json',
                data: { mid: "<%=ViewData["mid"]%>" },
                success: function (data) { 
                    var html = template('unreadlist', data); 
                    $("#unreadgroup").append(html);
                }
            });
        }


        function backSendList() {

            if("<%=ViewData["from"] %>"=="send")
            {
                window.location.href = "../../Message/SendList";
            }else{
                window.location.href = "../../Message/ReceiveList";
            }
        }

        function reply(username,userid,groupid){
            window.location.href = "../../Message/SendBox?groupid="+groupid+"&userid="+userid+"&username="+username;
        }

        function savetable(){ 

            if ($.trim($("#table_title").val()) == "") {
                layer.alert("请输入项目名称！", 5);
                return;
            }

            if ($.trim($("#table_project").val()) == "") {
                layer.alert("请输入项目内容！", 5);
                return;
            }

            app.ajaxForm("../../Message/savetable", saveCallBack); 
        }

        
        function saveCallBack(data) {
            if (data.result == "success") {
                app.msg("签收成功！", 2);
                app.redirect("../../Message/ReceiveList");
            } else {
                layer.alert("签收失败！", 5);
            }
        }
  
    </script>

    <!--自定义类表格-->
    <script id="tablelist" type="text/html">
        <table  class="tablec">
            <tr>
                <td style="width:20%;">项目名</td>
                <td style="width:60%;">项目内容</td>
                <td style="width:20%;">签收人</td>
            </tr>
        {{each list as value}}
            <tr style="background:#efefe">
                 <td style="width:20%;"> {{=value.table_title}}</td>
                 <td style="width:60%;"> {{=value.table_project}}</td>
                 <td style="width:20%;"> {{=value.name}}</td>
            </tr>
         {{/each}}

            <tr id="addform">
                <td style="width:20%;"> 
                <textarea style="width:100%;height:120px;" id="table_title" name="table_title"></textarea>
                </td>
                <td  style="width:60%;"><textarea style="width:100%;height:120px;" id="table_project" name="table_project"></textarea></td>
                 <td  style="width:20%;">
                  <input type="button" id="Button1" name="search" class="title-btn" value="签收保存" onclick="savetable()">
                 </td>
            </tr> 
        </table>
    </script>
    <!--自定义类表格-->

     <!--已签收人员列表-->
    <script id="readlist" type="text/html">
        <ul>
        {{each list as value}}
            <li class="role_userlist" >
                <a href="javascript:;">{{=value.name}}({{=value.receiver_time}})</a> 
            </li>
        {{/each}}
        </ul>
    </script>
    <!--已签收人员列表-->

     <!--未签收人员列表-->
    <script id="unreadlist" type="text/html">
        <ul>
        {{each list as value}}
            <li class="role_userlist">
                <a href="javascript:;">{{=value.name}}</a> 
            </li>
        {{/each}}
        </ul>
    </script>
    <!--未签收人员列表-->
 
    <div>
        <form id="actionform" class="form-horizontal ui-formwizard"  method="post">
 
            <div class="con-title">
               <h1 class="con-title-h1">
                系统自动统计已签收和未签收人员</h1>
            <div class="con-title-btns">
               <input type="button" id="Button3" name="search" class="title-btn" value="返回" onclick="backSendList()" />
            </div>
            </div>
            <div class="box">
                <div class="seach-box">
                <div class="seach-box-tit"><%=ViewData["Msg_Title"]%></div>
                <div class="seach-box-con">
                <table border="0" cellpadding="0" cellspacing="0" class="from-tab">
                    <tr>
                        <td class="right" style="width:140px;">发件人：</td>
                        
                        <td>
                              <div id="Div1">
                                    <%=ViewData["fromusername"]%>
                                    <input type="hidden" name="" value="<%=ViewData["fromuserid"]%>"/>
                                    <input type="hidden" name="" value="<%=ViewData["roleid"]%>"/>
                                    <input type="hidden" id="msgtype" value="<%=ViewData["type"]%>"/> 
                                    <input type="hidden" id="mid" name="Msg_Id" value="<%=ViewData["mid"]%>"/> 
                                     <input type="hidden" id="Hidden1" name="Record_Id" value="<%=ViewData["Record_Id"]%>"/> 
                                    
                              </div>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="right" style="width:140px;">收件人：</td> 
                        <td>
                              <span style="color:red;font-weight:bold;">已签收:</span>
                              <div id="readgroup" style="width:815;height:100px; overflow-x:hidden;overflow-y:auto;">
                                
                              </div>
                                <span style="color:red;font-weight:bold;">未签收:</span>
                              <div id="unreadgroup"  style="width:815;height:100px; overflow-x:hidden;overflow-y:auto;">
                                    
                              </div>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="right" style="width:140px;">主题：</td> 
                        <td>
                            
                        </td>
                    </tr>
                      
                     <tr> 
                        <td class="right" style="width:140px;"></td>
                        
                        <td> 
                             <%=ViewData["Msg_Content"]%> 
                        </td>
                    </tr>
                    
                    
                </table>
            </div>
            </div>
            <div id="tablegroup"></div>
             

            <div class="sub-box" style="text-align:center;">
                <%
                    if (ViewData["from"].ToString() != "send")
                    {    
                 %>
                <input type="button" class="sub-btn" value="回复" onclick="reply('<%=ViewData["fromusername"]%>','<%=ViewData["fromuserid"]%>','<%=ViewData["roleid"]%>');"/> 
                <%
                    }
                         %>
            </div>
            </div>
        </form> 
    </div>
</asp:Content>
