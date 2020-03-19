<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <style>
        .listbar
        {
	        z-index:9999;width:100%;position:absolute;left:20px;top:360px;text-align:center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //初始化取数据
            GetInitList();
        });

        // 初始化数据列表
        function GetInitList() {
            app.ajaxForm("../../Message/GetReceiveList", MsgCallBack);
        }

        function MsgCallBack(data) {
            var html = template('MsgList', data);
            $("#div_list").html(html);
        } 
       

       function DeleteMsg(id) {
           $.ajax({
               type: 'post',
               url: "../../Message/DeleteReceiveMsg",
               dataType: 'json',
               data: { rid: id },
               success: function (data) {
                   if (data.result == "success") {
                       layer.alert("删除成功！", 1);
                       GetInitList();
                   } else {
                       layer.alert("删除失败！", 5);
                   }
               }
           });
        }
    </script>

    <!--发件列表-->
        <script id="MsgList" type="text/html">
            <table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
            <thead>
                <tr>
                    
                    <th width="50px">
                        序号
                    </th>
                     <th width="160px">
                        来自
                    </th>
                    <th width="500px">
                        标题
                    </th>
                    <th>
                        发件时间
                    </th>
                    <th>
                        操作
                    </th>
                </tr>
            </thead>
            <tbody id="tbody">
                
                        {{each list as value}}
				            <tr>
				                
				                <td>{{=value.id0}}</td>
                                <td>{{=value.name}}</td>
				                <td><a href="../../Message/readMsg?mid={{=value.msg_id}}&rid={{=value.record_id}}&from=receive">{{=value.msg_title}}</a></td>
				                <td>{{=value.send_time}}</td>
				                <td>
				                    <a style="text-decoration: none;" onclick="DeleteMsg('{{=value.record_id}}');" title="删除">删除</a>
				                </td>
				            </tr>
				        {{/each}} 
            </tbody>
        </table>
        <div style="text-align: right;" class="yahoo">
	        {{=linkpage}}
        </div>
        </script>
<!--发件列表-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    收件箱
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="actionform" action="" class="form-horizontal ui-formwizard" method="post"
    novalidate="novalidate" runat="server"> 
    <div>
        <div class="con-title">
            <h1 class="con-title-h1">
                收件箱</h1>
            <div class="con-title-btns">
               
            </div>
        </div>
        <div class="box">
            
            <div>
                 
            </div>
            <div id="div_list">
            </div>
        </div>
    </div>
    </form>
</asp:Content>
